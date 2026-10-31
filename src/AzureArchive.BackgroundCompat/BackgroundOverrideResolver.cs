using System;
using System.Collections.Generic;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Selects the first matching local override, falling back to the first
    /// matching global override. This mirrors the recovered Android/Windows
    /// resolution order; callers supply the verified resource-ID predicate.
    /// </summary>
    public static class BackgroundOverrideResolver
    {
        /// <summary>
        /// Resolves an Android 1.0 background ID from already source-filtered
        /// local and global override sequences. The Android IL calls Overrides
        /// with ResourceType 2 for local and 1 for global, then applies the
        /// same captured-ID predicate and FirstOrDefault to each sequence.
        /// </summary>
        public static bool TryResolve(
            uint requestedId,
            IEnumerable<BackgroundResourceDescriptor>? localOverrides,
            IEnumerable<BackgroundResourceDescriptor>? globalOverrides,
            out BackgroundResourceDescriptor result)
        {
            return TryResolve(
                localOverrides,
                globalOverrides,
                candidate => candidate != null && candidate.Id == requestedId,
                out result);
        }

        public static bool TryResolve<T>(
            IEnumerable<T>? localOverrides,
            IEnumerable<T>? globalOverrides,
            Func<T, bool> matches,
            out T result)
        {
            if (matches == null)
                throw new ArgumentNullException(nameof(matches));

            if (TryFirst(localOverrides, matches, out result))
                return true;

            return TryFirst(globalOverrides, matches, out result);
        }

        private static bool TryFirst<T>(
            IEnumerable<T>? candidates,
            Func<T, bool> matches,
            out T result)
        {
            if (candidates != null)
            {
                foreach (T candidate in candidates)
                {
                    if (matches(candidate))
                    {
                        result = candidate;
                        return true;
                    }
                }
            }

            result = default!;
            return false;
        }
    }
}
