#if UNITY_2021_3_OR_NEWER
using System;
using System.IO;
using UnityEngine;

namespace AzureArchive.UnityCompat
{
    /// <summary>
    /// Unity-side candidate for decoding custom static background images.
    /// Call only from Unity's main thread. This file has not yet been compiled
    /// against the game's Unity project or exercised in an iOS player.
    /// </summary>
    public static class BackgroundImageDecoder
    {
        public static Texture2D Decode(byte[] bytes, string name)
        {
            if (bytes == null || bytes.Length == 0)
                throw new InvalidDataException("Cannot decode background image: " + (name ?? string.Empty));

            // Matches the recovered Android/Windows IL constructor arguments:
            // 2x2 initial size, TextureFormat enum value 4 (RGBA32), no mip chain.
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.name = name ?? string.Empty;
            texture.wrapMode = TextureWrapMode.Clamp;

            bool loaded;
            try
            {
                // The recovered game IL passes markNonReadable=true.
                loaded = ImageConversion.LoadImage(texture, bytes, true);
            }
            catch
            {
                UnityEngine.Object.Destroy(texture);
                throw;
            }

            if (!loaded)
            {
                UnityEngine.Object.Destroy(texture);
                throw new InvalidDataException("Cannot decode background image: " + (name ?? string.Empty));
            }

            return texture;
        }
    }
}
#endif
