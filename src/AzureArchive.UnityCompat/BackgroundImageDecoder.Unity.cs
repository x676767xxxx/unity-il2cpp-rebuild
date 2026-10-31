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
        /// <summary>
        /// Reads a custom background image from the app's filesystem and decodes it.
        /// Invoke on Unity's main thread; Unity texture creation/destruction is not worker-thread safe.
        /// </summary>
        public static Texture2D DecodeFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidDataException("Cannot decode background image: path is empty.");

            byte[] bytes = File.ReadAllBytes(path);
            return Decode(bytes, Path.GetFileName(path));
        }

        public static Texture2D Decode(byte[] bytes, string name)
        {
            if (bytes == null || bytes.Length == 0)
                throw new InvalidDataException("Cannot decode background image: " + (name ?? string.Empty));

            // Matches the recovered Android/Windows IL constructor arguments:
            // 2x2 initial size, TextureFormat enum value 4 (RGBA32), no mip chain.
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                texture.name = name ?? string.Empty;
                texture.wrapMode = TextureWrapMode.Clamp;

                // The recovered game IL passes markNonReadable=true.
                bool loaded = ImageConversion.LoadImage(texture, bytes, true);
                if (!loaded)
                    throw new InvalidDataException("Cannot decode background image: " + (name ?? string.Empty));

                return texture;
            }
            catch
            {
                // Cover failures from texture setup as well as image decoding.
                // Unity calls must be made on the main thread.
                try { UnityEngine.Object.Destroy(texture); }
                catch { /* Preserve the original decode/setup exception. */ }
                throw;
            }
        }
    }
}
#endif
