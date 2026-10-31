using System;
using System.Text;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Port of the Android 1.0 Util.CalculateHash(string) input contract:
    /// UTF-8 bytes are passed to the game's xxHash32 implementation with seed 0.
    /// Null is mapped to zero by the native method. The recovered implementation
    /// has a separate path for strings longer than 256 UTF-16 code units; this
    /// portable implementation rejects that uncommon case rather than guessing.
    /// </summary>
    public static class BackgroundResourceIdHash
    {
        private const uint Prime1 = 2654435761U;
        private const uint Prime2 = 2246822519U;
        private const uint Prime3 = 3266489917U;
        private const uint Prime4 = 668265263U;
        private const uint Prime5 = 374761393U;

        public static uint Calculate(string? value)
        {
            if (value == null)
                return 0;

            if (value.Length > 256)
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Android 1.0 uses a separate long-string hash path; this port does not guess that behavior.");

            return XxHash32(Encoding.UTF8.GetBytes(value));
        }

        private static uint XxHash32(byte[] data)
        {
            unchecked
            {
                int offset = 0;
                int length = data.Length;
                uint hash;

                if (length >= 16)
                {
                    uint v1 = Prime1 + Prime2;
                    uint v2 = Prime2;
                    uint v3 = 0;
                    uint v4 = 0U - Prime1;
                    int limit = length - 16;

                    do
                    {
                        v1 = Round(v1, ReadUInt32(data, offset)); offset += 4;
                        v2 = Round(v2, ReadUInt32(data, offset)); offset += 4;
                        v3 = Round(v3, ReadUInt32(data, offset)); offset += 4;
                        v4 = Round(v4, ReadUInt32(data, offset)); offset += 4;
                    }
                    while (offset <= limit);

                    hash = RotateLeft(v1, 1) + RotateLeft(v2, 7) +
                           RotateLeft(v3, 12) + RotateLeft(v4, 18);
                }
                else
                {
                    hash = Prime5;
                }

                hash += (uint)length;

                while (offset <= length - 4)
                {
                    hash += ReadUInt32(data, offset) * Prime3;
                    hash = RotateLeft(hash, 17) * Prime4;
                    offset += 4;
                }

                while (offset < length)
                {
                    hash += data[offset] * Prime5;
                    hash = RotateLeft(hash, 11) * Prime1;
                    offset++;
                }

                hash ^= hash >> 15;
                hash *= Prime2;
                hash ^= hash >> 13;
                hash *= Prime3;
                hash ^= hash >> 16;
                return hash;
            }
        }

        private static uint Round(uint accumulator, uint input)
        {
            unchecked
            {
                accumulator += input * Prime2;
                accumulator = RotateLeft(accumulator, 13);
                return accumulator * Prime1;
            }
        }

        private static uint ReadUInt32(byte[] data, int offset)
        {
            return (uint)data[offset] |
                   ((uint)data[offset + 1] << 8) |
                   ((uint)data[offset + 2] << 16) |
                   ((uint)data[offset + 3] << 24);
        }

        private static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }
    }
}
