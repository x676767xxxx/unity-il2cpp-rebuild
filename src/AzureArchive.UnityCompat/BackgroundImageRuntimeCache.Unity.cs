#if UNITY_2021_3_OR_NEWER
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
            public readonly List<Action<Texture>> Success = new List<Action<Texture>>();
            public readonly List<Action<Exception>> Failure = new List<Action<Exception>>();
        }

        private readonly Func<string, string> correctNativePath;
        private readonly Dictionary<string, Texture2D> customTextures =
            new Dictionary<string, Texture2D>(StringComparer.Ordinal);
        private readonly Dictionary<string, AddressableAssetLease<AsyncOperationHandle<Texture>, Texture>> nativeLeases =
            new Dictionary<string, AddressableAssetLease<AsyncOperationHandle<Texture2D>, Texture2D>>(StringComparer.Ordinal);
        private readonly Dictionary<string, PendingNativeLoad> pendingNative =
            new Dictionary<string, PendingNativeLoad>(StringComparer.Ordinal);
        private bool disposed;

        public BackgroundImageRuntimeCache(Func<string, string> correctNativePath)
        {
            this.correctNativePath = correctNativePath ??
                throw new ArgumentNullException(nameof(correctNativePath));
        }

        /// <summary>
        /// Creates the cache using the live game's instance CorrectPath(string) method.
        /// This avoids copying the manifest path-correction rules into the port.
        /// </summary>
        public static BackgroundImageRuntimeCache CreateFromGameRuntime(object scenarioResourceManager)
        {
            if (scenarioResourceManager == null)
                throw new ArgumentNullException(nameof(scenarioResourceManager));

            MethodInfo? method = scenarioResourceManager.GetType().GetMethod(
                "CorrectPath",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(string) },
                null);

            if (method == null || method.ReturnType != typeof(string))
                throw new InvalidOperationException(
                    "ScenarioResourceManager must expose an instance string CorrectPath(string) method.");

            var correctPath = Delegate.CreateDelegate(
                typeof(Func<string, string>), scenarioResourceManager, method) as Func<string, string>;

            if (correctPath == null)
                throw new InvalidOperationException("Could not bind ScenarioResourceManager.CorrectPath(string).");

            return new BackgroundImageRuntimeCache(correctPath);
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

        /// <summary>
        /// Resolves the game's inherited Singleton&lt;ScenarioResourceManager&gt;.Instance
        /// property, then binds its instance CorrectPath(string) method.
        /// Call after the game singleton has been initialized.
        /// </summary>
        public static BackgroundImageRuntimeCache CreateFromGameRuntimeSingleton(Type scenarioResourceManagerType)
        {
            if (scenarioResourceManagerType == null)
                throw new ArgumentNullException(nameof(scenarioResourceManagerType));

            PropertyInfo? instanceProperty = scenarioResourceManagerType.GetProperty(
                "Instance",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            if (instanceProperty == null || !instanceProperty.CanRead ||
                !scenarioResourceManagerType.IsAssignableFrom(instanceProperty.PropertyType))
                throw new InvalidOperationException(
                    "ScenarioResourceManager must expose a static Instance property returning its own type.");

            object? instance = instanceProperty.GetValue(null);
            if (instance == null)
                throw new InvalidOperationException("ScenarioResourceManager.Instance is not initialized.");

            return CreateFromGameRuntime(instance);
        }

        private void LoadCustom(
            BackgroundResourceDescriptor resource,
            Action<Texture2D> onLoaded,
            Action<Exception>? onFailed)
        {
            Texture2D texture;
            try
            {
                string fullPath = resource.FullPath ??
                    throw new InvalidDataException("Custom background resource has no resolved path.");

                if (customTextures.TryGetValue(fullPath, out Texture2D? cached) && cached != null)
                {
                    texture = cached;
                }
                else
                {
                    texture = BackgroundImageDecoder.DecodeCustomResource(resource);
                    customTextures[fullPath] = texture;
                }
            }
            catch (Exception exception)
            {
                if (onFailed != null)
                {
                    onFailed(exception);
                    return;
                }
                throw;
            }

            // Keep consumer callback failures distinct from file/decoder failures.
            onLoaded(texture);
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

            AddressablesRuntimeAdapter.Load<Texture>(
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
