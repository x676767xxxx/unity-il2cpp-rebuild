#if UNITY_2021_3_OR_NEWER
using System;
using System.Collections.Generic;
using System.IO;
using AzureArchive.BackgroundCompat;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AzureArchive.UnityCompat
{
    /// <summary>
    /// Mirrors Android 1.0's static Image background path:
    /// custom resources are read from FullPath and cached by path;
    /// native resources are loaded through Addressables and retained by lease.
    /// Call every method on Unity's main thread.
    /// </summary>
    public sealed class BackgroundImageRuntimeCache : IDisposable
    {
        private sealed class PendingNativeLoad
        {
            public readonly List<Action<Texture2D>> Success = new List<Action<Texture2D>>();
            public readonly List<Action<Exception>> Failure = new List<Action<Exception>>();
        }

        private readonly Func<string, string> correctNativePath;
        private readonly Dictionary<string, Texture2D> customTextures =
            new Dictionary<string, Texture2D>(StringComparer.Ordinal);
        private readonly Dictionary<string, AddressableAssetLease<AsyncOperationHandle<Texture2D>, Texture2D>> nativeLeases =
            new Dictionary<string, AddressableAssetLease<AsyncOperationHandle<Texture2D>, Texture2D>>(StringComparer.Ordinal);
        private readonly Dictionary<string, PendingNativeLoad> pendingNative =
            new Dictionary<string, PendingNativeLoad>(StringComparer.Ordinal);
        private bool disposed;

        public BackgroundImageRuntimeCache(Func<string, string> correctNativePath)
        {
            this.correctNativePath = correctNativePath ??
                throw new ArgumentNullException(nameof(correctNativePath));
        }

        public int CachedCustomCount => customTextures.Count;
        public int CachedNativeCount => nativeLeases.Count;

        public void Load(
            BackgroundResourceDescriptor resource,
            Action<Texture2D> onLoaded,
            Action<Exception>? onFailed = null)
        {
            ThrowIfDisposed();
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (onLoaded == null) throw new ArgumentNullException(nameof(onLoaded));
            if (resource.Kind != BackgroundResourceKind.Image)
                throw new InvalidOperationException("Only Image resources can use the static background texture cache.");

            if (resource.Origin == BackgroundResourceOrigin.Native)
                LoadNative(resource, onLoaded, onFailed);
            else
                LoadCustom(resource, onLoaded, onFailed);
        }

        private void LoadCustom(
            BackgroundResourceDescriptor resource,
            Action<Texture2D> onLoaded,
            Action<Exception>? onFailed)
        {
            try
            {
                string fullPath = resource.FullPath ??
                    throw new InvalidDataException("Custom background resource has no resolved path.");

                if (customTextures.TryGetValue(fullPath, out Texture2D cached) && cached != null)
                {
                    onLoaded(cached);
                    return;
                }

                Texture2D decoded = BackgroundImageDecoder.DecodeCustomResource(resource);
                customTextures[fullPath] = decoded;
                onLoaded(decoded);
            }
            catch (Exception exception)
            {
                if (onFailed != null) onFailed(exception);
                else throw;
            }
        }

        private void LoadNative(
            BackgroundResourceDescriptor resource,
            Action<Texture2D> onLoaded,
            Action<Exception>? onFailed)
        {
            string originalPath = resource.Path ??
                throw new InvalidDataException("Native background resource has no Addressables path.");
            string key = correctNativePath(originalPath);
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidDataException("The game's native path resolver returned an empty Addressables key.");

            if (nativeLeases.TryGetValue(key, out var cachedLease))
            {
                onLoaded(cachedLease.Asset);
                return;
            }

            if (pendingNative.TryGetValue(key, out PendingNativeLoad? existingPending))
            {
                existingPending.Success.Add(onLoaded);
                if (onFailed != null) existingPending.Failure.Add(onFailed);
                return;
            }

            var pending = new PendingNativeLoad();
            pending.Success.Add(onLoaded);
            if (onFailed != null) pending.Failure.Add(onFailed);
            pendingNative.Add(key, pending);

            AddressablesRuntimeAdapter.Load<Texture2D>(
                key,
                lease => CompleteNativeSuccess(key, pending, lease),
                exception => CompleteNativeFailure(key, pending, exception));
        }

        private void CompleteNativeSuccess(
            string key,
            PendingNativeLoad pending,
            AddressableAssetLease<AsyncOperationHandle<Texture2D>, Texture2D> lease)
        {
            if (!pendingNative.TryGetValue(key, out PendingNativeLoad? current) ||
                !ReferenceEquals(current, pending))
            {
                lease.Dispose();
                return;
            }

            pendingNative.Remove(key);
            if (disposed)
            {
                lease.Dispose();
                return;
            }

            nativeLeases.Add(key, lease);
            Exception? callbackFailure = null;
            foreach (Action<Texture2D> callback in pending.Success)
            {
                try { callback(lease.Asset); }
                catch (Exception exception) { if (callbackFailure == null) callbackFailure = exception; }
            }

            if (callbackFailure != null)
            {
                nativeLeases.Remove(key);
                lease.Dispose();
                throw callbackFailure;
            }
        }

        private void CompleteNativeFailure(
            string key,
            PendingNativeLoad pending,
            Exception exception)
        {
            if (!pendingNative.TryGetValue(key, out PendingNativeLoad? current) ||
                !ReferenceEquals(current, pending))
                return;

            pendingNative.Remove(key);
            if (disposed) return;

            if (pending.Failure.Count == 0)
                throw exception;

            foreach (Action<Exception> callback in pending.Failure)
                callback(exception);
        }

        private void ThrowIfDisposed()
        {
            if (disposed) throw new ObjectDisposedException(nameof(BackgroundImageRuntimeCache));
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            foreach (Texture2D texture in customTextures.Values)
            {
                if (texture != null)
                    UnityEngine.Object.Destroy(texture);
            }
            customTextures.Clear();

            foreach (var lease in nativeLeases.Values)
                lease.Dispose();
            nativeLeases.Clear();

            // In-flight Addressables operations cannot be cancelled here. Their
            // completion callback observes disposed=true and releases its lease.
            pendingNative.Clear();
        }
    }
}
#endif
