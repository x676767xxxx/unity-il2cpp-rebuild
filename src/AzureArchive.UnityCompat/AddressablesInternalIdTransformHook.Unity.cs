#if UNITY_2021_3_OR_NEWER
using System;
using System.Reflection;
using AzureArchive.BackgroundCompat;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace AzureArchive.UnityCompat
{
    /// <summary>
    /// Installs the verified catalog-token substitution into Addressables before
    /// catalog-backed asset loads begin. The base URL is supplied by the game's
    /// runtime configuration; this class never chooses a CDN.
    /// </summary>
    public sealed class AddressablesInternalIdTransformHook : IDisposable
    {
        private readonly Func<IResourceLocation, string>? previous;
        private readonly Func<IResourceLocation, string> installed;
        private bool disposed;

        private AddressablesInternalIdTransformHook(Func<string> getAddrLoadUrl)
        {
            if (getAddrLoadUrl == null) throw new ArgumentNullException(nameof(getAddrLoadUrl));

            previous = Addressables.InternalIdTransformFunc;
            installed = location =>
            {
                if (location == null) throw new ArgumentNullException(nameof(location));
                string original = previous != null ? previous(location) : location.InternalId;
                if (string.IsNullOrEmpty(original) ||
                    !original.StartsWith(AddressableInternalIdResolver.AddrLoadUrlToken, StringComparison.Ordinal))
                    return original;

                return AddressableInternalIdResolver.Resolve(original, getAddrLoadUrl());
            };

            Addressables.InternalIdTransformFunc = installed;
        }

        /// <summary>
        /// Installs the hook. Call before Addressables initializes or starts
        /// loading catalog-backed content. Dispose restores the previous hook
        /// only when this instance still owns the global transform slot.
        /// </summary>
        public static AddressablesInternalIdTransformHook Install(Func<string> getAddrLoadUrl)
        {
            return new AddressablesInternalIdTransformHook(getAddrLoadUrl);
        }

        /// <summary>
        /// Installs the hook using the actual static AddrLoadUrl field on the
        /// game's ScenarioResourceManager type. The field is read lazily for each
        /// matching catalog ID so installation can happen before the game assigns
        /// its runtime URL. Call before Addressables begins catalog-backed loads.
        /// </summary>
        public static AddressablesInternalIdTransformHook InstallFromRuntimeType(Type scenarioResourceManagerType)
        {
            if (scenarioResourceManagerType == null)
                throw new ArgumentNullException(nameof(scenarioResourceManagerType));

            FieldInfo? field = scenarioResourceManagerType.GetField(
                "AddrLoadUrl", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null || field.FieldType != typeof(string))
                throw new InvalidOperationException(
                    "ScenarioResourceManager must expose a static string AddrLoadUrl field.");

            return Install(() => field.GetValue(null) as string ?? string.Empty);
        }

        /// <summary>
        /// Finds the global ScenarioResourceManager type in Assembly-CSharp and
        /// installs the verified AddrLoadUrl transform without a compile-time
        /// reference to the game's Assembly-CSharp.dll.
        /// </summary>
        public static AddressablesInternalIdTransformHook InstallFromGameRuntime()
        {
            Type? managerType = Type.GetType("ScenarioResourceManager, Assembly-CSharp", false);
            if (managerType == null)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    managerType = assembly.GetType("ScenarioResourceManager", false, false);
                    if (managerType != null) break;
                }
            }

            if (managerType == null)
                throw new InvalidOperationException(
                    "Could not find the game's ScenarioResourceManager type in loaded assemblies.");

            return InstallFromRuntimeType(managerType);
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Do not overwrite a transform installed by another subsystem later.
            if (ReferenceEquals(Addressables.InternalIdTransformFunc, installed))
                Addressables.InternalIdTransformFunc = previous;
        }
    }
}
#endif
