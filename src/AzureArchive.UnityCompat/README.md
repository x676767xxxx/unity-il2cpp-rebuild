# Unity compatibility source candidates

`BackgroundImageDecoder.Unity.cs` is a Unity-side source candidate based on the recovered Android 1.0 / Windows 1.0.1 IL and the old iOS 0.7.9.2 metadata API surface.

## Evidence mapping
- Initial texture: 2 x 2.
- Format: `TextureFormat.RGBA32` (Unity enum value 4).
- Mip chain: false.
- Wrap mode: `TextureWrapMode.Clamp` (enum value 1).
- Decode: `ImageConversion.LoadImage(texture, bytes, true)`.
- False return: `InvalidDataException` with the recovered message prefix.

## Status
This is candidate Unity C# source, not an integrated game patch. It has **not** been compiled against UnityEngine assemblies, run in Unity, or tested on iOS. Unity texture creation and destruction must occur on the Unity main thread. The cleanup behavior on decoder exceptions is a conservative implementation choice; the corresponding original IL contains unresolved decompiler regions.

The standalone .NET Standard path utility in `../AzureArchive.BackgroundCompat` is independently build-tested, but it is not connected to this decoder yet.
