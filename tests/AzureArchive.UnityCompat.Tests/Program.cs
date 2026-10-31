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
            AssertThrows<InvalidDataException>(() => BackgroundImageDecoder.DecodeFile(resourceRoot, "../outside.png"), "reject parent traversal");
            AssertThrows<InvalidDataException>(() => BackgroundImageDecoder.DecodeFile(resourceRoot, rootedImage), "reject absolute resource path");
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

        AssertThrows<ArgumentNullException>(() => AddressablesRuntimeAdapter.Load<object>(null!, lease => { }), "null Addressables key");
        AssertThrows<ArgumentNullException>(() => AddressablesRuntimeAdapter.Load<object>("bundle://key", null!), "null Addressables success callback");

        Console.WriteLine("PASS: 13 Unity decoder checks; 4 Spine bridge argument checks; 11 Addressables async adapter checks");
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
