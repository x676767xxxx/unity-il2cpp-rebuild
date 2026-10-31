#if UNITY_2021_3_OR_NEWER
using System;
using AzureArchive.BackgroundCompat;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AzureArchive.UnityCompat
{
    /// <summary>
    /// Thin callback adapter for the Addressables API already present in the
    /// legacy iOS runtime. Successful handles transfer to an idempotent lease;
    /// failed operations are released before the error callback is invoked.
    /// Invoke callbacks on Unity's normal Addressables completion thread.
    /// </summary>
    public static class AddressablesRuntimeAdapter
    {
        public static void Load<TAsset>(
            object key,
            Action<AddressableAssetLease<AsyncOperationHandle<TAsset>, TAsset>> onLoaded,
            Action<Exception> onFailed = null)
            where TAsset : class
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (onLoaded == null) throw new ArgumentNullException(nameof(onLoaded));

            AsyncOperationHandle<TAsset> operation;
            try
            {
                operation = Addressables.LoadAssetAsync<TAsset>(key);
            }
            catch (Exception exception)
            {
                if (onFailed != null) onFailed(exception);
                else throw;
                return;
            }

            operation.Completed += completed =>
            {
                if (completed.Status != AsyncOperationStatus.Succeeded || completed.Result == null)
                {
                    Exception failure = completed.OperationException ??
                        new InvalidOperationException("Addressables failed to load key: " + key);
                    Addressables.Release(completed);
                    if (onFailed != null) onFailed(failure);
                    return;
                }

                var lease = new AddressableAssetLease<AsyncOperationHandle<TAsset>, TAsset>(
                    completed,
                    completed.Result,
                    handle => Addressables.Release(handle));

                try
                {
                    onLoaded(lease);
                }
                catch
                {
                    lease.Dispose();
                    throw;
                }
            };
        }
    }
}
#endif
