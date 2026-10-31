using System;
using System.IO;
using AzureArchive.UnityCompat;
using UnityEngine;

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
        Console.WriteLine("PASS: 10 Unity decoder contract checks; 4 Spine iOS bridge argument checks");
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
