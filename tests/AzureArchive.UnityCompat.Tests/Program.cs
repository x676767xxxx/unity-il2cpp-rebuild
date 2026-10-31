using System;
using System.IO;
using AzureArchive.UnityCompat;
using AzureArchive.BackgroundCompat;
using UnityEngine;
using UnityEngine.AddressableAssets;

internal static class Program
{
    private static int Main()
    {
        // Spine owned-object cleanup follows the Android Destroy/DestroyImmediate split.
        Application.isPlaying = true;
        var playModeObject = new UnityEngine.Object();
        SpineOwnedObjectCleanup.DestroyOwned(playModeObject);
        Assert(playModeObject.Destroyed && !playModeObject.DestroyImmediateCalled,
            "Spine cleanup uses deferred Destroy while playing");

        Application.isPlaying = false;
        var editModeObject = new UnityEngine.Object();
        SpineOwnedObjectCleanup.DestroyOwned(editModeObject);
        Assert(editModeObject.Destroyed && editModeObject.DestroyImmediateCalled,
            "Spine cleanup uses DestroyImmediate outside play mode");

        SpineOwnedObjectCleanup.DestroyOwned(null!);
        var rollbackPlayObject = new UnityEngine.Object();
        var rollbackEditObject = new UnityEngine.Object();
        SpineOwnedObjectCleanup.Rollback(new UnityEngine.Object[] { rollbackPlayObject, null!, rollbackEditObject });
        Assert(rollbackPlayObject.Destroyed && rollbackPlayObject.DestroyImmediateCalled,
            "Spine rollback destroys first edit-mode object");
        Assert(rollbackEditObject.Destroyed && rollbackEditObject.DestroyImmediateCalled,
            "Spine rollback continues through null entries");
        AssertThrows<ArgumentNullException>(() => SpineOwnedObjectCleanup.Rollback(null!),
            "Spine rollback rejects null collection");

        ImageConversion.Reset();
        Texture2D decoded = BackgroundImageDecoder.Decode(new byte[] { 1, 2, 3 }, "scene");
        Assert(decoded.Width == 2 && decoded.Height == 2, "initial dimensions");
        Assert(decoded.Format == TextureFormat.RGBA32 && !decoded.MipChain, "texture format/mips");
        Assert(decoded.wrapMode == TextureWrapMode.Clamp && decoded.name == "scene", "texture metadata");
        Assert(!decoded.Destroyed, "successful texture remains alive");

        string tempPath = Path.Combine(Path.GetTempPath(), "azurearchive-background-decoder-test.png");
        try
        {
            File.WriteAllBytes(tempPath, new byte[] { 9, 8, 7, 6 });
            ImageConversion.Reset();
            Texture2D fileDecoded = BackgroundImageDecoder.DecodeFile(tempPath);
            Assert(fileDecoded.name == "azurearchive-background-decoder-test.png", "file decode uses filename");
        }
        finally
        {
            if (File.Exists(tempPath)) File.Delete(tempPath);
        }

        string resourceRoot = Path.Combine(Path.GetTempPath(), "azurearchive-resource-root-" + Guid.NewGuid().ToString("N"));
        string nestedDirectory = Path.Combine(resourceRoot, "backgrounds");
        Directory.CreateDirectory(nestedDirectory);
        string rootedImage = Path.Combine(nestedDirectory, "scene.png");
        try
        {
            File.WriteAllBytes(rootedImage, new byte[] { 4, 3, 2, 1 });
            ImageConversion.Reset();
            Texture2D rootedDecoded = BackgroundImageDecoder.DecodeFile(resourceRoot, "backgrounds/scene.png");
            Assert(rootedDecoded.name == "scene.png", "root-confined file decode");

            ImageConversion.Reset();
            var customDescriptor = new BackgroundResourceDescriptor(
                91u, "scene", BackgroundResourceKind.Image, BackgroundResourceOrigin.LocalCustom,
                "backgrounds/scene.png", resourceRoot, false);
            Texture2D descriptorDecoded = BackgroundImageDecoder.DecodeCustomResource(customDescriptor);
            Assert(descriptorDecoded.name == "scene.png", "descriptor FullPath feeds custom image decoder");
            var nativeDescriptor = new BackgroundResourceDescriptor(
                92u, "native", BackgroundResourceKind.Image, BackgroundResourceOrigin.Native,
                "Addressables/Backgrounds/native.png", null, false);
            AssertThrows<InvalidOperationException>(
                () => BackgroundImageDecoder.DecodeCustomResource(nativeDescriptor),
                "native resource is not sent through custom file decoder");
            var spineDescriptor = new BackgroundResourceDescriptor(
                93u, "spine", BackgroundResourceKind.Spine, BackgroundResourceOrigin.LocalCustom,
                "spine/scene.skel", resourceRoot, true);
            AssertThrows<InvalidOperationException>(
                () => BackgroundImageDecoder.DecodeCustomResource(spineDescriptor),
                "Spine resource is not sent through static image decoder");
            AssertThrows<InvalidDataException>(() => BackgroundImageDecoder.DecodeFile(resourceRoot, "../outside.png"), "reject parent traversal");
            AssertThrows<InvalidDataException>(() => BackgroundImageDecoder.DecodeFile(resourceRoot, rootedImage), "reject absolute resource path");

            var imageCache = new BackgroundImageRuntimeCache(path => "corrected:" + path);
            Texture? cachedCustomA = null;
            Texture? cachedCustomB = null;
            imageCache.Load(customDescriptor, texture => cachedCustomA = texture);
            imageCache.Load(customDescriptor, texture => cachedCustomB = texture);
            Assert(cachedCustomA != null && ReferenceEquals(cachedCustomA, cachedCustomB),
                "custom background cache reuses decoded texture");
            Assert(imageCache.CachedCustomCount == 1, "custom background cache stores one FullPath entry");
            imageCache.Dispose();
            Assert(cachedCustomA!.Destroyed, "custom background cache destroys owned texture on dispose");
        }
        finally
        {
            if (Directory.Exists(resourceRoot)) Directory.Delete(resourceRoot, recursive: true);
        }

        AssertThrows<InvalidDataException>(() => BackgroundImageDecoder.DecodeFile(""), "empty file path");
        AssertThrows<FileNotFoundException>(() => BackgroundImageDecoder.DecodeFile(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".png")), "missing file");

