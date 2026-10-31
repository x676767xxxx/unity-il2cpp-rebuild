using System;
using System.IO;
using System.Linq;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Resolves a relative background resource path beneath a trusted resource root.
    /// This is a standalone safety primitive; it is not yet wired into the Unity game.
    /// </summary>
    public static class SafeResourcePath
    {
        public static string ResolveUnderRoot(string root, string relative)
        {
            if (string.IsNullOrWhiteSpace(root) || string.IsNullOrWhiteSpace(relative))
                throw new InvalidDataException("A background resource root and relative path are required.");

            string normalized = relative.Replace('\\', '/');

            if (Path.IsPathRooted(normalized) ||
                normalized.StartsWith("//", StringComparison.Ordinal) ||
                IsDriveQualified(normalized))
            {
                throw new InvalidDataException("Background paths must be relative to the resource directory.");
            }

            string[] segments = normalized.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Any(segment => segment == ".."))
                throw new InvalidDataException("Background path escapes its resource directory.");

            string fullRoot = Path.GetFullPath(root);
            string platformRelative = normalized.Replace('/', Path.DirectorySeparatorChar);
            string fullPath = Path.GetFullPath(Path.Combine(fullRoot, platformRelative));

            string rootPrefix = fullRoot.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal)
                ? fullRoot
                : fullRoot + Path.DirectorySeparatorChar;
            StringComparison comparison = Path.DirectorySeparatorChar == '\\'
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            if (!fullPath.StartsWith(rootPrefix, comparison) &&
                !string.Equals(fullPath, fullRoot, comparison))
            {
                throw new InvalidDataException("Background path escapes its resource directory.");
            }

            return fullPath;
        }

        private static bool IsDriveQualified(string value)
        {
            return value.Length >= 2 &&
                   ((value[0] >= 'A' && value[0] <= 'Z') || (value[0] >= 'a' && value[0] <= 'z')) &&
                   value[1] == ':';
        }
    }
}
