using System;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Engine-independent reference-count state for RuntimeSpineAsset.
    /// The initial cache-owned reference is represented by the constructor count.
    /// This class does not own or destroy Unity objects.
    /// </summary>
    public sealed class SpineAssetReferenceLedger
    {
        private int references;

        public SpineAssetReferenceLedger(int initialReferences = 1)
        {
            if (initialReferences < 0)
                throw new ArgumentOutOfRangeException(nameof(initialReferences));
            references = initialReferences;
        }

        public int ReferenceCount => references;

        /// <summary>
        /// Android 1.0 HasConsumers is references &gt; 1: one reference is cache ownership.
        /// </summary>
        public bool HasConsumers => references > 1;

        /// <summary>
        /// Retain is only valid while the asset is alive; a zero-count asset cannot be revived.
        /// </summary>
        public void Retain()
        {
            if (references <= 0)
                throw new ObjectDisposedException(nameof(SpineAssetReferenceLedger));
            if (references == int.MaxValue)
                throw new OverflowException("Spine asset reference count cannot be incremented.");
            references++;
        }

        /// <summary>
        /// Releases one reference. Returns true exactly on the transition to zero,
        /// allowing the Unity adapter to perform final owned-object cleanup once.
        /// Further releases at zero are harmless and return false.
        /// </summary>
        public bool Release()
        {
            if (references <= 0) return false;
            references--;
            return references == 0;
        }
    }
}