        ImageConversion.Reset();
        ImageConversion.ReturnValue = false;
        AssertThrows<InvalidDataException>(() => BackgroundImageDecoder.Decode(new byte[] { 1 }, "bad"), "false decoder result");
        Assert(ImageConversion.LastTexture != null && ImageConversion.LastTexture.Destroyed, "false result destroys texture");

        ImageConversion.Reset();
        ImageConversion.ThrowOnLoad = true;
        AssertThrows<InvalidOperationException>(() => BackgroundImageDecoder.Decode(new byte[] { 1 }, "throws"), "decoder exception");
        Assert(ImageConversion.LastTexture != null && ImageConversion.LastTexture.Destroyed, "exception destroys texture");

        AssertThrows<InvalidDataException>(() => BackgroundImageDecoder.Decode(Array.Empty<byte>(), "empty"), "empty input");

        // These validation paths execute without invoking the iOS-only native symbols.
        AssertThrows<ArgumentNullException>(() => SpineConverterNative.JsonToSpine42Binary(null!), "null Spine JSON");
        AssertThrows<ArgumentException>(() => SpineConverterNative.JsonToSpine42Binary(Array.Empty<byte>()), "empty Spine JSON");
        AssertThrows<ArgumentNullException>(() => SpineConverterNative.ConvertToSpine42Binary(null!), "null Spine skeleton");
        AssertThrows<ArgumentException>(() => SpineConverterNative.ConvertToSpine42Binary(Array.Empty<byte>()), "empty Spine skeleton");


