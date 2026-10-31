using System;
using System.IO;
using AzureArchive.BackgroundCompat;

internal static class Program
{
    private static int Main()
    {
        string root = Path.Combine(Path.GetTempPath(), "azurearchive-background-tests", "resources");
        string expected = Path.GetFullPath(Path.Combine(root, "images", "scene.png"));

        AssertEqual(expected, SafeResourcePath.ResolveUnderRoot(root, "images/scene.png"), "nested path");
        AssertEqual(expected, SafeResourcePath.ResolveUnderRoot(root, @"images\scene.png"), "Windows separators");


        // Android 1.0 BackgroundResource model: enum values and FullPath semantics.
        AssertEqual("1", ((int)BackgroundResourceKind.Image).ToString(), "image kind numeric value");
        AssertEqual("3", ((int)BackgroundResourceKind.Spine).ToString(), "Spine kind numeric value");
        AssertEqual("0", ((int)BackgroundResourceOrigin.Native).ToString(), "native origin numeric value");
        AssertEqual("1", ((int)BackgroundResourceOrigin.GlobalCustom).ToString(), "global custom origin numeric value");
        AssertEqual("2", ((int)BackgroundResourceOrigin.LocalCustom).ToString(), "local custom origin numeric value");
        var nativeResource = new BackgroundResourceDescriptor(
            7, "native-scene", BackgroundResourceKind.Image, BackgroundResourceOrigin.Native,
            "Addressables/Backgrounds/scene.png", root, false);
        AssertEqual("Addressables/Backgrounds/scene.png", nativeResource.FullPath, "native FullPath is unchanged");
        var customResource = new BackgroundResourceDescriptor(
            8, "custom-scene", BackgroundResourceKind.Image, BackgroundResourceOrigin.GlobalCustom,
            "images/scene.png", root, false);
        AssertEqual(expected, customResource.FullPath, "custom FullPath resolves beneath root");
        var customSpine = new BackgroundResourceDescriptor(
            9, "custom-spine", BackgroundResourceKind.Spine, BackgroundResourceOrigin.LocalCustom,
            "spine/scene.skel", root, true);
        AssertTrue(customSpine.IsCustomSpine, "Spine metadata marks custom Spine resource");
        AssertEqual(Path.GetFullPath(Path.Combine(root, "spine", "scene.skel")),
            customSpine.FullPath, "custom Spine path resolves beneath root");
        var nativeSpine = new BackgroundResourceDescriptor(
            10, "native-spine", BackgroundResourceKind.Spine, BackgroundResourceOrigin.Native,
            "Addressables/Spine/scene.skel", root, false);
        AssertFalse(nativeSpine.IsCustomSpine, "native Spine without override metadata is not custom Spine");
        var unsafeCustom = new BackgroundResourceDescriptor(
            11, "unsafe", BackgroundResourceKind.Image, BackgroundResourceOrigin.LocalCustom,
            "../outside.png", root, false);
        AssertThrows<InvalidDataException>(() => { var ignored = unsafeCustom.FullPath; },
            "custom FullPath rejects root traversal");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot("", "a.png"), "empty root");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, " "), "empty relative");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "../outside.png"), "parent traversal");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, @"images\..\..\outside.png"), "mixed traversal");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, Path.GetFullPath("outside.png")), "absolute path");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, @"C:\outside.png"), "drive-qualified path");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/./scene.png"), "dot segment");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/file:stream.png"), "alternate data stream");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/trailing./scene.png"), "trailing dot segment");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/\0scene.png"), "null character");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/\nscene.png"), "control character");

        var local = new[] { "local-other", "local-match", "local-second-match" };
        var global = new[] { "global-match" };
        AssertTrue(BackgroundOverrideResolver.TryResolve(local, global, x => x.EndsWith("match", StringComparison.Ordinal), out string selected), "local override found");
        AssertEqual("local-match", selected, "local override precedes global");
        AssertTrue(BackgroundOverrideResolver.TryResolve(Array.Empty<string>(), global, x => x == "global-match", out selected), "global fallback");
        AssertEqual("global-match", selected, "global override selected");
        AssertFalse(BackgroundOverrideResolver.TryResolve<string>(null, null, x => true, out selected), "null lists have no result");
        AssertFalse(BackgroundOverrideResolver.TryResolve(local, global, x => x == "missing", out selected), "no matching override");

        var localBackgrounds = new[]
        {
            new BackgroundResourceDescriptor(41, "local-other", BackgroundResourceKind.Image, BackgroundResourceOrigin.LocalCustom, "other.png", root, false),
            new BackgroundResourceDescriptor(42, "local-first-match", BackgroundResourceKind.Image, BackgroundResourceOrigin.LocalCustom, "first.png", root, false),
            new BackgroundResourceDescriptor(42, "local-second-match", BackgroundResourceKind.Image, BackgroundResourceOrigin.LocalCustom, "second.png", root, false)
        };
        var globalBackgrounds = new[]
        {
            new BackgroundResourceDescriptor(42, "global-match", BackgroundResourceKind.Image, BackgroundResourceOrigin.GlobalCustom, "global.png", root, false)
        };
        AssertTrue(BackgroundOverrideResolver.TryResolve(42u, localBackgrounds, globalBackgrounds, out var idResolved),
            "ID-based local override resolution");
        AssertEqual("local-first-match", idResolved.Name, "ID-based resolution uses first local ID match before global");
        AssertTrue(BackgroundOverrideResolver.TryResolve(99u, localBackgrounds, globalBackgrounds, out idResolved) == false,
            "ID-based resolution returns false when no override matches");
        AssertTrue(BackgroundOverrideResolver.TryResolve(42u, Array.Empty<BackgroundResourceDescriptor>(), globalBackgrounds, out idResolved),
            "ID-based global override fallback");
        AssertEqual("global-match", idResolved.Name, "ID-based resolution selects global fallback");
        AssertFalse(BackgroundOverrideResolver.TryResolve(42u, null, null, out idResolved),
            "ID-based resolution accepts null override sequences");

        int scenarioLookups = 0;
        var nativeRow = new ScenarioBackgroundRecord(42u, "backgrounds/scene-night.png", BackgroundResourceKind.Image);
        var resolvedLocal = ScenarioBackgroundResolver.Resolve(
            42u, localBackgrounds, globalBackgrounds,
            requestedId => { scenarioLookups++; return nativeRow; });
        AssertTrue(resolvedLocal != null, "scenario resolver returns local override");
        AssertEqual("local-first-match", resolvedLocal!.Name, "scenario resolver local override wins before Excel");
        AssertEqual("0", scenarioLookups.ToString(), "scenario lookup skipped after override hit");
        var resolvedGlobal = ScenarioBackgroundResolver.Resolve(
            42u, Array.Empty<BackgroundResourceDescriptor>(), globalBackgrounds,
            requestedId => { scenarioLookups++; return nativeRow; });
        AssertTrue(resolvedGlobal != null, "scenario resolver returns global override");
        AssertEqual("global-match", resolvedGlobal!.Name, "scenario resolver global override wins before Excel");
        AssertEqual("0", scenarioLookups.ToString(), "scenario lookup skipped after global override hit");
        var resolvedNative = ScenarioBackgroundResolver.Resolve(
            42u, null, null,
            requestedId => { scenarioLookups++; return nativeRow; });
        AssertTrue(resolvedNative != null, "scenario resolver creates native fallback");
        AssertEqual("scene-night", resolvedNative!.Name, "native fallback name uses filename stem");
        AssertEqual("backgrounds/scene-night.png", resolvedNative.Path, "native fallback preserves Excel filename path");
        AssertEqual("Native", resolvedNative.Origin.ToString(), "native fallback source");
        AssertEqual("Image", resolvedNative.Kind.ToString(), "native fallback background type");
        AssertEqual("1", scenarioLookups.ToString(), "native fallback performs one scenario lookup");
        AssertTrue(ScenarioBackgroundResolver.TryGetBackgroundOverridePath(
            resolvedLocal, out string? localOverridePath), "custom Image exposes override path");
        AssertEqual(Path.GetFullPath(Path.Combine(root, "first.png")), localOverridePath,
            "override path uses the resolved custom image FullPath");
        AssertFalse(ScenarioBackgroundResolver.TryGetBackgroundOverridePath(
            resolvedNative, out string? nativeOverridePath), "native Image does not expose custom override path");
        AssertTrue(nativeOverridePath == null, "native override path output is null");
        AssertFalse(ScenarioBackgroundResolver.TryGetBackgroundOverridePath(
            null, out string? missingOverridePath), "missing resource has no override path");
        AssertTrue(missingOverridePath == null, "missing override path output is null");
        var zeroId = ScenarioBackgroundResolver.Resolve(
            0u, null, null,
            requestedId => { scenarioLookups++; return nativeRow; });
        AssertTrue(zeroId == null, "zero background ID resolves to null");
        AssertEqual("1", scenarioLookups.ToString(), "zero background ID skips scenario lookup");
        var mismatchedRow = ScenarioBackgroundResolver.Resolve(
            43u, null, null,
            requestedId => new ScenarioBackgroundRecord(42u, "backgrounds/wrong.png", BackgroundResourceKind.Image));
        AssertTrue(mismatchedRow == null, "scenario resolver rejects mismatched Name ID");
        var missingRow = ScenarioBackgroundResolver.Resolve(
            44u, null, null,
            requestedId => null);
        AssertTrue(missingRow == null, "scenario resolver returns null when Excel row is absent");
        AssertThrows<ArgumentNullException>(() => ScenarioBackgroundResolver.Resolve(
            42u, null, null, null!), "scenario resolver requires lookup delegate");

        AssertEqual("0", BackgroundResourceIdHash.Calculate(null).ToString(), "null resource ID hash");
        AssertEqual("2CC5D05", BackgroundResourceIdHash.Calculate("").ToString("X"), "xxHash32 empty UTF-8 vector");
        AssertEqual("550D7456", BackgroundResourceIdHash.Calculate("a").ToString("X"), "xxHash32 single-byte UTF-8 vector");
        AssertEqual("32D153FF", BackgroundResourceIdHash.Calculate("abc").ToString("X"), "xxHash32 abc UTF-8 vector");
        AssertThrows<ArgumentOutOfRangeException>(() => BackgroundResourceIdHash.Calculate(new string('x', 257)),
            "long resource ID uses unported Android branch");
        var imageOverride = BackgroundImageOverrideFactory.Create(
            @"scenes\night.bg.png", root, BackgroundResourceOrigin.LocalCustom);
        AssertEqual("scenes/night.bg.png", imageOverride.Path, "image override normalizes Windows separators");
        AssertEqual("night.bg", imageOverride.Name, "image override name uses filename stem");
        AssertEqual(BackgroundResourceIdHash.Calculate("night.bg").ToString(), imageOverride.Id.ToString(),
            "image override ID hashes filename stem");
        AssertEqual(expected = Path.GetFullPath(Path.Combine(root, "scenes", "night.bg.png")),
            imageOverride.FullPath, "image override FullPath remains root-relative");
        AssertThrows<ArgumentOutOfRangeException>(() => BackgroundImageOverrideFactory.Create(
            "scene.png", root, BackgroundResourceOrigin.Native), "image override rejects native origin");

        var spineRecord = new SpineBackgroundOverrideRecord(
            "scene-spine", @"spine\night\night.skel", "Idle", false, "Night",
            @"preview\night.png", 2.5f, 1.25f, -12f, 8f);
        var spineDescriptor = SpineBackgroundOverrideFactory.Create(
            spineRecord, root, BackgroundResourceOrigin.LocalCustom);
        AssertEqual(BackgroundResourceIdHash.Calculate("scene-spine").ToString(), spineDescriptor.Id.ToString(),
            "Spine override ID hashes Identifier");
        AssertEqual("scene-spine", spineDescriptor.Name, "Spine override name is Identifier");
        AssertEqual("Spine", spineDescriptor.Kind.ToString(), "Spine override kind");
        AssertEqual("LocalCustom", spineDescriptor.Origin.ToString(), "Spine override source");
        AssertEqual("spine/night/night.skel", spineDescriptor.Path, "Spine path separators normalized");
        AssertEqual(Path.GetFullPath(Path.Combine(root, "spine", "night", "night.skel")),
            spineDescriptor.FullPath, "Spine FullPath resolves under basePath");
        AssertTrue(spineDescriptor.IsCustomSpine, "Spine descriptor carries custom Spine metadata");
        AssertFalse(ScenarioBackgroundResolver.TryGetBackgroundOverridePath(
            spineDescriptor, out string? spineOverridePath), "custom Spine does not expose static image override path");
        AssertTrue(spineOverridePath == null, "Spine override path output is null");
        AssertTrue(spineDescriptor.SpineOverride != null, "Spine descriptor retains runtime settings");
        AssertEqual("Idle", spineDescriptor.SpineOverride!.Animation, "Spine animation retained");
        AssertEqual("Night", spineDescriptor.SpineOverride.Skin, "Spine skin retained");
        AssertEqual("preview/night.png", spineDescriptor.SpineOverride.PreviewPath, "preview path normalized and retained");
        AssertEqual(Path.GetFullPath(Path.Combine(root, "preview", "night.png")),
            spineDescriptor.SpinePreviewFullPath, "Spine preview path resolves beneath basePath");
        var unsafePreview = SpineBackgroundOverrideFactory.Create(
            new SpineBackgroundOverrideRecord("unsafe-preview", "scene.skel", previewPath: "../outside.png"),
            root, BackgroundResourceOrigin.LocalCustom);
        AssertThrows<InvalidDataException>(() => { var ignored = unsafePreview.SpinePreviewFullPath; },
            "Spine preview path rejects root traversal");
        AssertEqual("False", spineDescriptor.SpineOverride.Loop.ToString(), "Spine loop setting retained");
        AssertEqual("1.25", spineDescriptor.SpineOverride.Scale.ToString(System.Globalization.CultureInfo.InvariantCulture),
            "Spine scale retained");
        AssertEqual("-12", spineDescriptor.SpineOverride.OffsetX.ToString(System.Globalization.CultureInfo.InvariantCulture),
            "Spine X offset retained");
        AssertEqual("8", spineDescriptor.SpineOverride.OffsetY.ToString(System.Globalization.CultureInfo.InvariantCulture),
            "Spine Y offset retained");
        var defaultSpine = new SpineBackgroundOverrideRecord("default", "default.skel");
        AssertTrue(defaultSpine.Loop, "Spine default loop is true");
        AssertEqual("1", defaultSpine.Scale.ToString(System.Globalization.CultureInfo.InvariantCulture),
            "Spine default scale is one");
        AssertEqual("0", defaultSpine.PreviewTime.ToString(System.Globalization.CultureInfo.InvariantCulture),
            "Spine default preview time is zero");
        AssertThrows<InvalidDataException>(() => SpineBackgroundOverrideFactory.Create(
            new SpineBackgroundOverrideRecord(" ", "scene.skel"), root, BackgroundResourceOrigin.LocalCustom),
            "Spine requires identifier");
        AssertThrows<InvalidDataException>(() => SpineBackgroundOverrideFactory.Create(
            new SpineBackgroundOverrideRecord("no-path", " "), root, BackgroundResourceOrigin.LocalCustom),
            "Spine requires skeleton path");
        AssertThrows<InvalidDataException>(() => SpineBackgroundOverrideFactory.Create(
            new SpineBackgroundOverrideRecord("bad-scale", "scene.skel", scale: 0f), root, BackgroundResourceOrigin.LocalCustom),
            "Spine scale must be positive");
        AssertThrows<InvalidDataException>(() => SpineBackgroundOverrideFactory.Create(
            new SpineBackgroundOverrideRecord("bad-offset", "scene.skel", offsetX: float.NaN), root, BackgroundResourceOrigin.LocalCustom),
            "Spine offset must be finite");
        AssertThrows<InvalidDataException>(() => SpineBackgroundOverrideFactory.Create(
            new SpineBackgroundOverrideRecord("bad-preview", "scene.skel", previewTime: -1f), root, BackgroundResourceOrigin.LocalCustom),
            "Spine preview time must be nonnegative");
        AssertThrows<InvalidDataException>(() => SpineBackgroundOverrideFactory.Create(
            new SpineBackgroundOverrideRecord("bad-infinity", "scene.skel", offsetY: float.PositiveInfinity), root, BackgroundResourceOrigin.LocalCustom),
            "Spine framing must reject infinity");
        AssertThrows<InvalidDataException>(() => SpineBackgroundOverrideFactory.Create(
            new SpineBackgroundOverrideRecord("zero-hash", null), root, BackgroundResourceOrigin.LocalCustom),
            "Spine zero hash / missing path rejected");
        AssertThrows<ArgumentOutOfRangeException>(() => SpineBackgroundOverrideFactory.Create(
            defaultSpine, root, BackgroundResourceOrigin.Native), "Spine factory rejects native origin");
        AssertThrows<InvalidDataException>(() => SpineBackgroundOverrideFactory.CreateMany(
            new[] { defaultSpine, defaultSpine }, root, BackgroundResourceOrigin.LocalCustom),
            "duplicate Spine IDs rejected");
        var mixedOverrides = BackgroundOverrideSetFactory.Create(
            new[] { "image.png" },
            new[] { defaultSpine },
            root,
            BackgroundResourceOrigin.GlobalCustom);
        AssertEqual("2", mixedOverrides.Count.ToString(), "mixed override set includes image and Spine");
        AssertEqual("Image", mixedOverrides[0].Kind.ToString(), "image overrides enumerate before Spine");
        AssertEqual("Spine", mixedOverrides[1].Kind.ToString(), "Spine overrides follow image overrides");
        AssertEqual("GlobalCustom", mixedOverrides[1].Origin.ToString(), "mixed override source preserved");

        var deduplicatedOverrides = BackgroundOverrideSetFactory.Create(
            new[] { "image.png", "image.png", @"image\image.png" },
            Array.Empty<SpineBackgroundOverrideRecord>(),
            root,
            BackgroundResourceOrigin.LocalCustom);
        AssertEqual("2", deduplicatedOverrides.Count.ToString(), "image override paths deduplicated by exact path");
        AssertEqual("LocalCustom", deduplicatedOverrides[0].Origin.ToString(), "local override source preserved");
        AssertEqual("0", BackgroundOverrideSetFactory.Create(
            null, null, root, BackgroundResourceOrigin.GlobalCustom).Count.ToString(),
            "null override collections produce empty set");
        AssertThrows<InvalidDataException>(() => BackgroundOverrideSetFactory.Create(
            new[] { (string)null! }, null, root, BackgroundResourceOrigin.LocalCustom),
            "null image path rejected");
        AssertThrows<ArgumentOutOfRangeException>(() => BackgroundOverrideSetFactory.Create(
            null, null, root, BackgroundResourceOrigin.Native),
            "native origin rejected for override set");
        AssertTrue(BackgroundOverrideResolver.TryResolve(
            spineDescriptor.Id,
            new[] { spineDescriptor },
            Array.Empty<BackgroundResourceDescriptor>(),
            out var spineSelected) && spineSelected.Kind == BackgroundResourceKind.Spine,
            "ID resolver selects Spine override descriptor");
        AssertThrows<ArgumentNullException>(() => BackgroundOverrideResolver.TryResolve(local, global, null!, out selected), "null predicate");

        const string catalogToken = AddressableInternalIdResolver.AddrLoadUrlToken;
        AssertEqual("https://cdn.example/game/avatars_assets_all.bundle",
            AddressableInternalIdResolver.Resolve(catalogToken + "/avatars_assets_all.bundle", "https://cdn.example/game/"),
            "catalog token replaced with configured base URL");
        AssertEqual("https://cdn.example/characters_assets_all.bundle",
            AddressableInternalIdResolver.Resolve(catalogToken + "/characters_assets_all.bundle", "https://cdn.example///"),
            "base URL trailing separators normalized");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve("", "https://cdn.example"), "empty catalog internal ID");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "/a.bundle", ""), "empty AddrLoadUrl");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve("/a.bundle", "https://cdn.example"), "unrecognized catalog token");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "/../escape.bundle", "https://cdn.example"), "catalog parent traversal");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "/%2e%2e/escape.bundle", "https://cdn.example"), "encoded catalog traversal");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "/a%2fb.bundle", "https://cdn.example"), "encoded catalog separator");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "//evil.example/a.bundle", "https://cdn.example"), "authority override");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "/a.bundle", "file:///tmp"), "non-http base URL");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "/a.bundle", "https://user:pass@cdn.example"), "base URL credentials");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "/a.bundle", "https://cdn.example?x=1"), "base URL query");
        AssertThrows<InvalidDataException>(() => AddressableInternalIdResolver.Resolve(catalogToken + "/a.bundle", "https://cdn.example#fragment"), "base URL fragment");

        var pages = SpineAtlasPageNames.Parse("page-one.png\nsize: 1024, 1024\nformat: RGBA8888\nfilter: Linear,Linear\nrepeat: none\n\nregion-one\n  rotate: false\n  xy: 0, 0\n  size: 64, 64\n\nnested/page-two.png\nsize: 512, 512\nformat: RGBA8888\n");
        AssertEqual("page-one.png", pages[0], "first Spine atlas page");
        AssertEqual("nested/page-two.png", pages[1], "second Spine atlas page");
        AssertEqual("2", pages.Count.ToString(), "Spine atlas page count");
        AssertThrows<InvalidDataException>(() => SpineAtlasPageNames.Parse(""), "empty Spine atlas");
        AssertThrows<InvalidDataException>(() => SpineAtlasPageNames.Parse("region\n  rotate: false\n  xy: 0, 0\n"), "atlas without page metadata");
        var resourceKeys = SpineAtlasPageResourceResolver.ResolveResourceKeys(
            pages, name => name == "page-one.png" ? "bundle://atlas/one" : "bundle://atlas/two");
        AssertEqual("bundle://atlas/one", resourceKeys["page-one.png"], "first page resource mapping");
        AssertEqual("bundle://atlas/two", resourceKeys["nested/page-two.png"], "second page resource mapping");
        AssertThrows<InvalidDataException>(() => SpineAtlasPageResourceResolver.ResolveResourceKeys(
            pages, name => name == "page-one.png" ? "bundle://atlas/one" : null!), "missing atlas page resource");
        AssertThrows<InvalidDataException>(() => SpineAtlasPageResourceResolver.ResolveResourceKeys(
            new[] { "../escape.png" }, name => "bundle://escape"), "unsafe atlas page resource name");
        int textureLoads = 0;
        int textureReleases = 0;
        var pageCache = new SpineAtlasPageCache<object>(
            key => { textureLoads++; return new object(); },
            texture => textureReleases++);
        var pageLeaseA = pageCache.Acquire("atlas/page.png");
        var pageLeaseB = pageCache.Acquire("atlas/page.png");
        AssertTrue(ReferenceEquals(pageLeaseA.Value, pageLeaseB.Value), "atlas texture shared by key");
        AssertEqual("1", textureLoads.ToString(), "atlas texture loaded once");
        pageLeaseA.Dispose();
        AssertEqual("0", textureReleases.ToString(), "atlas texture retained while leased");
        pageLeaseB.Dispose();
        AssertEqual("1", textureReleases.ToString(), "atlas texture released after final lease");
        var deferredLease = pageCache.Acquire("atlas/deferred.png");
        pageCache.Dispose();
        AssertEqual("1", textureReleases.ToString(), "cache dispose defers active texture release");
        deferredLease.Dispose();
        AssertEqual("2", textureReleases.ToString(), "active texture released after cache disposal and final lease");
        AssertThrows<ObjectDisposedException>(() => pageCache.Acquire("atlas/after-dispose.png"), "acquire after cache disposal");
        using (var nullPageCache = new SpineAtlasPageCache<object>(key => null!, texture => { }))
            AssertThrows<InvalidOperationException>(() => nullPageCache.Acquire("atlas/missing.png"), "null texture load");

        int onceLoads = 0;
        int onceReleases = 0;
        var onceCache = new SpineAtlasPageCache<object>(
            key => { onceLoads++; return new object(); },
            texture => onceReleases++);
        var onceLease = onceCache.Acquire("atlas/once.png");
        onceLease.Dispose();
        onceLease.Dispose();
        AssertEqual("1", onceLoads.ToString(), "single lease loads once");
        AssertEqual("1", onceReleases.ToString(), "disposing one lease twice releases once");
        onceCache.Dispose();
        onceCache.Dispose();
        AssertEqual("1", onceReleases.ToString(), "disposing cache twice does not double-release");

        var independentCache = new SpineAtlasPageCache<object>(
            key => new object(),
            texture => { });
        var independentA = independentCache.Acquire("atlas/a.png");
        var independentB = independentCache.Acquire("atlas/b.png");
        AssertFalse(ReferenceEquals(independentA.Value, independentB.Value), "different page keys own different textures");
        independentA.Dispose();
        independentB.Dispose();
        independentCache.Dispose();

        int handleReleases = 0;
        var addressableAsset = new object();
        var addressableLease = new AddressableAssetLease<int, object>(
            42, addressableAsset, handle => handleReleases++);
        AssertTrue(ReferenceEquals(addressableAsset, addressableLease.Asset), "Addressables lease exposes loaded asset");
        AssertEqual("42", addressableLease.Handle.ToString(), "Addressables lease preserves operation handle");
        AssertFalse(addressableLease.IsReleased, "Addressables lease starts active");
        addressableLease.Dispose();
        addressableLease.Dispose();
        AssertTrue(addressableLease.IsReleased, "Addressables lease records release");
        AssertEqual("1", handleReleases.ToString(), "Addressables operation handle released exactly once");
        AssertThrows<ObjectDisposedException>(() => { var ignored = addressableLease.Handle; }, "released Addressables handle cannot be reused");
        AssertThrows<ObjectDisposedException>(() => { var ignored = addressableLease.Asset; }, "released Addressables asset cannot be reused");

        var concurrentLease = new AddressableAssetLease<int, object>(7, new object(), handle => handleReleases++);
        System.Threading.Tasks.Parallel.For(0, 32, _ => concurrentLease.Dispose());
        AssertEqual("2", handleReleases.ToString(), "concurrent Addressables lease disposal releases once");
        AssertThrows<ArgumentNullException>(() => new AddressableAssetLease<int, object>(1, null!, handle => { }), "null Addressables asset");
        AssertThrows<ArgumentNullException>(() => new AddressableAssetLease<int, object>(1, new object(), null!), "null Addressables release callback");

        var previewQueue = new BackgroundPreviewRequestCoalescer<string>();
        int previewStarts = 0;
        Action<string> previewA = value => { };
        Action<string> previewB = value => { };
        Action<string> previewC = value => { };
        if (previewQueue.TryEnqueue("scene-a|thumbnail", previewA)) previewStarts++;
        if (previewQueue.TryEnqueue("scene-a|thumbnail", previewB)) previewStarts++;
        if (previewQueue.TryEnqueue("scene-a|thumbnail", previewC)) previewStarts++;
        AssertEqual("1", previewStarts.ToString(), "same preview key starts one in-flight load");
        AssertEqual("1", previewQueue.PendingCount.ToString(), "coalesced preview uses one pending entry");
        AssertTrue(previewQueue.TryComplete("scene-a|thumbnail", out Action<string>[] previewCallbacks),
            "preview completion retrieves pending callbacks");
        AssertEqual("3", previewCallbacks.Length.ToString(), "preview completion returns every joined callback");
        AssertTrue(ReferenceEquals(previewA, previewCallbacks[0]) &&
            ReferenceEquals(previewB, previewCallbacks[1]) &&
            ReferenceEquals(previewC, previewCallbacks[2]),
            "preview callbacks retain registration order");
        AssertFalse(previewQueue.TryComplete("scene-a|thumbnail", out Action<string>[] missingPreviewCallbacks),
            "completed preview key is removed");
        AssertEqual("0", missingPreviewCallbacks.Length.ToString(), "missing preview completion returns empty callbacks");
        AssertTrue(previewQueue.TryEnqueue("scene-b|full", previewA), "different preview key starts independently");
        AssertTrue(previewQueue.Cancel("scene-b|full"), "pending preview can be abandoned");
        AssertFalse(previewQueue.Cancel("scene-b|full"), "cancelling absent preview is harmless");
        AssertThrows<ArgumentException>(() => previewQueue.TryEnqueue(" ", previewA),
            "preview key must be non-empty");
        AssertThrows<ArgumentNullException>(() => previewQueue.TryEnqueue("scene-c", null!),
            "preview callback must be non-null");

        // Verified preview-cache ledger: reference counts and LRU eligibility/order.
        var previewLedger = new BackgroundPreviewCacheLedger<object>();
        var previewTextureA = new object();
        var previewTextureB = new object();
        var previewTextureC = new object();
        previewLedger.Add("preview-a", previewTextureA, false, "scene/a", true, 0, 10);
        previewLedger.Add("preview-b", previewTextureB, true, "scene/b", false, 0, 5);
        previewLedger.Add("preview-c", previewTextureC, false, "scene/c", false, 1, 1);
        AssertEqual("3", previewLedger.Count.ToString(), "preview ledger registers cache records");
        AssertTrue(previewLedger.TryAcquire("preview-a", 20, out object? acquiredA),
            "preview ledger acquires cached texture");
        AssertTrue(ReferenceEquals(previewTextureA, acquiredA), "preview ledger returns same texture instance");
        AssertTrue(previewLedger.TryGet("preview-a", out var entryA), "preview ledger retrieves entry");
        AssertEqual("1", entryA!.Users.ToString(), "acquire increments preview user count");
        AssertEqual("20", entryA.LastUse.ToString(), "acquire updates preview last-use frame");
        AssertTrue(entryA.PreferThumbnail, "preview record retains thumbnail preference");
        AssertFalse(entryA.Addressable, "preview record retains owned-texture flag");
        AssertTrue(previewLedger.TryRelease(previewTextureA, 21, out var releasedA),
            "preview ledger releases by texture identity");
        AssertTrue(ReferenceEquals(entryA, releasedA), "release returns matching cache entry");
        AssertEqual("0", entryA.Users.ToString(), "release decrements preview user count");
        AssertEqual("21", entryA.LastUse.ToString(), "release updates preview last-use frame");
        AssertTrue(previewLedger.TryGet("preview-b", out var entryB) && entryB!.Addressable,
            "preview record retains Addressables ownership flag");
        AssertTrue(previewLedger.TryAcquire("preview-c", 22, out _), "active preview can be acquired");
        AssertEqual("2", previewLedger.GetUnusedLeastRecentlyUsed().Length.ToString(),
            "LRU candidate set excludes in-use preview");
        var unusedPreviews = previewLedger.GetUnusedLeastRecentlyUsed();
        AssertEqual("preview-b", unusedPreviews[0].Key, "unused preview candidates sort by oldest LastUse");
        AssertEqual("preview-a", unusedPreviews[1].Key, "newer unused preview follows older candidate");
        AssertFalse(previewLedger.TryRelease(new object(), 23, out _),
            "release of unregistered texture does not mutate cache");
        AssertThrows<InvalidOperationException>(() => previewLedger.Remove("preview-c", out _),
            "in-use preview cannot be removed");
        AssertTrue(previewLedger.TryRelease(previewTextureC, 24, out _), "second preview release succeeds");
        AssertTrue(previewLedger.Remove("preview-b", out var removedB),
            "unused preview can be removed");
        AssertTrue(removedB!.Addressable, "removed entry preserves ownership metadata for adapter cleanup");
        previewLedger.ClearUnused();
        AssertEqual("0", previewLedger.Count.ToString(), "clear unused removes remaining idle entries");
        AssertFalse(previewLedger.TryGet("preview-a", out _), "cleared preview is absent");
        AssertThrows<ArgumentException>(() => previewLedger.Add(" ", new object(), false, null, false, 0, 0),
            "preview ledger requires key");
        AssertThrows<ArgumentNullException>(() => previewLedger.Add("null-texture", null!, false, null, false, 0, 0),
            "preview ledger requires texture");
        AssertThrows<ArgumentOutOfRangeException>(() => previewLedger.Add("negative-users", new object(), false, null, false, -1, 0),
            "preview ledger rejects negative initial users");
        previewLedger.Add("duplicate", new object(), false, null, false, 0, 0);
        AssertThrows<InvalidOperationException>(() => previewLedger.Add("duplicate", new object(), false, null, false, 0, 0),
            "preview ledger rejects duplicate cache key");
        AssertThrows<ArgumentException>(() => previewLedger.TryGet("", out _),
            "preview ledger lookup requires key");

        Console.WriteLine("PASS: 13 SafeResourcePath checks; 11 BackgroundResourceDescriptor checks; 5 BackgroundOverrideResolver checks; 24 ScenarioBackgroundResolver checks; 5 BackgroundResourceIdHash checks; 5 BackgroundImageOverrideFactory checks; 5 BackgroundOverrideSetFactory checks; 13 Addressables internal-ID checks; 5 Spine atlas metadata checks; 4 page resource mapping checks; 12 texture cache lifecycle checks; 8 Addressables handle ownership checks; 11 preview request coalescer checks; 24 preview cache ledger checks");
        return 0;
    }

    private static void AssertTrue(bool value, string name)
    {
        if (!value) throw new Exception(name);
    }

    private static void AssertFalse(bool value, string name)
    {
        if (value) throw new Exception(name);
    }

    private static void AssertEqual(string? expected, string? actual, string name)
    {
        if (!string.Equals(expected, actual, StringComparison.Ordinal))
            throw new Exception(name + ": expected '" + expected + "', got '" + actual + "'");
    }

    private static void AssertThrows<T>(Action action, string name) where T : Exception
    {
        try
        {
            action();
        }
        catch (T)
        {
            return;
        }

        throw new Exception(name + ": expected " + typeof(T).Name);
    }
}
