namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Portable projection of Android 1.0 ResourceOverrideData.BgSpineOverrideData.
    /// Optional animation/skin/preview settings are retained for the Spine runtime adapter.
    /// </summary>
    public sealed class SpineBackgroundOverrideRecord
    {
        public SpineBackgroundOverrideRecord(
            string? identifier,
            string? spinePath,
            string? animation = null,
            bool loop = true,
            string? skin = null,
            string? previewPath = null,
            float previewTime = 0f,
            float scale = 1f,
            float offsetX = 0f,
            float offsetY = 0f)
        {
            Identifier = identifier;
            SpinePath = spinePath;
            Animation = animation;
            Loop = loop;
            Skin = skin;
            PreviewPath = previewPath;
            PreviewTime = previewTime;
            Scale = scale;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public string? Identifier { get; }
        public string? SpinePath { get; }
        public string? Animation { get; }
        public bool Loop { get; }
        public string? Skin { get; }
        public string? PreviewPath { get; }
        public float PreviewTime { get; }
        public float Scale { get; }
        public float OffsetX { get; }
        public float OffsetY { get; }
    }
}
