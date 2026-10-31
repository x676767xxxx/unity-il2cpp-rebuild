#if UNITY_2021_3_OR_NEWER
using System;
using AzureArchive.BackgroundCompat;
using UnityEngine;

namespace AzureArchive.UnityCompat
{
    /// <summary>
    /// Installs the catalog-token transform before scene Awake methods can start
    /// ScenarioResourceManager's Addressables update coroutine. This source must
    /// be included in the Unity player build; it is not a binary patch.
    /// </summary>
    public static class AddressablesTransformBootstrap
    {
        private static AddressablesInternalIdTransformHook? hook;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstallBeforeSceneLoad()
        {
            try
            {
                InstallNow();
            }
            catch (Exception exception)
            {
                Debug.LogError("AzureArchive Addressables URL hook was not installed: " + exception);
            }
        }

        /// <summary>
        /// Explicit entry point for integration tests or a game-owned bootstrap.
        /// Repeated calls are idempotent.
        /// </summary>
        public static void InstallNow()
        {
            if (hook != null)
                return;

            hook = AddressablesInternalIdTransformHook.InstallFromGameRuntime();
        }
    }
}
#endif
