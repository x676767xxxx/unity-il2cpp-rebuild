#if UNITY_2021_3_OR_NEWER
using System;
using AzureArchive.BackgroundCompat;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace AzureArchive.UnityCompat
{
    /// <summary>
    /// Installs the verified catalog-token substitution into Addressables before
    /// catalog-backed asset loads begin. The base URL is supplied by the game's
    /// runtime configuration; this class never chooses a CDN.
    /// </summary>
    public sealed class AddressablesInternalIdTransformHook : IDisposable
    {
        private readonly Func<IResourceLocation, string>? previous;
        private readonly Func<IResourceLocation, string> installed;
        private bool disposed;

        private AddressablesInternalIdTransformHook(Func<string> getAddrLoadUrl)
        {
            if (getAddrLoadUrl == null) throw new ArgumentNullException(nameof(getAddrLoadUrl));

            previous = Addressables.InternalIdTransformFunc;
            installed = location =>
            {
                if (location == null) throw new ArgumentNullException(nameof(location));
                string original = previous != null ? previous(location) : location.InternalId;
                if (string.IsNullOrEmpty(original) ||
                    !original.StartsWith(AddressableInternalIdResolver.AddrLoadUrlToken, StringComparison.Ordinal))
                    return original;

                return AddressableInternalIdResolver.Resolve(original, getAddrLoadUrl());
            };

            Addressables.InternalIdTransformFunc = installed;
        }

        /// <summary>
        /// Installs the hook. Call before Addressables initializes or starts
        /// loading catalog-backed content. Dispose restores the previous hook
        /// only when this instance still owns the global transform slot.
        /// </summary>
        public static AddressablesInternalIdTransformHook Install(Func<string> getAddrLoadUrl)
        {
            return new AddressablesInternalIdTransformHook(getAddrLoadUrl);
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Do not overwrite a transform installed by another subsystem later.
            if (ReferenceEquals(Addressables.InternalIdTransformFunc, installed))
                Addressables.InternalIdTransformFunc = previous;
        }
    }
}
#endif
