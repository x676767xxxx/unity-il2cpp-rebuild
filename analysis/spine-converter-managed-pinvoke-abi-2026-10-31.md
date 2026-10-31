# Spine converter managed wrapper surface (2026-10-31)

Input: Cpp2IL recovered Android 1.0 and Windows 1.0.1 spine-unity assemblies, inspected with ILSpy. The input is the Windows release's BepInEx IL2CPP interop assembly. ILSpy shows proxy methods that use Il2CppInterop.Runtime.il2cpp_runtime_invoke; these are NOT direct DllImport/PInvoke declarations and must not be treated as the native C ABI. The report records the managed wrapper surface only.

## windows
### Converter (SHA256 a4a924cf78aea978702d10b0786ff633c22dd8248246485e299b18fd1122f64a)
```il
// IL code: spine-unity
.class public auto ansi abstract sealed beforefieldinit Native.SpineConverter.Converter
	extends [Il2Cppmscorlib]Il2CppSystem.Object
{
	// Fields
	.field private static initonly native int NativeFieldInfoPtr_DLL_NAME_WIN_LNX_MAC_ANDROID
	.field private static initonly native int NativeMethodInfoPtr_get_Name_Private_Static_get_String_0
	.field private static initonly native int NativeMethodInfoPtr_CastSkeletonDataToDstVersionNative_Private_Static_Int32_Il2CppStructArray_1_Byte_Int32_Int32_byref_IntPtr_byref_Int32_0
	.field private static initonly native int NativeMethodInfoPtr_FreeBufferNative_Private_Static_Void_IntPtr_0
	.field private static initonly native int NativeMethodInfoPtr_CastJsonSkeletonToBinary_Private_Static_Int32_String_byref_IntPtr_byref_Int32_0
	.field private static initonly native int NativeMethodInfoPtr_CastJsonToBinary_Public_Static_Il2CppStructArray_1_Byte_String_0
	.field private static initonly native int NativeMethodInfoPtr_CastSkeletonDataToDstVersion_Public_Static_Il2CppStructArray_1_Byte_Il2CppStructArray_1_Byte_SpineVersion_0

	// Methods
	.method private hidebysig specialname rtspecialname static 
		void .cctor () cil managed 
	{
		// Method begins at RVA 0x6448
		// Header size: 12
		// Code size: 176 (0xb0)
		.maxstack 3

		IL_0000: ldstr "spine-unity.dll"
		IL_0005: ldstr "Native.SpineConverter"
		IL_000a: ldstr "Converter"
		IL_000f: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::GetIl2CppClass(string, string, string)
		IL_0014: stsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_0019: ldsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_001e: call void [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_runtime_class_init(native int)
		IL_0023: ldsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_0028: ldstr "DLL_NAME_WIN_LNX_MAC_ANDROID"
		IL_002d: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::GetIl2CppField(native int, string)
		IL_0032: stsfld native int Native.SpineConverter.Converter::NativeFieldInfoPtr_DLL_NAME_WIN_LNX_MAC_ANDROID
		IL_0037: ldsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_003c: ldc.i4 100663299
		IL_0041: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::GetIl2CppMethodByToken(native int, int32)
		IL_0046: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_get_Name_Private_Static_get_String_0
		IL_004b: ldsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_0050: ldc.i4 100663300
		IL_0055: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::GetIl2CppMethodByToken(native int, int32)
		IL_005a: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastSkeletonDataToDstVersionNative_Private_Static_Int32_Il2CppStructArray_1_Byte_Int32_Int32_byref_IntPtr_byref_Int32_0
		IL_005f: ldsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_0064: ldc.i4 100663301
		IL_0069: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::GetIl2CppMethodByToken(native int, int32)
		IL_006e: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_FreeBufferNative_Private_Static_Void_IntPtr_0
		IL_0073: ldsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_0078: ldc.i4 100663302
		IL_007d: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::GetIl2CppMethodByToken(native int, int32)
		IL_0082: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastJsonSkeletonToBinary_Private_Static_Int32_String_byref_IntPtr_byref_Int32_0
		IL_0087: ldsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_008c: ldc.i4 100663303
		IL_0091: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::GetIl2CppMethodByToken(native int, int32)
		IL_0096: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastJsonToBinary_Public_Static_Il2CppStructArray_1_Byte_String_0
		IL_009b: ldsfld native int class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppClassPointerStore`1<class Native.SpineConverter.Converter>::NativeClassPtr
		IL_00a0: ldc.i4 100663304
		IL_00a5: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::GetIl2CppMethodByToken(native int, int32)
		IL_00aa: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastSkeletonDataToDstVersion_Public_Static_Il2CppStructArray_1_Byte_Il2CppStructArray_1_Byte_SpineVersion_0
		IL_00af: ret
	} // end of method Converter::.cctor

	.method public hidebysig specialname static 
		string get_Name () cil managed 
	{
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CallerCountAttribute::.ctor(int32) = (
			01 00 00 00 00 00 00 00
		)
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CachedScanResultsAttribute::.ctor() = (
			01 00 06 00 53 08 0d 52 65 66 52 61 6e 67 65 53
			74 61 72 74 00 00 00 00 53 08 0b 52 65 66 52 61
			6e 67 65 45 6e 64 00 00 00 00 53 08 0e 58 72 65
			66 52 61 6e 67 65 53 74 61 72 74 40 e7 10 00 53
			08 0c 58 72 65 66 52 61 6e 67 65 45 6e 64 42 e7
			10 00 53 0a 14 4d 65 74 61 64 61 74 61 49 6e 69
			74 54 6f 6b 65 6e 52 76 61 00 00 00 00 00 00 00
			00 53 0a 13 4d 65 74 61 64 61 74 61 49 6e 69 74
			46 6c 61 67 52 76 61 00 00 00 00 00 00 00 00
		)
		// Method begins at RVA 0x6504
		// Header size: 12
		// Code size: 31 (0x1f)
		.maxstack 4
		.locals (
			[0] native int,
			[1] native int*,
			[2] native int,
			[3] string
		)

		IL_0000: ldc.i4.0
		IL_0001: conv.u
		IL_0002: stloc.1
		IL_0003: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_get_Name_Private_Static_get_String_0
		IL_0008: ldc.i4.0
		IL_0009: ldloc.1
		IL_000a: ldloca.s 0
		IL_000c: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_runtime_invoke(native int, native int, void**, native int&)
		IL_0011: stloc.2
		IL_0012: ldloc.0
		IL_0013: call void [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppException::RaiseExceptionIfNecessary(native int)
		IL_0018: ldloc.2
		IL_0019: call string [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::Il2CppStringToManaged(native int)
		IL_001e: ret
	} // end of method Converter::get_Name

	.method public hidebysig static 
		int32 CastSkeletonDataToDstVersionNative (
			class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8> inBuf,
			int32 inLen,
			int32 dstVersion,
			[out] native int& outPtr,
			[out] int32& outLen
		) cil managed 
	{
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CallerCountAttribute::.ctor(int32) = (
			01 00 00 00 00 00 00 00
		)
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CachedScanResultsAttribute::.ctor() = (
			01 00 06 00 53 08 0d 52 65 66 52 61 6e 67 65 53
			74 61 72 74 00 00 00 00 53 08 0b 52 65 66 52 61
			6e 67 65 45 6e 64 00 00 00 00 53 08 0e 58 72 65
			66 52 61 6e 67 65 53 74 61 72 74 42 e7 10 00 53
			08 0c 58 72 65 66 52 61 6e 67 65 45 6e 64 44 e7
			10 00 53 0a 14 4d 65 74 61 64 61 74 61 49 6e 69
			74 54 6f 6b 65 6e 52 76 61 00 00 00 00 00 00 00
			00 53 0a 13 4d 65 74 61 64 61 74 61 49 6e 69 74
			46 6c 61 67 52 76 61 00 00 00 00 00 00 00 00
		)
		// Method begins at RVA 0x6530
		// Header size: 12
		// Code size: 110 (0x6e)
		.maxstack 4
		.locals (
			[0] native int,
			[1] native int*,
			[2] native int,
			[3] int32
		)

		IL_0000: ldc.i4.5
		IL_0001: conv.u
		IL_0002: sizeof [System.Runtime]System.IntPtr
		IL_0008: mul.ovf.un
		IL_0009: localloc
		IL_000b: stloc.1
		IL_000c: ldloc.1
		IL_000d: ldarg.0
		IL_000e: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::Il2CppObjectBaseToPtr(class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)
		IL_0013: stind.i
		IL_0014: ldloc.1
		IL_0015: ldc.i4.1
		IL_0016: conv.u
		IL_0017: sizeof [System.Runtime]System.IntPtr
		IL_001d: mul.ovf.un
		IL_001e: add
		IL_001f: ldarga.s inLen
		IL_0021: stind.i
		IL_0022: ldloc.1
		IL_0023: ldc.i4.2
		IL_0024: conv.u
		IL_0025: sizeof [System.Runtime]System.IntPtr
		IL_002b: mul.ovf.un
		IL_002c: add
		IL_002d: ldarga.s dstVersion
		IL_002f: stind.i
		IL_0030: ldloc.1
		IL_0031: ldc.i4.3
		IL_0032: conv.u
		IL_0033: sizeof [System.Runtime]System.IntPtr
		IL_0039: mul.ovf.un
		IL_003a: add
		IL_003b: ldarg.3
		IL_003c: conv.i
		IL_003d: stind.i
		IL_003e: ldloc.1
		IL_003f: ldc.i4.4
		IL_0040: conv.u
		IL_0041: sizeof [System.Runtime]System.IntPtr
		IL_0047: mul.ovf.un
		IL_0048: add
		IL_0049: ldarg.s outLen
		IL_004b: conv.i
		IL_004c: stind.i
		IL_004d: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastSkeletonDataToDstVersionNative_Private_Static_Int32_Il2CppStructArray_1_Byte_Int32_Int32_byref_IntPtr_byref_Int32_0
		IL_0052: ldc.i4.0
		IL_0053: ldloc.1
		IL_0054: ldloca.s 0
		IL_0056: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_runtime_invoke(native int, native int, void**, native int&)
		IL_005b: stloc.2
		IL_005c: ldloc.0
		IL_005d: call void [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppException::RaiseExceptionIfNecessary(native int)
		IL_0062: ldloc.2
		IL_0063: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_object_unbox(native int)
		IL_0068: ldobj [System.Runtime]System.Int32
		IL_006d: ret
	} // end of method Converter::CastSkeletonDataToDstVersionNative

	.method public hidebysig static 
		void FreeBufferNative (
			native int p
		) cil managed 
	{
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CallerCountAttribute::.ctor(int32) = (
			01 00 02 00 00 00 00 00
		)
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CachedScanResultsAttribute::.ctor() = (
			01 00 06 00 53 08 0d 52 65 66 52 61 6e 67 65 53
			74 61 72 74 46 e7 10 00 53 08 0b 52 65 66 52 61
			6e 67 65 45 6e 64 48 e7 10 00 53 08 0e 58 72 65
			66 52 61 6e 67 65 53 74 61 72 74 44 e7 10 00 53
			08 0c 58 72 65 66 52 61 6e 67 65 45 6e 64 46 e7
			10 00 53 0a 14 4d 65 74 61 64 61 74 61 49 6e 69
			74 54 6f 6b 65 6e 52 76 61 00 00 00 00 00 00 00
			00 53 0a 13 4d 65 74 61 64 61 74 61 49 6e 69 74
			46 6c 61 67 52 76 61 00 00 00 00 00 00 00 00
		)
		// Method begins at RVA 0x65ac
		// Header size: 12
		// Code size: 38 (0x26)
		.maxstack 4
		.locals (
			[0] native int,
			[1] native int*,
			[2] native int
		)

		IL_0000: ldc.i4.1
		IL_0001: conv.u
		IL_0002: sizeof [System.Runtime]System.IntPtr
		IL_0008: mul.ovf.un
		IL_0009: localloc
		IL_000b: stloc.1
		IL_000c: ldloc.1
		IL_000d: ldarga.s p
		IL_000f: stind.i
		IL_0010: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_FreeBufferNative_Private_Static_Void_IntPtr_0
		IL_0015: ldc.i4.0
		IL_0016: ldloc.1
		IL_0017: ldloca.s 0
		IL_0019: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_runtime_invoke(native int, native int, void**, native int&)
		IL_001e: stloc.2
		IL_001f: ldloc.0
		IL_0020: call void [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppException::RaiseExceptionIfNecessary(native int)
		IL_0025: ret
	} // end of method Converter::FreeBufferNative

	.method public hidebysig static 
		int32 CastJsonSkeletonToBinary (
			string input,
			[out] native int& outPtr,
			[out] int32& outLen
		) cil managed 
	{
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CallerCountAttribute::.ctor(int32) = (
			01 00 00 00 00 00 00 00
		)
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CachedScanResultsAttribute::.ctor() = (
			01 00 06 00 53 08 0d 52 65 66 52 61 6e 67 65 53
			74 61 72 74 00 00 00 00 53 08 0b 52 65 66 52 61
			6e 67 65 45 6e 64 00 00 00 00 53 08 0e 58 72 65
			66 52 61 6e 67 65 53 74 61 72 74 48 e7 10 00 53
			08 0c 58 72 65 66 52 61 6e 67 65 45 6e 64 4c e7
			10 00 53 0a 14 4d 65 74 61 64 61 74 61 49 6e 69
			74 54 6f 6b 65 6e 52 76 61 00 00 00 00 00 00 00
			00 53 0a 13 4d 65 74 61 64 61 74 61 49 6e 69 74
			46 6c 61 67 52 76 61 00 00 00 00 00 00 00 00
		)
		// Method begins at RVA 0x65e0
		// Header size: 12
		// Code size: 81 (0x51)
		.maxstack 4
		.locals (
			[0] native int,
			[1] native int*,
			[2] native int,
			[3] int32
		)

		IL_0000: ldc.i4.3
		IL_0001: conv.u
		IL_0002: sizeof [System.Runtime]System.IntPtr
		IL_0008: mul.ovf.un
		IL_0009: localloc
		IL_000b: stloc.1
		IL_000c: ldloc.1
		IL_000d: ldarg.0
		IL_000e: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::ManagedStringToIl2Cpp(string)
		IL_0013: stind.i
		IL_0014: ldloc.1
		IL_0015: ldc.i4.1
		IL_0016: conv.u
		IL_0017: sizeof [System.Runtime]System.IntPtr
		IL_001d: mul.ovf.un
		IL_001e: add
		IL_001f: ldarg.1
		IL_0020: conv.i
		IL_0021: stind.i
		IL_0022: ldloc.1
		IL_0023: ldc.i4.2
		IL_0024: conv.u
		IL_0025: sizeof [System.Runtime]System.IntPtr
		IL_002b: mul.ovf.un
		IL_002c: add
		IL_002d: ldarg.2
		IL_002e: conv.i
		IL_002f: stind.i
		IL_0030: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastJsonSkeletonToBinary_Private_Static_Int32_String_byref_IntPtr_byref_Int32_0
		IL_0035: ldc.i4.0
		IL_0036: ldloc.1
		IL_0037: ldloca.s 0
		IL_0039: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_runtime_invoke(native int, native int, void**, native int&)
		IL_003e: stloc.2
		IL_003f: ldloc.0
		IL_0040: call void [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppException::RaiseExceptionIfNecessary(native int)
		IL_0045: ldloc.2
		IL_0046: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_object_unbox(native int)
		IL_004b: ldobj [System.Runtime]System.Int32
		IL_0050: ret
	} // end of method Converter::CastJsonSkeletonToBinary

	.method public hidebysig static 
		class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8> CastJsonToBinary (
			string str
		) cil managed 
	{
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CallerCountAttribute::.ctor(int32) = (
			01 00 02 00 00 00 00 00
		)
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CachedScanResultsAttribute::.ctor() = (
			01 00 06 00 53 08 0d 52 65 66 52 61 6e 67 65 53
			74 61 72 74 58 e7 10 00 53 08 0b 52 65 66 52 61
			6e 67 65 45 6e 64 5a e7 10 00 53 08 0e 58 72 65
			66 52 61 6e 67 65 53 74 61 72 74 4c e7 10 00 53
			08 0c 58 72 65 66 52 61 6e 67 65 45 6e 64 58 e7
			10 00 53 0a 14 4d 65 74 61 64 61 74 61 49 6e 69
			74 54 6f 6b 65 6e 52 76 61 00 00 00 00 00 00 00
			00 53 0a 13 4d 65 74 61 64 61 74 61 49 6e 69 74
			46 6c 61 67 52 76 61 00 00 00 00 00 00 00 00
		)
		// Method begins at RVA 0x6640
		// Header size: 12
		// Code size: 56 (0x38)
		.maxstack 4
		.locals (
			[0] native int,
			[1] native int*,
			[2] native int,
			[3] class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8>
		)

		IL_0000: ldc.i4.1
		IL_0001: conv.u
		IL_0002: sizeof [System.Runtime]System.IntPtr
		IL_0008: mul.ovf.un
		IL_0009: localloc
		IL_000b: stloc.1
		IL_000c: ldloc.1
		IL_000d: ldarg.0
		IL_000e: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::ManagedStringToIl2Cpp(string)
		IL_0013: stind.i
		IL_0014: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastJsonToBinary_Public_Static_Il2CppStructArray_1_Byte_String_0
		IL_0019: ldc.i4.0
		IL_001a: ldloc.1
		IL_001b: ldloca.s 0
		IL_001d: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_runtime_invoke(native int, native int, void**, native int&)
		IL_0022: stloc.2
		IL_0023: ldloc.0
		IL_0024: call void [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppException::RaiseExceptionIfNecessary(native int)
		IL_0029: ldloc.2
		IL_002a: dup
		IL_002b: brtrue.s IL_0031

		IL_002d: pop
		IL_002e: ldnull
		IL_002f: br.s IL_0036

		IL_0031: call !!0 [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Runtime.Il2CppObjectPool::Get<class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8>>(native int)

		IL_0036: nop
		IL_0037: ret
	} // end of method Converter::CastJsonToBinary

	.method public hidebysig static 
		class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8> CastSkeletonDataToDstVersion (
			class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8> input,
			valuetype Native.SpineConverter.SpineVersion dstVersion
		) cil managed 
	{
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CallerCountAttribute::.ctor(int32) = (
			01 00 04 00 00 00 00 00
		)
		.custom instance void [Il2CppInterop.Common]Il2CppInterop.Common.Attributes.CachedScanResultsAttribute::.ctor() = (
			01 00 06 00 53 08 0d 52 65 66 52 61 6e 67 65 53
			74 61 72 74 6b e7 10 00 53 08 0b 52 65 66 52 61
			6e 67 65 45 6e 64 6f e7 10 00 53 08 0e 58 72 65
			66 52 61 6e 67 65 53 74 61 72 74 5a e7 10 00 53
			08 0c 58 72 65 66 52 61 6e 67 65 45 6e 64 6b e7
			10 00 53 0a 14 4d 65 74 61 64 61 74 61 49 6e 69
			74 54 6f 6b 65 6e 52 76 61 00 00 00 00 00 00 00
			00 53 0a 13 4d 65 74 61 64 61 74 61 49 6e 69 74
			46 6c 61 67 52 76 61 00 00 00 00 00 00 00 00
		)
		// Method begins at RVA 0x6684
		// Header size: 12
		// Code size: 70 (0x46)
		.maxstack 4
		.locals (
			[0] native int,
			[1] native int*,
			[2] native int,
			[3] class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8>
		)

		IL_0000: ldc.i4.2
		IL_0001: conv.u
		IL_0002: sizeof [System.Runtime]System.IntPtr
		IL_0008: mul.ovf.un
		IL_0009: localloc
		IL_000b: stloc.1
		IL_000c: ldloc.1
		IL_000d: ldarg.0
		IL_000e: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::Il2CppObjectBaseToPtr(class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)
		IL_0013: stind.i
		IL_0014: ldloc.1
		IL_0015: ldc.i4.1
		IL_0016: conv.u
		IL_0017: sizeof [System.Runtime]System.IntPtr
		IL_001d: mul.ovf.un
		IL_001e: add
		IL_001f: ldarga.s dstVersion
		IL_0021: stind.i
		IL_0022: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastSkeletonDataToDstVersion_Public_Static_Il2CppStructArray_1_Byte_Il2CppStructArray_1_Byte_SpineVersion_0
		IL_0027: ldc.i4.0
		IL_0028: ldloc.1
		IL_0029: ldloca.s 0
		IL_002b: call native int [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_runtime_invoke(native int, native int, void**, native int&)
		IL_0030: stloc.2
		IL_0031: ldloc.0
		IL_0032: call void [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Il2CppException::RaiseExceptionIfNecessary(native int)
		IL_0037: ldloc.2
		IL_0038: dup
		IL_0039: brtrue.s IL_003f

		IL_003b: pop
		IL_003c: ldnull
		IL_003d: br.s IL_0044

		IL_003f: call !!0 [Il2CppInterop.Runtime]Il2CppInterop.Runtime.Runtime.Il2CppObjectPool::Get<class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8>>(native int)

		IL_0044: nop
		IL_0045: ret
	} // end of method Converter::CastSkeletonDataToDstVersion

	.method public hidebysig specialname rtspecialname 
		instance void .ctor (
			native int pointer
		) cil managed 
	{
		// Method begins at RVA 0x2059
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldarg.1
		IL_0002: call instance void [Il2Cppmscorlib]Il2CppSystem.Object::.ctor(native int)
		IL_0007: ret
	} // end of method Converter::.ctor

	.method public hidebysig specialname static 
		string get_DLL_NAME_WIN_LNX_MAC_ANDROID () cil managed 
	{
		// Method begins at RVA 0x66d8
		// Header size: 12
		// Code size: 20 (0x14)
		.maxstack 2
		.locals (
			[0] native int
		)

		IL_0000: ldsfld native int Native.SpineConverter.Converter::NativeFieldInfoPtr_DLL_NAME_WIN_LNX_MAC_ANDROID
		IL_0005: ldloca.s 0
		IL_0007: conv.u
		IL_0008: call void [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::il2cpp_field_static_get_value(native int, void*)
		IL_000d: ldloc.0
		IL_000e: call string [Il2CppInterop.Runtime]Il2CppInterop.Runtime.IL2CPP::Il2CppStringToManaged(native int)
		IL_0013: ret
	} // end of method Converter::get_DLL_NAME_WIN_LNX_MAC_ANDROID

	.method public hidebysig specialname static 
		void set_DLL_NAME_WIN_LNX_MAC_ANDROID (
			string ''
		) cil managed 
	{
		// Method begins at RVA 0x2062
		// Header size: 1
		// Code size: 17 (0x11)
		.maxstack 8

		IL_0000: ldsfld native int Native.SpineConverter.Converter::NativeFieldInfoPtr_DLL_NAME_WIN_LNX_MAC_ANDROID
		IL_0005: ldarg.0
```
#### Native import declarations
.field private static initonly native int NativeMethodInfoPtr_CastSkeletonDataToDstVersionNative_Private_Static_Int32_Il2CppStructArray_1_Byte_Int32_Int32_byref_IntPtr_byref_Int32_0
.field private static initonly native int NativeMethodInfoPtr_FreeBufferNative_Private_Static_Void_IntPtr_0
.field private static initonly native int NativeMethodInfoPtr_CastJsonSkeletonToBinary_Private_Static_Int32_String_byref_IntPtr_byref_Int32_0
.field private static initonly native int NativeMethodInfoPtr_CastSkeletonDataToDstVersion_Public_Static_Il2CppStructArray_1_Byte_Il2CppStructArray_1_Byte_SpineVersion_0
IL_005a: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastSkeletonDataToDstVersionNative_Private_Static_Int32_Il2CppStructArray_1_Byte_Int32_Int32_byref_IntPtr_byref_Int32_0
IL_006e: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_FreeBufferNative_Private_Static_Void_IntPtr_0
IL_0082: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastJsonSkeletonToBinary_Private_Static_Int32_String_byref_IntPtr_byref_Int32_0
IL_00aa: stsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastSkeletonDataToDstVersion_Public_Static_Il2CppStructArray_1_Byte_Il2CppStructArray_1_Byte_SpineVersion_0
int32 CastSkeletonDataToDstVersionNative (
IL_004d: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastSkeletonDataToDstVersionNative_Private_Static_Int32_Il2CppStructArray_1_Byte_Int32_Int32_byref_IntPtr_byref_Int32_0
} // end of method Converter::CastSkeletonDataToDstVersionNative
void FreeBufferNative (
IL_0010: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_FreeBufferNative_Private_Static_Void_IntPtr_0
} // end of method Converter::FreeBufferNative
int32 CastJsonSkeletonToBinary (
IL_0030: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastJsonSkeletonToBinary_Private_Static_Int32_String_byref_IntPtr_byref_Int32_0
} // end of method Converter::CastJsonSkeletonToBinary
class [Il2CppInterop.Runtime]Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1<uint8> CastSkeletonDataToDstVersion (
IL_0022: ldsfld native int Native.SpineConverter.Converter::NativeMethodInfoPtr_CastSkeletonDataToDstVersion_Public_Static_Il2CppStructArray_1_Byte_Il2CppStructArray_1_Byte_SpineVersion_0
} // end of method Converter::CastSkeletonDataToDstVersion
### SpineVersion (SHA256 bf7a2f317e4fafca0fa6928071a62bd1851481ffb4306274049471ca45f466d8)
```il
// IL code: spine-unity
.class public auto ansi sealed beforefieldinit Native.SpineConverter.SpineVersion
	extends [System.Runtime]System.Enum
{
	// Fields
	.field public specialname rtspecialname int32 value__
	.field public static literal valuetype Native.SpineConverter.SpineVersion Version35 = int32(0)
	.field public static literal valuetype Native.SpineConverter.SpineVersion Version36 = int32(1)
	.field public static literal valuetype Native.SpineConverter.SpineVersion Version37 = int32(2)
	.field public static literal valuetype Native.SpineConverter.SpineVersion Version38 = int32(3)
	.field public static literal valuetype Native.SpineConverter.SpineVersion Version40 = int32(4)
	.field public static literal valuetype Native.SpineConverter.SpineVersion Version41 = int32(5)
	.field public static literal valuetype Native.SpineConverter.SpineVersion Version42 = int32(6)
	.field public static literal valuetype Native.SpineConverter.SpineVersion Invalid = int32(-1)

} // end of class Native.SpineConverter.SpineVersion
```
#### Native import declarations
No P/Invoke marker found in exported excerpt.

## Interpretation
- The managed wrapper exposes CastSkeletonDataToDstVersionNative(byte[] inBuf, int inLen, int dstVersion, out IntPtr outPtr, out int outLen), FreeBufferNative(IntPtr p), and CastJsonSkeletonToBinary(string input, out IntPtr outPtr, out int outLen), plus higher-level managed byte[] wrappers. These are IL2CPP wrapper signatures, not proof of the underlying native C ABI.
- The native exports observed in the separate binary inspection are CastJsonSkeletonToBinary, CastSkeletonDataToDstVersion and FreeBuffer. Do not assume their ABI from export names alone.
- The Android ELF and Windows PE native converter binaries are different architectures and toolchains. Neither binary can be embedded into iOS.
- The iOS 0.7.9.2 app inventory did not include a standalone Spine converter file, and its UnityFramework string scan did not find these converter identifiers. This is absence-of-evidence only; static linking/stripping remains possible.
- The field name DLL_NAME_WIN_LNX_MAC_ANDROID is a clue about the target platforms named by the managed wrapper, but not conclusive proof that no iOS-specific implementation exists. Do not implement a DllImport declaration for iOS until an arm64-apple-ios implementation and actual ABI are independently verified.
- No iOS converter, Unity assembly, or IPA is produced by this workflow.


## Cross-check against original IL2CPP metadata dump.cs
The actual Android/Windows metadata dumps (not the BepInEx interop proxy) independently show these managed extern declarations:
- `private static extern int CastSkeletonDataToDstVersionNative(byte[] inBuf, int inLen, int dstVersion, out IntPtr outPtr, out int outLen)`
- `private static extern void FreeBufferNative(IntPtr p)`
- `private static extern int CastJsonSkeletonToBinary(string input, out IntPtr outPtr, out int outLen)`
- `DLL_NAME_WIN_LNX_MAC_ANDROID = "SpineSkeletonDataConverterLib"`
- `SpineVersion.Version42 = 6`

The exact metadata source is the three-platform dump artifact from [run 41172721121](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41172721121); the verified iOS absence is documented in `analysis/ios-spine-api-gap-verified-2026-10-31.md`.

The managed `extern` declarations confirm the managed-to-native boundary and the out-pointer/out-length ownership protocol. They still do not reveal the complete C ABI details (calling convention, string encoding, allocator contract, error-code meanings) or provide source code. Those must be verified from the native implementation or tested ABI harness before writing an iOS replacement.
