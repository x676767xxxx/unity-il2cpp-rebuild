using System;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Background kinds recovered from the Android 1.0 ScenarioBGType enum.
    /// Numeric values intentionally match the game's serialized enum values.
    /// </summary>
    public enum BackgroundResourceKind
    {
        None = 0,
        Image = 1,
        BlurRenderTexture = 2,
        Spine = 3,
        Hide = 4
    }

    /// <summary>
    /// Resource origins recovered from ScenarioResourceManager.ResourceType.
    /// </summary>
    public enum BackgroundResourceOrigin
    {
        Native = 0,
        GlobalCustom = 1,
        LocalCustom = 2
    }

    /// <summary>
    /// Immutable, platform-neutral description of an Android 1.0 background resource.
    /// This models identity and path semantics only; it does not load Unity assets.
    /// </summary>
    public sealed class BackgroundResourceDescriptor
    {
        public BackgroundResourceDescriptor(
            uint id,
            string? name,
            BackgroundResourceKind kind,
            BackgroundResourceOrigin origin,
            string? path,
            string? root,
            bool hasSpineOverrideData)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Origin = origin;
            Path = path;
            Root = root;
            HasSpineOverrideData = hasSpineOverrideData;
        }

        public uint Id { get; }
        public string? Name { get; }
        public BackgroundResourceKind Kind { get; }
        public BackgroundResourceOrigin Origin { get; }
        public string? Path { get; }
        public string? Root { get; }
        public bool HasSpineOverrideData { get; }

        /// <summary>
        /// Mirrors Android 1.0 FullPath: Native resources return Path unchanged;
        /// custom resources resolve Root + Path through the guarded relative-path resolver.
        /// </summary>
        public string? FullPath
        {
            get
            {
                if (Origin == BackgroundResourceOrigin.Native)
                    return Path;

                return SafeResourcePath.ResolveUnderRoot(Root ?? string.Empty, Path ?? string.Empty);
            }
        }

        /// <summary>
        /// Mirrors the recovered IsCustomSpine predicate (Spine metadata is non-null).
        /// It is deliberately independent of Kind so malformed descriptors remain observable.
        /// </summary>
        public bool IsCustomSpine => HasSpineOverrideData;
    }
}