        Addressables.Reset();
        var nativeImageCache = new BackgroundImageRuntimeCache(path => "manifest:" + path);
        var nativeImageDescriptor = new BackgroundResourceDescriptor(
            94u, "native-image", BackgroundResourceKind.Image, BackgroundResourceOrigin.Native,
            "Addressables/Backgrounds/native.png", null, false);
        Texture? nativeLoadedA = null;
        Texture? nativeLoadedB = null;
        nativeImageCache.Load(nativeImageDescriptor, texture => nativeLoadedA = texture);
        nativeImageCache.Load(nativeImageDescriptor, texture => nativeLoadedB = texture);
        Assert(nativeLoadedA == null && nativeLoadedB == null, "native background load remains asynchronous");
        Assert(Addressables.LastKey?.ToString() == "manifest:Addressables/Backgrounds/native.png",
            "native background key passes through game CorrectPath adapter");
        var nativeImageOperation = Addressables.Last<Texture>();
        var nativeImage = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        nativeImageOperation.Complete(nativeImage);
        Assert(ReferenceEquals(nativeImage, nativeLoadedA) && ReferenceEquals(nativeImage, nativeLoadedB),
            "concurrent native background requests share one Addressables result");
        Assert(nativeImageCache.CachedNativeCount == 1, "native background cache retains one Addressables lease");
        Texture? nativeLoadedCached = null;
        nativeImageCache.Load(nativeImageDescriptor, texture => nativeLoadedCached = texture);
        Assert(ReferenceEquals(nativeImage, nativeLoadedCached), "native background cache serves retained asset");
        Assert(Addressables.ReleaseCount == 0, "native background lease remains retained until cache disposal");
        nativeImageCache.Dispose();
        Assert(Addressables.ReleaseCount == 1 && nativeImageOperation.Released,
            "native background cache releases retained Addressables handle exactly once");
        Assert(!nativeImage.Destroyed, "Addressables-owned texture is not manually destroyed");

        Addressables.Reset();
        var failedNativeCache = new BackgroundImageRuntimeCache(path => path);
        Exception? nativeLoadFailure = null;
        failedNativeCache.Load(nativeImageDescriptor, texture => throw new Exception("unexpected native load success"),
            exception => nativeLoadFailure = exception);
        var failedNativeImageOperation = Addressables.Last<Texture>();
        var expectedNativeFailure = new InvalidOperationException("native background missing");
        failedNativeImageOperation.Fail(expectedNativeFailure);
        Assert(ReferenceEquals(expectedNativeFailure, nativeLoadFailure),
            "native background Addressables failure reaches caller");
        Assert(Addressables.ReleaseCount == 1 && failedNativeImageOperation.Released,
            "failed native background handle released");
        failedNativeCache.Dispose();

        Addressables.Reset();
        var gamePathFixture = new ScenarioResourceManagerPathFixture();
        var gameBoundCache = BackgroundImageRuntimeCache.CreateFromGameRuntime(gamePathFixture);
        gameBoundCache.Load(nativeImageDescriptor, texture => { });
        Assert(Addressables.LastKey?.ToString() == "game-corrected:Addressables/Backgrounds/native.png",
            "runtime cache binds the live ScenarioResourceManager CorrectPath method");
        gameBoundCache.Dispose();
        var inFlightAfterDispose = Addressables.Last<Texture>();
        inFlightAfterDispose.Complete(new Texture2D(2, 2, TextureFormat.RGBA32, false));
        Assert(Addressables.ReleaseCount == 1 && inFlightAfterDispose.Released,
            "in-flight native texture handle is released when cache was disposed before completion");
        AssertThrows<InvalidOperationException>(() =>
            BackgroundImageRuntimeCache.CreateFromGameRuntime(new InvalidCorrectPathFixture()),
            "runtime cache rejects a missing CorrectPath method");

