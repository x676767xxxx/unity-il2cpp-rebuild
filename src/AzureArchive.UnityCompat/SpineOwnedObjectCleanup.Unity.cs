#if UNITY_2021_3_OR_NEWER
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AzureArchive.UnityCompat
{
    /// <summary>
    /// Unity-main-thread cleanup adapter for objects owned by RuntimeSpineAsset.
    /// Mirrors Android 1.0 RuntimeSpineLoader.DestroyOwned: ignore Unity-null
    /// objects, use deferred Destroy while playing, and DestroyImmediate otherwise.
    /// </summary>
    public static class SpineOwnedObjectCleanup
    {
        public static void DestroyOwned(UnityEngine.Object value)
        {
            if (value == null)
                return;

            if (Application.isPlaying)
                UnityEngine.Object.Destroy(value);
            else
                UnityEngine.Object.DestroyImmediate(value);
        }

        /// <summary>
        /// Rolls back a partial construction snapshot returned by
        /// SpineAssetConstructionLedger.Abort(). Call only on Unity's main thread.
        /// </summary>
        public static void Rollback(IEnumerable<UnityEngine.Object> ownedObjects)
        {
            if (ownedObjects == null)
                throw new ArgumentNullException(nameof(ownedObjects));

            foreach (UnityEngine.Object value in ownedObjects)
                DestroyOwned(value);
        }
    }
}
#endif
