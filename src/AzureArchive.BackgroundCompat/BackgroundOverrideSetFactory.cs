using System;
using System.Collections.Generic;
using System.IO;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Projects ResourceOverrideData's image-path HashSet and Spine override List
    /// into one source-specific sequence, matching Android 1.0 Overrides enumeration:
    /// image entries first, followed by Spine entries. Input collections are expected
    /// to be deserialized already; this class does not parse manifest.json.
    /// </summary>
    public static class BackgroundOverrideSetFactory
    {
        public static IReadOnlyList<BackgroundResourceDescriptor> Create(
            IEnumerable<string>? imagePaths,
            IEnumerable<SpineBackgroundOverrideRecord>? spineOverrides,
            string? root,
            BackgroundResourceOrigin origin)
        {
            if (origin != BackgroundResourceOrigin.GlobalCustom &&
                origin != BackgroundResourceOrigin.LocalCustom)
                throw new ArgumentOutOfRangeException(nameof(origin),
                    "Override sets must be global-custom or local-custom resources.");

            var result = new List<BackgroundResourceDescriptor>();
            var seenImagePaths = new HashSet<string>(StringComparer.Ordinal);

            if (imagePaths != null)
            {
                foreach (string path in imagePaths)
                {
                    if (path == null)
                        throw new InvalidDataException("Background image override path cannot be null.");

                    // ResourceOverrideData.BgOverrides is a HashSet<string>.
                    // Preserve exact-string uniqueness if a caller supplies a list.
                    if (!seenImagePaths.Add(path))
                        continue;

                    result.Add(BackgroundImageOverrideFactory.Create(path, root, origin));
                }
            }

            // Android validates Spine entries and rejects duplicate hashed IDs
            // before yielding the Spine portion of the iterator.
            IReadOnlyList<BackgroundResourceDescriptor> spineResources =
                SpineBackgroundOverrideFactory.CreateMany(spineOverrides, root, origin);

            for (int i = 0; i < spineResources.Count; i++)
                result.Add(spineResources[i]);

            return result.AsReadOnly();
        }
    }
}
