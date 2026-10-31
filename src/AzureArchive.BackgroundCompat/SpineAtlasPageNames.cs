using System;
using System.Collections.Generic;
using System.IO;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Extracts Spine atlas page image names from atlas text without depending
    /// on Spine runtime or Unity assemblies. This is a metadata reader only.
    /// </summary>
    public static class SpineAtlasPageNames
    {
        private static readonly HashSet<string> PageMetadataKeys = new HashSet<string>(StringComparer.Ordinal)
        {
            "size", "format", "filter", "repeat", "pma"
        };

        public static IReadOnlyList<string> Parse(string atlasText)
        {
            if (atlasText == null)
                throw new ArgumentNullException(nameof(atlasText));
            if (atlasText.Length == 0)
                throw new InvalidDataException("Spine atlas is empty.");

            string[] lines = atlasText.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            var pages = new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < lines.Length; i++)
            {
                // Page and region names are unindented, while region properties
                // (xy, size, rotate, index, etc.) are indented. Do not treat
                // indented region properties as possible page names.
                if (lines[i].Length > 0 && char.IsWhiteSpace(lines[i][0]))
                    continue;

                string candidate = lines[i].Trim();
                if (candidate.Length == 0)
                    continue;
                if (i == 0 && candidate.Length > 0 && candidate[0] == '\uFEFF')
                    candidate = candidate.Substring(1).Trim();
                if (candidate.Length == 0)
                    continue;

                int next = i + 1;
                while (next < lines.Length && string.IsNullOrWhiteSpace(lines[next]))
                    next++;
                if (next >= lines.Length || !IsPageMetadata(lines[next]))
                    continue;

                ValidatePageName(candidate);
                if (!seen.Add(candidate))
                    throw new InvalidDataException("Duplicate Spine atlas page name: " + candidate);

                pages.Add(candidate);
                i = next;
                while (i < lines.Length && IsPageMetadata(lines[i]))
                    i++;
            }

            if (pages.Count == 0)
                throw new InvalidDataException("Spine atlas contains no recognizable texture pages.");

            return pages.AsReadOnly();
        }

        private static bool IsPageMetadata(string line)
        {
            int separator = line.IndexOf(':');
            if (separator <= 0)
                return false;

            string key = line.Substring(0, separator).Trim();
            return PageMetadataKeys.Contains(key);
        }

        private static void ValidatePageName(string pageName)
        {
            if (string.IsNullOrWhiteSpace(pageName))
                throw new InvalidDataException("Spine atlas page name is empty.");
            if (pageName[0] == '/' || pageName[0] == '\\' ||
                pageName.IndexOf(':') >= 0 || pageName.IndexOf('\0') >= 0)
                throw new InvalidDataException("Spine atlas page path must be relative: " + pageName);

            string normalized = pageName.Replace('\\', '/');
            string[] segments = normalized.Split('/');
            foreach (string segment in segments)
            {
                if (segment.Length == 0 || segment == "." || segment == ".." ||
                    segment.EndsWith(".", StringComparison.Ordinal) ||
                    segment.EndsWith(" ", StringComparison.Ordinal))
                    throw new InvalidDataException("Spine atlas page path contains an unsafe segment: " + pageName);

                foreach (char c in segment)
                {
                    if (char.IsControl(c))
                        throw new InvalidDataException("Spine atlas page path contains a control character.");
                }
            }
        }
    }
}
