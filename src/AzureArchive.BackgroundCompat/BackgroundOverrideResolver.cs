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
