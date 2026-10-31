using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Engine-independent ownership ledger for the game's preview cache.
    /// Mirrors the verified record fields and eviction eligibility/order only.
    /// Unity object destruction and Addressables release remain the runtime adapter's job.
    /// </summary>
    public sealed class BackgroundPreviewCacheLedger<TTexture>
        where TTexture : class
    {
        public sealed class Entry
        {
            internal Entry(string key, TTexture texture, bool addressable,
                string? resource, bool preferThumbnail, int users, int lastUse)
            {
                Key = key;
                Texture = texture;
                Addressable = addressable;
                Resource = resource;
                PreferThumbnail = preferThumbnail;
                Users = users;
                LastUse = lastUse;
            }

            public string Key { get; }
            public TTexture Texture { get; }
            public bool Addressable { get; }
            public int Users { get; internal set; }
            public int LastUse { get; internal set; }
            public string? Resource { get; }
            public bool PreferThumbnail { get; }
        }

        private readonly Dictionary<string, Entry> entries =
            new Dictionary<string, Entry>(StringComparer.Ordinal);

        public int Count => entries.Count;

        public bool TryGet(string key, out Entry? entry)
        {
            ValidateKey(key);
            return entries.TryGetValue(key, out entry);
        }

        public void Add(string key, TTexture texture, bool addressable,
            string? resource, bool preferThumbnail, int initialUsers, int lastUse)
        {
            ValidateKey(key);
            if (texture == null) throw new ArgumentNullException(nameof(texture));
            if (initialUsers < 0) throw new ArgumentOutOfRangeException(nameof(initialUsers));
            if (entries.ContainsKey(key))
                throw new InvalidOperationException("A preview cache key is already registered.");

            entries.Add(key, new Entry(key, texture, addressable, resource,
                preferThumbnail, initialUsers, lastUse));
        }

        public bool TryAcquire(string key, int frame, out TTexture? texture)
        {
            ValidateKey(key);
            if (!entries.TryGetValue(key, out Entry? entry))
            {
                texture = null;
                return false;
            }

            if (entry.Users == int.MaxValue)
                throw new OverflowException("Preview user count cannot be incremented.");

            entry.Users++;
            entry.LastUse = frame;
            texture = entry.Texture;
            return true;
        }

        public bool TryRelease(TTexture texture, int frame, out Entry? released)
        {
            if (texture == null) throw new ArgumentNullException(nameof(texture));

            released = entries.Values.FirstOrDefault(
                entry => ReferenceEquals(entry.Texture, texture));
            if (released == null) return false;
            if (released.Users <= 0)
                throw new InvalidOperationException("Preview cache user count is already zero.");

            released.Users--;
            released.LastUse = frame;
            return true;
        }

        /// <summary>
        /// Returns unused entries ordered by the verified LastUse sort key.
        /// Does not decide when trimming starts or destroy/release any Unity object.
        /// </summary>
        public Entry[] GetUnusedLeastRecentlyUsed()
        {
            return entries.Values
                .Where(entry => entry.Users == 0)
                .OrderBy(entry => entry.LastUse)
                .ToArray();
        }

        public bool Remove(string key, out Entry? removed)
        {
            ValidateKey(key);
            if (!entries.TryGetValue(key, out removed))
                return false;
            if (removed.Users != 0)
                throw new InvalidOperationException("An in-use preview cannot be removed.");
            return entries.Remove(key);
        }

        public void ClearUnused()
        {
            string[] keys = entries.Values
                .Where(entry => entry.Users == 0)
                .Select(entry => entry.Key)
                .ToArray();
            foreach (string key in keys) entries.Remove(key);
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("A preview cache key is required.", nameof(key));
        }
    }
}
