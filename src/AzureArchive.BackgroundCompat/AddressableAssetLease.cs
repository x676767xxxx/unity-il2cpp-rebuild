using System;
using System.Threading;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Owns one successful Addressables-style operation handle and its asset.
    /// The caller supplies the platform-specific release callback (for Unity,
    /// Addressables.Release(handle)). Dispose is idempotent and releases exactly once.
    /// This type deliberately has no Unity dependency so its ownership contract
    /// can be tested outside the Unity editor.
    /// </summary>
    public sealed class AddressableAssetLease<THandle, TAsset> : IDisposable where TAsset : class
    {
        private readonly THandle handle;
        private readonly Action<THandle> releaseHandle;
        private int released;

        public AddressableAssetLease(THandle handle, TAsset asset, Action<THandle> releaseHandle)
        {
            Asset = asset ?? throw new ArgumentNullException(nameof(asset));
            this.handle = handle;
            this.releaseHandle = releaseHandle ?? throw new ArgumentNullException(nameof(releaseHandle));
        }

        public TAsset Asset { get; }

        public THandle Handle
        {
            get
            {
                ThrowIfReleased();
                return handle;
            }
        }

        public bool IsReleased => Volatile.Read(ref released) != 0;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref released, 1) != 0)
                return;

            releaseHandle(handle);
        }

        private void ThrowIfReleased()
        {
            if (IsReleased)
                throw new ObjectDisposedException(nameof(AddressableAssetLease<THandle, TAsset>));
        }
    }
}
