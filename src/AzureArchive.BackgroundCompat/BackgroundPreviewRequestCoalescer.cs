using System;
using System.Collections.Generic;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Tracks one in-flight preview request per cache key and collects callbacks
    /// waiting for that request. The Unity-facing owner remains responsible for
    /// starting/stopping coroutines, validating Texture/RenderTexture objects,
    /// invoking returned callbacks on the main thread, and owning preview assets.
    /// </summary>
    public sealed class BackgroundPreviewRequestCoalescer<TTexture>
    {
        private readonly Dictionary<string, List<Action<TTexture>>> pending =
            new Dictionary<string, List<Action<TTexture>>>(StringComparer.Ordinal);

        public int PendingCount => pending.Count;

        /// <summary>
        /// Adds a callback to an in-flight key. Returns true only for the first
        /// callback, which means the caller should start the actual load operation.
        /// Later callbacks join the existing request and must not start another load.
        /// </summary>
        public bool TryEnqueue(string key, Action<TTexture> callback)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("A preview cache key is required.", nameof(key));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (pending.TryGetValue(key, out List<Action<TTexture>>? callbacks))
            {
                callbacks.Add(callback);
                return false;
            }

            pending.Add(key, new List<Action<TTexture>> { callback });
            return true;
        }

        /// <summary>
        /// Completes and removes a pending request, returning its callbacks in
        /// registration order. The caller decides how to invoke them and must do
        /// so on Unity's main thread when they touch Unity objects.
        /// </summary>
        public bool TryComplete(string key, out Action<TTexture>[] callbacks)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("A preview cache key is required.", nameof(key));

            if (!pending.TryGetValue(key, out List<Action<TTexture>>? waiting))
            {
                callbacks = Array.Empty<Action<TTexture>>();
                return false;
            }

            pending.Remove(key);
            callbacks = waiting.ToArray();
            return true;
        }

        /// <summary>
        /// Drops callbacks for a request that the owning loader has cancelled or
        /// abandoned. This does not stop an external coroutine or release assets.
        /// </summary>
        public bool Cancel(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("A preview cache key is required.", nameof(key));
            return pending.Remove(key);
        }
    }
}
