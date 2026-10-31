using System;
using System.IO;
using System.Collections.Generic;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Platform-neutral portion of Android 1.0 ScenarioResourceManager.ResolveBackground.
    /// Database access is supplied by the caller; this class owns override precedence
    /// and the mapping from ScenarioBGNameExcel to a native resource descriptor.
    /// </summary>
    public static class ScenarioBackgroundResolver
    {
        /// <summary>
        /// Resolve in Android 1.0 order: zero ID -> null; local override; global
        /// override; then the first scenario record whose Name field equals the ID.
        /// </summary>
        public static BackgroundResourceDescriptor? Resolve(
            uint id,
            IEnumerable<BackgroundResourceDescriptor>? localOverrides,
            IEnumerable<BackgroundResourceDescriptor>? globalOverrides,
            Func<uint, ScenarioBackgroundRecord?> findByNameId)
        {
            if (findByNameId == null)
                throw new ArgumentNullException(nameof(findByNameId));

            if (id == 0)
                return null;

            if (BackgroundOverrideResolver.TryResolve(id, localOverrides, globalOverrides, out var overridden))
                return overridden;

            ScenarioBackgroundRecord? record = findByNameId(id);
            if (record == null || record.NameId != id)
                return null;

            string? name = record.FileName == null
                ? null
                : Path.GetFileNameWithoutExtension(record.FileName);

            return new BackgroundResourceDescriptor(
                id,
                name,
                record.Kind,
                BackgroundResourceOrigin.Native,
                record.FileName,
                null,
                false);
        }
    }
}
