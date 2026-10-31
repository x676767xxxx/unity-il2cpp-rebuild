using System;
using System.Collections.Generic;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Pure decision planner for the Android 1.0 preview-cache trim loop.
    /// It selects cache keys only; the Unity adapter owns actual destruction/release.
    /// </summary>
    public static class BackgroundPreviewTrimPlanner
    {
        public const long MemoryThresholdBytes = 32L * 1024L * 1024L;
        public const int EntryCountThreshold = 25;

        public sealed class Candidate
        {
            public Candidate(string key, long estimatedBytes)
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("A preview cache key is required.", nameof(key));
                if (estimatedBytes < 0)
                    throw new ArgumentOutOfRangeException(nameof(estimatedBytes));
                Key = key;
                EstimatedBytes = estimatedBytes;
            }

            public string Key { get; }
            public long EstimatedBytes { get; }
        }

        /// <summary>
        /// Candidates must already be ordered by ascending LastUse (oldest first).
        /// Normal trim continues while total bytes are strictly greater than 32 MiB
        /// OR the current cache entry count is at least 25. Forced trim selects all
        /// supplied unused candidates regardless of either threshold.
        /// </summary>
        public static string[] Plan(IReadOnlyList<Candidate> candidates,
            long totalBytes, int cacheCount, bool allUnused)
        {
            if (candidates == null) throw new ArgumentNullException(nameof(candidates));
            if (totalBytes < 0) throw new ArgumentOutOfRangeException(nameof(totalBytes));
            if (cacheCount < 0) throw new ArgumentOutOfRangeException(nameof(cacheCount));

            var selected = new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                Candidate candidate = candidates[i] ??
                    throw new ArgumentException("Trim candidates cannot contain null.", nameof(candidates));
                if (!seen.Add(candidate.Key))
                    throw new ArgumentException("Trim candidate keys must be unique.", nameof(candidates));

                if (!allUnused && totalBytes <= MemoryThresholdBytes &&
                    cacheCount < EntryCountThreshold)
                    break;

                selected.Add(candidate.Key);
                totalBytes = Math.Max(0L, totalBytes - candidate.EstimatedBytes);
                if (cacheCount > 0) cacheCount--;
            }

            return selected.ToArray();
        }
    }
}
