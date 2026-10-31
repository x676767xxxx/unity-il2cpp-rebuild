using System.Collections.Generic;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Projects ResourceOverrideData's image-path HashSet and Spine override List
    /// into one source-specific sequence, matching Android Overrides enumeration:
    /// image entries first, followed by Spine entries.
    /// </summary>
    public static class BackgroundOverrideSetFactory
    {
        public static IReadOnlyList<BackgroundResourceDescriptor> Create(
            IEnumerable<string>? imagePaths,
            IEnumerable<SpineBackgroundOverrideRecord>? spineOverrides,
            string? root,
            BackgroundResourceOrigin origin)
        {
            var result = new List<BackgroundResourceDescriptor>();

            if (imagePaths != null)
            {
                foreach (string path in imagePaths)
                    result.Add(BackgroundImageOverrideFactory.Create(path, root, origin));
            }

            if (spineOverrides != null)
                result.AddRange(SpineBackgroundOverrideFactory.CreateMany(spineOverrides, root, origin));

            return result.AsReadOnly();
        }
    }
}
