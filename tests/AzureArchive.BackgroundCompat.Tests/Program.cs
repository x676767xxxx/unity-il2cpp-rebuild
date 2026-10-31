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
        AssertThrows<ArgumentNullException>(() => BackgroundOverrideResolver.TryResolve(local, global, null!, out selected), "null predicate");

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

        var concurrentLease = new AddressableAssetLease<int, object>(7, new object(), handle => handleReleases++);
        System.Threading.Tasks.Parallel.For(0, 32, _ => concurrentLease.Dispose());
        AssertEqual("2", handleReleases.ToString(), "concurrent Addressables lease disposal releases once");
        AssertThrows<ArgumentNullException>(() => new AddressableAssetLease<int, object>(1, null!, handle => { }), "null Addressables asset");
        AssertThrows<ArgumentNullException>(() => new AddressableAssetLease<int, object>(1, new object(), null!), "null Addressables release callback");

        Console.WriteLine("PASS: 13 SafeResourcePath checks; 5 BackgroundOverrideResolver checks; 5 Spine atlas metadata checks; 4 page resource mapping checks; 12 texture cache lifecycle checks; 8 Addressables handle ownership checks");
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

    private static void AssertEqual(string expected, string actual, string name)
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
