using System;
using System.Collections.Generic;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Reference-counted atlas-page cache. The loader/releaser are supplied by
    /// the Unity integration and must be invoked on Unity's main thread.
    /// </summary>
    public sealed class SpineAtlasPageCache<TTexture> : IDisposable where TTexture : class
    {
        internal sealed class Entry
        {
            public readonly TTexture Texture;
            public int References;

            public Entry(TTexture texture)
            {
                Texture = texture;
                References = 1;
            }
        }

        private readonly object gate = new object();
        private readonly Dictionary<string, Entry> entries = new Dictionary<string, Entry>(StringComparer.Ordinal);
        private readonly Func<string, TTexture> load;
        private readonly Action<TTexture> release;
        private bool disposed;

        public SpineAtlasPageCache(Func<string, TTexture> load, Action<TTexture> release)
        {
            this.load = load ?? throw new ArgumentNullException(nameof(load));
            this.release = release ?? throw new ArgumentNullException(nameof(release));
        }

        public Lease Acquire(string resourceKey)
        {
            if (string.IsNullOrWhiteSpace(resourceKey))
                throw new ArgumentException("A Spine atlas resource key is required.", nameof(resourceKey));

            lock (gate)
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(SpineAtlasPageCache<TTexture>));

                if (entries.TryGetValue(resourceKey, out Entry existing))
                {
                    if (existing.References == int.MaxValue)
                        throw new OverflowException("Spine atlas page lease count cannot be incremented.");
                    existing.References++;
                    return new Lease(this, resourceKey, existing);
                }

                TTexture texture = load(resourceKey);
                if (texture == null)
                    throw new InvalidOperationException("The Spine atlas page loader returned null for: " + resourceKey);

                var created = new Entry(texture);
                entries.Add(resourceKey, created);
                return new Lease(this, resourceKey, created);
            }
        }

        private void ReleaseLease(string resourceKey, Entry entry)
        {
            lock (gate)
            {
                if (!entries.TryGetValue(resourceKey, out Entry current) || !ReferenceEquals(current, entry))
                    return;

                if (current.References > 0)
                    current.References--;

                if (current.References == 0 && (disposed || entries.ContainsKey(resourceKey)))
                {
                    entries.Remove(resourceKey);
                    release(current.Texture);
                }
            }
        }

        public void Dispose()
        {
            lock (gate)
            {
                if (disposed) return;
                disposed = true;

                var keysToRelease = new List<string>();
                foreach (KeyValuePair<string, Entry> pair in entries)
                {
                    if (pair.Value.References == 0)
                        keysToRelease.Add(pair.Key);
                }

                foreach (string key in keysToRelease)
                {
                    Entry entry = entries[key];
                    entries.Remove(key);
                    release(entry.Texture);
                }
            }
        }

        public sealed class Lease : IDisposable
        {
            private SpineAtlasPageCache<TTexture> owner;
            private readonly string resourceKey;
            private readonly Entry entry;

            internal Lease(SpineAtlasPageCache<TTexture> owner, string resourceKey, Entry entry)
            {
                this.owner = owner;
                this.resourceKey = resourceKey;
                this.entry = entry;
            }

            public TTexture Value
            {
                get
                {
                    if (owner == null)
                        throw new ObjectDisposedException(nameof(Lease));
                    return entry.Texture;
                }
            }

            public void Dispose()
            {
                SpineAtlasPageCache<TTexture> current = owner;
                if (current == null) return;
                owner = null!;
                current.ReleaseLease(resourceKey, entry);
            }
        }
    }
}
