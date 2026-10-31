#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace AzureArchive.UnityCompat
{
    /// <summary>
    /// Managed bridge for the project's custom iOS Spine converter C ABI.
    /// The native static archive must be linked into the Unity iOS target before use.
    /// All native output buffers are copied into managed arrays and freed by the
    /// matching native allocator before returning.
    /// </summary>
    public static class SpineConverterNative
    {
        private const int Spine42Version = 6;

        [DllImport("__Internal", EntryPoint = "AzureArchiveSpineJsonToBinary",
            CallingConvention = CallingConvention.Cdecl)]
        private static extern int JsonToBinaryNative(
            byte[] jsonUtf8, int jsonLength, out IntPtr output, out int outputLength);

        [DllImport("__Internal", EntryPoint = "AzureArchiveSpineConvertBuffer",
            CallingConvention = CallingConvention.Cdecl)]
        private static extern int ConvertBufferNative(
            byte[] input, int inputLength, int destinationVersion,
            out IntPtr output, out int outputLength);

        [DllImport("__Internal", EntryPoint = "AzureArchiveSpineFreeBuffer",
            CallingConvention = CallingConvention.Cdecl)]
        private static extern void FreeBufferNative(IntPtr buffer);

        public static byte[] JsonToSpine42Binary(byte[] utf8Json)
        {
            if (utf8Json == null) throw new ArgumentNullException(nameof(utf8Json));
            if (utf8Json.Length == 0) throw new ArgumentException("Spine JSON is empty.", nameof(utf8Json));

            IntPtr output = IntPtr.Zero;
            int outputLength = 0;
            int result = JsonToBinaryNative(utf8Json, utf8Json.Length, out output, out outputLength);
            return CopyAndFree(result, output, outputLength, "JSON-to-binary");
        }

        public static byte[] ConvertToSpine42Binary(byte[] skeletonData)
        {
            if (skeletonData == null) throw new ArgumentNullException(nameof(skeletonData));
            if (skeletonData.Length == 0) throw new ArgumentException("Spine skeleton data is empty.", nameof(skeletonData));

            IntPtr output = IntPtr.Zero;
            int outputLength = 0;
            int result = ConvertBufferNative(
                skeletonData, skeletonData.Length, Spine42Version, out output, out outputLength);
            return CopyAndFree(result, output, outputLength, "skeleton version conversion");
        }

        private static byte[] CopyAndFree(int result, IntPtr output, int outputLength, string operation)
        {
            try
            {
                if (result != 0)
                    throw new InvalidOperationException("Spine native " + operation + " failed with code " + result + ".");
                if (output == IntPtr.Zero || outputLength <= 0)
                    throw new InvalidOperationException("Spine native " + operation + " returned an empty buffer.");

                var managed = new byte[outputLength];
                Marshal.Copy(output, managed, 0, outputLength);
                return managed;
            }
            finally
            {
                if (output != IntPtr.Zero)
                    FreeBufferNative(output);
            }
        }
    }
}
#endif
