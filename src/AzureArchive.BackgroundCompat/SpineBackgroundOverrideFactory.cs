using System;
using System.Collections.Generic;
using System.IO;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Creates Spine background descriptors using Android 1.0's recovered field mapping.
    /// The actual Spine skeleton/atlas loading remains the Unity runtime's responsibility.
    /// </summary>
    public static class SpineBackgroundOverrideFactory
    {
        public static BackgroundResourceDescriptor Create(
            SpineBackgroundOverrideRecord record,
            string? root,
            BackgroundResourceOrigin origin)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            if (origin != BackgroundResourceOrigin.GlobalCustom &&
                origin != BackgroundResourceOrigin.LocalCustom)
                throw new ArgumentOutOfRangeException(nameof(origin),
                    "Spine overrides must be global-custom or local-custom resources.");

            Validate(record);

            string identifier = record.Identifier!;
            string spinePath = record.SpinePath!.Replace('\\', '/');
            string? previewPath = record.PreviewPath == null
                ? null
                : record.PreviewPath.Replace('\\', '/');

            var normalized = new SpineBackgroundOverrideRecord(
                identifier, spinePath, record.Animation, record.Loop, record.Skin,
                previewPath, record.PreviewTime, record.Scale, record.OffsetX, record.OffsetY);

            return new BackgroundResourceDescriptor(
                BackgroundResourceIdHash.Calculate(identifier),
                identifier,
                BackgroundResourceKind.Spine,
                origin,
                spinePath,
                root,
                true,
                normalized);
        }

        public static IReadOnlyList<BackgroundResourceDescriptor> CreateMany(
            IEnumerable<SpineBackgroundOverrideRecord>? records,
            string? root,
            BackgroundResourceOrigin origin)
        {
            var result = new List<BackgroundResourceDescriptor>();
            var seenIds = new HashSet<uint>();

            if (records == null)
                return result.AsReadOnly();

            foreach (SpineBackgroundOverrideRecord record in records)
            {
                BackgroundResourceDescriptor descriptor = Create(record, root, origin);
                if (!seenIds.Add(descriptor.Id))
                    throw new InvalidDataException(
                        "Duplicate Spine background identifier: " + descriptor.Name);
                result.Add(descriptor);
            }

            return result.AsReadOnly();
        }

        public static void Validate(SpineBackgroundOverrideRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            if (string.IsNullOrWhiteSpace(record.Identifier) ||
                string.IsNullOrWhiteSpace(record.SpinePath))
                throw new InvalidDataException("Spine backgrounds require Identifier and SpinePath.");

            if (BackgroundResourceIdHash.Calculate(record.Identifier) == 0)
                throw new InvalidDataException("Background identifier hashes to the reserved no-change ID.");

            if (!IsFinite(record.Scale) || record.Scale <= 0f ||
                !IsFinite(record.OffsetX) || !IsFinite(record.OffsetY) ||
                !IsFinite(record.PreviewTime) || record.PreviewTime < 0f)
                throw new InvalidDataException(
                    "Invalid framing or PreviewTime for background " + record.Identifier + ".");
        }

        private static bool IsFinite(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }
    }
}
