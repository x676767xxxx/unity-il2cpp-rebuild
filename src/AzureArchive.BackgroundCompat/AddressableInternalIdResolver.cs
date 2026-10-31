using System;
using System.IO;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Resolves the exact Addressables internal-ID token observed in the
    /// AzureArchive 0.7.9.2/1.0 catalogs. The actual CDN/base URL remains
    /// supplied by the game's runtime configuration; it is never guessed here.
    /// </summary>
    public static class AddressableInternalIdResolver
    {
        public const string AddrLoadUrlToken = "{ScenarioResourceManager.AddrLoadUrl}";

        public static string Resolve(string internalId, string addrLoadUrl)
        {
            if (string.IsNullOrWhiteSpace(internalId))
                throw new InvalidDataException("Addressables internal ID is empty.");
            if (string.IsNullOrWhiteSpace(addrLoadUrl))
                throw new InvalidDataException("ScenarioResourceManager.AddrLoadUrl is empty.");
            if (!internalId.StartsWith(AddrLoadUrlToken, StringComparison.Ordinal))
                throw new InvalidDataException("Addressables internal ID does not start with the verified AddrLoadUrl token.");

            if (!Uri.TryCreate(addrLoadUrl, UriKind.Absolute, out Uri? baseUri) ||
                (baseUri.Scheme != Uri.UriSchemeHttp && baseUri.Scheme != Uri.UriSchemeHttps) ||
                string.IsNullOrEmpty(baseUri.Host) ||
                !string.IsNullOrEmpty(baseUri.UserInfo) ||
                !string.IsNullOrEmpty(baseUri.Query) ||
                !string.IsNullOrEmpty(baseUri.Fragment))
                throw new InvalidDataException("AddrLoadUrl must be an absolute HTTP(S) base URL without credentials, query or fragment.");

            string suffix = internalId.Substring(AddrLoadUrlToken.Length);
            ValidateBundleSuffix(suffix);

            string resolved = addrLoadUrl.TrimEnd('/') + suffix;
            if (!Uri.TryCreate(resolved, UriKind.Absolute, out Uri? resolvedUri) ||
                !string.Equals(resolvedUri.Scheme, baseUri.Scheme, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(resolvedUri.Host, baseUri.Host, StringComparison.OrdinalIgnoreCase) ||
                resolvedUri.Port != baseUri.Port)
                throw new InvalidDataException("Resolved Addressables URL is invalid or escaped its configured origin.");

            return resolved;
        }

        private static void ValidateBundleSuffix(string suffix)
        {
            if (string.IsNullOrEmpty(suffix) || suffix[0] != '/' ||
                suffix.StartsWith("//", StringComparison.Ordinal) ||
                suffix.IndexOf('\\') >= 0 ||
                suffix.IndexOf('?') >= 0 ||
                suffix.IndexOf('#') >= 0 ||
                suffix.IndexOf('\0') >= 0)
                throw new InvalidDataException("Addressables bundle suffix must be a relative URL path.");

            string[] segments = suffix.Substring(1).Split('/');
            foreach (string segment in segments)
            {
                if (segment.Length == 0)
                    throw new InvalidDataException("Addressables bundle path contains an empty segment.");

                string decoded;
                try { decoded = Uri.UnescapeDataString(segment); }
                catch (UriFormatException) { throw new InvalidDataException("Addressables bundle path has invalid escaping."); }

                if (decoded == "." || decoded == ".." ||
                    decoded.IndexOf('/') >= 0 || decoded.IndexOf('\\') >= 0 ||
                    decoded.IndexOf(':') >= 0 ||
                    decoded.IndexOf('\0') >= 0)
                    throw new InvalidDataException("Addressables bundle path contains an unsafe segment.");
            }
        }
    }
}
