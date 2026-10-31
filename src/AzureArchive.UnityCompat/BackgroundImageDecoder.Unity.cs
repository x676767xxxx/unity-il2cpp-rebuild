#if UNITY_2021_3_OR_NEWER
using System;
using System.IO;
using AzureArchive.BackgroundCompat;
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
        /// <summary>
        /// Decodes a custom Image descriptor using its recovered FullPath semantics.
        /// Native Addressables resources and Spine resources must use their own runtime paths.
        /// </summary>
        public static Texture2D DecodeCustomResource(BackgroundResourceDescriptor resource)
        {
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            if (resource.Kind != BackgroundResourceKind.Image)
                throw new InvalidOperationException("Only Image background resources can use the static image decoder.");

            if (resource.Origin == BackgroundResourceOrigin.Native)
                throw new InvalidOperationException("Native background resources must be loaded through Addressables.");

            string fullPath = resource.FullPath ??
                throw new InvalidDataException("Custom background resource has no resolved path.");

            return DecodeFile(fullPath);
        }

        public static Texture2D DecodeFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidDataException("Cannot decode background image: path is empty.");

            byte[] bytes = File.ReadAllBytes(path);
            return Decode(bytes, Path.GetFileName(path));
        }

        /// <summary>
        /// Resolves a game-relative resource path beneath the trusted override root
        /// before reading it. Use this overload for paths originating in manifests,
        /// atlas metadata, or user-provided override data.
        /// </summary>
        public static Texture2D DecodeFile(string resourceRoot, string relativePath)
        {
            string safePath = SafeResourcePath.ResolveUnderRoot(resourceRoot, relativePath);
            byte[] bytes = File.ReadAllBytes(safePath);
            return Decode(bytes, Path.GetFileName(safePath));
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