        Addressables.Reset();
        var singletonBoundCache = BackgroundImageRuntimeCache.CreateFromGameRuntimeSingleton(
            typeof(ScenarioResourceManagerPathFixture));
        singletonBoundCache.Load(nativeImageDescriptor, texture => { });
        Assert(Addressables.LastKey?.ToString() == "game-corrected:Addressables/Backgrounds/native.png",
            "runtime cache resolves inherited game Singleton.Instance before binding CorrectPath");
        singletonBoundCache.Dispose();
        var singletonInFlight = Addressables.Last<Texture>();
        singletonInFlight.Complete(new Texture2D(2, 2, TextureFormat.RGBA32, false));
        Assert(Addressables.ReleaseCount == 1 && singletonInFlight.Released,
            "singleton-bound cache releases operation completing after disposal");
        AssertThrows<InvalidOperationException>(() =>
            BackgroundImageRuntimeCache.CreateFromGameRuntimeSingleton(typeof(InvalidCorrectPathFixture)),
            "runtime cache rejects a missing game Singleton.Instance property");

        Addressables.Reset();
        int previousTransformCalls = 0;
        Func<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation, string> previousTransform =
            location => { previousTransformCalls++; return location.InternalId; };
        Addressables.InternalIdTransformFunc = previousTransform;
        var tokenLocation = new UnityEngine.ResourceManagement.ResourceLocations.TestResourceLocation(
            "{ScenarioResourceManager.AddrLoadUrl}/backgrounds/scene.bundle");
        var untouchedLocation = new UnityEngine.ResourceManagement.ResourceLocations.TestResourceLocation(
            "https://static.example.org/unchanged.bundle");
        var hook = AddressablesInternalIdTransformHook.Install(() => "https://cdn.example.org/game/");
        Func<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation, string> activeTransform =
            Addressables.InternalIdTransformFunc!;
        Assert(activeTransform(tokenLocation) == "https://cdn.example.org/game/backgrounds/scene.bundle",
            "Addressables hook resolves verified catalog token");
        Assert(activeTransform(untouchedLocation) == untouchedLocation.InternalId,
            "Addressables hook preserves unrelated IDs");
        Assert(previousTransformCalls == 2, "Addressables hook chains previous transform");
        hook.Dispose();
        Assert(ReferenceEquals(Addressables.InternalIdTransformFunc, previousTransform),
            "Addressables hook restores previous transform");

        Addressables.Reset();
        ScenarioResourceManager.AddrLoadUrl = "https://runtime.example.org/mobile/";
        var runtimeHook = AddressablesInternalIdTransformHook.InstallFromRuntimeType(
            typeof(ScenarioResourceManager));
        var runtimeLocation = new UnityEngine.ResourceManagement.ResourceLocations.TestResourceLocation(
            "{ScenarioResourceManager.AddrLoadUrl}/backgrounds/scene.bundle");
        Assert(Addressables.InternalIdTransformFunc!(runtimeLocation) ==
            "https://runtime.example.org/mobile/backgrounds/scene.bundle",
            "runtime hook reads the verified ScenarioResourceManager.AddrLoadUrl field");
        ScenarioResourceManager.AddrLoadUrl = "https://runtime.example.org/updated/";
        Assert(Addressables.InternalIdTransformFunc!(runtimeLocation) ==
            "https://runtime.example.org/updated/backgrounds/scene.bundle",
            "runtime hook reads AddrLoadUrl lazily after installation");
        runtimeHook.Dispose();

        AssertThrows<InvalidOperationException>(() =>
            AddressablesInternalIdTransformHook.InstallFromRuntimeType(typeof(InvalidAddrLoadUrlFixture)),
            "runtime hook rejects a missing AddrLoadUrl field");

        Addressables.Reset();
        var hookWithLaterOwner = AddressablesInternalIdTransformHook.Install(() => "https://cdn.example.org/game");
        Func<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation, string> laterTransform =
            location => "later:" + location.InternalId;
        Addressables.InternalIdTransformFunc = laterTransform;
        hookWithLaterOwner.Dispose();
        Assert(ReferenceEquals(Addressables.InternalIdTransformFunc, laterTransform),
            "Addressables hook does not overwrite a later owner");

