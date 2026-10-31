using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Pure decision planner for the Android 1.0 retained Spine skeleton cache.
    /// It selects keys only; the Unity adapter owns asset disposal and Unity-object cleanup.
    /// </summary>
    public static class BackgroundSkeletonTrimPlanner
    {
        public const long MemoryThresholdBytes = 768L * 1024L * 1024L;

        public sealed class Candidate
        {
            public Candidate(string key, long estimatedBytes, bool hasConsumers, int lastAccess)
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("A skeleton cache key is required.", nameof(key));
                if (estimatedBytes < 0)
                    throw new ArgumentOutOfRangeException(nameof(estimatedBytes));

                Key = key;
                EstimatedBytes = estimatedBytes;
                HasConsumers = hasConsumers;
                LastAccess = lastAccess;
            }

            public string Key { get; }
            public long EstimatedBytes { get; }
            public bool HasConsumers { get; }
            public int LastAccess { get; }
        }

        /// <summary>
        /// Selects least-recently-accessed assets with no consumers until the
        /// estimated cache total is at or below 768 MiB. Assets with consumers
        /// are never selected. Input order breaks equal-access ties.
        /// </summary>
        public static string[] Plan(IReadOnlyList<Candidate> candidates, long totalBytes)
        {
            if (candidates == null) throw new ArgumentNullException(nameof(candidates));
            if (totalBytes < 0) throw new ArgumentOutOfRangeException(nameof(totalBytes));

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                Candidate candidate = candidates[i] ??
                    throw new ArgumentException("Trim candidates cannot contain null.", nameof(candidates));
                if (!seen.Add(candidate.Key))
                    throw new ArgumentException("Trim candidate keys must be unique.", nameof(candidates));
            }

            if (totalBytes <= MemoryThresholdBytes) return Array.Empty<string>();

            var selected = new List<string>();
            foreach (Candidate candidate in candidates
                .Select((value, index) => new { value, index })
                .OrderBy(item => item.value.LastAccess)
                .ThenBy(item => item.index)
                .Select(item => item.value))
            {
                if (totalBytes <= MemoryThresholdBytes) break;
                if (candidate.HasConsumers) continue;

                selected.Add(candidate.Key);
                totalBytes = Math.Max(0L, totalBytes - candidate.EstimatedBytes);
            }

            return selected.ToArray();
        }
    }
}
