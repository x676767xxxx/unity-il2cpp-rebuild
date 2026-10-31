using System;
using System.IO;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Builds the Image descriptor shape emitted for a path in ResourceOverrideData.BgOverrides.
    /// Android normalizes '\\' to '/', takes the filename without extension, hashes that
    /// name for the resource ID, and retains the normalized relative path plus basePath.
    /// </summary>
    public static class BackgroundImageOverrideFactory
    {
        public static BackgroundResourceDescriptor Create(
            string relativePath,
            string? root,
            BackgroundResourceOrigin origin)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new InvalidDataException("A background image override path is required.");

            if (origin != BackgroundResourceOrigin.GlobalCustom &&
                origin != BackgroundResourceOrigin.LocalCustom)
                throw new ArgumentOutOfRangeException(nameof(origin),
                    "Image overrides must be global-custom or local-custom resources.");

            string normalizedPath = relativePath.Replace('\\', '/');
            string name = Path.GetFileNameWithoutExtension(normalizedPath);
            uint id = BackgroundResourceIdHash.Calculate(name);

            return new BackgroundResourceDescriptor(
                id,
                name,
                BackgroundResourceKind.Image,
                origin,
                normalizedPath,
                root,
                false);
        }
    }
}
