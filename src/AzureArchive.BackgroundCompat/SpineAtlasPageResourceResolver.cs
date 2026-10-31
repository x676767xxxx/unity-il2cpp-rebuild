using System;
using System.Collections.Generic;
using System.IO;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Maps atlas page filenames to caller-owned game resource keys.
    /// The callback keeps Addressables/bundle lookup policy outside this library.
    /// </summary>
    public static class SpineAtlasPageResourceResolver
    {
        public static IReadOnlyDictionary<string, string> ResolveResourceKeys(
            IReadOnlyList<string> pageNames,
            Func<string, string> resolveResourceKey)
        {
            if (pageNames == null) throw new ArgumentNullException(nameof(pageNames));
            if (resolveResourceKey == null) throw new ArgumentNullException(nameof(resolveResourceKey));

            var resolved = new Dictionary<string, string>(StringComparer.Ordinal);
            for (int i = 0; i < pageNames.Count; i++)
            {
                string pageName = pageNames[i];
                if (string.IsNullOrWhiteSpace(pageName))
                    throw new InvalidDataException("Spine atlas page name is empty.");

                // Reuse the same path-traversal checks used for other game resources.
                SafeResourcePath.ResolveUnderRoot(".", pageName);

                if (resolved.ContainsKey(pageName))
                    throw new InvalidDataException("Duplicate Spine atlas page name: " + pageName);

                string resourceKey = resolveResourceKey(pageName);
                if (string.IsNullOrWhiteSpace(resourceKey))
                    throw new InvalidDataException("No game resource was found for Spine atlas page: " + pageName);

                resolved.Add(pageName, resourceKey);
            }

            return resolved;
        }
    }
}