        Addressables.Reset();
        AddressableAssetLease<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>, object>? loadedLease = null;
        Exception? loadError = null;
        AddressablesRuntimeAdapter.Load<object>("bundle://scene", lease => loadedLease = lease, error => loadError = error);
        var successOperation = Addressables.Last<object>();
        Assert(loadedLease == null && loadError == null && Addressables.ReleaseCount == 0, "Addressables waits for async completion");
        var loadedAsset = new object();
        successOperation.Complete(loadedAsset);
        Assert(loadedLease != null && ReferenceEquals(loadedAsset, loadedLease.Asset), "Addressables returns loaded asset lease");
        Assert(loadError == null && Addressables.ReleaseCount == 0, "successful Addressables handle remains owned");
        loadedLease!.Dispose();
        loadedLease.Dispose();
        Assert(Addressables.ReleaseCount == 1 && successOperation.Released, "successful Addressables handle released once");

        Addressables.Reset();
        Exception? failedError = null;
        AddressablesRuntimeAdapter.Load<object>("bundle://missing", lease => throw new Exception("unexpected success"), error => failedError = error);
        var failedOperation = Addressables.Last<object>();
        var expectedFailure = new InvalidOperationException("synthetic missing key");
        failedOperation.Fail(expectedFailure);
        Assert(ReferenceEquals(expectedFailure, failedError), "Addressables failure reaches error callback");
        Assert(Addressables.ReleaseCount == 1 && failedOperation.Released, "failed Addressables handle released");

        Addressables.Reset();
        Exception? startError = null;
        Addressables.ThrowOnLoad = true;
        AddressablesRuntimeAdapter.Load<object>("bundle://throw", lease => throw new Exception("unexpected success"), error => startError = error);
        Assert(startError is InvalidOperationException && Addressables.ReleaseCount == 0, "synchronous Addressables start failure reported");

        Addressables.Reset();
        AddressablesRuntimeAdapter.Load<object>("bundle://callback", lease => throw new InvalidOperationException("consumer callback"));
        var callbackOperation = Addressables.Last<object>();
        AssertThrows<InvalidOperationException>(() => callbackOperation.Complete(new object()), "consumer callback exception");
        Assert(Addressables.ReleaseCount == 1 && callbackOperation.Released, "consumer callback exception releases successful handle");

        Addressables.Reset();
        ScenarioResourceManager.AddrLoadUrl = "https://runtime.example.org/bootstrap/";
        AddressablesTransformBootstrap.InstallNow();
        var bootstrapTransform = Addressables.InternalIdTransformFunc;
        Assert(bootstrapTransform != null, "startup bootstrap installs Addressables transform");
        Assert(bootstrapTransform!(runtimeLocation) ==
            "https://runtime.example.org/bootstrap/backgrounds/scene.bundle",
            "startup bootstrap resolves the game runtime URL");
        AddressablesTransformBootstrap.InstallNow();
        Assert(ReferenceEquals(bootstrapTransform, Addressables.InternalIdTransformFunc),
            "startup bootstrap installation is idempotent");

        AssertThrows<ArgumentNullException>(() => AddressablesRuntimeAdapter.Load<object>(null!, lease => { }), "null Addressables key");
        AssertThrows<ArgumentNullException>(() => AddressablesRuntimeAdapter.Load<object>("bundle://key", null!), "null Addressables success callback");

        Console.WriteLine("PASS: Unity decoder and path checks; 4 Spine bridge argument checks; 6 Spine cleanup checks; Addressables token hook, startup bootstrap and async ownership checks");
        return 0;
    }

    private static void Assert(bool condition, string name)
    {
        if (!condition) throw new Exception(name);
    }

    private static void AssertThrows<T>(Action action, string name) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new Exception(name + ": expected " + typeof(T).Name);
    }
}


internal static class ScenarioResourceManager
{
    public static string AddrLoadUrl = string.Empty;
}

internal static class InvalidAddrLoadUrlFixture
{
    public static int AddrLoadUrl = 0;
}

internal sealed class ScenarioResourceManagerPathFixture
{
    public static ScenarioResourceManagerPathFixture Instance { get; } = new ScenarioResourceManagerPathFixture();
    public string CorrectPath(string path) => "game-corrected:" + path;
}

internal sealed class InvalidCorrectPathFixture
{
    public int CorrectPath(int path) => path;
}
