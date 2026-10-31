namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Minimal projection of FlatData.ScenarioBGNameExcel fields used by
    /// Android 1.0 ScenarioResourceManager.ResolveBackground.
    /// The game's key is the Excel field named "Name" (a uint ID), not a
    /// separately named "Id" field.
    /// </summary>
    public sealed class ScenarioBackgroundRecord
    {
        public ScenarioBackgroundRecord(uint nameId, string? fileName, BackgroundResourceKind kind)
        {
            NameId = nameId;
            FileName = fileName;
            Kind = kind;
        }

        public uint NameId { get; }
        public string? FileName { get; }
        public BackgroundResourceKind Kind { get; }
    }
}
