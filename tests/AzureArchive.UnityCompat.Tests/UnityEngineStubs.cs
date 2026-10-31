using System;
namespace UnityEngine
{
    public class Object
    {
        public bool Destroyed { get; private set; }
        public static void Destroy(Object value) { value.Destroyed = true; }
    }
    public enum TextureFormat { RGBA32 = 4 }
    public enum TextureWrapMode { Clamp = 1 }
    public sealed class Texture2D : Object
    {
        public Texture2D(int width, int height, TextureFormat format, bool mipChain)
        { Width=width; Height=height; Format=format; MipChain=mipChain; }
        public int Width { get; }
        public int Height { get; }
        public TextureFormat Format { get; }
        public bool MipChain { get; }
        public string name { get; set; } = string.Empty;
        public TextureWrapMode wrapMode { get; set; }
    }
    public static class ImageConversion
    {
        public static bool ReturnValue { get; set; } = true;
        public static bool ThrowOnLoad { get; set; }
        public static Texture2D? LastTexture { get; private set; }
        public static void Reset() { ReturnValue=true; ThrowOnLoad=false; LastTexture=null; }
        public static bool LoadImage(Texture2D texture, byte[] bytes, bool markNonReadable)
        {
            LastTexture=texture;
            if (ThrowOnLoad) throw new InvalidOperationException("synthetic decoder failure");
            return ReturnValue;
        }
    }
}
