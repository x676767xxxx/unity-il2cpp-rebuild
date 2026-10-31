# Exact background method IL evidence (2026-10-31)

Inputs: Cpp2IL recovered Android 1.0 and Windows 1.0.1 assemblies; ILSpy IL output. The ILSpy formatter wraps method declarations across multiple lines, so this report extracts bounded contexts around every exact symbol occurrence rather than assuming the name is on the .method line. This is recovered IL, not original C# source.

## BackgroundResource
### android (SHA256 87f103b680a7fabd5d1492c364a07596644d3fb3052889e46d1b52a04e2eb701)
#### ResolveOverride: 6 occurrences
Context at line 156:
```il
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass28_0'::.ctor

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__0' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x674cec
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__0'

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__1' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x674d4c
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
```
Context at line 199:
```il
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__0'

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__1' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x674d4c
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private class BackgroundResource '<>2__current'
		.field private int32 '<>l__initialThreadId'
		.field private class ResourceOverrideData data
		.field public class ResourceOverrideData '<>3__data'
		.field private valuetype ScenarioResourceManager/ResourceType source
		.field public valuetype ScenarioResourceManager/ResourceType '<>3__source'
		.field private class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData> '<spines>5__2'
		.field private valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string> '<>7__wrap2'
		.field private valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData> '<>7__wrap3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x674dac
			// Header size: 12
			// Code size: 41 (0x29)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0015: stloc 2
			IL_0019: br IL_001e

			IL_001e: ldarg.0
			IL_001f: ldloc 2
			IL_0023: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0028: ret
		} // end of method '<Overrides>d__29'::.ctor
```
Context at line 202:
```il

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__0'

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__1' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x674d4c
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private class BackgroundResource '<>2__current'
		.field private int32 '<>l__initialThreadId'
		.field private class ResourceOverrideData data
		.field public class ResourceOverrideData '<>3__data'
		.field private valuetype ScenarioResourceManager/ResourceType source
		.field public valuetype ScenarioResourceManager/ResourceType '<>3__source'
		.field private class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData> '<spines>5__2'
		.field private valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string> '<>7__wrap2'
		.field private valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData> '<>7__wrap3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x674dac
			// Header size: 12
			// Code size: 41 (0x29)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0015: stloc 2
			IL_0019: br IL_001e

			IL_001e: ldarg.0
			IL_001f: ldloc 2
			IL_0023: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0028: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
```
Context at line 245:
```il
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private class BackgroundResource '<>2__current'
		.field private int32 '<>l__initialThreadId'
		.field private class ResourceOverrideData data
		.field public class ResourceOverrideData '<>3__data'
		.field private valuetype ScenarioResourceManager/ResourceType source
		.field public valuetype ScenarioResourceManager/ResourceType '<>3__source'
		.field private class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData> '<spines>5__2'
		.field private valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string> '<>7__wrap2'
		.field private valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData> '<>7__wrap3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x674dac
			// Header size: 12
			// Code size: 41 (0x29)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0015: stloc 2
			IL_0019: br IL_001e

			IL_001e: ldarg.0
			IL_001f: ldloc 2
			IL_0023: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0028: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x674de4
			// Header size: 12
			// Code size: 87 (0x57)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] class BackgroundResource/'<Overrides>d__29',
				[5] native int,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object,
				[12] object,
				[13] object,
				[14] object,
				[15] object,
				[16] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_0056

			IL_0028: br IL_002d

			IL_002d: ldc.i4 18477388
			IL_0032: stloc 2
			IL_0036: ldstr "Unmanaged memory load: [v16 @ rcx_v1+v2 @ rax_v2*4]"
```
Context at line 2252:
```il
		IL_009b: add
		IL_009c: stloc 11
		IL_00a0: ldstr "Method not found @325DFB3"
		IL_00a5: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00aa: ret
	} // end of method BackgroundResource::.ctor

	.method public hidebysig static 
		class BackgroundResource ResolveOverride (
			uint32 id,
			class ResourceOverrideData local,
			class ResourceOverrideData global
		) cil managed 
	{
		// Method begins at RVA 0x163dd4
		// Header size: 12
		// Code size: 301 (0x12d)
		.maxstack 0
		.locals init (
			[0] class ResourceOverrideData,
			[1] class ResourceOverrideData,
			[2] uint32,
			[3] native int,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] object,
			[16] bool,
			[17] class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
			[18] class [mscorlib]System.NullReferenceException,
			[19] native int,
			[20] class [mscorlib]System.Func`2<object, bool>,
			[21] native int,
			[22] class BackgroundResource,
			[23] class BackgroundResource,
			[24] bool,
			[25] native int,
			[26] class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
			[27] class [mscorlib]System.Func`2<object, bool>,
			[28] native int
		)

		IL_0000: br IL_00ef
		// loop start (head: IL_00ef)
			IL_0005: ldarg id
			IL_0009: pop
			IL_000a: ldarg local
			IL_000e: ldc.i4 2
			IL_0013: call class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> BackgroundResource::Overrides(class ResourceOverrideData, valuetype ScenarioResourceManager/ResourceType)
			IL_0018: stloc 17
			IL_001c: ldtoken class [mscorlib]System.Func`2<class BackgroundResource, bool>
			IL_0021: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			IL_0026: stloc 19
			IL_002a: ldstr "Method not found @325E234"
			IL_002f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0034: br IL_0039

			IL_0039: ldc.i4.0
			IL_003a: conv.i
			IL_003b: stloc 21
			IL_003f: ldloc 20
			IL_0043: ldloc 15
			IL_0047: ldloc 21
			IL_004b: call instance void class [mscorlib]System.Func`2<object, bool>::.ctor(object, native int)
			IL_0050: ldc.i4.0
			IL_0051: conv.i
			IL_0052: stloc 25
			IL_0056: ldloc 17
			IL_005a: ldloc 20
			IL_005e: call !!0 [System.Core]System.Linq.Enumerable::FirstOrDefault<object>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, bool>)
			IL_0063: stloc 22
			IL_0067: br IL_006c

			IL_006c: ldloc 22
			IL_0070: ldc.i4 0
			IL_0075: ceq
			IL_0077: stloc 24
			IL_007b: ldloc 24
			IL_007f: brtrue IL_008e

			IL_0084: br IL_0089

			IL_0089: ldloc 22
			IL_008d: ret

			IL_008e: ldarg global
			IL_0092: ldc.i4 1
			IL_0097: call class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> BackgroundResource::Overrides(class ResourceOverrideData, valuetype ScenarioResourceManager/ResourceType)
			IL_009c: stloc 26
			IL_00a0: ldstr "Method not found @325E234"
			IL_00a5: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00aa: br IL_00af

			IL_00af: ldc.i4.0
			IL_00b0: conv.i
			IL_00b1: stloc 28
			IL_00b5: ldloc 27
			IL_00b9: ldloc 15
			IL_00bd: ldloc 28
			IL_00c1: call instance void class [mscorlib]System.Func`2<object, bool>::.ctor(object, native int)
```
Context at line 2383:
```il
				IL_0119: ceq
				IL_011b: stloc 16
				IL_011f: ldloc 16
				IL_0123: brtrue IL_00e1
			// end loop

			IL_0128: br IL_0005
		// end loop
	} // end of method BackgroundResource::ResolveOverride

	.method public hidebysig static 
		class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> Overrides (
			class ResourceOverrideData data,
			valuetype ScenarioResourceManager/ResourceType source
		) cil managed 
	{
		// Method begins at RVA 0x163f10
		// Header size: 12
		// Code size: 106 (0x6a)
		.maxstack 0
		.locals init (
			[0] valuetype ScenarioResourceManager/ResourceType,
			[1] class ResourceOverrideData,
			[2] native int,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] object,
			[16] int32,
			[17] object
		)

		IL_0000: br IL_0031
		// loop start (head: IL_0031)
			IL_0005: ldloc 16
			IL_0009: pop
			IL_000a: ldloc 15
			IL_000e: ldc.i4 48
			IL_0013: add
			IL_0014: stloc 17
			IL_0018: ldarg data
			IL_001c: pop
			IL_001d: ldstr "Method not found @325DFB3"
			IL_0022: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0027: ldarg source
			IL_002b: pop
			IL_002c: ldloc 15
			IL_0030: ret

			IL_0031: ldtoken BackgroundResource/'<Overrides>d__29'
			IL_0036: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			IL_003b: stloc 14
			IL_003f: ldstr "Method not found @325E234"
			IL_0044: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0049: ldloc 15
			IL_004d: call instance void [mscorlib]System.Object::.ctor()
			IL_0052: ldc.i8 4294967294
			IL_005b: pop
			IL_005c: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0061: stloc 16
			IL_0065: br IL_0005
		// end loop
	} // end of method BackgroundResource::Overrides

	.method public hidebysig static 
		void Validate (
			class ResourceOverrideData/BgSpineOverrideData spine
		) cil managed 
	{
		// Method begins at RVA 0x163f88
		// Header size: 12
		// Code size: 727 (0x2d7)
		.maxstack 0
		.locals init (
			[0] class ResourceOverrideData/BgSpineOverrideData,
			[1] bool,
			[2] bool,
			[3] bool,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] class [System]System.IO.InvalidDataException,
			[15] class [System]System.IO.InvalidDataException,
			[16] native int,
			[17] object,
			[18] bool,
			[19] string,
			[20] uint32,
			[21] object,
			[22] object,
			[23] float32,
			[24] class [System]System.IO.InvalidDataException,
```
#### ResolvePath: 5 occurrences
Context at line 87:
```il
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c'::.ctor

		.method assembly hidebysig 
			instance bool '<ResolvePath>b__32_0' (
				string x
			) cil managed 
		{
			// Method begins at RVA 0x674c78
			// Header size: 12
			// Code size: 82 (0x52)
			.maxstack 0
			.locals init (
				[0] string,
				[1] bool,
				[2] object,
				[3] bool
			)

			IL_0000: ldstr "Unmanaged memory load: [759BA34]"
			IL_0005: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_000a: ldc.i4.0
			IL_000b: conv.i
			IL_000c: ldc.i4 0
			IL_0011: ceq
			IL_0013: ldc.i4.0
			IL_0014: ceq
			IL_0016: stloc 1
			IL_001a: ldloc 1
			IL_001e: brtrue IL_0033

			IL_0023: br IL_0028

			IL_0028: ldc.i4 1
			IL_002d: pop
			IL_002e: br IL_0033

			IL_0033: ldstr ".."
			IL_0038: stloc 2
			IL_003c: ldarg x
			IL_0040: ldloc 2
			IL_0044: call bool [mscorlib]System.String::op_Equality(string, string)
			IL_0049: stloc 3
			IL_004d: ldloc 3
			IL_0051: ret
		} // end of method '<>c'::'<ResolvePath>b__32_0'

	} // end of class <>c

	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass28_0'
		extends [mscorlib]System.Object
	{
		// Fields
		.field public uint32 id

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x674cd8
			// Header size: 12
			// Code size: 7 (0x7)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass28_0'::.ctor

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__0' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x674cec
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
```
Context at line 128:
```il
			IL_0033: ldstr ".."
			IL_0038: stloc 2
			IL_003c: ldarg x
			IL_0040: ldloc 2
			IL_0044: call bool [mscorlib]System.String::op_Equality(string, string)
			IL_0049: stloc 3
			IL_004d: ldloc 3
			IL_0051: ret
		} // end of method '<>c'::'<ResolvePath>b__32_0'

	} // end of class <>c

	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass28_0'
		extends [mscorlib]System.Object
	{
		// Fields
		.field public uint32 id

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x674cd8
			// Header size: 12
			// Code size: 7 (0x7)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass28_0'::.ctor

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__0' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x674cec
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__0'

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__1' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x674d4c
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

```
Context at line 1912:
```il
		IL_0015: brtrue IL_0039

		IL_001a: br IL_001f

		IL_001f: ldarg.0
		IL_0020: ldfld string BackgroundResource::'<Root>k__BackingField'
		IL_0025: ldarg.0
		IL_0026: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_002b: call string BackgroundResource::ResolvePath(string, string)
		IL_0030: stloc 1
		IL_0034: ldloc 1
		IL_0038: ret

		IL_0039: ldarg.0
		IL_003a: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_003f: ret
	} // end of method BackgroundResource::get_FullPath

	.method public hidebysig specialname 
		instance string get_CacheKey () cil managed 
	{
		// Method begins at RVA 0x163a48
		// Header size: 12
		// Code size: 710 (0x2c6)
		.maxstack 0
		.locals init (
			[0] object,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] native int,
			[13] string[],
			[14] native int,
			[15] class [mscorlib]System.Enum,
			[16] string,
			[17] bool,
			[18] class [mscorlib]System.NullReferenceException,
			[19] object,
			[20] object,
			[21] string,
			[22] string,
			[23] object,
			[24] object,
			[25] uint32,
			[26] object,
			[27] object,
			[28] object,
			[29] string,
			[30] object,
			[31] object,
			[32] native int,
			[33] object,
			[34] bool,
			[35] object,
			[36] object,
			[37] object,
			[38] class BackgroundResource,
			[39] uint32
		)

		IL_0000: br IL_0259
		// loop start (head: IL_0259)
			IL_0005: ldloc 13
			IL_0009: ldc.i4 32
			IL_000e: add
			IL_000f: stloc 19
			IL_0013: ldloc 13
			IL_0017: ldc.i4 0
			IL_001c: ldloc 16
			IL_0020: stelem [mscorlib]System.String
			IL_0025: ldstr "Method not found @325DFB3"
			IL_002a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_002f: br IL_0034

			IL_0034: ldstr "\n"
			IL_0039: stloc 20
			IL_003d: ldloc 13
			IL_0041: ldc.i4 40
			IL_0046: add
			IL_0047: stloc 23
			IL_004b: ldloc 13
			IL_004f: ldc.i4 1
			IL_0054: ldloc 20
			IL_0058: stelem [mscorlib]System.String
			IL_005d: ldstr "Method not found @325DFB3"
			IL_0062: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0067: br IL_006c

			IL_006c: ldloc 13
			IL_0070: ldc.i4 48
			IL_0075: add
			IL_0076: stloc 24
			IL_007a: ldloc 13
			IL_007e: ldc.i4 2
			IL_0083: ldarg.0
			IL_0084: ldfld string BackgroundResource::'<Root>k__BackingField'
			IL_0089: stelem [mscorlib]System.String
			IL_008e: ldstr "Method not found @325DFB3"
			IL_0093: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0098: br IL_009d
```
Context at line 2747:
```il
		IL_002f: ldc.i4.0
		IL_0030: ceq
		IL_0032: stloc 4
		IL_0036: ldloc 4
		IL_003a: ret
	} // end of method BackgroundResource::Finite

	.method public hidebysig static 
		string ResolvePath (
			string root,
			string relative
		) cil managed 
	{
		// Method begins at RVA 0x1642b4
		// Header size: 12
		// Code size: 1815 (0x717)
		.maxstack 0
		.locals init (
			[0] string,
			[1] string,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] bool,
			[13] bool,
			[14] object,
			[15] class [System]System.IO.InvalidDataException,
			[16] bool,
			[17] string,
			[18] native int,
			[19] native int,
			[20] bool,
			[21] char[],
			[22] string,
			[23] bool,
			[24] bool,
			[25] bool,
			[26] bool,
			[27] bool,
			[28] bool,
			[29] string,
			[30] object,
			[31] native int,
			[32] string,
			[33] char,
			[34] class [System]System.IO.InvalidDataException,
			[35] class [System]System.IO.InvalidDataException,
			[36] object,
			[37] class [System]System.IO.InvalidDataException,
			[38] string,
			[39] object,
			[40] bool,
			[41] bool,
			[42] bool,
			[43] class [mscorlib]System.Func`2<object, bool>,
			[44] object,
			[45] object,
			[46] object,
			[47] string,
			[48] valuetype [mscorlib]System.StringComparison,
			[49] bool,
			[50] string[],
			[51] bool,
			[52] object,
			[53] object,
			[54] bool,
			[55] bool,
			[56] object,
			[57] native int,
			[58] object,
			[59] object,
			[60] object,
			[61] native int,
			[62] class [mscorlib]System.Func`2<object, bool>,
			[63] native int,
			[64] object,
			[65] object,
			[66] object,
			[67] string,
			[68] native int,
			[69] bool,
			[70] object,
			[71] bool,
			[72] bool,
			[73] object,
			[74] string,
			[75] object,
			[76] object,
			[77] object,
			[78] object,
			[79] char,
			[80] string,
			[81] string,
			[82] string,
			[83] object,
			[84] object,
			[85] object,
			[86] bool,
			[87] object
		)

```
Context at line 3376:
```il
		IL_06f8: call bool [System.Core]System.Linq.Enumerable::Any<object>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, bool>)
		IL_06fd: stloc 42
		IL_0701: ldloc 43
		IL_0705: stloc 33
		IL_0709: ldloc 42
		IL_070d: brtrue IL_0528

		IL_0712: br IL_021d
	} // end of method BackgroundResource::ResolvePath

	// Properties
	.property instance uint32 Id()
	{
		.get instance uint32 BackgroundResource::get_Id()
	}
	.property instance string Name()
	{
		.get instance string BackgroundResource::get_Name()
	}
	.property instance valuetype ScenarioBGType Type()
	{
		.get instance valuetype ScenarioBGType BackgroundResource::get_Type()
	}
	.property instance valuetype ScenarioResourceManager/ResourceType Source()
	{
		.get instance valuetype ScenarioResourceManager/ResourceType BackgroundResource::get_Source()
	}
	.property instance string Root()
	{
		.get instance string BackgroundResource::get_Root()
	}
	.property instance string Path()
	{
		.get instance string BackgroundResource::get_Path()
	}
	.property instance class ResourceOverrideData/BgSpineOverrideData Spine()
	{
		.get instance class ResourceOverrideData/BgSpineOverrideData BackgroundResource::get_Spine()
	}
	.property instance bool IsCustomSpine()
	{
		.get instance bool BackgroundResource::get_IsCustomSpine()
	}
	.property instance string FullPath()
	{
		.get instance string BackgroundResource::get_FullPath()
	}
	.property instance string CacheKey()
	{
		.get instance string BackgroundResource::get_CacheKey()
	}

} // end of class BackgroundResource
```
#### Overrides: 94 occurrences
Context at line 249:
```il
			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private class BackgroundResource '<>2__current'
		.field private int32 '<>l__initialThreadId'
		.field private class ResourceOverrideData data
		.field public class ResourceOverrideData '<>3__data'
		.field private valuetype ScenarioResourceManager/ResourceType source
		.field public valuetype ScenarioResourceManager/ResourceType '<>3__source'
		.field private class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData> '<spines>5__2'
		.field private valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string> '<>7__wrap2'
		.field private valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData> '<>7__wrap3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x674dac
			// Header size: 12
			// Code size: 41 (0x29)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0015: stloc 2
			IL_0019: br IL_001e

			IL_001e: ldarg.0
			IL_001f: ldloc 2
			IL_0023: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0028: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x674de4
			// Header size: 12
			// Code size: 87 (0x57)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] class BackgroundResource/'<Overrides>d__29',
				[5] native int,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object,
				[12] object,
				[13] object,
				[14] object,
				[15] object,
				[16] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_0056

			IL_0028: br IL_002d

			IL_002d: ldc.i4 18477388
			IL_0032: stloc 2
			IL_0036: ldstr "Unmanaged memory load: [v16 @ rcx_v1+v2 @ rax_v2*4]"
			IL_003b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0040: ldc.i4.0
			IL_0041: conv.i
			IL_0042: ldc.i4 18477388
```
Context at line 281:
```il
			) cil managed 
		{
			// Method begins at RVA 0x674dac
			// Header size: 12
			// Code size: 41 (0x29)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0015: stloc 2
			IL_0019: br IL_001e

			IL_001e: ldarg.0
			IL_001f: ldloc 2
			IL_0023: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0028: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x674de4
			// Header size: 12
			// Code size: 87 (0x57)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] class BackgroundResource/'<Overrides>d__29',
				[5] native int,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object,
				[12] object,
				[13] object,
				[14] object,
				[15] object,
				[16] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_0056

			IL_0028: br IL_002d

			IL_002d: ldc.i4 18477388
			IL_0032: stloc 2
			IL_0036: ldstr "Unmanaged memory load: [v16 @ rcx_v1+v2 @ rax_v2*4]"
			IL_003b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0040: ldc.i4.0
			IL_0041: conv.i
			IL_0042: ldc.i4 18477388
			IL_0047: add
			IL_0048: stloc 3
			IL_004c: ldstr "Indirect jump: v18 @ rax_v4 (should have been resolved before IL gen)"
			IL_0051: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_0056: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x674e48
			// Header size: 12
			// Code size: 3120 (0xc30)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<Overrides>d__29',
				[1] native int,
				[2] object,
				[3] object,
				[4] object,
				[5] object,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] bool,
				[12] bool,
				[13] class [mscorlib]System.Collections.Generic.List`1<object>,
				[14] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
```
Context at line 289:
```il
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0015: stloc 2
			IL_0019: br IL_001e

			IL_001e: ldarg.0
			IL_001f: ldloc 2
			IL_0023: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0028: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x674de4
			// Header size: 12
			// Code size: 87 (0x57)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] class BackgroundResource/'<Overrides>d__29',
				[5] native int,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object,
				[12] object,
				[13] object,
				[14] object,
				[15] object,
				[16] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_0056

			IL_0028: br IL_002d

			IL_002d: ldc.i4 18477388
			IL_0032: stloc 2
			IL_0036: ldstr "Unmanaged memory load: [v16 @ rcx_v1+v2 @ rax_v2*4]"
			IL_003b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0040: ldc.i4.0
			IL_0041: conv.i
			IL_0042: ldc.i4 18477388
			IL_0047: add
			IL_0048: stloc 3
			IL_004c: ldstr "Indirect jump: v18 @ rax_v4 (should have been resolved before IL gen)"
			IL_0051: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_0056: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x674e48
			// Header size: 12
			// Code size: 3120 (0xc30)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<Overrides>d__29',
				[1] native int,
				[2] object,
				[3] object,
				[4] object,
				[5] object,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] bool,
				[12] bool,
				[13] class [mscorlib]System.Collections.Generic.List`1<object>,
				[14] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[15] object,
				[16] object,
				[17] bool,
				[18] class BackgroundResource/'<Overrides>d__29',
				[19] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[20] native int,
				[21] bool,
				[22] bool,
```
Context at line 296:
```il
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0015: stloc 2
			IL_0019: br IL_001e

			IL_001e: ldarg.0
			IL_001f: ldloc 2
			IL_0023: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0028: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x674de4
			// Header size: 12
			// Code size: 87 (0x57)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] class BackgroundResource/'<Overrides>d__29',
				[5] native int,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object,
				[12] object,
				[13] object,
				[14] object,
				[15] object,
				[16] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_0056

			IL_0028: br IL_002d

			IL_002d: ldc.i4 18477388
			IL_0032: stloc 2
			IL_0036: ldstr "Unmanaged memory load: [v16 @ rcx_v1+v2 @ rax_v2*4]"
			IL_003b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0040: ldc.i4.0
			IL_0041: conv.i
			IL_0042: ldc.i4 18477388
			IL_0047: add
			IL_0048: stloc 3
			IL_004c: ldstr "Indirect jump: v18 @ rax_v4 (should have been resolved before IL gen)"
			IL_0051: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_0056: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x674e48
			// Header size: 12
			// Code size: 3120 (0xc30)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<Overrides>d__29',
				[1] native int,
				[2] object,
				[3] object,
				[4] object,
				[5] object,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] bool,
				[12] bool,
				[13] class [mscorlib]System.Collections.Generic.List`1<object>,
				[14] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[15] object,
				[16] object,
				[17] bool,
				[18] class BackgroundResource/'<Overrides>d__29',
				[19] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[20] native int,
				[21] bool,
				[22] bool,
				[23] native int,
				[24] native int,
				[25] native int,
				[26] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[27] object,
				[28] class BackgroundResource/'<Overrides>d__29',
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
```
Context at line 298:
```il
			IL_0010: call int32 [mscorlib]System.Environment::get_CurrentManagedThreadId()
			IL_0015: stloc 2
			IL_0019: br IL_001e

			IL_001e: ldarg.0
			IL_001f: ldloc 2
			IL_0023: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0028: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x674de4
			// Header size: 12
			// Code size: 87 (0x57)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] class BackgroundResource/'<Overrides>d__29',
				[5] native int,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object,
				[12] object,
				[13] object,
				[14] object,
				[15] object,
				[16] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_0056

			IL_0028: br IL_002d

			IL_002d: ldc.i4 18477388
			IL_0032: stloc 2
			IL_0036: ldstr "Unmanaged memory load: [v16 @ rcx_v1+v2 @ rax_v2*4]"
			IL_003b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0040: ldc.i4.0
			IL_0041: conv.i
			IL_0042: ldc.i4 18477388
			IL_0047: add
			IL_0048: stloc 3
			IL_004c: ldstr "Indirect jump: v18 @ rax_v4 (should have been resolved before IL gen)"
			IL_0051: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_0056: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x674e48
			// Header size: 12
			// Code size: 3120 (0xc30)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<Overrides>d__29',
				[1] native int,
				[2] object,
				[3] object,
				[4] object,
				[5] object,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] bool,
				[12] bool,
				[13] class [mscorlib]System.Collections.Generic.List`1<object>,
				[14] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[15] object,
				[16] object,
				[17] bool,
				[18] class BackgroundResource/'<Overrides>d__29',
				[19] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[20] native int,
				[21] bool,
				[22] bool,
				[23] native int,
				[24] native int,
				[25] native int,
				[26] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[27] object,
				[28] class BackgroundResource/'<Overrides>d__29',
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[30] native int,
				[31] bool,
```
Context at line 313:
```il
			// Header size: 12
			// Code size: 87 (0x57)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] class BackgroundResource/'<Overrides>d__29',
				[5] native int,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object,
				[12] object,
				[13] object,
				[14] object,
				[15] object,
				[16] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_0056

			IL_0028: br IL_002d

			IL_002d: ldc.i4 18477388
			IL_0032: stloc 2
			IL_0036: ldstr "Unmanaged memory load: [v16 @ rcx_v1+v2 @ rax_v2*4]"
			IL_003b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0040: ldc.i4.0
			IL_0041: conv.i
			IL_0042: ldc.i4 18477388
			IL_0047: add
			IL_0048: stloc 3
			IL_004c: ldstr "Indirect jump: v18 @ rax_v4 (should have been resolved before IL gen)"
			IL_0051: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_0056: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x674e48
			// Header size: 12
			// Code size: 3120 (0xc30)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<Overrides>d__29',
				[1] native int,
				[2] object,
				[3] object,
				[4] object,
				[5] object,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] bool,
				[12] bool,
				[13] class [mscorlib]System.Collections.Generic.List`1<object>,
				[14] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[15] object,
				[16] object,
				[17] bool,
				[18] class BackgroundResource/'<Overrides>d__29',
				[19] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[20] native int,
				[21] bool,
				[22] bool,
				[23] native int,
				[24] native int,
				[25] native int,
				[26] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[27] object,
				[28] class BackgroundResource/'<Overrides>d__29',
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[30] native int,
				[31] bool,
				[32] bool,
				[33] class ResourceOverrideData,
				[34] bool,
				[35] bool,
				[36] bool,
				[37] class ResourceOverrideData/BgSpineOverrideData,
				[38] bool,
				[39] bool,
				[40] class BackgroundResource/'<Overrides>d__29',
				[41] native int,
				[42] native int,
				[43] native int,
				[44] class [mscorlib]System.NullReferenceException,
				[45] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[46] char,
```
Context at line 329:
```il
				[12] object,
				[13] object,
				[14] object,
				[15] object,
				[16] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_0056

			IL_0028: br IL_002d

			IL_002d: ldc.i4 18477388
			IL_0032: stloc 2
			IL_0036: ldstr "Unmanaged memory load: [v16 @ rcx_v1+v2 @ rax_v2*4]"
			IL_003b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0040: ldc.i4.0
			IL_0041: conv.i
			IL_0042: ldc.i4 18477388
			IL_0047: add
			IL_0048: stloc 3
			IL_004c: ldstr "Indirect jump: v18 @ rax_v4 (should have been resolved before IL gen)"
			IL_0051: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_0056: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x674e48
			// Header size: 12
			// Code size: 3120 (0xc30)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<Overrides>d__29',
				[1] native int,
				[2] object,
				[3] object,
				[4] object,
				[5] object,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] bool,
				[12] bool,
				[13] class [mscorlib]System.Collections.Generic.List`1<object>,
				[14] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[15] object,
				[16] object,
				[17] bool,
				[18] class BackgroundResource/'<Overrides>d__29',
				[19] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[20] native int,
				[21] bool,
				[22] bool,
				[23] native int,
				[24] native int,
				[25] native int,
				[26] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[27] object,
				[28] class BackgroundResource/'<Overrides>d__29',
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[30] native int,
				[31] bool,
				[32] bool,
				[33] class ResourceOverrideData,
				[34] bool,
				[35] bool,
				[36] bool,
				[37] class ResourceOverrideData/BgSpineOverrideData,
				[38] bool,
				[39] bool,
				[40] class BackgroundResource/'<Overrides>d__29',
				[41] native int,
				[42] native int,
				[43] native int,
				[44] class [mscorlib]System.NullReferenceException,
				[45] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[46] char,
				[47] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[48] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[49] object,
				[50] bool,
				[51] bool,
				[52] bool,
				[53] bool,
				[54] uint32,
				[55] char,
				[56] native int,
				[57] class [mscorlib]System.Collections.Generic.List`1<object>,
				[58] class [mscorlib]System.Collections.Generic.List`1<object>,
				[59] object,
				[60] string,
				[61] class ResourceOverrideData,
				[62] bool,
```
Context at line 355:
```il
			IL_0041: conv.i
			IL_0042: ldc.i4 18477388
			IL_0047: add
			IL_0048: stloc 3
			IL_004c: ldstr "Indirect jump: v18 @ rax_v4 (should have been resolved before IL gen)"
			IL_0051: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_0056: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x674e48
			// Header size: 12
			// Code size: 3120 (0xc30)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<Overrides>d__29',
				[1] native int,
				[2] object,
				[3] object,
				[4] object,
				[5] object,
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] bool,
				[12] bool,
				[13] class [mscorlib]System.Collections.Generic.List`1<object>,
				[14] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[15] object,
				[16] object,
				[17] bool,
				[18] class BackgroundResource/'<Overrides>d__29',
				[19] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[20] native int,
				[21] bool,
				[22] bool,
				[23] native int,
				[24] native int,
				[25] native int,
				[26] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[27] object,
				[28] class BackgroundResource/'<Overrides>d__29',
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[30] native int,
				[31] bool,
				[32] bool,
				[33] class ResourceOverrideData,
				[34] bool,
				[35] bool,
				[36] bool,
				[37] class ResourceOverrideData/BgSpineOverrideData,
				[38] bool,
				[39] bool,
				[40] class BackgroundResource/'<Overrides>d__29',
				[41] native int,
				[42] native int,
				[43] native int,
				[44] class [mscorlib]System.NullReferenceException,
				[45] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[46] char,
				[47] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[48] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[49] object,
				[50] bool,
				[51] bool,
				[52] bool,
				[53] bool,
				[54] uint32,
				[55] char,
				[56] native int,
				[57] class [mscorlib]System.Collections.Generic.List`1<object>,
				[58] class [mscorlib]System.Collections.Generic.List`1<object>,
				[59] object,
				[60] string,
				[61] class ResourceOverrideData,
				[62] bool,
				[63] native int,
				[64] class [System.Core]System.Collections.Generic.HashSet`1<uint32>,
				[65] native int,
				[66] object,
				[67] bool,
				[68] native int,
				[69] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[70] native int,
				[71] class BackgroundResource,
				[72] object,
				[73] string,
				[74] class ResourceOverrideData,
				[75] bool,
				[76] object,
				[77] bool,
				[78] uint32,
				[79] object,
				[80] native int,
				[81] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[82] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[83] native int,
				[84] class BackgroundResource,
				[85] class ResourceOverrideData,
				[86] bool,
				[87] object,
				[88] bool,
```
#### FullPath: 6 occurrences
Context at line 1886:
```il
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 1
		IL_001c: ldloc 1
		IL_0020: ret
	} // end of method BackgroundResource::get_IsCustomSpine

	.method public hidebysig specialname 
		instance string get_FullPath () cil managed 
	{
		// Method begins at RVA 0x1639fc
		// Header size: 12
		// Code size: 64 (0x40)
		.maxstack 0
		.locals init (
			[0] bool,
			[1] string,
			[2] class BackgroundResource
		)

		IL_0000: ldarg.0
		IL_0001: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0006: ldc.i4 0
		IL_000b: ceq
		IL_000d: stloc 0
		IL_0011: ldloc 0
		IL_0015: brtrue IL_0039

		IL_001a: br IL_001f

		IL_001f: ldarg.0
		IL_0020: ldfld string BackgroundResource::'<Root>k__BackingField'
		IL_0025: ldarg.0
		IL_0026: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_002b: call string BackgroundResource::ResolvePath(string, string)
		IL_0030: stloc 1
		IL_0034: ldloc 1
		IL_0038: ret

		IL_0039: ldarg.0
		IL_003a: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_003f: ret
	} // end of method BackgroundResource::get_FullPath

	.method public hidebysig specialname 
		instance string get_CacheKey () cil managed 
	{
		// Method begins at RVA 0x163a48
		// Header size: 12
		// Code size: 710 (0x2c6)
		.maxstack 0
		.locals init (
			[0] object,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] native int,
			[13] string[],
			[14] native int,
			[15] class [mscorlib]System.Enum,
			[16] string,
			[17] bool,
			[18] class [mscorlib]System.NullReferenceException,
			[19] object,
			[20] object,
			[21] string,
			[22] string,
			[23] object,
			[24] object,
			[25] uint32,
			[26] object,
			[27] object,
			[28] object,
			[29] string,
			[30] object,
			[31] object,
			[32] native int,
			[33] object,
			[34] bool,
			[35] object,
			[36] object,
			[37] object,
			[38] class BackgroundResource,
			[39] uint32
		)

		IL_0000: br IL_0259
		// loop start (head: IL_0259)
			IL_0005: ldloc 13
			IL_0009: ldc.i4 32
			IL_000e: add
			IL_000f: stloc 19
			IL_0013: ldloc 13
			IL_0017: ldc.i4 0
			IL_001c: ldloc 16
			IL_0020: stelem [mscorlib]System.String
			IL_0025: ldstr "Method not found @325DFB3"
			IL_002a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_002f: br IL_0034

```
Context at line 1920:
```il
		IL_002b: call string BackgroundResource::ResolvePath(string, string)
		IL_0030: stloc 1
		IL_0034: ldloc 1
		IL_0038: ret

		IL_0039: ldarg.0
		IL_003a: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_003f: ret
	} // end of method BackgroundResource::get_FullPath

	.method public hidebysig specialname 
		instance string get_CacheKey () cil managed 
	{
		// Method begins at RVA 0x163a48
		// Header size: 12
		// Code size: 710 (0x2c6)
		.maxstack 0
		.locals init (
			[0] object,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] native int,
			[13] string[],
			[14] native int,
			[15] class [mscorlib]System.Enum,
			[16] string,
			[17] bool,
			[18] class [mscorlib]System.NullReferenceException,
			[19] object,
			[20] object,
			[21] string,
			[22] string,
			[23] object,
			[24] object,
			[25] uint32,
			[26] object,
			[27] object,
			[28] object,
			[29] string,
			[30] object,
			[31] object,
			[32] native int,
			[33] object,
			[34] bool,
			[35] object,
			[36] object,
			[37] object,
			[38] class BackgroundResource,
			[39] uint32
		)

		IL_0000: br IL_0259
		// loop start (head: IL_0259)
			IL_0005: ldloc 13
			IL_0009: ldc.i4 32
			IL_000e: add
			IL_000f: stloc 19
			IL_0013: ldloc 13
			IL_0017: ldc.i4 0
			IL_001c: ldloc 16
			IL_0020: stelem [mscorlib]System.String
			IL_0025: ldstr "Method not found @325DFB3"
			IL_002a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_002f: br IL_0034

			IL_0034: ldstr "\n"
			IL_0039: stloc 20
			IL_003d: ldloc 13
			IL_0041: ldc.i4 40
			IL_0046: add
			IL_0047: stloc 23
			IL_004b: ldloc 13
			IL_004f: ldc.i4 1
			IL_0054: ldloc 20
			IL_0058: stelem [mscorlib]System.String
			IL_005d: ldstr "Method not found @325DFB3"
			IL_0062: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0067: br IL_006c

			IL_006c: ldloc 13
			IL_0070: ldc.i4 48
			IL_0075: add
			IL_0076: stloc 24
			IL_007a: ldloc 13
			IL_007e: ldc.i4 2
			IL_0083: ldarg.0
			IL_0084: ldfld string BackgroundResource::'<Root>k__BackingField'
			IL_0089: stelem [mscorlib]System.String
			IL_008e: ldstr "Method not found @325DFB3"
			IL_0093: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0098: br IL_009d

			IL_009d: ldloc 13
			IL_00a1: ldc.i4 56
			IL_00a6: add
			IL_00a7: stloc 30
			IL_00ab: ldloc 13
			IL_00af: ldc.i4 3
			IL_00b4: ldloc 20
```
Context at line 3021:
```il

		IL_0248: br IL_024d

		IL_024d: ldstr "Method not found @325E158"
		IL_0252: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0257: br IL_025c

		IL_025c: ldloc 29
		IL_0260: call string [mscorlib]System.IO.Path::GetFullPath(string)
		IL_0265: stloc 67
		IL_0269: ldtoken char[]
		IL_026e: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0273: stloc 68
		IL_0277: ldstr "Method not found @325E0D3"
		IL_027c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0281: br IL_0286

		IL_0286: ldloc 21
		IL_028a: ldc.i4 0
		IL_028f: ceq
		IL_0291: stloc 26
		IL_0295: ldc.i4 2
		IL_029a: stloc 33
		IL_029e: ldloc 26
		IL_02a2: brtrue IL_051d

		IL_02a7: br IL_02ac

		IL_02ac: ldloc 21
		IL_02b0: ldlen
		IL_02b1: conv.i4
		IL_02b2: stloc 30
		IL_02b6: ldloc 21
		IL_02ba: ldlen
		IL_02bb: conv.i4
		IL_02bc: ldc.i4 0
		IL_02c1: ceq
		IL_02c3: stloc 69
		IL_02c7: ldc.i4 2
		IL_02cc: stloc 32
		IL_02d0: ldloc 69
		IL_02d4: brtrue IL_058e

		IL_02d9: br IL_02de

		IL_02de: ldloc 19
		IL_02e2: stloc 70
		IL_02e6: ldstr "Unmanaged memory load: [v497 @ rdx_v12+B8]"
		IL_02eb: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_02f0: ldc.i4.0
		IL_02f1: conv.i
		IL_02f2: stloc 31
		IL_02f6: ldstr "Unmanaged memory load: [v222 @ rdx_v10 (Il2CppMethodInfo)+A]"
		IL_02fb: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0300: ldc.i4.0
		IL_0301: conv.i
		IL_0302: stloc 33
		IL_0306: ldloc 21
		IL_030a: ldc.i4 0
		IL_030f: ldstr "Unmanaged memory load: [v222 @ rdx_v10 (Il2CppMethodInfo)+A]"
		IL_0314: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0319: ldc.i4.0
		IL_031a: conv.i
		IL_031b: stelem [mscorlib]System.Char
		IL_0320: ldloc 21
		IL_0324: ldlen
		IL_0325: conv.i4
		IL_0326: ldc.i4 1
		IL_032b: ceq
		IL_032d: stloc 71
		IL_0331: ldstr "Unmanaged memory load: [v222 @ rdx_v10 (Il2CppMethodInfo)+A]"
		IL_0336: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_033b: ldc.i4.0
		IL_033c: conv.i
		IL_033d: stloc 32
		IL_0341: ldloc 71
		IL_0345: brtrue IL_058e

		IL_034a: br IL_034f

		IL_034f: ldstr "Unmanaged memory load: [v222 @ rdx_v10 (Il2CppMethodInfo)+8]"
		IL_0354: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0359: ldc.i4.0
		IL_035a: conv.i
		IL_035b: stloc 30
		IL_035f: ldloc 21
		IL_0363: ldc.i4 1
		IL_0368: ldstr "Unmanaged memory load: [v222 @ rdx_v10 (Il2CppMethodInfo)+8]"
		IL_036d: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0372: ldc.i4.0
		IL_0373: conv.i
		IL_0374: stelem [mscorlib]System.Char
		IL_0379: ldloc 67
		IL_037d: ldc.i4 0
		IL_0382: ceq
		IL_0384: stloc 27
		IL_0388: ldloc 27
		IL_038c: brtrue IL_051d

		IL_0391: br IL_0396

		IL_0396: ldloc 67
		IL_039a: ldloc 21
		IL_039e: call instance string [mscorlib]System.String::TrimEnd(char[])
		IL_03a3: stloc 74
		IL_03a7: ldc.i4 123410272
		IL_03ac: stloc 75
		IL_03b0: ldstr "Unmanaged memory load: [v533 @ rax_v36+88]"
```
Context at line 3166:
```il
		IL_042f: stloc 81
		IL_0433: br IL_0438

		IL_0438: ldloc 81
		IL_043c: ldloc 17
		IL_0440: call string [mscorlib]System.IO.Path::Combine(string, string)
		IL_0445: stloc 82
		IL_0449: ldloc 82
		IL_044d: call string [mscorlib]System.IO.Path::GetFullPath(string)
		IL_0452: stloc 22
		IL_0456: br IL_045b

		IL_045b: ldloc 22
		IL_045f: ldc.i4 0
		IL_0464: ceq
		IL_0466: stloc 28
		IL_046a: ldnull
		IL_046b: stloc 31
		IL_046f: ldc.i4 0
		IL_0474: stloc 33
		IL_0478: ldloc 28
		IL_047c: brtrue IL_051d

		IL_0481: br IL_0486

		IL_0486: ldloc 19
		IL_048a: stloc 83
		IL_048e: ldstr "Unmanaged memory load: [v559 @ rax_v43+B8]"
		IL_0493: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0498: ldc.i4.0
		IL_0499: conv.i
		IL_049a: stloc 84
		IL_049e: ldstr "Unmanaged memory load: [v560 @ rax_v44+A]"
		IL_04a3: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_04a8: ldc.i4.0
		IL_04a9: conv.i
		IL_04aa: ldc.i4 92
		IL_04af: sub
		IL_04b0: stloc 85
		IL_04b4: ldloc 85
		IL_04b8: ldc.i4 0
		IL_04bd: ceq
		IL_04bf: stloc 86
		IL_04c3: ldloc 86
		IL_04c7: ldc.i4 4
		IL_04cc: or
		IL_04cd: stloc 48
		IL_04d1: ldloc 22
		IL_04d5: ldloc 81
		IL_04d9: ldloc 48
		IL_04dd: call instance bool [mscorlib]System.String::StartsWith(string, valuetype [mscorlib]System.StringComparison)
		IL_04e2: stloc 72
		IL_04e6: ldloc 72
		IL_04ea: ldc.i4.0
		IL_04eb: ceq
		IL_04ed: stloc 49
		IL_04f1: ldc.i4 0
		IL_04f6: stloc 30
		IL_04fa: ldloc 48
		IL_04fe: stloc 31
		IL_0502: ldloc 81
		IL_0506: stloc 32
		IL_050a: ldloc 49
		IL_050e: brtrue IL_0599

		IL_0513: br IL_0518

		IL_0518: ldloc 22
		IL_051c: ret

		IL_051d: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
		IL_0522: throw

		IL_0523: br IL_0528

		IL_0528: ldstr "Method not found @325E011"
		IL_052d: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0532: ldstr "Method not found @325E234"
		IL_0537: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_053c: br IL_0541

		IL_0541: ldloc 33
		IL_0545: stloc 32
		IL_0549: ldstr "Background paths must be relative to the resource directory."
		IL_054e: stloc 36
		IL_0552: ldloc 35
		IL_0556: stloc 37
		IL_055a: br IL_06bc

		IL_055f: ldstr "Method not found @325E011"
		IL_0564: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0569: ldstr "Method not found @325E234"
		IL_056e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0573: br IL_0578

		IL_0578: ldstr "A background resource root and relative path are required."
		IL_057d: stloc 36
		IL_0581: ldloc 15
		IL_0585: stloc 37
		IL_0589: br IL_06bc

		IL_058e: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_0593: throw

		IL_0594: br IL_0599

		IL_0599: ldstr "Method not found @325E011"
		IL_059e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
```
Context at line 3411:
```il
	.property instance class ResourceOverrideData/BgSpineOverrideData Spine()
	{
		.get instance class ResourceOverrideData/BgSpineOverrideData BackgroundResource::get_Spine()
	}
	.property instance bool IsCustomSpine()
	{
		.get instance bool BackgroundResource::get_IsCustomSpine()
	}
	.property instance string FullPath()
	{
		.get instance string BackgroundResource::get_FullPath()
	}
	.property instance string CacheKey()
	{
		.get instance string BackgroundResource::get_CacheKey()
	}

} // end of class BackgroundResource
```
Context at line 3413:
```il
		.get instance class ResourceOverrideData/BgSpineOverrideData BackgroundResource::get_Spine()
	}
	.property instance bool IsCustomSpine()
	{
		.get instance bool BackgroundResource::get_IsCustomSpine()
	}
	.property instance string FullPath()
	{
		.get instance string BackgroundResource::get_FullPath()
	}
	.property instance string CacheKey()
	{
		.get instance string BackgroundResource::get_CacheKey()
	}

} // end of class BackgroundResource
```

### windows (SHA256 f05914a3beba821fec2b1b9a5bae6c8d512c5ff6b7c03eece13c7549933fc0ce)
#### ResolveOverride: 8 occurrences
Context at line 122:
```il
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass28_0'::.ctor

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__0' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x4959cc
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__0'

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__1' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x495a2c
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
```
Context at line 165:
```il
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__0'

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__1' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x495a2c
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private class BackgroundResource '<>2__current'
		.field private int32 '<>l__initialThreadId'
		.field private class ResourceOverrideData data
		.field public class ResourceOverrideData '<>3__data'
		.field private valuetype ScenarioResourceManager/ResourceType source
		.field public valuetype ScenarioResourceManager/ResourceType '<>3__source'
		.field private class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData> '<spines>5__2'
		.field private valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string> '<>7__wrap2'
		.field private valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData> '<>7__wrap3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x495a8c
			// Header size: 12
			// Code size: 42 (0x2a)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: ldstr "Method not found @1828217F0"
			IL_0015: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001a: br IL_001f

			IL_001f: ldarg.0
			IL_0020: ldloc 2
			IL_0024: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0029: ret
		} // end of method '<Overrides>d__29'::.ctor
```
Context at line 168:
```il

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__0'

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__1' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x495a2c
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private class BackgroundResource '<>2__current'
		.field private int32 '<>l__initialThreadId'
		.field private class ResourceOverrideData data
		.field public class ResourceOverrideData '<>3__data'
		.field private valuetype ScenarioResourceManager/ResourceType source
		.field public valuetype ScenarioResourceManager/ResourceType '<>3__source'
		.field private class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData> '<spines>5__2'
		.field private valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string> '<>7__wrap2'
		.field private valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData> '<>7__wrap3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x495a8c
			// Header size: 12
			// Code size: 42 (0x2a)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: ldstr "Method not found @1828217F0"
			IL_0015: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001a: br IL_001f

			IL_001f: ldarg.0
			IL_0020: ldloc 2
			IL_0024: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0029: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
```
Context at line 211:
```il
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private class BackgroundResource '<>2__current'
		.field private int32 '<>l__initialThreadId'
		.field private class ResourceOverrideData data
		.field public class ResourceOverrideData '<>3__data'
		.field private valuetype ScenarioResourceManager/ResourceType source
		.field public valuetype ScenarioResourceManager/ResourceType '<>3__source'
		.field private class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData> '<spines>5__2'
		.field private valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string> '<>7__wrap2'
		.field private valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData> '<>7__wrap3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x495a8c
			// Header size: 12
			// Code size: 42 (0x2a)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: ldstr "Method not found @1828217F0"
			IL_0015: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001a: br IL_001f

			IL_001f: ldarg.0
			IL_0020: ldloc 2
			IL_0024: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0029: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x495ac4
			// Header size: 12
			// Code size: 111 (0x6f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] object,
				[5] class BackgroundResource/'<Overrides>d__29',
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
```
Context at line 1946:
```il

		IL_004c: ldarg.0
		IL_004d: ldloc 7
		IL_0051: stfld class ResourceOverrideData/BgSpineOverrideData BackgroundResource::'<Spine>k__BackingField'
		IL_0056: ret
	} // end of method BackgroundResource::.ctor

	.method public hidebysig static 
		class BackgroundResource ResolveOverride (
			uint32 id,
			class ResourceOverrideData local,
			class ResourceOverrideData global
		) cil managed 
	{
		// Method begins at RVA 0xef580
		// Header size: 12
		// Code size: 254 (0xfe)
		.maxstack 0
		.locals init (
			[0] class ResourceOverrideData,
			[1] class ResourceOverrideData,
			[2] uint32,
			[3] class BackgroundResource/'<>c__DisplayClass28_0',
			[4] bool,
			[5] class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
			[6] class [mscorlib]System.NullReferenceException,
			[7] class [mscorlib]System.Func`2<class BackgroundResource, bool>,
			[8] class BackgroundResource,
			[9] bool,
			[10] object,
			[11] class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
			[12] object,
			[13] class [mscorlib]System.Func`2<class BackgroundResource, bool>
		)

		IL_0000: br IL_00d7
		// loop start (head: IL_00d7)
			IL_0005: ldloc 3
			IL_0009: ldarg id
			IL_000d: stfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_0012: ldarg local
			IL_0016: ldc.i4 2
			IL_001b: call class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> BackgroundResource::Overrides(class ResourceOverrideData, valuetype ScenarioResourceManager/ResourceType)
			IL_0020: stloc 5
			IL_0024: ldloc 3
			IL_0028: ldftn instance bool BackgroundResource/'<>c__DisplayClass28_0'::'<ResolveOverride>b__0'(class BackgroundResource)
			IL_002e: newobj instance void class [mscorlib]System.Func`2<class BackgroundResource, bool>::.ctor(object, native int)
			IL_0033: stloc 7
			IL_0037: br IL_003c

			IL_003c: nop
			IL_003d: ldloc 5
			IL_0041: ldloc 7
			IL_0045: call !!0 [System.Core]System.Linq.Enumerable::FirstOrDefault<object>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, bool>)
			IL_004a: stloc 10
			IL_004e: br IL_0053

			IL_0053: ldloc 10
			IL_0057: ldc.i4 0
			IL_005c: ceq
			IL_005e: ldc.i4.0
			IL_005f: ceq
			IL_0061: stloc 9
			IL_0065: ldloc 10
			IL_0069: stloc 8
			IL_006d: ldloc 9
			IL_0071: brtrue IL_00c4

			IL_0076: br IL_007b

			IL_007b: ldarg global
			IL_007f: ldc.i4 1
			IL_0084: call class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> BackgroundResource::Overrides(class ResourceOverrideData, valuetype ScenarioResourceManager/ResourceType)
			IL_0089: stloc 11
			IL_008d: ldloc 3
			IL_0091: ldftn instance bool BackgroundResource/'<>c__DisplayClass28_0'::'<ResolveOverride>b__1'(class BackgroundResource)
			IL_0097: newobj instance void class [mscorlib]System.Func`2<class BackgroundResource, bool>::.ctor(object, native int)
			IL_009c: stloc 13
			IL_00a0: br IL_00a5

			IL_00a5: nop
			IL_00a6: ldloc 11
			IL_00aa: ldloc 13
			IL_00ae: call !!0 [System.Core]System.Linq.Enumerable::FirstOrDefault<object>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, bool>)
			IL_00b3: stloc 12
			IL_00b7: ldloc 12
			IL_00bb: stloc 8
			IL_00bf: br IL_00c4

			IL_00c4: ldloc 8
			IL_00c8: ret
			// loop start (head: IL_00d7)
				IL_00c9: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
				IL_00ce: stloc 6
				IL_00d2: ldloc 6
				IL_00d6: ret

				IL_00d7: newobj instance void BackgroundResource/'<>c__DisplayClass28_0'::.ctor()
				IL_00dc: stloc 3
				IL_00e0: nop
				IL_00e1: ldloc 3
				IL_00e5: ldc.i4 0
				IL_00ea: ceq
				IL_00ec: stloc 4
				IL_00f0: ldloc 4
				IL_00f4: brtrue IL_00c9
			// end loop

```
Context at line 1983:
```il
			IL_0005: ldloc 3
			IL_0009: ldarg id
			IL_000d: stfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_0012: ldarg local
			IL_0016: ldc.i4 2
			IL_001b: call class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> BackgroundResource::Overrides(class ResourceOverrideData, valuetype ScenarioResourceManager/ResourceType)
			IL_0020: stloc 5
			IL_0024: ldloc 3
			IL_0028: ldftn instance bool BackgroundResource/'<>c__DisplayClass28_0'::'<ResolveOverride>b__0'(class BackgroundResource)
			IL_002e: newobj instance void class [mscorlib]System.Func`2<class BackgroundResource, bool>::.ctor(object, native int)
			IL_0033: stloc 7
			IL_0037: br IL_003c

			IL_003c: nop
			IL_003d: ldloc 5
			IL_0041: ldloc 7
			IL_0045: call !!0 [System.Core]System.Linq.Enumerable::FirstOrDefault<object>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, bool>)
			IL_004a: stloc 10
			IL_004e: br IL_0053

			IL_0053: ldloc 10
			IL_0057: ldc.i4 0
			IL_005c: ceq
			IL_005e: ldc.i4.0
			IL_005f: ceq
			IL_0061: stloc 9
			IL_0065: ldloc 10
			IL_0069: stloc 8
			IL_006d: ldloc 9
			IL_0071: brtrue IL_00c4

			IL_0076: br IL_007b

			IL_007b: ldarg global
			IL_007f: ldc.i4 1
			IL_0084: call class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> BackgroundResource::Overrides(class ResourceOverrideData, valuetype ScenarioResourceManager/ResourceType)
			IL_0089: stloc 11
			IL_008d: ldloc 3
			IL_0091: ldftn instance bool BackgroundResource/'<>c__DisplayClass28_0'::'<ResolveOverride>b__1'(class BackgroundResource)
			IL_0097: newobj instance void class [mscorlib]System.Func`2<class BackgroundResource, bool>::.ctor(object, native int)
			IL_009c: stloc 13
			IL_00a0: br IL_00a5

			IL_00a5: nop
			IL_00a6: ldloc 11
			IL_00aa: ldloc 13
			IL_00ae: call !!0 [System.Core]System.Linq.Enumerable::FirstOrDefault<object>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, bool>)
			IL_00b3: stloc 12
			IL_00b7: ldloc 12
			IL_00bb: stloc 8
			IL_00bf: br IL_00c4

			IL_00c4: ldloc 8
			IL_00c8: ret
			// loop start (head: IL_00d7)
				IL_00c9: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
				IL_00ce: stloc 6
				IL_00d2: ldloc 6
				IL_00d6: ret

				IL_00d7: newobj instance void BackgroundResource/'<>c__DisplayClass28_0'::.ctor()
				IL_00dc: stloc 3
				IL_00e0: nop
				IL_00e1: ldloc 3
				IL_00e5: ldc.i4 0
				IL_00ea: ceq
				IL_00ec: stloc 4
				IL_00f0: ldloc 4
				IL_00f4: brtrue IL_00c9
			// end loop

			IL_00f9: br IL_0005
		// end loop
	} // end of method BackgroundResource::ResolveOverride

	.method public hidebysig static 
		class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> Overrides (
			class ResourceOverrideData data,
			valuetype ScenarioResourceManager/ResourceType source
		) cil managed 
	{
		// Method begins at RVA 0xef68c
		// Header size: 12
		// Code size: 97 (0x61)
		.maxstack 0
		.locals init (
			[0] valuetype ScenarioResourceManager/ResourceType,
			[1] class ResourceOverrideData,
			[2] class BackgroundResource/'<Overrides>d__29',
			[3] int32
		)

		IL_0000: br IL_0031
		// loop start (head: IL_0031)
			IL_0005: ldloc 2
			IL_0009: ldloc 3
			IL_000d: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0012: ldloc 2
			IL_0016: ldarg data
			IL_001a: stfld class ResourceOverrideData BackgroundResource/'<Overrides>d__29'::'<>3__data'
			IL_001f: ldloc 2
			IL_0023: ldarg source
			IL_0027: stfld valuetype ScenarioResourceManager/ResourceType BackgroundResource/'<Overrides>d__29'::'<>3__source'
			IL_002c: ldloc 2
			IL_0030: ret

			IL_0031: ldc.i4 0
			IL_0036: newobj instance void BackgroundResource/'<Overrides>d__29'::.ctor(int32)
```
Context at line 2013:
```il

			IL_0076: br IL_007b

			IL_007b: ldarg global
			IL_007f: ldc.i4 1
			IL_0084: call class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> BackgroundResource::Overrides(class ResourceOverrideData, valuetype ScenarioResourceManager/ResourceType)
			IL_0089: stloc 11
			IL_008d: ldloc 3
			IL_0091: ldftn instance bool BackgroundResource/'<>c__DisplayClass28_0'::'<ResolveOverride>b__1'(class BackgroundResource)
			IL_0097: newobj instance void class [mscorlib]System.Func`2<class BackgroundResource, bool>::.ctor(object, native int)
			IL_009c: stloc 13
			IL_00a0: br IL_00a5

			IL_00a5: nop
			IL_00a6: ldloc 11
			IL_00aa: ldloc 13
			IL_00ae: call !!0 [System.Core]System.Linq.Enumerable::FirstOrDefault<object>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, bool>)
			IL_00b3: stloc 12
			IL_00b7: ldloc 12
			IL_00bb: stloc 8
			IL_00bf: br IL_00c4

			IL_00c4: ldloc 8
			IL_00c8: ret
			// loop start (head: IL_00d7)
				IL_00c9: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
				IL_00ce: stloc 6
				IL_00d2: ldloc 6
				IL_00d6: ret

				IL_00d7: newobj instance void BackgroundResource/'<>c__DisplayClass28_0'::.ctor()
				IL_00dc: stloc 3
				IL_00e0: nop
				IL_00e1: ldloc 3
				IL_00e5: ldc.i4 0
				IL_00ea: ceq
				IL_00ec: stloc 4
				IL_00f0: ldloc 4
				IL_00f4: brtrue IL_00c9
			// end loop

			IL_00f9: br IL_0005
		// end loop
	} // end of method BackgroundResource::ResolveOverride

	.method public hidebysig static 
		class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> Overrides (
			class ResourceOverrideData data,
			valuetype ScenarioResourceManager/ResourceType source
		) cil managed 
	{
		// Method begins at RVA 0xef68c
		// Header size: 12
		// Code size: 97 (0x61)
		.maxstack 0
		.locals init (
			[0] valuetype ScenarioResourceManager/ResourceType,
			[1] class ResourceOverrideData,
			[2] class BackgroundResource/'<Overrides>d__29',
			[3] int32
		)

		IL_0000: br IL_0031
		// loop start (head: IL_0031)
			IL_0005: ldloc 2
			IL_0009: ldloc 3
			IL_000d: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0012: ldloc 2
			IL_0016: ldarg data
			IL_001a: stfld class ResourceOverrideData BackgroundResource/'<Overrides>d__29'::'<>3__data'
			IL_001f: ldloc 2
			IL_0023: ldarg source
			IL_0027: stfld valuetype ScenarioResourceManager/ResourceType BackgroundResource/'<Overrides>d__29'::'<>3__source'
			IL_002c: ldloc 2
			IL_0030: ret

			IL_0031: ldc.i4 0
			IL_0036: newobj instance void BackgroundResource/'<Overrides>d__29'::.ctor(int32)
			IL_003b: stloc 2
			IL_003f: nop
			IL_0040: ldloc 2
			IL_0044: ldc.i8 4294967294
			IL_004d: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0052: ldstr "Method not found @1828217F0"
			IL_0057: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_005c: br IL_0005
		// end loop
	} // end of method BackgroundResource::Overrides

	.method public hidebysig static 
		void Validate (
			class ResourceOverrideData/BgSpineOverrideData spine
		) cil managed 
	{
		// Method begins at RVA 0xef6fc
		// Header size: 12
		// Code size: 1039 (0x40f)
		.maxstack 0
		.locals init (
			[0] class ResourceOverrideData/BgSpineOverrideData,
			[1] bool,
			[2] bool,
			[3] float32,
			[4] bool,
			[5] native int,
			[6] object,
			[7] object,
			[8] object,
```
Context at line 2048:
```il
				IL_00ea: ceq
				IL_00ec: stloc 4
				IL_00f0: ldloc 4
				IL_00f4: brtrue IL_00c9
			// end loop

			IL_00f9: br IL_0005
		// end loop
	} // end of method BackgroundResource::ResolveOverride

	.method public hidebysig static 
		class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> Overrides (
			class ResourceOverrideData data,
			valuetype ScenarioResourceManager/ResourceType source
		) cil managed 
	{
		// Method begins at RVA 0xef68c
		// Header size: 12
		// Code size: 97 (0x61)
		.maxstack 0
		.locals init (
			[0] valuetype ScenarioResourceManager/ResourceType,
			[1] class ResourceOverrideData,
			[2] class BackgroundResource/'<Overrides>d__29',
			[3] int32
		)

		IL_0000: br IL_0031
		// loop start (head: IL_0031)
			IL_0005: ldloc 2
			IL_0009: ldloc 3
			IL_000d: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0012: ldloc 2
			IL_0016: ldarg data
			IL_001a: stfld class ResourceOverrideData BackgroundResource/'<Overrides>d__29'::'<>3__data'
			IL_001f: ldloc 2
			IL_0023: ldarg source
			IL_0027: stfld valuetype ScenarioResourceManager/ResourceType BackgroundResource/'<Overrides>d__29'::'<>3__source'
			IL_002c: ldloc 2
			IL_0030: ret

			IL_0031: ldc.i4 0
			IL_0036: newobj instance void BackgroundResource/'<Overrides>d__29'::.ctor(int32)
			IL_003b: stloc 2
			IL_003f: nop
			IL_0040: ldloc 2
			IL_0044: ldc.i8 4294967294
			IL_004d: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0052: ldstr "Method not found @1828217F0"
			IL_0057: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_005c: br IL_0005
		// end loop
	} // end of method BackgroundResource::Overrides

	.method public hidebysig static 
		void Validate (
			class ResourceOverrideData/BgSpineOverrideData spine
		) cil managed 
	{
		// Method begins at RVA 0xef6fc
		// Header size: 12
		// Code size: 1039 (0x40f)
		.maxstack 0
		.locals init (
			[0] class ResourceOverrideData/BgSpineOverrideData,
			[1] bool,
			[2] bool,
			[3] float32,
			[4] bool,
			[5] native int,
			[6] object,
			[7] object,
			[8] object,
			[9] class [System]System.IO.InvalidDataException,
			[10] uint32,
			[11] object,
			[12] bool,
			[13] object,
			[14] bool,
			[15] object,
			[16] float32,
			[17] object,
			[18] object,
			[19] object,
			[20] bool,
			[21] bool,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] bool,
			[26] object,
			[27] object,
			[28] object,
			[29] object,
			[30] object,
			[31] object,
			[32] bool,
			[33] bool,
			[34] bool,
			[35] class [System]System.IO.InvalidDataException,
			[36] string,
			[37] native int,
			[38] string,
			[39] object,
			[40] class [System]System.IO.InvalidDataException,
			[41] object
		)

```
#### ResolvePath: 6 occurrences
Context at line 56:
```il
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c'::.ctor

		.method assembly hidebysig 
			instance bool '<ResolvePath>b__32_0' (
				string x
			) cil managed 
		{
			// Method begins at RVA 0x495960
			// Header size: 12
			// Code size: 74 (0x4a)
			.maxstack 0
			.locals init (
				[0] string,
				[1] bool,
				[2] bool
			)

			IL_0000: ldstr "Unmanaged memory load: [18498CAD7]"
			IL_0005: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_000a: ldc.i4.0
			IL_000b: conv.i
			IL_000c: ldc.i4 0
			IL_0011: ceq
			IL_0013: ldc.i4.0
			IL_0014: ceq
			IL_0016: stloc 1
			IL_001a: ldloc 1
			IL_001e: brtrue IL_0033

			IL_0023: br IL_0028

			IL_0028: ldc.i4 1
			IL_002d: pop
			IL_002e: br IL_0033

			IL_0033: ldarg x
			IL_0037: ldstr ".."
			IL_003c: call bool [mscorlib]System.String::op_Equality(string, string)
			IL_0041: stloc 2
			IL_0045: ldloc 2
			IL_0049: ret
		} // end of method '<>c'::'<ResolvePath>b__32_0'

	} // end of class <>c

	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass28_0'
		extends [mscorlib]System.Object
	{
		// Fields
		.field public uint32 id

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x4959b8
			// Header size: 12
			// Code size: 7 (0x7)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass28_0'::.ctor

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__0' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x4959cc
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
```
Context at line 94:
```il
			IL_002e: br IL_0033

			IL_0033: ldarg x
			IL_0037: ldstr ".."
			IL_003c: call bool [mscorlib]System.String::op_Equality(string, string)
			IL_0041: stloc 2
			IL_0045: ldloc 2
			IL_0049: ret
		} // end of method '<>c'::'<ResolvePath>b__32_0'

	} // end of class <>c

	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass28_0'
		extends [mscorlib]System.Object
	{
		// Fields
		.field public uint32 id

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x4959b8
			// Header size: 12
			// Code size: 7 (0x7)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass28_0'::.ctor

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__0' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x4959cc
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

			IL_001d: ldarg x
			IL_0021: ldfld uint32 BackgroundResource::'<Id>k__BackingField'
			IL_0026: ldarg.0
			IL_0027: ldfld uint32 BackgroundResource/'<>c__DisplayClass28_0'::id
			IL_002c: sub
			IL_002d: stloc 2
			IL_0031: ldloc 2
			IL_0035: ldc.i4 0
			IL_003a: ceq
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ret

			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__0'

		.method assembly hidebysig 
			instance bool '<ResolveOverride>b__1' (
				class BackgroundResource x
			) cil managed 
		{
			// Method begins at RVA 0x495a2c
			// Header size: 12
			// Code size: 83 (0x53)
			.maxstack 0
			.locals init (
				[0] class BackgroundResource,
				[1] bool,
				[2] object,
				[3] bool,
				[4] class [mscorlib]System.NullReferenceException,
				[5] class BackgroundResource/'<>c__DisplayClass28_0'
			)

			IL_0000: ldarg x
			IL_0004: ldc.i4 0
			IL_0009: ceq
			IL_000b: stloc 1
			IL_000f: ldloc 1
			IL_0013: brtrue IL_0045

			IL_0018: br IL_001d

```
Context at line 1662:
```il
		IL_0022: ldarg.0
		IL_0023: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_0028: ret

		IL_0029: ldarg.0
		IL_002a: ldfld string BackgroundResource::'<Root>k__BackingField'
		IL_002f: ldarg.0
		IL_0030: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_0035: call string BackgroundResource::ResolvePath(string, string)
		IL_003a: stloc 1
		IL_003e: ldloc 1
		IL_0042: ret
	} // end of method BackgroundResource::get_FullPath

	.method public hidebysig specialname 
		instance string get_CacheKey () cil managed 
	{
		// Method begins at RVA 0xef2c4
		// Header size: 12
		// Code size: 588 (0x24c)
		.maxstack 0
		.locals init (
			[0] string[],
			[1] string,
			[2] bool,
			[3] string,
			[4] string,
			[5] bool,
			[6] bool,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] bool,
			[13] class [mscorlib]System.IndexOutOfRangeException,
			[14] string,
			[15] native int,
			[16] class BackgroundResource,
			[17] uint32
		)

		IL_0000: br IL_021e
		// loop start (head: IL_021e)
			IL_0005: ldloc 0
			IL_0009: ldlen
			IL_000a: conv.i4
			IL_000b: ldc.i4 0
			IL_0010: cgt
			IL_0012: ldc.i4.0
			IL_0013: ceq
			IL_0015: stloc 2
			IL_0019: ldloc 2
			IL_001d: brtrue IL_023e

			IL_0022: br IL_0027

			IL_0027: ldloc 0
			IL_002b: ldc.i4 0
			IL_0030: ldloc 1
			IL_0034: stelem [mscorlib]System.String
			IL_0039: ldloc 0
			IL_003d: ldlen
			IL_003e: conv.i4
			IL_003f: ldc.i4 1
			IL_0044: cgt
			IL_0046: ldc.i4.0
			IL_0047: ceq
			IL_0049: stloc 5
			IL_004d: ldloc 5
			IL_0051: brtrue IL_023e

			IL_0056: br IL_005b

			IL_005b: ldloc 0
			IL_005f: ldc.i4 1
			IL_0064: ldstr "\n"
			IL_0069: stelem [mscorlib]System.String
			IL_006e: ldloc 0
			IL_0072: ldlen
			IL_0073: conv.i4
			IL_0074: ldc.i4 2
			IL_0079: cgt
			IL_007b: ldc.i4.0
			IL_007c: ceq
			IL_007e: stloc 6
			IL_0082: ldloc 6
			IL_0086: brtrue IL_023e

			IL_008b: br IL_0090

			IL_0090: ldloc 0
			IL_0094: ldc.i4 2
			IL_0099: ldarg.0
			IL_009a: ldfld string BackgroundResource::'<Root>k__BackingField'
			IL_009f: stelem [mscorlib]System.String
			IL_00a4: ldloc 0
			IL_00a8: ldlen
			IL_00a9: conv.i4
			IL_00aa: ldc.i4 3
			IL_00af: cgt
			IL_00b1: ldc.i4.0
			IL_00b2: ceq
			IL_00b4: stloc 7
			IL_00b8: ldloc 7
			IL_00bc: brtrue IL_023e

			IL_00c1: br IL_00c6
```
Context at line 2496:
```il
		IL_006b: ldc.i4.0
		IL_006c: ceq
		IL_006e: stloc 6
		IL_0072: ldloc 6
		IL_0076: ret
	} // end of method BackgroundResource::Finite

	.method public hidebysig static 
		string ResolvePath (
			string root,
			string relative
		) cil managed 
	{
		// Method begins at RVA 0xefb9c
		// Header size: 12
		// Code size: 1257 (0x4e9)
		.maxstack 0
		.locals init (
			[0] string,
			[1] string,
			[2] native int,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] bool,
			[8] bool,
			[9] object,
			[10] object,
			[11] class [System]System.IO.InvalidDataException,
			[12] bool,
			[13] string,
			[14] class [mscorlib]System.Func`2<object, bool>,
			[15] object,
			[16] bool,
			[17] char[],
			[18] string,
			[19] bool,
			[20] bool,
			[21] bool,
			[22] bool,
			[23] bool,
			[24] class [mscorlib]System.NullReferenceException,
			[25] string,
			[26] bool,
			[27] char,
			[28] char,
			[29] bool,
			[30] bool,
			[31] object,
			[32] valuetype [mscorlib]System.StringComparison,
			[33] string,
			[34] bool,
			[35] object,
			[36] class [System]System.IO.InvalidDataException,
			[37] class [System]System.IO.InvalidDataException,
			[38] string[],
			[39] object,
			[40] char,
			[41] bool,
			[42] class [mscorlib]System.Func`2<string, bool>,
			[43] bool,
			[44] string,
			[45] native int,
			[46] native int,
			[47] string,
			[48] native int,
			[49] object,
			[50] string,
			[51] string,
			[52] string,
			[53] object,
			[54] bool,
			[55] native int,
			[56] native int,
			[57] native int,
			[58] native int,
			[59] native int,
			[60] native int
		)

		IL_0000: br IL_040a

		IL_0005: ldarg relative
		IL_0009: call bool [mscorlib]System.String::IsNullOrWhiteSpace(string)
		IL_000e: stloc 8
		IL_0012: ldc.i4 0
		IL_0017: stloc 9
		IL_001b: ldloc 8
		IL_001f: brtrue IL_035b

		IL_0024: br IL_0029

		IL_0029: ldarg relative
		IL_002d: ldc.i4 0
		IL_0032: ceq
		IL_0034: stloc 12
		IL_0038: ldloc 12
		IL_003c: brtrue IL_042e

		IL_0041: br IL_0046

		IL_0046: ldarg relative
		IL_004a: ldc.i4 92
		IL_004f: ldc.i4 47
		IL_0054: call instance string [mscorlib]System.String::Replace(char, char)
		IL_0059: stloc 13
		IL_005d: br IL_0062
```
Context at line 2911:
```il
		IL_0475: ldc.i4 0
		IL_047a: stloc 15
		IL_047e: ldloc 41
		IL_0482: brtrue IL_0111

		IL_0487: br IL_010c

		IL_048c: ldsfld class BackgroundResource/'<>c' BackgroundResource/'<>c'::'<>9'
		IL_0491: ldftn instance bool BackgroundResource/'<>c'::'<ResolvePath>b__32_0'(string)
		IL_0497: newobj instance void class [mscorlib]System.Func`2<string, bool>::.ctor(object, native int)
		IL_049c: stloc 42
		IL_04a0: nop
		IL_04a1: ldloc 42
		IL_04a5: stsfld class [mscorlib]System.Func`2<string, bool> BackgroundResource/'<>c'::'<>9__32_0'
		IL_04aa: ldloc 42
		IL_04ae: stloc 14
		IL_04b2: ldc.i4 0
		IL_04b7: stloc 15
		IL_04bb: br IL_0111

		IL_04c0: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_04c5: throw

		IL_04c6: nop
		IL_04c7: br IL_043c

		IL_04cc: ldstr "Method not found @1804EC860"
		IL_04d1: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_04d6: ldstr "Background path escapes its resource directory."
		IL_04db: newobj instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_04e0: stloc 37
		IL_04e4: br IL_03a9
	} // end of method BackgroundResource::ResolvePath

	// Properties
	.property instance uint32 Id()
	{
		.get instance uint32 BackgroundResource::get_Id()
	}
	.property instance string Name()
	{
		.get instance string BackgroundResource::get_Name()
	}
	.property instance valuetype ScenarioBGType Type()
	{
		.get instance valuetype ScenarioBGType BackgroundResource::get_Type()
	}
	.property instance valuetype ScenarioResourceManager/ResourceType Source()
	{
		.get instance valuetype ScenarioResourceManager/ResourceType BackgroundResource::get_Source()
	}
	.property instance string Root()
	{
		.get instance string BackgroundResource::get_Root()
	}
	.property instance string Path()
	{
		.get instance string BackgroundResource::get_Path()
	}
	.property instance class ResourceOverrideData/BgSpineOverrideData Spine()
	{
		.get instance class ResourceOverrideData/BgSpineOverrideData BackgroundResource::get_Spine()
	}
	.property instance bool IsCustomSpine()
	{
		.get instance bool BackgroundResource::get_IsCustomSpine()
	}
	.property instance string FullPath()
	{
		.get instance string BackgroundResource::get_FullPath()
	}
	.property instance string CacheKey()
	{
		.get instance string BackgroundResource::get_CacheKey()
	}

} // end of class BackgroundResource
```
Context at line 2935:
```il
		IL_04c7: br IL_043c

		IL_04cc: ldstr "Method not found @1804EC860"
		IL_04d1: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_04d6: ldstr "Background path escapes its resource directory."
		IL_04db: newobj instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_04e0: stloc 37
		IL_04e4: br IL_03a9
	} // end of method BackgroundResource::ResolvePath

	// Properties
	.property instance uint32 Id()
	{
		.get instance uint32 BackgroundResource::get_Id()
	}
	.property instance string Name()
	{
		.get instance string BackgroundResource::get_Name()
	}
	.property instance valuetype ScenarioBGType Type()
	{
		.get instance valuetype ScenarioBGType BackgroundResource::get_Type()
	}
	.property instance valuetype ScenarioResourceManager/ResourceType Source()
	{
		.get instance valuetype ScenarioResourceManager/ResourceType BackgroundResource::get_Source()
	}
	.property instance string Root()
	{
		.get instance string BackgroundResource::get_Root()
	}
	.property instance string Path()
	{
		.get instance string BackgroundResource::get_Path()
	}
	.property instance class ResourceOverrideData/BgSpineOverrideData Spine()
	{
		.get instance class ResourceOverrideData/BgSpineOverrideData BackgroundResource::get_Spine()
	}
	.property instance bool IsCustomSpine()
	{
		.get instance bool BackgroundResource::get_IsCustomSpine()
	}
	.property instance string FullPath()
	{
		.get instance string BackgroundResource::get_FullPath()
	}
	.property instance string CacheKey()
	{
		.get instance string BackgroundResource::get_CacheKey()
	}

} // end of class BackgroundResource
```
#### Overrides: 64 occurrences
Context at line 215:
```il
			IL_0045: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
			IL_004a: stloc 4
			IL_004e: ldloc 4
			IL_0052: ret
		} // end of method '<>c__DisplayClass28_0'::'<ResolveOverride>b__1'

	} // end of class <>c__DisplayClass28_0

	.class nested private auto ansi sealed beforefieldinit '<Overrides>d__29'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerable,
		           class [mscorlib]System.Collections.Generic.IEnumerator`1<class BackgroundResource>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private class BackgroundResource '<>2__current'
		.field private int32 '<>l__initialThreadId'
		.field private class ResourceOverrideData data
		.field public class ResourceOverrideData '<>3__data'
		.field private valuetype ScenarioResourceManager/ResourceType source
		.field public valuetype ScenarioResourceManager/ResourceType '<>3__source'
		.field private class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData> '<spines>5__2'
		.field private valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string> '<>7__wrap2'
		.field private valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData> '<>7__wrap3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x495a8c
			// Header size: 12
			// Code size: 42 (0x2a)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: ldstr "Method not found @1828217F0"
			IL_0015: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001a: br IL_001f

			IL_001f: ldarg.0
			IL_0020: ldloc 2
			IL_0024: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0029: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x495ac4
			// Header size: 12
			// Code size: 111 (0x6f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] object,
				[5] class BackgroundResource/'<Overrides>d__29',
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
			IL_004f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0054: ldc.i4.0
			IL_0055: conv.i
			IL_0056: ldc.i8 6442450944
```
Context at line 247:
```il
			) cil managed 
		{
			// Method begins at RVA 0x495a8c
			// Header size: 12
			// Code size: 42 (0x2a)
			.maxstack 0
			.locals init (
				[0] int32,
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: ldstr "Method not found @1828217F0"
			IL_0015: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001a: br IL_001f

			IL_001f: ldarg.0
			IL_0020: ldloc 2
			IL_0024: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0029: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x495ac4
			// Header size: 12
			// Code size: 111 (0x6f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] object,
				[5] class BackgroundResource/'<Overrides>d__29',
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
			IL_004f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0054: ldc.i4.0
			IL_0055: conv.i
			IL_0056: ldc.i8 6442450944
			IL_005f: add
			IL_0060: stloc 4
			IL_0064: ldstr "Indirect jump: v23 @ rax_v4 (should have been resolved before IL gen)"
			IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_006e: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x495b40
			// Header size: 12
			// Code size: 2207 (0x89f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] object,
				[2] native int,
				[3] bool,
				[4] object,
				[5] bool,
				[6] bool,
				[7] object,
				[8] bool,
				[9] object,
				[10] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[11] bool,
				[12] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[13] bool,
				[14] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
```
Context at line 255:
```il
				[1] class BackgroundResource/'<Overrides>d__29',
				[2] int32
			)

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ldarg.0
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: ldstr "Method not found @1828217F0"
			IL_0015: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001a: br IL_001f

			IL_001f: ldarg.0
			IL_0020: ldloc 2
			IL_0024: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0029: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x495ac4
			// Header size: 12
			// Code size: 111 (0x6f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] object,
				[5] class BackgroundResource/'<Overrides>d__29',
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
			IL_004f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0054: ldc.i4.0
			IL_0055: conv.i
			IL_0056: ldc.i8 6442450944
			IL_005f: add
			IL_0060: stloc 4
			IL_0064: ldstr "Indirect jump: v23 @ rax_v4 (should have been resolved before IL gen)"
			IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_006e: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x495b40
			// Header size: 12
			// Code size: 2207 (0x89f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] object,
				[2] native int,
				[3] bool,
				[4] object,
				[5] bool,
				[6] bool,
				[7] object,
				[8] bool,
				[9] object,
				[10] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[11] bool,
				[12] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[13] bool,
				[14] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[15] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[16] bool,
				[17] bool,
				[18] bool,
				[19] bool,
				[20] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[21] bool,
				[22] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
```
Context at line 262:
```il
			IL_0007: ldarg '<>1__state'
			IL_000b: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0010: ldstr "Method not found @1828217F0"
			IL_0015: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001a: br IL_001f

			IL_001f: ldarg.0
			IL_0020: ldloc 2
			IL_0024: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0029: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x495ac4
			// Header size: 12
			// Code size: 111 (0x6f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] object,
				[5] class BackgroundResource/'<Overrides>d__29',
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
			IL_004f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0054: ldc.i4.0
			IL_0055: conv.i
			IL_0056: ldc.i8 6442450944
			IL_005f: add
			IL_0060: stloc 4
			IL_0064: ldstr "Indirect jump: v23 @ rax_v4 (should have been resolved before IL gen)"
			IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_006e: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x495b40
			// Header size: 12
			// Code size: 2207 (0x89f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] object,
				[2] native int,
				[3] bool,
				[4] object,
				[5] bool,
				[6] bool,
				[7] object,
				[8] bool,
				[9] object,
				[10] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[11] bool,
				[12] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[13] bool,
				[14] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[15] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[16] bool,
				[17] bool,
				[18] bool,
				[19] bool,
				[20] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[21] bool,
				[22] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[23] bool,
				[24] bool,
				[25] bool,
				[26] bool,
				[27] bool,
				[28] class [System.Core]System.Collections.Generic.HashSet`1<uint32>,
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string>,
```
Context at line 264:
```il
			IL_0010: ldstr "Method not found @1828217F0"
			IL_0015: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001a: br IL_001f

			IL_001f: ldarg.0
			IL_0020: ldloc 2
			IL_0024: stfld int32 BackgroundResource/'<Overrides>d__29'::'<>l__initialThreadId'
			IL_0029: ret
		} // end of method '<Overrides>d__29'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			.override method instance void [mscorlib]System.IDisposable::Dispose()
			// Method begins at RVA 0x495ac4
			// Header size: 12
			// Code size: 111 (0x6f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] object,
				[5] class BackgroundResource/'<Overrides>d__29',
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
			IL_004f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0054: ldc.i4.0
			IL_0055: conv.i
			IL_0056: ldc.i8 6442450944
			IL_005f: add
			IL_0060: stloc 4
			IL_0064: ldstr "Indirect jump: v23 @ rax_v4 (should have been resolved before IL gen)"
			IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_006e: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x495b40
			// Header size: 12
			// Code size: 2207 (0x89f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] object,
				[2] native int,
				[3] bool,
				[4] object,
				[5] bool,
				[6] bool,
				[7] object,
				[8] bool,
				[9] object,
				[10] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[11] bool,
				[12] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[13] bool,
				[14] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[15] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[16] bool,
				[17] bool,
				[18] bool,
				[19] bool,
				[20] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[21] bool,
				[22] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[23] bool,
				[24] bool,
				[25] bool,
				[26] bool,
				[27] bool,
				[28] class [System.Core]System.Collections.Generic.HashSet`1<uint32>,
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string>,
				[30] bool,
				[31] string,
```
Context at line 280:
```il
			// Code size: 111 (0x6f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] bool,
				[2] object,
				[3] object,
				[4] object,
				[5] class BackgroundResource/'<Overrides>d__29',
				[6] object,
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
			IL_004f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0054: ldc.i4.0
			IL_0055: conv.i
			IL_0056: ldc.i8 6442450944
			IL_005f: add
			IL_0060: stloc 4
			IL_0064: ldstr "Indirect jump: v23 @ rax_v4 (should have been resolved before IL gen)"
			IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_006e: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x495b40
			// Header size: 12
			// Code size: 2207 (0x89f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] object,
				[2] native int,
				[3] bool,
				[4] object,
				[5] bool,
				[6] bool,
				[7] object,
				[8] bool,
				[9] object,
				[10] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[11] bool,
				[12] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[13] bool,
				[14] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[15] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[16] bool,
				[17] bool,
				[18] bool,
				[19] bool,
				[20] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[21] bool,
				[22] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[23] bool,
				[24] bool,
				[25] bool,
				[26] bool,
				[27] bool,
				[28] class [System.Core]System.Collections.Generic.HashSet`1<uint32>,
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string>,
				[30] bool,
				[31] string,
				[32] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string>,
				[33] valuetype ScenarioResourceManager/ResourceType,
				[34] uint32,
				[35] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData>,
				[36] bool,
				[37] string,
				[38] object,
				[39] object,
				[40] class BackgroundResource,
				[41] bool,
				[42] uint32,
				[43] string,
				[44] string,
				[45] string,
				[46] class ResourceOverrideData/BgSpineOverrideData,
				[47] bool,
```
Context at line 290:
```il
				[7] object,
				[8] object,
				[9] object,
				[10] object,
				[11] object
			)

			IL_0000: ldarg.0
			IL_0001: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0006: ldc.i4 4
			IL_000b: add
			IL_000c: stloc 0
			IL_0010: ldloc 0
			IL_0014: ldc.i4 6
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
			IL_004f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0054: ldc.i4.0
			IL_0055: conv.i
			IL_0056: ldc.i8 6442450944
			IL_005f: add
			IL_0060: stloc 4
			IL_0064: ldstr "Indirect jump: v23 @ rax_v4 (should have been resolved before IL gen)"
			IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_006e: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x495b40
			// Header size: 12
			// Code size: 2207 (0x89f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] object,
				[2] native int,
				[3] bool,
				[4] object,
				[5] bool,
				[6] bool,
				[7] object,
				[8] bool,
				[9] object,
				[10] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[11] bool,
				[12] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[13] bool,
				[14] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[15] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[16] bool,
				[17] bool,
				[18] bool,
				[19] bool,
				[20] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[21] bool,
				[22] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[23] bool,
				[24] bool,
				[25] bool,
				[26] bool,
				[27] bool,
				[28] class [System.Core]System.Collections.Generic.HashSet`1<uint32>,
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string>,
				[30] bool,
				[31] string,
				[32] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string>,
				[33] valuetype ScenarioResourceManager/ResourceType,
				[34] uint32,
				[35] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData>,
				[36] bool,
				[37] string,
				[38] object,
				[39] object,
				[40] class BackgroundResource,
				[41] bool,
				[42] uint32,
				[43] string,
				[44] string,
				[45] string,
				[46] class ResourceOverrideData/BgSpineOverrideData,
				[47] bool,
				[48] bool,
				[49] bool,
				[50] bool,
				[51] class BackgroundResource,
				[52] bool,
				[53] bool,
				[54] uint32,
				[55] bool,
				[56] bool,
				[57] string,
```
Context at line 304:
```il
			IL_0019: cgt
			IL_001b: stloc 1
			IL_001f: ldloc 1
			IL_0023: brtrue IL_006e

			IL_0028: br IL_002d

			IL_002d: ldarg.0
			IL_002e: ldfld int32 BackgroundResource/'<Overrides>d__29'::'<>1__state'
			IL_0033: ldc.i4 4
			IL_0038: add
			IL_0039: stloc 2
			IL_003d: ldc.i8 6442450944
			IL_0046: stloc 3
			IL_004a: ldstr "Unmanaged memory load: [v21 @ rdx_v2+9C2E98+v19 @ rax_v2*4]"
			IL_004f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0054: ldc.i4.0
			IL_0055: conv.i
			IL_0056: ldc.i8 6442450944
			IL_005f: add
			IL_0060: stloc 4
			IL_0064: ldstr "Indirect jump: v23 @ rax_v4 (should have been resolved before IL gen)"
			IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)

			IL_006e: ret
		} // end of method '<Overrides>d__29'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			.override method instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
			// Method begins at RVA 0x495b40
			// Header size: 12
			// Code size: 2207 (0x89f)
			.maxstack 0
			.locals init (
				[0] object,
				[1] object,
				[2] native int,
				[3] bool,
				[4] object,
				[5] bool,
				[6] bool,
				[7] object,
				[8] bool,
				[9] object,
				[10] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[11] bool,
				[12] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<object>,
				[13] bool,
				[14] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[15] class [mscorlib]System.Collections.Generic.List`1<class ResourceOverrideData/BgSpineOverrideData>,
				[16] bool,
				[17] bool,
				[18] bool,
				[19] bool,
				[20] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[21] bool,
				[22] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[23] bool,
				[24] bool,
				[25] bool,
				[26] bool,
				[27] bool,
				[28] class [System.Core]System.Collections.Generic.HashSet`1<uint32>,
				[29] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string>,
				[30] bool,
				[31] string,
				[32] valuetype [System.Core]System.Collections.Generic.HashSet`1/Enumerator<string>,
				[33] valuetype ScenarioResourceManager/ResourceType,
				[34] uint32,
				[35] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<class ResourceOverrideData/BgSpineOverrideData>,
				[36] bool,
				[37] string,
				[38] object,
				[39] object,
				[40] class BackgroundResource,
				[41] bool,
				[42] uint32,
				[43] string,
				[44] string,
				[45] string,
				[46] class ResourceOverrideData/BgSpineOverrideData,
				[47] bool,
				[48] bool,
				[49] bool,
				[50] bool,
				[51] class BackgroundResource,
				[52] bool,
				[53] bool,
				[54] uint32,
				[55] bool,
				[56] bool,
				[57] string,
				[58] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<object>,
				[59] class ResourceOverrideData/BgSpineOverrideData,
				[60] bool,
				[61] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[62] string,
				[63] class [System.Core]System.Collections.Generic.HashSet`1<string>,
				[64] object,
				[65] object,
				[66] string,
				[67] class [System]System.IO.InvalidDataException,
				[68] bool,
				[69] string,
				[70] native int,
				[71] bool,
```
#### FullPath: 6 occurrences
Context at line 1630:
```il
		IL_003c: ldloc 2
		IL_0040: and
		IL_0041: stloc 4
		IL_0045: ldloc 4
		IL_0049: ret
	} // end of method BackgroundResource::get_IsCustomSpine

	.method public hidebysig specialname 
		instance string get_FullPath () cil managed 
	{
		// Method begins at RVA 0xef274
		// Header size: 12
		// Code size: 67 (0x43)
		.maxstack 0
		.locals init (
			[0] bool,
			[1] string,
			[2] class BackgroundResource
		)

		IL_0000: ldarg.0
		IL_0001: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0006: ldc.i4 0
		IL_000b: ceq
		IL_000d: ldc.i4.0
		IL_000e: ceq
		IL_0010: stloc 0
		IL_0014: ldloc 0
		IL_0018: brtrue IL_0029

		IL_001d: br IL_0022

		IL_0022: ldarg.0
		IL_0023: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_0028: ret

		IL_0029: ldarg.0
		IL_002a: ldfld string BackgroundResource::'<Root>k__BackingField'
		IL_002f: ldarg.0
		IL_0030: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_0035: call string BackgroundResource::ResolvePath(string, string)
		IL_003a: stloc 1
		IL_003e: ldloc 1
		IL_0042: ret
	} // end of method BackgroundResource::get_FullPath

	.method public hidebysig specialname 
		instance string get_CacheKey () cil managed 
	{
		// Method begins at RVA 0xef2c4
		// Header size: 12
		// Code size: 588 (0x24c)
		.maxstack 0
		.locals init (
			[0] string[],
			[1] string,
			[2] bool,
			[3] string,
			[4] string,
			[5] bool,
			[6] bool,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] bool,
			[13] class [mscorlib]System.IndexOutOfRangeException,
			[14] string,
			[15] native int,
			[16] class BackgroundResource,
			[17] uint32
		)

		IL_0000: br IL_021e
		// loop start (head: IL_021e)
			IL_0005: ldloc 0
			IL_0009: ldlen
			IL_000a: conv.i4
			IL_000b: ldc.i4 0
			IL_0010: cgt
			IL_0012: ldc.i4.0
			IL_0013: ceq
			IL_0015: stloc 2
			IL_0019: ldloc 2
			IL_001d: brtrue IL_023e

			IL_0022: br IL_0027

			IL_0027: ldloc 0
			IL_002b: ldc.i4 0
			IL_0030: ldloc 1
			IL_0034: stelem [mscorlib]System.String
			IL_0039: ldloc 0
			IL_003d: ldlen
			IL_003e: conv.i4
			IL_003f: ldc.i4 1
			IL_0044: cgt
			IL_0046: ldc.i4.0
			IL_0047: ceq
			IL_0049: stloc 5
			IL_004d: ldloc 5
			IL_0051: brtrue IL_023e

			IL_0056: br IL_005b

			IL_005b: ldloc 0
			IL_005f: ldc.i4 1
```
Context at line 1666:
```il
		IL_0029: ldarg.0
		IL_002a: ldfld string BackgroundResource::'<Root>k__BackingField'
		IL_002f: ldarg.0
		IL_0030: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_0035: call string BackgroundResource::ResolvePath(string, string)
		IL_003a: stloc 1
		IL_003e: ldloc 1
		IL_0042: ret
	} // end of method BackgroundResource::get_FullPath

	.method public hidebysig specialname 
		instance string get_CacheKey () cil managed 
	{
		// Method begins at RVA 0xef2c4
		// Header size: 12
		// Code size: 588 (0x24c)
		.maxstack 0
		.locals init (
			[0] string[],
			[1] string,
			[2] bool,
			[3] string,
			[4] string,
			[5] bool,
			[6] bool,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] bool,
			[13] class [mscorlib]System.IndexOutOfRangeException,
			[14] string,
			[15] native int,
			[16] class BackgroundResource,
			[17] uint32
		)

		IL_0000: br IL_021e
		// loop start (head: IL_021e)
			IL_0005: ldloc 0
			IL_0009: ldlen
			IL_000a: conv.i4
			IL_000b: ldc.i4 0
			IL_0010: cgt
			IL_0012: ldc.i4.0
			IL_0013: ceq
			IL_0015: stloc 2
			IL_0019: ldloc 2
			IL_001d: brtrue IL_023e

			IL_0022: br IL_0027

			IL_0027: ldloc 0
			IL_002b: ldc.i4 0
			IL_0030: ldloc 1
			IL_0034: stelem [mscorlib]System.String
			IL_0039: ldloc 0
			IL_003d: ldlen
			IL_003e: conv.i4
			IL_003f: ldc.i4 1
			IL_0044: cgt
			IL_0046: ldc.i4.0
			IL_0047: ceq
			IL_0049: stloc 5
			IL_004d: ldloc 5
			IL_0051: brtrue IL_023e

			IL_0056: br IL_005b

			IL_005b: ldloc 0
			IL_005f: ldc.i4 1
			IL_0064: ldstr "\n"
			IL_0069: stelem [mscorlib]System.String
			IL_006e: ldloc 0
			IL_0072: ldlen
			IL_0073: conv.i4
			IL_0074: ldc.i4 2
			IL_0079: cgt
			IL_007b: ldc.i4.0
			IL_007c: ceq
			IL_007e: stloc 6
			IL_0082: ldloc 6
			IL_0086: brtrue IL_023e

			IL_008b: br IL_0090

			IL_0090: ldloc 0
			IL_0094: ldc.i4 2
			IL_0099: ldarg.0
			IL_009a: ldfld string BackgroundResource::'<Root>k__BackingField'
			IL_009f: stelem [mscorlib]System.String
			IL_00a4: ldloc 0
			IL_00a8: ldlen
			IL_00a9: conv.i4
			IL_00aa: ldc.i4 3
			IL_00af: cgt
			IL_00b1: ldc.i4.0
			IL_00b2: ceq
			IL_00b4: stloc 7
			IL_00b8: ldloc 7
			IL_00bc: brtrue IL_023e

			IL_00c1: br IL_00c6

			IL_00c6: ldloc 0
			IL_00ca: ldc.i4 3
			IL_00cf: ldstr "\n"
```
Context at line 2661:
```il
		IL_0130: ldloc 30
		IL_0134: brtrue IL_043c

		IL_0139: br IL_013e

		IL_013e: br IL_0143

		IL_0143: ldarg root
		IL_0147: call string [mscorlib]System.IO.Path::GetFullPath(string)
		IL_014c: stloc 44
		IL_0150: ldc.i4 2
		IL_0155: newarr [mscorlib]System.Char
		IL_015a: stloc 17
		IL_015e: br IL_0163

		IL_0163: ldtoken [mscorlib]System.IO.Path
		IL_0168: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_016d: stloc 45
		IL_0171: nop
		IL_0172: ldloc 17
		IL_0176: ldc.i4 0
		IL_017b: ceq
		IL_017d: stloc 21
		IL_0181: ldloc 21
		IL_0185: brtrue IL_042e

		IL_018a: br IL_018f

		IL_018f: ldloc 17
		IL_0193: ldlen
		IL_0194: conv.i4
		IL_0195: ldc.i4 0
		IL_019a: cgt
		IL_019c: ldc.i4.0
		IL_019d: ceq
		IL_019f: stloc 43
		IL_01a3: ldc.i4.0
		IL_01a4: conv.i
		IL_01a5: stloc 27
		IL_01a9: ldstr "Unmanaged memory load: [v529 @ rcx_v39 (Il2CppClass<System.IO.Path>)+B8]"
		IL_01ae: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_01b3: ldc.i4.0
		IL_01b4: conv.i
		IL_01b5: stloc 28
		IL_01b9: ldloc 43
		IL_01bd: brtrue IL_04c0

		IL_01c2: br IL_01c7

		IL_01c7: ldloc 17
		IL_01cb: ldc.i4 0
		IL_01d0: ldsfld char [mscorlib]System.IO.Path::DirectorySeparatorChar
		IL_01d5: stelem [mscorlib]System.Char
		IL_01da: ldtoken [mscorlib]System.IO.Path
		IL_01df: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_01e4: stloc 46
		IL_01e8: nop
		IL_01e9: ldloc 17
		IL_01ed: ldlen
		IL_01ee: conv.i4
		IL_01ef: ldc.i4 1
		IL_01f4: cgt
		IL_01f6: ldc.i4.0
		IL_01f7: ceq
		IL_01f9: stloc 19
		IL_01fd: ldc.i4.0
		IL_01fe: conv.i
		IL_01ff: stloc 27
		IL_0203: ldstr "Unmanaged memory load: [v532 @ rcx_v41 (Il2CppClass<System.IO.Path>)+B8]"
		IL_0208: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_020d: ldc.i4.0
		IL_020e: conv.i
		IL_020f: stloc 28
		IL_0213: ldloc 19
		IL_0217: brtrue IL_04c0

		IL_021c: br IL_0221

		IL_0221: ldloc 17
		IL_0225: ldc.i4 1
		IL_022a: ldsfld char [mscorlib]System.IO.Path::AltDirectorySeparatorChar
		IL_022f: stelem [mscorlib]System.Char
		IL_0234: ldloc 44
		IL_0238: ldc.i4 0
		IL_023d: ceq
		IL_023f: stloc 22
		IL_0243: ldloc 22
		IL_0247: brtrue IL_042e

		IL_024c: br IL_0251

		IL_0251: ldloc 44
		IL_0255: ldloc 17
		IL_0259: call instance string [mscorlib]System.String::TrimEnd(char[])
		IL_025e: stloc 47
		IL_0262: br IL_0267

		IL_0267: ldtoken [mscorlib]System.IO.Path
		IL_026c: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0271: stloc 48
		IL_0275: ldstr "Unmanaged memory load: [v553 @ rcx_v45 (Il2CppClass<System.IO.Path>)+B8]"
		IL_027a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_027f: ldc.i4.0
		IL_0280: conv.i
		IL_0281: ldc.i4 10
		IL_0286: add
		IL_0287: stloc 49
		IL_028b: ldstr "Method not found @18270E560"
```
Context at line 2773:
```il
		IL_02a2: stloc 51
		IL_02a6: br IL_02ab

		IL_02ab: ldloc 51
		IL_02af: ldloc 13
		IL_02b3: call string [mscorlib]System.IO.Path::Combine(string, string)
		IL_02b8: stloc 52
		IL_02bc: ldloc 52
		IL_02c0: call string [mscorlib]System.IO.Path::GetFullPath(string)
		IL_02c5: stloc 18
		IL_02c9: br IL_02ce

		IL_02ce: ldloc 18
		IL_02d2: ldc.i4 0
		IL_02d7: ceq
		IL_02d9: stloc 23
		IL_02dd: ldloc 23
		IL_02e1: brtrue IL_042e

		IL_02e6: br IL_02eb

		IL_02eb: ldsfld char [mscorlib]System.IO.Path::DirectorySeparatorChar
		IL_02f0: ldc.i4 92
		IL_02f5: sub
		IL_02f6: stloc 53
		IL_02fa: ldloc 53
		IL_02fe: ldc.i4 0
		IL_0303: ceq
		IL_0305: stloc 54
		IL_0309: ldloc 54
		IL_030d: ldc.i4 4
		IL_0312: add
		IL_0313: stloc 32
		IL_0317: ldloc 18
		IL_031b: ldloc 51
		IL_031f: ldloc 32
		IL_0323: call instance bool [mscorlib]System.String::StartsWith(string, valuetype [mscorlib]System.StringComparison)
		IL_0328: stloc 34
		IL_032c: ldloc 34
		IL_0330: ldc.i4.0
		IL_0331: ceq
		IL_0333: stloc 26
		IL_0337: ldc.i4 0
		IL_033c: stloc 15
		IL_0340: ldloc 51
		IL_0344: stloc 33
		IL_0348: ldloc 26
		IL_034c: brtrue IL_04cc

		IL_0351: br IL_0356

		IL_0356: ldloc 18
		IL_035a: ret

		IL_035b: ldstr "Method not found @1804EC860"
		IL_0360: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0365: ldstr "A background resource root and relative path are required."
		IL_036a: newobj instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_036f: stloc 11
		IL_0373: br IL_0378

		IL_0378: nop
		IL_0379: br IL_037e

		IL_037e: ldstr "Method not found @1804EC860"
		IL_0383: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0388: ldloc 11
		IL_038c: throw

		IL_038d: br IL_0392

		IL_0392: nop
		IL_0393: ldc.i4 0
		IL_0398: stloc 32
		IL_039c: ldloc 25
		IL_03a0: stloc 33
		IL_03a4: br IL_04cc

		IL_03a9: ldloc 37
		IL_03ad: ldstr "Background path escapes its resource directory."
		IL_03b2: call instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_03b7: ldstr "Method not found @1804EC860"
		IL_03bc: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03c1: br IL_03c6

		IL_03c6: ldloc 37
		IL_03ca: throw

		IL_03cb: nop
		IL_03cc: ldc.i4 0
		IL_03d1: stloc 27
		IL_03d5: ldloc 40
		IL_03d9: stloc 28
		IL_03dd: br IL_04c0

		IL_03e2: ldloc 36
		IL_03e6: ldstr "Background paths must be relative to the resource directory."
		IL_03eb: call instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_03f0: ldstr "Method not found @1804EC860"
		IL_03f5: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03fa: br IL_03ff

		IL_03ff: ldloc 36
		IL_0403: throw

		IL_0404: nop
		IL_0405: br IL_042e

```
Context at line 2970:
```il
	.property instance class ResourceOverrideData/BgSpineOverrideData Spine()
	{
		.get instance class ResourceOverrideData/BgSpineOverrideData BackgroundResource::get_Spine()
	}
	.property instance bool IsCustomSpine()
	{
		.get instance bool BackgroundResource::get_IsCustomSpine()
	}
	.property instance string FullPath()
	{
		.get instance string BackgroundResource::get_FullPath()
	}
	.property instance string CacheKey()
	{
		.get instance string BackgroundResource::get_CacheKey()
	}

} // end of class BackgroundResource
```
Context at line 2972:
```il
		.get instance class ResourceOverrideData/BgSpineOverrideData BackgroundResource::get_Spine()
	}
	.property instance bool IsCustomSpine()
	{
		.get instance bool BackgroundResource::get_IsCustomSpine()
	}
	.property instance string FullPath()
	{
		.get instance string BackgroundResource::get_FullPath()
	}
	.property instance string CacheKey()
	{
		.get instance string BackgroundResource::get_CacheKey()
	}

} // end of class BackgroundResource
```

## ScenarioResourceManager
### android (SHA256 4a3fdf5a82e7759430e211d09d1a4808381ab940b02e0b05dd8dd2978b99679c)
#### ResolveBackground: 6 occurrences
Context at line 17002:
```il
	.field private initonly class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.AudioModule]UnityEngine.AudioClip> _loadedCustomVoices
	.field private initonly class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.AudioModule]UnityEngine.AudioClip> _loadedCustomSounds
	.field private bool _isManualInstalling
	.field private static literal uint64 ResStructureVersion = uint64(11)
	.field private string _manualInstallPath

	// Methods
	.method public hidebysig 
		instance class BackgroundResource ResolveBackground (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x436330
		// Header size: 12
		// Code size: 1555 (0x613)
		.maxstack 0
		.locals init (
			[0] uint32,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] native int,
			[11] class ScenarioResourceManager/'<>c__DisplayClass9_0',
			[12] bool,
			[13] bool,
			[14] native int,
			[15] object,
			[16] bool,
			[17] class BackgroundResource,
			[18] native int,
			[19] class Studio.Scripts.Databases.ScenarioBgNameDbSchema,
			[20] bool,
			[21] bool,
			[22] bool,
			[23] class [mscorlib]System.NullReferenceException,
			[24] class [mscorlib]System.Reflection.MethodBase,
			[25] bool,
			[26] bool,
			[27] bool,
			[28] class BackgroundResource,
			[29] object,
			[30] class [mscorlib]System.NullReferenceException,
			[31] bool,
			[32] native int,
			[33] native int,
			[34] class BackgroundResource,
			[35] object,
			[36] object,
			[37] class [mscorlib]System.NullReferenceException,
			[38] bool,
			[39] object,
			[40] native int,
			[41] object,
			[42] object,
			[43] bool,
			[44] object,
			[45] class [mscorlib]System.Type,
			[46] native int,
			[47] object,
			[48] bool,
			[49] object,
			[50] object,
			[51] class [System.Core]System.Linq.Expressions.ParameterExpression,
			[52] native int,
			[53] bool,
			[54] native int,
			[55] class [System.Core]System.Linq.Expressions.MemberExpression,
			[56] native int,
			[57] class [mscorlib]System.Type,
			[58] class [System.Core]System.Linq.Expressions.ConstantExpression,
			[59] native int,
			[60] class [mscorlib]System.Reflection.FieldInfo,
			[61] class [System.Core]System.Linq.Expressions.MemberExpression,
			[62] class [System.Core]System.Linq.Expressions.BinaryExpression,
			[63] native int,
			[64] bool,
			[65] object,
			[66] object,
			[67] native int,
			[68] object,
			[69] valuetype FlatData.ScenarioBGNameExcel,
			[70] native int,
			[71] object,
			[72] bool,
			[73] object,
			[74] string,
			[75] native int,
			[76] object,
			[77] object,
			[78] string,
			[79] valuetype ScenarioBGType,
			[80] string,
			[81] native int,
			[82] class BackgroundResource,
			[83] class ResourceOverrideData/BgSpineOverrideData,
			[84] class ScenarioResourceManager,
			[85] valuetype FlatData.ScenarioBGNameExcel
		)

		IL_0000: br IL_05c7
		// loop start (head: IL_05c7)
			IL_0005: ldarg id
```
Context at line 17557:
```il
			IL_0600: ldloc 12
			IL_0604: brtrue IL_0582

			IL_0609: br IL_0005
		// end loop

		IL_060e: ldloc 28
		IL_0612: ret
	} // end of method ScenarioResourceManager::ResolveBackground

	.method public hidebysig 
		instance class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> EnumerateBackgrounds () cil managed 
	{
		// Method begins at RVA 0x436950
		// Header size: 12
		// Code size: 121 (0x79)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] native int,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] class ScenarioResourceManager/'<EnumerateBackgrounds>d__10',
			[16] bool,
			[17] object,
			[18] class [mscorlib]System.NullReferenceException
		)

		IL_0000: br IL_0032
		// loop start (head: IL_0032)
			IL_0005: ldloc 15
			IL_0009: ldc.i4 40
			IL_000e: add
			IL_000f: stloc 17
			IL_0013: ldarg.0
			IL_0014: pop
			IL_0015: ldstr "Method not found @325DFB3"
			IL_001a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_001f: ldloc 15
			IL_0023: ret
			// loop start (head: IL_0032)
				IL_0024: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
				IL_0029: stloc 18
				IL_002d: ldloc 18
				IL_0031: ret

				IL_0032: ldtoken ScenarioResourceManager/'<EnumerateBackgrounds>d__10'
				IL_0037: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
				IL_003c: stloc 14
				IL_0040: ldstr "Method not found @325E234"
				IL_0045: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
				IL_004a: ldloc 15
				IL_004e: ldc.i8 4294967294
				IL_0057: call instance void ScenarioResourceManager/'<EnumerateBackgrounds>d__10'::.ctor(int32)
				IL_005c: ldloc 15
				IL_0060: ldc.i4 0
				IL_0065: ceq
				IL_0067: stloc 16
				IL_006b: ldloc 16
				IL_006f: brtrue IL_0024
			// end loop

			IL_0074: br IL_0005
		// end loop
	} // end of method ScenarioResourceManager::EnumerateBackgrounds

	.method public hidebysig 
		instance bool TryGetBackgroundOverridePath (
			uint32 id,
			[out] string& path
		) cil managed 
	{
		// Method begins at RVA 0x4369d8
		// Header size: 12
		// Code size: 271 (0x10f)
		.maxstack 0
		.locals init (
			[0] string&,
			[1] class BackgroundResource,
			[2] class ScenarioResourceManager,
			[3] uint32,
			[4] bool,
			[5] bool,
			[6] bool,
			[7] bool,
			[8] string,
			[9] class BackgroundResource,
			[10] string,
			[11] native int,
			[12] bool,
			[13] bool
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
```
Context at line 17655:
```il
			[10] string,
			[11] native int,
			[12] bool,
			[13] bool
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldloc 1
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 4
		IL_001d: ldloc 1
		IL_0021: ldc.i4 0
		IL_0026: ceq
		IL_0028: ldc.i4.0
		IL_0029: ceq
		IL_002b: stloc 5
		IL_002f: ldarg path
		IL_0033: stloc 9
		IL_0037: ldloc 5
		IL_003b: brtrue IL_00cf

		IL_0040: br IL_0045

		IL_0045: ldloc 1
		IL_0049: stloc 9
		IL_004d: br IL_00cf
		// loop start (head: IL_00cf)
			IL_0052: ldloc 1
			IL_0056: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
			IL_005b: ldc.i4 0
			IL_0060: ceq
			IL_0062: stloc 6
			IL_0066: ldloc 6
			IL_006a: brtrue IL_00b3

			IL_006f: br IL_0074

			IL_0074: ldloc 1
			IL_0078: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
			IL_007d: ldc.i4 1
			IL_0082: ceq
			IL_0084: ldc.i4.0
			IL_0085: ceq
			IL_0087: stloc 7
			IL_008b: ldloc 7
			IL_008f: brtrue IL_00c5

			IL_0094: br IL_0099

			IL_0099: ldloc 1
			IL_009d: call instance string BackgroundResource::get_FullPath()
			IL_00a2: stloc 10
			IL_00a6: ldloc 10
			IL_00aa: stloc 8
			IL_00ae: br IL_00dd

			IL_00b3: ldnull
			IL_00b4: stloc 8
			IL_00b8: ldarg path
			IL_00bc: stloc 9
			IL_00c0: br IL_00dd

			IL_00c5: ldnull
			IL_00c6: stloc 8
			IL_00ca: br IL_00dd

			IL_00cf: ldloc 4
			IL_00d3: brtrue IL_00b3

			IL_00d8: br IL_0052
		// end loop

		IL_00dd: ldloc 8
		IL_00e1: stloc 9
		IL_00e5: ldstr "Method not found @325DFB3"
		IL_00ea: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00ef: ldarg path
		IL_00f3: ldind.ref
		IL_00f4: ldc.i4 0
		IL_00f9: ceq
		IL_00fb: stloc 12
		IL_00ff: ldloc 12
		IL_0103: ldc.i4.0
		IL_0104: ceq
		IL_0106: stloc 13
		IL_010a: ldloc 13
		IL_010e: ret
	} // end of method ScenarioResourceManager::TryGetBackgroundOverridePath

	.method public hidebysig 
		instance valuetype ScenarioBGType GetBackgroundType (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x436af4
		// Header size: 12
		// Code size: 80 (0x50)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] class ScenarioResourceManager,
			[2] uint32,
			[3] bool,
			[4] valuetype ScenarioBGType
```
Context at line 17759:
```il
			[1] class ScenarioResourceManager,
			[2] uint32,
			[3] bool,
			[4] valuetype ScenarioBGType
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 0
		IL_000e: ldloc 0
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 3
		IL_001d: ldloc 3
		IL_0021: brtrue IL_003d

		IL_0026: br IL_002b

		IL_002b: ldloc 0
		IL_002f: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_0034: stloc 4
		IL_0038: br IL_004b

		IL_003d: ldc.i4 1
		IL_0042: stloc 4
		IL_0046: br IL_004b

		IL_004b: ldloc 4
		IL_004f: ret
	} // end of method ScenarioResourceManager::GetBackgroundType

	.method public hidebysig specialname 
		instance bool get_Preloading () cil managed 
	{
		// Method begins at RVA 0x436b50
		// Header size: 12
		// Code size: 7 (0x7)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager
		)

		IL_0000: ldarg.0
		IL_0001: ldfld bool ScenarioResourceManager::'<Preloading>k__BackingField'
		IL_0006: ret
	} // end of method ScenarioResourceManager::get_Preloading

	.method private hidebysig specialname 
		instance void set_Preloading (
			bool 'value'
		) cil managed 
	{
		// Method begins at RVA 0x436b64
		// Header size: 12
		// Code size: 11 (0xb)
		.maxstack 0
		.locals init (
			[0] bool,
			[1] class ScenarioResourceManager
		)

		IL_0000: ldarg.0
		IL_0001: ldarg 'value'
		IL_0005: stfld bool ScenarioResourceManager::'<Preloading>k__BackingField'
		IL_000a: ret
	} // end of method ScenarioResourceManager::set_Preloading

	.method public hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator CoTryPreloadBackgroundTexture (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x436b7c
		// Header size: 12
		// Code size: 122 (0x7a)
		.maxstack 0
		.locals init (
			[0] uint32,
			[1] class ScenarioResourceManager,
			[2] native int,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] class ScenarioResourceManager/'<CoTryPreloadBackgroundTexture>d__17',
			[16] bool,
			[17] object,
			[18] class [mscorlib]System.NullReferenceException
		)

		IL_0000: br IL_0037
		// loop start (head: IL_0037)
			IL_0005: ldloc 15
			IL_0009: ldc.i4 32
			IL_000e: add
			IL_000f: stloc 17
			IL_0013: ldarg.0
			IL_0014: pop
			IL_0015: ldstr "Method not found @325DFB3"
```
Context at line 18517:
```il
		.locals init (
			[0] class ScenarioResourceManager,
			[1] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[2] uint32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 8 Invalid \"Jump target not found in method: 0x3546040\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x4372a0
		// Header size: 12
		// Code size: 1139 (0x473)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] bool,
			[10] bool,
			[11] native int,
			[12] class BackgroundResource,
			[13] object,
			[14] bool,
			[15] object,
			[16] class [System]System.IO.InvalidDataException,
			[17] string,
			[18] string,
			[19] class [mscorlib]System.InvalidOperationException,
			[20] bool,
			[21] native int,
			[22] object,
			[23] bool,
			[24] native int,
			[25] object,
			[26] bool,
			[27] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[28] string,
			[29] bool,
			[30] bool,
			[31] bool,
			[32] object,
			[33] native int,
			[34] object,
			[35] class [System]System.IO.InvalidDataException,
			[36] class [System]System.IO.InvalidDataException,
			[37] bool,
			[38] native int,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] object,
			[44] string,
			[45] object,
			[46] native int,
			[47] object,
			[48] bool,
			[49] bool,
			[50] string,
			[51] native int,
			[52] object,
			[53] bool,
			[54] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[55] object,
			[56] uint8[],
			[57] object,
			[58] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[59] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[60] object,
			[61] object,
			[62] bool,
			[63] object,
			[64] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[65] string,
			[66] object,
			[67] class BackgroundResource,
			[68] class [mscorlib]System.InvalidOperationException
		)

		IL_0000: br IL_043b

		IL_0005: ldloc 67
		IL_0009: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_000e: ldc.i4 1
		IL_0013: ceq
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 10
		IL_001c: ldloc 10
		IL_0020: brtrue IL_034d
```
Context at line 19174:
```il
			[16] object,
			[17] object,
			[18] object,
			[19] object
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 3
		IL_000e: ldloc 3
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 5
		IL_001d: ldloc 5
		IL_0021: brtrue IL_0043

		IL_0026: br IL_002b

		IL_002b: ldarg.0
		IL_002c: ldloc 3
		IL_0030: ldarg preferThumbnail
		IL_0034: ldarg callback
		IL_0038: call instance void ScenarioResourceManager::GetBackgroundPreviewAsync(class BackgroundResource, bool, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
		IL_003d: br IL_0042

		IL_0042: ret

		IL_0043: ldarg callback
		IL_0047: ldc.i4 0
		IL_004c: ceq
		IL_004e: stloc 6
		IL_0052: ldloc 6
		IL_0056: brtrue IL_0042

		IL_005b: br IL_0060

		IL_0060: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+40]"
		IL_0065: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_006a: ldc.i4.0
		IL_006b: conv.i
		IL_006c: stloc 7
		IL_0070: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+18]"
		IL_0075: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_007a: ldc.i4.0
		IL_007b: conv.i
		IL_007c: stloc 8
		IL_0080: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+28]"
		IL_0085: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_008a: ldc.i4.0
		IL_008b: conv.i
		IL_008c: stloc 9
		IL_0090: ldstr "Indirect jump: v64 @ rax_v2 (should have been resolved before IL gen)"
		IL_0095: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundTextureAsync

	.method public hidebysig 
		instance void GetBackgroundPreviewAsync (
			class BackgroundResource resource,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4379cc
		// Header size: 12
		// Code size: 799 (0x31f)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] native int,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] bool,
			[15] bool,
			[16] object,
			[17] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[18] object,
			[19] object,
			[20] object,
			[21] string,
			[22] object,
			[23] object,
			[24] native int,
			[25] object&,
			[26] bool,
			[27] object,
			[28] object,
			[29] bool,
			[30] bool,
			[31] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[32] bool,
			[33] bool,
			[34] object,
			[35] bool,
			[36] bool,
			[37] bool,
			[38] native int,
			[39] object,
```
#### GetBackgroundTexture: 8 occurrences
Context at line 18501:
```il
		IL_0650: stloc 81
		IL_0654: ldloc 81
		IL_0658: brtrue IL_05fa

		IL_065d: br IL_05eb
	} // end of method ScenarioResourceManager::AcquireBackgroundSpine

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x437274
		// Header size: 12
		// Code size: 29 (0x1d)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[2] uint32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 8 Invalid \"Jump target not found in method: 0x3546040\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x4372a0
		// Header size: 12
		// Code size: 1139 (0x473)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] bool,
			[10] bool,
			[11] native int,
			[12] class BackgroundResource,
			[13] object,
			[14] bool,
			[15] object,
			[16] class [System]System.IO.InvalidDataException,
			[17] string,
			[18] string,
			[19] class [mscorlib]System.InvalidOperationException,
			[20] bool,
			[21] native int,
			[22] object,
			[23] bool,
			[24] native int,
			[25] object,
			[26] bool,
			[27] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[28] string,
			[29] bool,
			[30] bool,
			[31] bool,
			[32] object,
			[33] native int,
			[34] object,
			[35] class [System]System.IO.InvalidDataException,
			[36] class [System]System.IO.InvalidDataException,
			[37] bool,
			[38] native int,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] object,
			[44] string,
			[45] object,
			[46] native int,
			[47] object,
			[48] bool,
			[49] bool,
			[50] string,
			[51] native int,
			[52] object,
			[53] bool,
			[54] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[55] object,
			[56] uint8[],
			[57] object,
			[58] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[59] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[60] object,
			[61] object,
			[62] bool,
			[63] object,
			[64] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[65] string,
```
Context at line 18523:
```il
		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 8 Invalid \"Jump target not found in method: 0x3546040\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x4372a0
		// Header size: 12
		// Code size: 1139 (0x473)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] bool,
			[10] bool,
			[11] native int,
			[12] class BackgroundResource,
			[13] object,
			[14] bool,
			[15] object,
			[16] class [System]System.IO.InvalidDataException,
			[17] string,
			[18] string,
			[19] class [mscorlib]System.InvalidOperationException,
			[20] bool,
			[21] native int,
			[22] object,
			[23] bool,
			[24] native int,
			[25] object,
			[26] bool,
			[27] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[28] string,
			[29] bool,
			[30] bool,
			[31] bool,
			[32] object,
			[33] native int,
			[34] object,
			[35] class [System]System.IO.InvalidDataException,
			[36] class [System]System.IO.InvalidDataException,
			[37] bool,
			[38] native int,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] object,
			[44] string,
			[45] object,
			[46] native int,
			[47] object,
			[48] bool,
			[49] bool,
			[50] string,
			[51] native int,
			[52] object,
			[53] bool,
			[54] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[55] object,
			[56] uint8[],
			[57] object,
			[58] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[59] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[60] object,
			[61] object,
			[62] bool,
			[63] object,
			[64] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[65] string,
			[66] object,
			[67] class BackgroundResource,
			[68] class [mscorlib]System.InvalidOperationException
		)

		IL_0000: br IL_043b

		IL_0005: ldloc 67
		IL_0009: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_000e: ldc.i4 1
		IL_0013: ceq
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 10
		IL_001c: ldloc 10
		IL_0020: brtrue IL_034d

		IL_0025: br IL_002a

		IL_002a: ldloc 67
		IL_002e: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0033: ldc.i4 0
```
Context at line 18526:
```il
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 8 Invalid \"Jump target not found in method: 0x3546040\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x4372a0
		// Header size: 12
		// Code size: 1139 (0x473)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] bool,
			[10] bool,
			[11] native int,
			[12] class BackgroundResource,
			[13] object,
			[14] bool,
			[15] object,
			[16] class [System]System.IO.InvalidDataException,
			[17] string,
			[18] string,
			[19] class [mscorlib]System.InvalidOperationException,
			[20] bool,
			[21] native int,
			[22] object,
			[23] bool,
			[24] native int,
			[25] object,
			[26] bool,
			[27] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[28] string,
			[29] bool,
			[30] bool,
			[31] bool,
			[32] object,
			[33] native int,
			[34] object,
			[35] class [System]System.IO.InvalidDataException,
			[36] class [System]System.IO.InvalidDataException,
			[37] bool,
			[38] native int,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] object,
			[44] string,
			[45] object,
			[46] native int,
			[47] object,
			[48] bool,
			[49] bool,
			[50] string,
			[51] native int,
			[52] object,
			[53] bool,
			[54] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[55] object,
			[56] uint8[],
			[57] object,
			[58] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[59] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[60] object,
			[61] object,
			[62] bool,
			[63] object,
			[64] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[65] string,
			[66] object,
			[67] class BackgroundResource,
			[68] class [mscorlib]System.InvalidOperationException
		)

		IL_0000: br IL_043b

		IL_0005: ldloc 67
		IL_0009: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_000e: ldc.i4 1
		IL_0013: ceq
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 10
		IL_001c: ldloc 10
		IL_0020: brtrue IL_034d

		IL_0025: br IL_002a

		IL_002a: ldloc 67
		IL_002e: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0033: ldc.i4 0
		IL_0038: ceq
		IL_003a: stloc 14
		IL_003e: ldloc 14
```
Context at line 18871:
```il

		IL_034d: ldstr "Method not found @325E011"
		IL_0352: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0357: ldstr "Method not found @325E234"
		IL_035c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0361: br IL_0366

		IL_0366: ldloc 19
		IL_036a: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_036f: call instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		IL_0374: br IL_0379

		IL_0379: ldc.i4 0
		IL_037e: stloc 41
		IL_0382: ldc.i4 0
		IL_0387: stloc 42
		IL_038b: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0390: stloc 44
		IL_0394: br IL_0422

		IL_0399: ldstr "Method not found @325E011"
		IL_039e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03a3: ldstr "Method not found @2E4643A"
		IL_03a8: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03ad: br IL_03b2

		IL_03b2: ldstr "Method not found @325E011"
		IL_03b7: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03bc: ldloc 64
		IL_03c0: call void [Unity.Addressables]UnityEngine.AddressableAssets.Addressables::Release<object>(valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<!!0>)
		IL_03c5: br IL_03ca

		IL_03ca: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
		IL_03cf: throw

		IL_03d0: br IL_03d5

		IL_03d5: ldstr "Cannot load background: "
		IL_03da: ldloc 67
		IL_03de: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_03e3: call string [mscorlib]System.String::Concat(string, string)
		IL_03e8: stloc 65
		IL_03ec: ldstr "Method not found @325E011"
		IL_03f1: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03f6: br IL_03fb

		IL_03fb: ldstr "Method not found @325E234"
		IL_0400: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0405: ldloc 27
		IL_0409: stloc 41
		IL_040d: ldloc 65
		IL_0411: stloc 44
		IL_0415: ldloc 35
		IL_0419: stloc 36
		IL_041d: br IL_0458

		IL_0422: ldstr "Method not found @325E011"
		IL_0427: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_042c: ldloc 68
		IL_0430: throw

		IL_0431: br IL_0436

		IL_0436: ldloc 54
		IL_043a: ret

		IL_043b: ldloc 67
		IL_043f: ldc.i4 0
		IL_0444: ceq
		IL_0446: stloc 9
		IL_044a: ldloc 9
		IL_044e: brtrue IL_0315

		IL_0453: br IL_0005

		IL_0458: ldloc 36
		IL_045c: ldloc 44
		IL_0460: call instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_0465: ldc.i4 0
		IL_046a: stloc 42
		IL_046e: br IL_0422
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x437720
		// Header size: 12
		// Code size: 501 (0x1f5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] native int,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
```
Context at line 18879:
```il
		IL_036a: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_036f: call instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		IL_0374: br IL_0379

		IL_0379: ldc.i4 0
		IL_037e: stloc 41
		IL_0382: ldc.i4 0
		IL_0387: stloc 42
		IL_038b: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0390: stloc 44
		IL_0394: br IL_0422

		IL_0399: ldstr "Method not found @325E011"
		IL_039e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03a3: ldstr "Method not found @2E4643A"
		IL_03a8: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03ad: br IL_03b2

		IL_03b2: ldstr "Method not found @325E011"
		IL_03b7: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03bc: ldloc 64
		IL_03c0: call void [Unity.Addressables]UnityEngine.AddressableAssets.Addressables::Release<object>(valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<!!0>)
		IL_03c5: br IL_03ca

		IL_03ca: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
		IL_03cf: throw

		IL_03d0: br IL_03d5

		IL_03d5: ldstr "Cannot load background: "
		IL_03da: ldloc 67
		IL_03de: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_03e3: call string [mscorlib]System.String::Concat(string, string)
		IL_03e8: stloc 65
		IL_03ec: ldstr "Method not found @325E011"
		IL_03f1: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03f6: br IL_03fb

		IL_03fb: ldstr "Method not found @325E234"
		IL_0400: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0405: ldloc 27
		IL_0409: stloc 41
		IL_040d: ldloc 65
		IL_0411: stloc 44
		IL_0415: ldloc 35
		IL_0419: stloc 36
		IL_041d: br IL_0458

		IL_0422: ldstr "Method not found @325E011"
		IL_0427: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_042c: ldloc 68
		IL_0430: throw

		IL_0431: br IL_0436

		IL_0436: ldloc 54
		IL_043a: ret

		IL_043b: ldloc 67
		IL_043f: ldc.i4 0
		IL_0444: ceq
		IL_0446: stloc 9
		IL_044a: ldloc 9
		IL_044e: brtrue IL_0315

		IL_0453: br IL_0005

		IL_0458: ldloc 36
		IL_045c: ldloc 44
		IL_0460: call instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_0465: ldc.i4 0
		IL_046a: stloc 42
		IL_046e: br IL_0422
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x437720
		// Header size: 12
		// Code size: 501 (0x1f5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] native int,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[16] bool,
			[17] bool,
			[18] string,
			[19] object,
			[20] class [System]System.IO.InvalidDataException,
			[21] valuetype [UnityEngine.CoreModule]UnityEngine.TextureFormat,
```
Context at line 18944:
```il
		IL_0453: br IL_0005

		IL_0458: ldloc 36
		IL_045c: ldloc 44
		IL_0460: call instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_0465: ldc.i4 0
		IL_046a: stloc 42
		IL_046e: br IL_0422
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x437720
		// Header size: 12
		// Code size: 501 (0x1f5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] native int,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[16] bool,
			[17] bool,
			[18] string,
			[19] object,
			[20] class [System]System.IO.InvalidDataException,
			[21] valuetype [UnityEngine.CoreModule]UnityEngine.TextureFormat,
			[22] object,
			[23] object,
			[24] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[25] object,
			[26] object,
			[27] object,
			[28] object,
			[29] object,
			[30] bool,
			[31] object,
			[32] object,
			[33] object,
			[34] object,
			[35] object,
			[36] object,
			[37] object,
			[38] object
		)

		IL_0000: br IL_01bb
		// loop start (head: IL_01bb)
			IL_0005: ldloc 15
			IL_0009: ldarg name
			IL_000d: call instance void [UnityEngine.CoreModule]UnityEngine.Object::set_name(string)
			IL_0012: ldloc 15
			IL_0016: ldc.i4 1
			IL_001b: call instance void [UnityEngine.CoreModule]UnityEngine.Texture::set_wrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.TextureWrapMode)
			IL_0020: br IL_0025

			IL_0025: ldloc 15
			IL_0029: ldarg bytes
			IL_002d: ldc.i4 1
			IL_0032: call bool [UnityEngine.ImageConversionModule]UnityEngine.ImageConversion::LoadImage(class [UnityEngine.CoreModule]UnityEngine.Texture2D, uint8[], bool)
			IL_0037: stloc 16
			IL_003b: ldloc 16
			IL_003f: ldc.i4.0
			IL_0040: ceq
			IL_0042: stloc 17
			IL_0046: ldc.i4 4
			IL_004b: stloc 21
			IL_004f: ldloc 17
			IL_0053: brtrue IL_0062

			IL_0058: br IL_005d

			IL_005d: ldloc 15
			IL_0061: ret

			IL_0062: ldstr "Cannot decode background image: "
			IL_0067: ldarg name
			IL_006b: call string [mscorlib]System.String::Concat(string, string)
			IL_0070: stloc 18
			IL_0074: br IL_0079

			IL_0079: ldstr "Method not found @325E011"
			IL_007e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0083: ldstr "Method not found @325E234"
			IL_0088: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_008d: br IL_0092

			IL_0092: ldloc 20
			IL_0096: ldloc 18
			IL_009a: call instance void [System]System.IO.InvalidDataException::.ctor(string)
			IL_009f: ldstr "Method not found @325E011"
			IL_00a4: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
```
Context at line 19139:
```il
			IL_01e1: ldc.i4 4
			IL_01e6: ldc.i4 0
			IL_01eb: call instance void [UnityEngine.CoreModule]UnityEngine.Texture2D::.ctor(int32, int32, valuetype [UnityEngine.CoreModule]UnityEngine.TextureFormat, bool)
			IL_01f0: br IL_0005
		// end loop
	} // end of method ScenarioResourceManager::DecodeBackgroundImage

	.method public hidebysig 
		instance void GetBackgroundTextureAsync (
			uint32 id,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x437924
		// Header size: 12
		// Code size: 154 (0x9a)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class ScenarioResourceManager,
			[3] class BackgroundResource,
			[4] uint32,
			[5] bool,
			[6] bool,
			[7] object,
			[8] object,
			[9] object,
			[10] native int,
			[11] object,
			[12] object,
			[13] object,
			[14] object,
			[15] object,
			[16] object,
			[17] object,
			[18] object,
			[19] object
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 3
		IL_000e: ldloc 3
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 5
		IL_001d: ldloc 5
		IL_0021: brtrue IL_0043

		IL_0026: br IL_002b

		IL_002b: ldarg.0
		IL_002c: ldloc 3
		IL_0030: ldarg preferThumbnail
		IL_0034: ldarg callback
		IL_0038: call instance void ScenarioResourceManager::GetBackgroundPreviewAsync(class BackgroundResource, bool, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
		IL_003d: br IL_0042

		IL_0042: ret

		IL_0043: ldarg callback
		IL_0047: ldc.i4 0
		IL_004c: ceq
		IL_004e: stloc 6
		IL_0052: ldloc 6
		IL_0056: brtrue IL_0042

		IL_005b: br IL_0060

		IL_0060: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+40]"
		IL_0065: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_006a: ldc.i4.0
		IL_006b: conv.i
		IL_006c: stloc 7
		IL_0070: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+18]"
		IL_0075: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_007a: ldc.i4.0
		IL_007b: conv.i
		IL_007c: stloc 8
		IL_0080: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+28]"
		IL_0085: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_008a: ldc.i4.0
		IL_008b: conv.i
		IL_008c: stloc 9
		IL_0090: ldstr "Indirect jump: v64 @ rax_v2 (should have been resolved before IL gen)"
		IL_0095: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundTextureAsync

	.method public hidebysig 
		instance void GetBackgroundPreviewAsync (
			class BackgroundResource resource,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4379cc
		// Header size: 12
		// Code size: 799 (0x31f)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] native int,
```
Context at line 19220:
```il
		IL_007c: stloc 8
		IL_0080: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+28]"
		IL_0085: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_008a: ldc.i4.0
		IL_008b: conv.i
		IL_008c: stloc 9
		IL_0090: ldstr "Indirect jump: v64 @ rax_v2 (should have been resolved before IL gen)"
		IL_0095: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundTextureAsync

	.method public hidebysig 
		instance void GetBackgroundPreviewAsync (
			class BackgroundResource resource,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4379cc
		// Header size: 12
		// Code size: 799 (0x31f)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] native int,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] bool,
			[15] bool,
			[16] object,
			[17] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[18] object,
			[19] object,
			[20] object,
			[21] string,
			[22] object,
			[23] object,
			[24] native int,
			[25] object&,
			[26] bool,
			[27] object,
			[28] object,
			[29] bool,
			[30] bool,
			[31] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[32] bool,
			[33] bool,
			[34] object,
			[35] bool,
			[36] bool,
			[37] bool,
			[38] native int,
			[39] object,
			[40] object,
			[41] bool,
			[42] native int,
			[43] object,
			[44] native int,
			[45] bool,
			[46] int32
		)

		IL_0000: br IL_02e8
		// loop start (head: IL_02e8)
			IL_0005: ldarg resource
			IL_0009: ldc.i4 0
			IL_000e: ceq
			IL_0010: stloc 15
			IL_0014: ldloc 15
			IL_0018: brtrue IL_02b2

			IL_001d: br IL_0022

			IL_0022: ldarg resource
			IL_0026: ldarg preferThumbnail
			IL_002a: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
			IL_002f: stloc 21
			IL_0033: br IL_0038

			IL_0038: ldc.i4.0
			IL_0039: conv.i
			IL_003a: stloc 24
			IL_003e: ldloc 24
			IL_0042: stloc 17
			IL_0046: ldarg.0
			IL_0047: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
			IL_004c: ldloc 21
			IL_0050: ldloc 25
			IL_0054: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
			IL_0059: stloc 26
			IL_005d: ldloc 26
			IL_0061: ldc.i4.0
			IL_0062: ceq
			IL_0064: stloc 35
			IL_0068: ldloc 35
			IL_006c: brtrue IL_0297

			IL_0071: br IL_0076

```
#### DecodeBackgroundImage: 3 occurrences
Context at line 18712:
```il
		IL_013e: ldloc 50
		IL_0142: call uint8[] [mscorlib]System.IO.File::ReadAllBytes(string)
		IL_0147: stloc 56
		IL_014b: br IL_0150

		IL_0150: ldloc 56
		IL_0154: ldloc 67
		IL_0158: ldfld string BackgroundResource::'<Name>k__BackingField'
		IL_015d: call class [UnityEngine.CoreModule]UnityEngine.Texture2D ScenarioResourceManager::DecodeBackgroundImage(uint8[], string)
		IL_0162: stloc 58
		IL_0166: ldloc 67
		IL_016a: call instance string BackgroundResource::get_FullPath()
		IL_016f: stloc 28
		IL_0173: br IL_0178

		IL_0178: ldarg.0
		IL_0179: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture> ScenarioResourceManager::_loadedCustomBackgroundTextures
		IL_017e: ldc.i4 0
		IL_0183: ceq
		IL_0185: stloc 30
		IL_0189: ldnull
		IL_018a: stloc 12
		IL_018e: ldloc 30
		IL_0192: brtrue IL_030a

		IL_0197: br IL_019c

		IL_019c: nop
		IL_019d: ldarg.0
		IL_019e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture> ScenarioResourceManager::_loadedCustomBackgroundTextures
		IL_01a3: ldloc 28
		IL_01a7: ldloc 58
		IL_01ab: call instance void class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::set_Item(!0, !1)
		IL_01b0: ldloc 58
		IL_01b4: stloc 59
		IL_01b8: br IL_01bd

		IL_01bd: ldloc 59
		IL_01c1: stloc 61
		IL_01c5: br IL_0305

		IL_01ca: ldarg.0
		IL_01cb: ldloc 67
		IL_01cf: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_01d4: call instance string ScenarioResourceManager::CorrectPath(string)
		IL_01d9: stloc 18
		IL_01dd: ldtoken [Unity.Addressables]UnityEngine.AddressableAssets.Addressables
		IL_01e2: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_01e7: stloc 21
		IL_01eb: ldloc 21
		IL_01ef: stloc 22
		IL_01f3: ldstr "Unmanaged memory load: [v155 @ rdi_v18+E4]"
		IL_01f8: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_01fd: ldc.i4.0
		IL_01fe: conv.i
		IL_01ff: ldc.i4 0
		IL_0204: ceq
		IL_0206: ldc.i4.0
		IL_0207: ceq
		IL_0209: stloc 23
		IL_020d: ldloc 23
		IL_0211: brtrue IL_022a

		IL_0216: br IL_021b

		IL_021b: ldstr "Method not found @325E158"
		IL_0220: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0225: br IL_022a

		IL_022a: ldc.i4.0
		IL_022b: conv.i
		IL_022c: stloc 33
		IL_0230: ldstr "Method not found @3D74F1D"
		IL_0235: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_023a: ldc.i4.0
		IL_023b: conv.i
		IL_023c: stloc 38
		IL_0240: ldloca 27
		IL_0244: call instance !0 valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>::WaitForCompletion()
		IL_0249: stloc 39
		IL_024d: br IL_0252

		IL_0252: ldtoken [UnityEngine.CoreModule]UnityEngine.Object
		IL_0257: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_025c: stloc 51
		IL_0260: ldloc 51
		IL_0264: stloc 52
		IL_0268: ldstr "Unmanaged memory load: [v373 @ rdi_v21+E4]"
		IL_026d: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0272: ldc.i4.0
		IL_0273: conv.i
		IL_0274: ldc.i4 0
		IL_0279: ceq
		IL_027b: ldc.i4.0
		IL_027c: ceq
		IL_027e: stloc 53
		IL_0282: ldloc 53
		IL_0286: brtrue IL_029f

		IL_028b: br IL_0290

		IL_0290: ldstr "Method not found @325E158"
		IL_0295: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_029a: br IL_029f

		IL_029f: ldloc 39
		IL_02a3: ldnull
		IL_02a4: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
```
Context at line 18947:
```il
		IL_045c: ldloc 44
		IL_0460: call instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_0465: ldc.i4 0
		IL_046a: stloc 42
		IL_046e: br IL_0422
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x437720
		// Header size: 12
		// Code size: 501 (0x1f5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] native int,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[16] bool,
			[17] bool,
			[18] string,
			[19] object,
			[20] class [System]System.IO.InvalidDataException,
			[21] valuetype [UnityEngine.CoreModule]UnityEngine.TextureFormat,
			[22] object,
			[23] object,
			[24] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[25] object,
			[26] object,
			[27] object,
			[28] object,
			[29] object,
			[30] bool,
			[31] object,
			[32] object,
			[33] object,
			[34] object,
			[35] object,
			[36] object,
			[37] object,
			[38] object
		)

		IL_0000: br IL_01bb
		// loop start (head: IL_01bb)
			IL_0005: ldloc 15
			IL_0009: ldarg name
			IL_000d: call instance void [UnityEngine.CoreModule]UnityEngine.Object::set_name(string)
			IL_0012: ldloc 15
			IL_0016: ldc.i4 1
			IL_001b: call instance void [UnityEngine.CoreModule]UnityEngine.Texture::set_wrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.TextureWrapMode)
			IL_0020: br IL_0025

			IL_0025: ldloc 15
			IL_0029: ldarg bytes
			IL_002d: ldc.i4 1
			IL_0032: call bool [UnityEngine.ImageConversionModule]UnityEngine.ImageConversion::LoadImage(class [UnityEngine.CoreModule]UnityEngine.Texture2D, uint8[], bool)
			IL_0037: stloc 16
			IL_003b: ldloc 16
			IL_003f: ldc.i4.0
			IL_0040: ceq
			IL_0042: stloc 17
			IL_0046: ldc.i4 4
			IL_004b: stloc 21
			IL_004f: ldloc 17
			IL_0053: brtrue IL_0062

			IL_0058: br IL_005d

			IL_005d: ldloc 15
			IL_0061: ret

			IL_0062: ldstr "Cannot decode background image: "
			IL_0067: ldarg name
			IL_006b: call string [mscorlib]System.String::Concat(string, string)
			IL_0070: stloc 18
			IL_0074: br IL_0079

			IL_0079: ldstr "Method not found @325E011"
			IL_007e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0083: ldstr "Method not found @325E234"
			IL_0088: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_008d: br IL_0092

			IL_0092: ldloc 20
			IL_0096: ldloc 18
			IL_009a: call instance void [System]System.IO.InvalidDataException::.ctor(string)
			IL_009f: ldstr "Method not found @325E011"
			IL_00a4: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00a9: br IL_00ae

			IL_00ae: ldloc 20
```
Context at line 19136:
```il
			IL_01d3: ldloc 15
			IL_01d7: ldc.i4 2
			IL_01dc: ldc.i4 2
			IL_01e1: ldc.i4 4
			IL_01e6: ldc.i4 0
			IL_01eb: call instance void [UnityEngine.CoreModule]UnityEngine.Texture2D::.ctor(int32, int32, valuetype [UnityEngine.CoreModule]UnityEngine.TextureFormat, bool)
			IL_01f0: br IL_0005
		// end loop
	} // end of method ScenarioResourceManager::DecodeBackgroundImage

	.method public hidebysig 
		instance void GetBackgroundTextureAsync (
			uint32 id,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x437924
		// Header size: 12
		// Code size: 154 (0x9a)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class ScenarioResourceManager,
			[3] class BackgroundResource,
			[4] uint32,
			[5] bool,
			[6] bool,
			[7] object,
			[8] object,
			[9] object,
			[10] native int,
			[11] object,
			[12] object,
			[13] object,
			[14] object,
			[15] object,
			[16] object,
			[17] object,
			[18] object,
			[19] object
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 3
		IL_000e: ldloc 3
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 5
		IL_001d: ldloc 5
		IL_0021: brtrue IL_0043

		IL_0026: br IL_002b

		IL_002b: ldarg.0
		IL_002c: ldloc 3
		IL_0030: ldarg preferThumbnail
		IL_0034: ldarg callback
		IL_0038: call instance void ScenarioResourceManager::GetBackgroundPreviewAsync(class BackgroundResource, bool, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
		IL_003d: br IL_0042

		IL_0042: ret

		IL_0043: ldarg callback
		IL_0047: ldc.i4 0
		IL_004c: ceq
		IL_004e: stloc 6
		IL_0052: ldloc 6
		IL_0056: brtrue IL_0042

		IL_005b: br IL_0060

		IL_0060: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+40]"
		IL_0065: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_006a: ldc.i4.0
		IL_006b: conv.i
		IL_006c: stloc 7
		IL_0070: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+18]"
		IL_0075: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_007a: ldc.i4.0
		IL_007b: conv.i
		IL_007c: stloc 8
		IL_0080: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+28]"
		IL_0085: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_008a: ldc.i4.0
		IL_008b: conv.i
		IL_008c: stloc 9
		IL_0090: ldstr "Indirect jump: v64 @ rax_v2 (should have been resolved before IL gen)"
		IL_0095: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundTextureAsync

	.method public hidebysig 
		instance void GetBackgroundPreviewAsync (
			class BackgroundResource resource,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4379cc
		// Header size: 12
		// Code size: 799 (0x31f)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
```
#### GetBackgroundPreviewAsync: 3 occurrences
Context at line 19189:
```il
		IL_0021: brtrue IL_0043

		IL_0026: br IL_002b

		IL_002b: ldarg.0
		IL_002c: ldloc 3
		IL_0030: ldarg preferThumbnail
		IL_0034: ldarg callback
		IL_0038: call instance void ScenarioResourceManager::GetBackgroundPreviewAsync(class BackgroundResource, bool, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
		IL_003d: br IL_0042

		IL_0042: ret

		IL_0043: ldarg callback
		IL_0047: ldc.i4 0
		IL_004c: ceq
		IL_004e: stloc 6
		IL_0052: ldloc 6
		IL_0056: brtrue IL_0042

		IL_005b: br IL_0060

		IL_0060: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+40]"
		IL_0065: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_006a: ldc.i4.0
		IL_006b: conv.i
		IL_006c: stloc 7
		IL_0070: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+18]"
		IL_0075: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_007a: ldc.i4.0
		IL_007b: conv.i
		IL_007c: stloc 8
		IL_0080: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+28]"
		IL_0085: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_008a: ldc.i4.0
		IL_008b: conv.i
		IL_008c: stloc 9
		IL_0090: ldstr "Indirect jump: v64 @ rax_v2 (should have been resolved before IL gen)"
		IL_0095: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundTextureAsync

	.method public hidebysig 
		instance void GetBackgroundPreviewAsync (
			class BackgroundResource resource,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4379cc
		// Header size: 12
		// Code size: 799 (0x31f)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] native int,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] bool,
			[15] bool,
			[16] object,
			[17] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[18] object,
			[19] object,
			[20] object,
			[21] string,
			[22] object,
			[23] object,
			[24] native int,
			[25] object&,
			[26] bool,
			[27] object,
			[28] object,
			[29] bool,
			[30] bool,
			[31] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[32] bool,
			[33] bool,
			[34] object,
			[35] bool,
			[36] bool,
			[37] bool,
			[38] native int,
			[39] object,
			[40] object,
			[41] bool,
			[42] native int,
			[43] object,
			[44] native int,
			[45] bool,
			[46] int32
		)

		IL_0000: br IL_02e8
		// loop start (head: IL_02e8)
			IL_0005: ldarg resource
			IL_0009: ldc.i4 0
			IL_000e: ceq
			IL_0010: stloc 15
```
Context at line 19223:
```il
		IL_008a: ldc.i4.0
		IL_008b: conv.i
		IL_008c: stloc 9
		IL_0090: ldstr "Indirect jump: v64 @ rax_v2 (should have been resolved before IL gen)"
		IL_0095: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundTextureAsync

	.method public hidebysig 
		instance void GetBackgroundPreviewAsync (
			class BackgroundResource resource,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4379cc
		// Header size: 12
		// Code size: 799 (0x31f)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] native int,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] bool,
			[15] bool,
			[16] object,
			[17] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[18] object,
			[19] object,
			[20] object,
			[21] string,
			[22] object,
			[23] object,
			[24] native int,
			[25] object&,
			[26] bool,
			[27] object,
			[28] object,
			[29] bool,
			[30] bool,
			[31] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[32] bool,
			[33] bool,
			[34] object,
			[35] bool,
			[36] bool,
			[37] bool,
			[38] native int,
			[39] object,
			[40] object,
			[41] bool,
			[42] native int,
			[43] object,
			[44] native int,
			[45] bool,
			[46] int32
		)

		IL_0000: br IL_02e8
		// loop start (head: IL_02e8)
			IL_0005: ldarg resource
			IL_0009: ldc.i4 0
			IL_000e: ceq
			IL_0010: stloc 15
			IL_0014: ldloc 15
			IL_0018: brtrue IL_02b2

			IL_001d: br IL_0022

			IL_0022: ldarg resource
			IL_0026: ldarg preferThumbnail
			IL_002a: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
			IL_002f: stloc 21
			IL_0033: br IL_0038

			IL_0038: ldc.i4.0
			IL_0039: conv.i
			IL_003a: stloc 24
			IL_003e: ldloc 24
			IL_0042: stloc 17
			IL_0046: ldarg.0
			IL_0047: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
			IL_004c: ldloc 21
			IL_0050: ldloc 25
			IL_0054: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
			IL_0059: stloc 26
			IL_005d: ldloc 26
			IL_0061: ldc.i4.0
			IL_0062: ceq
			IL_0064: stloc 35
			IL_0068: ldloc 35
			IL_006c: brtrue IL_0297

			IL_0071: br IL_0076

			IL_0076: ldc.i4 0
			IL_007b: stloc 28
			IL_007f: br IL_0084
```
Context at line 19531:
```il
		IL_0305: ldstr "Indirect call: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+18] (should have been resolved before IL gen)"
		IL_030a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_030f: br IL_0314

		IL_0314: ret

		IL_0315: ldstr "Warning: Method ends with non empty stack (-38), the output could be wrong!"
		IL_031a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundPreviewAsync

	.method private hidebysig static 
		string BackgroundPreviewKey (
			class BackgroundResource resource,
			bool preferThumbnail
		) cil managed 
	{
		// Method begins at RVA 0x437cf8
		// Header size: 12
		// Code size: 1515 (0x5eb)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] native int,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] valuetype ScenarioResourceManager/'<>c__DisplayClass24_0',
			[15] string,
			[16] native int,
			[17] object,
			[18] string,
			[19] string[],
			[20] string[],
			[21] object,
			[22] object,
			[23] object,
			[24] object,
			[25] object,
			[26] object,
			[27] bool,
			[28] bool,
			[29] bool,
			[30] bool,
			[31] bool,
			[32] bool,
			[33] object,
			[34] object,
			[35] string,
			[36] string,
			[37] string,
			[38] string,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] bool,
			[44] bool,
			[45] bool,
			[46] bool,
			[47] bool,
			[48] class [mscorlib]System.IndexOutOfRangeException,
			[49] object,
			[50] string,
			[51] bool,
			[52] bool,
			[53] string,
			[54] string,
			[55] bool,
			[56] string,
			[57] object,
			[58] string,
			[59] object,
			[60] string,
			[61] object,
			[62] string,
			[63] bool,
			[64] bool,
			[65] object,
			[66] string,
			[67] object,
			[68] string,
			[69] object,
			[70] bool
		)

		IL_0000: br IL_05a5

		IL_0005: ldloc 13
		IL_0009: call instance string BackgroundResource::get_CacheKey()
		IL_000e: stloc 15
		IL_0012: ldc.i4 123410272
		IL_0017: stloc 33
		IL_001b: ldstr "Unmanaged memory load: [v246 @ rax_v8+28]"
		IL_0020: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0025: ldc.i4.0
		IL_0026: conv.i
		IL_0027: stloc 34
		IL_002b: ldstr "Unmanaged memory load: [v247 @ rdi_v6+E4]"
		IL_0030: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
```
#### QueueBackgroundPreview: 4 occurrences
Context at line 19494:
```il
			IL_028e: stloc 20
			IL_0292: br IL_0305

			IL_0297: ldarg.0
			IL_0298: ldarg resource
			IL_029c: ldarg preferThumbnail
			IL_02a0: ldloc 21
			IL_02a4: ldarg callback
			IL_02a8: call instance void ScenarioResourceManager::QueueBackgroundPreview(class BackgroundResource, bool, string, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
			IL_02ad: br IL_0314

			IL_02b2: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+28]"
			IL_02b7: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_02bc: ldc.i4.0
			IL_02bd: conv.i
			IL_02be: stloc 16
			IL_02c2: ldstr "Unmanaged memory load: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+40]"
			IL_02c7: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_02cc: ldc.i4.0
			IL_02cd: conv.i
			IL_02ce: stloc 20
			IL_02d2: ldarg callback
			IL_02d6: stloc 17
			IL_02da: ldc.i4 0
			IL_02df: stloc 18
			IL_02e3: br IL_0305

			IL_02e8: ldarg callback
			IL_02ec: ldc.i4 0
			IL_02f1: ceq
			IL_02f3: stloc 14
			IL_02f7: ldloc 14
			IL_02fb: brtrue IL_0314

			IL_0300: br IL_0005
		// end loop

		IL_0305: ldstr "Indirect call: [callback @ rcx (System.Action`1<UnityEngine.Texture>)+18] (should have been resolved before IL gen)"
		IL_030a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_030f: br IL_0314

		IL_0314: ret

		IL_0315: ldstr "Warning: Method ends with non empty stack (-38), the output could be wrong!"
		IL_031a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundPreviewAsync

	.method private hidebysig static 
		string BackgroundPreviewKey (
			class BackgroundResource resource,
			bool preferThumbnail
		) cil managed 
	{
		// Method begins at RVA 0x437cf8
		// Header size: 12
		// Code size: 1515 (0x5eb)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] native int,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] valuetype ScenarioResourceManager/'<>c__DisplayClass24_0',
			[15] string,
			[16] native int,
			[17] object,
			[18] string,
			[19] string[],
			[20] string[],
			[21] object,
			[22] object,
			[23] object,
			[24] object,
			[25] object,
			[26] object,
			[27] bool,
			[28] bool,
			[29] bool,
			[30] bool,
			[31] bool,
			[32] bool,
			[33] object,
			[34] object,
			[35] string,
			[36] string,
			[37] string,
			[38] string,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] bool,
			[44] bool,
			[45] bool,
			[46] bool,
			[47] bool,
			[48] class [mscorlib]System.IndexOutOfRangeException,
			[49] object,
```
Context at line 20067:
```il
		IL_05d9: stloc 45
		IL_05dd: ldloc 45
		IL_05e1: brtrue IL_0597

		IL_05e6: br IL_0479
	} // end of method ScenarioResourceManager::BackgroundPreviewKey

	.method private hidebysig 
		instance void QueueBackgroundPreview (
			class BackgroundResource resource,
			bool preferThumbnail,
			string key,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4382f0
		// Header size: 12
		// Code size: 587 (0x24b)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] string,
			[2] bool,
			[3] class BackgroundResource,
			[4] class ScenarioResourceManager,
			[5] native int,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] object,
			[16] bool,
			[17] object[],
			[18] object[],
			[19] bool,
			[20] native int,
			[21] class [mscorlib]System.Collections.Generic.List`1<object>,
			[22] bool,
			[23] int32,
			[24] object,
			[25] object,
			[26] object,
			[27] bool,
			[28] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[29] int32,
			[30] object,
			[31] object,
			[32] object,
			[33] class [mscorlib]System.Collections.IEnumerator,
			[34] int32,
			[35] int32
		)

		IL_0000: br IL_0005

		IL_0005: ldc.i4.0
		IL_0006: conv.i
		IL_0007: stloc 14
		IL_000b: ldarg.0
		IL_000c: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>> ScenarioResourceManager::pendingBackgroundPreviews
		IL_0011: ldarg key
		IL_0015: ldloca 15
		IL_0019: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_001e: stloc 16
		IL_0022: ldloc 16
		IL_0026: ldc.i4.0
		IL_0027: ceq
		IL_0029: stloc 19
		IL_002d: ldloc 19
		IL_0031: brtrue IL_00fa

		IL_0036: br IL_003b

		IL_003b: nop
		IL_003c: nop
		IL_003d: ldloc 15
		IL_0041: ldfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_version
		IL_0046: ldc.i4 1
		IL_004b: add
		IL_004c: stloc 34
		IL_0050: ldloc 15
		IL_0054: ldloc 34
		IL_0058: stfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_version
		IL_005d: ldloc 15
		IL_0061: ldfld !0[] class [mscorlib]System.Collections.Generic.List`1<object>::_items
		IL_0066: stloc 17
		IL_006a: br IL_006f

		IL_006f: ldloc 15
		IL_0073: ldfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_size
		IL_0078: ldloc 17
		IL_007c: ldlen
		IL_007d: conv.i4
		IL_007e: clt
		IL_0080: ldc.i4.0
		IL_0081: ceq
		IL_0083: stloc 22
		IL_0087: ldloc 22
		IL_008b: brtrue IL_01e0

		IL_0090: br IL_0095

		IL_0095: ldloc 15
```
Context at line 20294:
```il

		IL_0237: ldarg.0
		IL_0238: ldloc 33
		IL_023c: call instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_0241: stloc 28
		IL_0245: br IL_024a

		IL_024a: ret
	} // end of method ScenarioResourceManager::QueueBackgroundPreview

	.method private hidebysig static 
		string BackgroundPreviewCachePath (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x438548
		// Header size: 12
		// Code size: 143 (0x8f)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] bool,
			[2] bool,
			[3] string,
			[4] class [mscorlib]System.NullReferenceException,
			[5] object,
			[6] string
		)

		IL_0000: ldstr "Unmanaged memory load: [759A8C2]"
		IL_0005: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_000a: ldc.i4.0
		IL_000b: conv.i
		IL_000c: ldc.i4 0
		IL_0011: ceq
		IL_0013: ldc.i4.0
		IL_0014: ceq
		IL_0016: stloc 1
		IL_001a: ldloc 1
		IL_001e: brtrue IL_0072

		IL_0023: br IL_0028

		IL_0028: ldc.i4 1
		IL_002d: pop
		IL_002e: br IL_0072
		// loop start (head: IL_0072)
			IL_0033: ldarg resource
			IL_0037: call instance string BackgroundResource::get_FullPath()
			IL_003c: stloc 3
			IL_0040: ldstr ".skel.preview.png"
			IL_0045: stloc 5
			IL_0049: ldloc 3
			IL_004d: ldloc 5
			IL_0051: call string [mscorlib]System.String::Concat(string, string)
			IL_0056: stloc 6
			IL_005a: br IL_005f

			IL_005f: ldloc 6
			IL_0063: ret
			// loop start (head: IL_0072)
				IL_0064: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
				IL_0069: stloc 4
				IL_006d: ldloc 4
				IL_0071: ret

				IL_0072: ldarg resource
				IL_0076: ldc.i4 0
				IL_007b: ceq
				IL_007d: stloc 2
				IL_0081: ldloc 2
				IL_0085: brtrue IL_0064
			// end loop

			IL_008a: br IL_0033
		// end loop
	} // end of method ScenarioResourceManager::BackgroundPreviewCachePath

	.method private hidebysig static 
		void SaveBackgroundPreview (
			class [UnityEngine.CoreModule]UnityEngine.RenderTexture texture,
			string path
		) cil managed 
	{
		// Method begins at RVA 0x4385e4
		// Header size: 12
		// Code size: 1434 (0x59a)
		.maxstack 0
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[1] native int,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] string,
			[11] object,
			[12] object,
			[13] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[14] object,
			[15] object,
			[16] valuetype [mscorlib]System.Guid,
			[17] valuetype [mscorlib]System.Guid,
			[18] object,
```
Context at line 23644:
```il
				IL_01c0: ldfld class BackgroundResource ScenarioResourceManager/BackgroundPreview::Resource
				IL_01c5: ldloc 22
				IL_01c9: ldfld bool ScenarioResourceManager/BackgroundPreview::PreferThumbnail
				IL_01ce: ldstr "Unmanaged memory load: [v61 @ rax_v4+20+v209 @ rax_v11]"
				IL_01d3: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
				IL_01d8: ldc.i4.0
				IL_01d9: conv.i
				IL_01da: ldloc 32
				IL_01de: call instance void ScenarioResourceManager::QueueBackgroundPreview(class BackgroundResource, bool, string, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
				IL_01e3: ldloc 22
				IL_01e7: ldfld bool ScenarioResourceManager/BackgroundPreview::PreferThumbnail
				IL_01ec: stloc 14
				IL_01f0: ldstr "Unmanaged memory load: [v61 @ rax_v4+20+v209 @ rax_v11]"
				IL_01f5: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
				IL_01fa: ldc.i4.0
				IL_01fb: conv.i
				IL_01fc: stloc 15
				IL_0200: ldloc 22
				IL_0204: ldfld class BackgroundResource ScenarioResourceManager/BackgroundPreview::Resource
				IL_0209: stloc 17
				IL_020d: br IL_025e

				IL_0212: ldloc 22
				IL_0216: call void ScenarioResourceManager::DestroyBackgroundPreview(class ScenarioResourceManager/BackgroundPreview)
				IL_021b: br IL_0220

				IL_0220: ldc.i4.0
				IL_0221: conv.i
				IL_0222: stloc 34
				IL_0226: ldloc 34
				IL_022a: stloc 14
				IL_022e: ldarg.0
				IL_022f: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
				IL_0234: ldstr "Unmanaged memory load: [v61 @ rax_v4+20+v209 @ rax_v11]"
				IL_0239: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
				IL_023e: ldc.i4.0
				IL_023f: conv.i
				IL_0240: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::Remove(!0)
				IL_0245: stloc 27
				IL_0249: ldstr "Unmanaged memory load: [v61 @ rax_v4+20+v209 @ rax_v11]"
				IL_024e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
				IL_0253: ldc.i4.0
				IL_0254: conv.i
				IL_0255: stloc 17
				IL_0259: br IL_025e

				IL_025e: ldloc 20
				IL_0262: ldc.i4 1
				IL_0267: add
				IL_0268: stloc 20
				IL_026c: ldloc 20
				IL_0270: ldstr "Unmanaged memory load: [v61 @ rax_v4+18]"
				IL_0275: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
				IL_027a: ldc.i4.0
				IL_027b: conv.i
				IL_027c: clt
				IL_027e: stloc 18
				IL_0282: ldloc 18
				IL_0286: brtrue IL_0066
			// end loop

			IL_028b: br IL_0290

			IL_0290: ret

			IL_0291: ldarg focused
			IL_0295: ldc.i4.0
			IL_0296: ceq
			IL_0298: stloc 11
			IL_029c: ldloc 11
			IL_02a0: brtrue IL_0290

			IL_02a5: br IL_0005
		// end loop

		IL_02aa: ldstr "Warning: Method ends with non empty stack (-48), the output could be wrong!"
		IL_02af: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::OnApplicationFocus

	.method private hidebysig static 
		void ReportBackgroundError (
			class [mscorlib]System.Exception 'error'
		) cil managed 
	{
		// Method begins at RVA 0x43aa78
		// Header size: 12
		// Code size: 534 (0x216)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Exception,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] bool,
			[14] string,
			[15] bool,
			[16] object,
			[17] string,
			[18] string,
```
#### BackgroundPreviewKey: 10 occurrences
Context at line 19296:
```il
			IL_0010: stloc 15
			IL_0014: ldloc 15
			IL_0018: brtrue IL_02b2

			IL_001d: br IL_0022

			IL_0022: ldarg resource
			IL_0026: ldarg preferThumbnail
			IL_002a: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
			IL_002f: stloc 21
			IL_0033: br IL_0038

			IL_0038: ldc.i4.0
			IL_0039: conv.i
			IL_003a: stloc 24
			IL_003e: ldloc 24
			IL_0042: stloc 17
			IL_0046: ldarg.0
			IL_0047: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
			IL_004c: ldloc 21
			IL_0050: ldloc 25
			IL_0054: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
			IL_0059: stloc 26
			IL_005d: ldloc 26
			IL_0061: ldc.i4.0
			IL_0062: ceq
			IL_0064: stloc 35
			IL_0068: ldloc 35
			IL_006c: brtrue IL_0297

			IL_0071: br IL_0076

			IL_0076: ldc.i4 0
			IL_007b: stloc 28
			IL_007f: br IL_0084

			IL_0084: ldtoken [UnityEngine.CoreModule]UnityEngine.Object
			IL_0089: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			IL_008e: stloc 38
			IL_0092: ldloc 38
			IL_0096: stloc 39
			IL_009a: ldstr "Unmanaged memory load: [v304 @ rdi_v10+E4]"
			IL_009f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00a4: ldc.i4.0
			IL_00a5: conv.i
			IL_00a6: ldc.i4 0
			IL_00ab: ceq
			IL_00ad: ldc.i4.0
			IL_00ae: ceq
			IL_00b0: stloc 32
			IL_00b4: ldloc 32
			IL_00b8: brtrue IL_00d1

			IL_00bd: br IL_00c2

			IL_00c2: ldstr "Method not found @325E158"
			IL_00c7: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00cc: br IL_00d1

			IL_00d1: ldstr "Unmanaged memory load: [v227 @ rax_v12+10]"
			IL_00d6: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00db: ldc.i4.0
			IL_00dc: conv.i
			IL_00dd: ldnull
			IL_00de: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_00e3: stloc 29
			IL_00e7: ldloc 29
			IL_00eb: ldc.i4.0
			IL_00ec: ceq
			IL_00ee: stloc 36
			IL_00f2: ldloc 36
			IL_00f6: brtrue IL_0297

			IL_00fb: br IL_0100

			IL_0100: ldc.i4 0
			IL_0105: stloc 34
			IL_0109: br IL_010e

			IL_010e: ldstr "Unmanaged memory load: [v265 @ rbp_v8+10]"
			IL_0113: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0118: ldc.i4.0
			IL_0119: conv.i
			IL_011a: stloc 31
			IL_011e: ldstr "Unmanaged memory load: [v265 @ rbp_v8+10]"
			IL_0123: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0128: ldc.i4.0
			IL_0129: conv.i
			IL_012a: ldc.i4 0
			IL_012f: ceq
			IL_0131: stloc 41
			IL_0135: ldc.i4 0
			IL_013a: stloc 23
			IL_013e: ldloc 41
			IL_0142: brtrue IL_0233

			IL_0147: br IL_014c

			IL_014c: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
			IL_0151: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			IL_0156: stloc 42
			IL_015a: ldloc 42
			IL_015e: stloc 43
			IL_0162: ldloc 31
			IL_0166: stloc 44
			IL_016a: ldstr "Unmanaged memory load: [v328 @ rax_v20+130]"
			IL_016f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0174: ldc.i4.0
```
Context at line 19534:
```il

		IL_0314: ret

		IL_0315: ldstr "Warning: Method ends with non empty stack (-38), the output could be wrong!"
		IL_031a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundPreviewAsync

	.method private hidebysig static 
		string BackgroundPreviewKey (
			class BackgroundResource resource,
			bool preferThumbnail
		) cil managed 
	{
		// Method begins at RVA 0x437cf8
		// Header size: 12
		// Code size: 1515 (0x5eb)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] native int,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] valuetype ScenarioResourceManager/'<>c__DisplayClass24_0',
			[15] string,
			[16] native int,
			[17] object,
			[18] string,
			[19] string[],
			[20] string[],
			[21] object,
			[22] object,
			[23] object,
			[24] object,
			[25] object,
			[26] object,
			[27] bool,
			[28] bool,
			[29] bool,
			[30] bool,
			[31] bool,
			[32] bool,
			[33] object,
			[34] object,
			[35] string,
			[36] string,
			[37] string,
			[38] string,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] bool,
			[44] bool,
			[45] bool,
			[46] bool,
			[47] bool,
			[48] class [mscorlib]System.IndexOutOfRangeException,
			[49] object,
			[50] string,
			[51] bool,
			[52] bool,
			[53] string,
			[54] string,
			[55] bool,
			[56] string,
			[57] object,
			[58] string,
			[59] object,
			[60] string,
			[61] object,
			[62] string,
			[63] bool,
			[64] bool,
			[65] object,
			[66] string,
			[67] object,
			[68] string,
			[69] object,
			[70] bool
		)

		IL_0000: br IL_05a5

		IL_0005: ldloc 13
		IL_0009: call instance string BackgroundResource::get_CacheKey()
		IL_000e: stloc 15
		IL_0012: ldc.i4 123410272
		IL_0017: stloc 33
		IL_001b: ldstr "Unmanaged memory load: [v246 @ rax_v8+28]"
		IL_0020: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0025: ldc.i4.0
		IL_0026: conv.i
		IL_0027: stloc 34
		IL_002b: ldstr "Unmanaged memory load: [v247 @ rdi_v6+E4]"
		IL_0030: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0035: ldc.i4.0
		IL_0036: conv.i
		IL_0037: ldc.i4 0
```
Context at line 19747:
```il
		IL_01a7: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_01ac: ldc.i4.0
		IL_01ad: conv.i
		IL_01ae: ldloc 57
		IL_01b2: call string [mscorlib]System.String::Concat(string, string)
		IL_01b7: stloc 58
		IL_01bb: ldloc 58
		IL_01bf: ldloca 14
		IL_01c3: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_01c8: stloc 36
		IL_01cc: br IL_01d1

		IL_01d1: ldloc 19
		IL_01d5: ldlen
		IL_01d6: conv.i4
		IL_01d7: ldc.i4 2
		IL_01dc: cgt
		IL_01de: ldc.i4.0
		IL_01df: ceq
		IL_01e1: stloc 43
		IL_01e5: ldloc 43
		IL_01e9: brtrue IL_0597

		IL_01ee: br IL_01f3

		IL_01f3: ldloc 19
		IL_01f7: ldc.i4 48
		IL_01fc: add
		IL_01fd: stloc 40
		IL_0201: ldloc 19
		IL_0205: ldc.i4 2
		IL_020a: ldloc 36
		IL_020e: stelem [mscorlib]System.String
		IL_0213: ldstr "Method not found @325DFB3"
		IL_0218: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_021d: ldloc 19
		IL_0221: ldlen
		IL_0222: conv.i4
		IL_0223: ldc.i4 3
		IL_0228: cgt
		IL_022a: ldc.i4.0
		IL_022b: ceq
		IL_022d: stloc 29
		IL_0231: ldloc 29
		IL_0235: brtrue IL_0597

		IL_023a: br IL_023f

		IL_023f: ldloc 19
		IL_0243: ldc.i4 56
		IL_0248: add
		IL_0249: stloc 24
		IL_024d: ldloc 19
		IL_0251: ldc.i4 3
		IL_0256: ldloc 17
		IL_025a: stelem [mscorlib]System.String
		IL_025f: ldstr "Method not found @325DFB3"
		IL_0264: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0269: br IL_026e

		IL_026e: ldstr ".atlas"
		IL_0273: stloc 59
		IL_0277: ldstr "Unmanaged memory load: [v43 @ stack_-28_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+30]"
		IL_027c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0281: ldc.i4.0
		IL_0282: conv.i
		IL_0283: ldloc 59
		IL_0287: call string [mscorlib]System.String::Concat(string, string)
		IL_028c: stloc 60
		IL_0290: ldloc 60
		IL_0294: ldloca 14
		IL_0298: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_029d: stloc 37
		IL_02a1: br IL_02a6

		IL_02a6: ldloc 19
		IL_02aa: ldlen
		IL_02ab: conv.i4
		IL_02ac: ldc.i4 4
		IL_02b1: cgt
		IL_02b3: ldc.i4.0
		IL_02b4: ceq
		IL_02b6: stloc 30
		IL_02ba: ldloc 30
		IL_02be: brtrue IL_0597

		IL_02c3: br IL_02c8

		IL_02c8: ldloc 19
		IL_02cc: ldc.i4 64
		IL_02d1: add
		IL_02d2: stloc 61
		IL_02d6: ldloc 19
		IL_02da: ldc.i4 4
		IL_02df: ldloc 37
		IL_02e3: stelem [mscorlib]System.String
		IL_02e8: ldstr "Method not found @325DFB3"
		IL_02ed: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_02f2: ldloc 19
		IL_02f6: call string [mscorlib]System.String::Concat(string[])
		IL_02fb: stloc 62
		IL_02ff: br IL_0304

		IL_0304: ldstr "Method not found @325E0D3"
		IL_0309: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_030e: br IL_0313

		IL_0313: ldloc 20
```
Context at line 19810:
```il
		IL_027c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0281: ldc.i4.0
		IL_0282: conv.i
		IL_0283: ldloc 59
		IL_0287: call string [mscorlib]System.String::Concat(string, string)
		IL_028c: stloc 60
		IL_0290: ldloc 60
		IL_0294: ldloca 14
		IL_0298: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_029d: stloc 37
		IL_02a1: br IL_02a6

		IL_02a6: ldloc 19
		IL_02aa: ldlen
		IL_02ab: conv.i4
		IL_02ac: ldc.i4 4
		IL_02b1: cgt
		IL_02b3: ldc.i4.0
		IL_02b4: ceq
		IL_02b6: stloc 30
		IL_02ba: ldloc 30
		IL_02be: brtrue IL_0597

		IL_02c3: br IL_02c8

		IL_02c8: ldloc 19
		IL_02cc: ldc.i4 64
		IL_02d1: add
		IL_02d2: stloc 61
		IL_02d6: ldloc 19
		IL_02da: ldc.i4 4
		IL_02df: ldloc 37
		IL_02e3: stelem [mscorlib]System.String
		IL_02e8: ldstr "Method not found @325DFB3"
		IL_02ed: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_02f2: ldloc 19
		IL_02f6: call string [mscorlib]System.String::Concat(string[])
		IL_02fb: stloc 62
		IL_02ff: br IL_0304

		IL_0304: ldstr "Method not found @325E0D3"
		IL_0309: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_030e: br IL_0313

		IL_0313: ldloc 20
		IL_0317: ldlen
		IL_0318: conv.i4
		IL_0319: ldc.i4 0
		IL_031e: ceq
		IL_0320: stloc 47
		IL_0324: ldloc 47
		IL_0328: brtrue IL_0597

		IL_032d: br IL_0332

		IL_0332: ldloc 20
		IL_0336: ldc.i4 32
		IL_033b: add
		IL_033c: stloc 41
		IL_0340: ldloc 20
		IL_0344: ldc.i4 0
		IL_0349: ldloc 62
		IL_034d: stelem [mscorlib]System.String
		IL_0352: ldstr "Method not found @325DFB3"
		IL_0357: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_035c: ldloc 20
		IL_0360: ldlen
		IL_0361: conv.i4
		IL_0362: ldc.i4 1
		IL_0367: cgt
		IL_0369: ldc.i4.0
		IL_036a: ceq
		IL_036c: stloc 31
		IL_0370: ldloc 31
		IL_0374: brtrue IL_0597

		IL_0379: br IL_037e

		IL_037e: ldloc 20
		IL_0382: ldc.i4 40
		IL_0387: add
		IL_0388: stloc 25
		IL_038c: ldloc 20
		IL_0390: ldc.i4 1
		IL_0395: ldloc 17
		IL_0399: stelem [mscorlib]System.String
		IL_039e: ldstr "Method not found @325DFB3"
		IL_03a3: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03a8: br IL_03ad

		IL_03ad: ldstr "Unmanaged memory load: [v43 @ stack_-28_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+38]"
		IL_03b2: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03b7: ldc.i4.0
		IL_03b8: conv.i
		IL_03b9: stloc 21
		IL_03bd: br IL_03c2

		IL_03c2: ldstr "Unmanaged memory load: [v99 @ rax_v38+38]"
		IL_03c7: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_03cc: ldc.i4.0
		IL_03cd: conv.i
		IL_03ce: call bool [mscorlib]System.String::IsNullOrWhiteSpace(string)
		IL_03d3: stloc 63
		IL_03d7: ldloc 63
		IL_03db: ldc.i4.0
		IL_03dc: ceq
		IL_03de: stloc 64
		IL_03e2: ldloc 64
```
Context at line 19925:
```il
		IL_03fd: stloc 35
		IL_0401: br IL_05c9

		IL_0406: ldstr "Unmanaged memory load: [v43 @ stack_-28_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+30]"
		IL_040b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0410: ldc.i4.0
		IL_0411: conv.i
		IL_0412: ldloca 14
		IL_0416: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_041b: stloc 56
		IL_041f: ldloc 18
		IL_0423: ldloc 17
		IL_0427: ldloc 56
		IL_042b: call string [mscorlib]System.String::Concat(string, string, string)
		IL_0430: stloc 53
		IL_0434: br IL_0439

		IL_0439: br IL_058a

		IL_043e: ldstr "Unmanaged memory load: [v43 @ stack_-28_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+38]"
		IL_0443: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0448: ldc.i4.0
		IL_0449: conv.i
		IL_044a: stloc 22
		IL_044e: br IL_0453

		IL_0453: ldstr "Unmanaged memory load: [v102 @ rax_v50+38]"
		IL_0458: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_045d: ldc.i4.0
		IL_045e: conv.i
		IL_045f: ldloca 14
		IL_0463: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0468: stloc 66
		IL_046c: ldloc 66
		IL_0470: stloc 35
		IL_0474: br IL_05c9

		IL_0479: ldloc 20
		IL_047d: ldc.i4 48
		IL_0482: add
		IL_0483: stloc 42
		IL_0487: ldloc 20
		IL_048b: ldc.i4 2
		IL_0490: ldloc 35
		IL_0494: stelem [mscorlib]System.String
		IL_0499: ldstr "Method not found @325DFB3"
		IL_049e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_04a3: ldloc 20
		IL_04a7: ldlen
		IL_04a8: conv.i4
		IL_04a9: ldc.i4 3
		IL_04ae: cgt
		IL_04b0: ldc.i4.0
		IL_04b1: ceq
		IL_04b3: stloc 32
		IL_04b7: ldloc 32
		IL_04bb: brtrue IL_0597

		IL_04c0: br IL_04c5

		IL_04c5: ldloc 20
		IL_04c9: ldc.i4 56
		IL_04ce: add
		IL_04cf: stloc 26
		IL_04d3: ldloc 20
		IL_04d7: ldc.i4 3
		IL_04dc: ldloc 17
		IL_04e0: stelem [mscorlib]System.String
		IL_04e5: ldstr "Method not found @325DFB3"
		IL_04ea: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_04ef: br IL_04f4

		IL_04f4: ldstr ".skel.preview.png"
		IL_04f9: stloc 67
		IL_04fd: ldstr "Unmanaged memory load: [v43 @ stack_-28_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+30]"
		IL_0502: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0507: ldc.i4.0
		IL_0508: conv.i
		IL_0509: ldloc 67
		IL_050d: call string [mscorlib]System.String::Concat(string, string)
		IL_0512: stloc 68
		IL_0516: ldloc 68
		IL_051a: ldloca 14
		IL_051e: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0523: stloc 38
		IL_0527: br IL_052c

		IL_052c: ldloc 20
		IL_0530: ldlen
		IL_0531: conv.i4
		IL_0532: ldc.i4 4
		IL_0537: cgt
		IL_0539: ldc.i4.0
		IL_053a: ceq
		IL_053c: stloc 44
		IL_0540: ldloc 44
		IL_0544: brtrue IL_0597

		IL_0549: br IL_054e

		IL_054e: ldloc 20
		IL_0552: ldc.i4 64
		IL_0557: add
		IL_0558: stloc 69
		IL_055c: ldloc 20
		IL_0560: ldc.i4 4
		IL_0565: ldloc 38
		IL_0569: stelem [mscorlib]System.String
```
Context at line 19948:
```il
		IL_044a: stloc 22
		IL_044e: br IL_0453

		IL_0453: ldstr "Unmanaged memory load: [v102 @ rax_v50+38]"
		IL_0458: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_045d: ldc.i4.0
		IL_045e: conv.i
		IL_045f: ldloca 14
		IL_0463: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0468: stloc 66
		IL_046c: ldloc 66
		IL_0470: stloc 35
		IL_0474: br IL_05c9

		IL_0479: ldloc 20
		IL_047d: ldc.i4 48
		IL_0482: add
		IL_0483: stloc 42
		IL_0487: ldloc 20
		IL_048b: ldc.i4 2
		IL_0490: ldloc 35
		IL_0494: stelem [mscorlib]System.String
		IL_0499: ldstr "Method not found @325DFB3"
		IL_049e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_04a3: ldloc 20
		IL_04a7: ldlen
		IL_04a8: conv.i4
		IL_04a9: ldc.i4 3
		IL_04ae: cgt
		IL_04b0: ldc.i4.0
		IL_04b1: ceq
		IL_04b3: stloc 32
		IL_04b7: ldloc 32
		IL_04bb: brtrue IL_0597

		IL_04c0: br IL_04c5

		IL_04c5: ldloc 20
		IL_04c9: ldc.i4 56
		IL_04ce: add
		IL_04cf: stloc 26
		IL_04d3: ldloc 20
		IL_04d7: ldc.i4 3
		IL_04dc: ldloc 17
		IL_04e0: stelem [mscorlib]System.String
		IL_04e5: ldstr "Method not found @325DFB3"
		IL_04ea: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_04ef: br IL_04f4

		IL_04f4: ldstr ".skel.preview.png"
		IL_04f9: stloc 67
		IL_04fd: ldstr "Unmanaged memory load: [v43 @ stack_-28_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+30]"
		IL_0502: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0507: ldc.i4.0
		IL_0508: conv.i
		IL_0509: ldloc 67
		IL_050d: call string [mscorlib]System.String::Concat(string, string)
		IL_0512: stloc 68
		IL_0516: ldloc 68
		IL_051a: ldloca 14
		IL_051e: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0523: stloc 38
		IL_0527: br IL_052c

		IL_052c: ldloc 20
		IL_0530: ldlen
		IL_0531: conv.i4
		IL_0532: ldc.i4 4
		IL_0537: cgt
		IL_0539: ldc.i4.0
		IL_053a: ceq
		IL_053c: stloc 44
		IL_0540: ldloc 44
		IL_0544: brtrue IL_0597

		IL_0549: br IL_054e

		IL_054e: ldloc 20
		IL_0552: ldc.i4 64
		IL_0557: add
		IL_0558: stloc 69
		IL_055c: ldloc 20
		IL_0560: ldc.i4 4
		IL_0565: ldloc 38
		IL_0569: stelem [mscorlib]System.String
		IL_056e: ldstr "Method not found @325DFB3"
		IL_0573: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0578: ldloc 20
		IL_057c: call string [mscorlib]System.String::Concat(string[])
		IL_0581: stloc 53
		IL_0585: br IL_058a

		IL_058a: ldloc 53
		IL_058e: stloc 54
		IL_0592: br IL_05c4

		IL_0597: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_059c: stloc 48
		IL_05a0: ldloc 48
		IL_05a4: ret

		IL_05a5: ldloca 14
		IL_05a9: stloc 13
		IL_05ad: ldarg resource
		IL_05b1: stloc 13
		IL_05b5: ldstr "Method not found @325DFB3"
		IL_05ba: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_05bf: br IL_0005
```
Context at line 20000:
```il
		IL_0502: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0507: ldc.i4.0
		IL_0508: conv.i
		IL_0509: ldloc 67
		IL_050d: call string [mscorlib]System.String::Concat(string, string)
		IL_0512: stloc 68
		IL_0516: ldloc 68
		IL_051a: ldloca 14
		IL_051e: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0523: stloc 38
		IL_0527: br IL_052c

		IL_052c: ldloc 20
		IL_0530: ldlen
		IL_0531: conv.i4
		IL_0532: ldc.i4 4
		IL_0537: cgt
		IL_0539: ldc.i4.0
		IL_053a: ceq
		IL_053c: stloc 44
		IL_0540: ldloc 44
		IL_0544: brtrue IL_0597

		IL_0549: br IL_054e

		IL_054e: ldloc 20
		IL_0552: ldc.i4 64
		IL_0557: add
		IL_0558: stloc 69
		IL_055c: ldloc 20
		IL_0560: ldc.i4 4
		IL_0565: ldloc 38
		IL_0569: stelem [mscorlib]System.String
		IL_056e: ldstr "Method not found @325DFB3"
		IL_0573: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0578: ldloc 20
		IL_057c: call string [mscorlib]System.String::Concat(string[])
		IL_0581: stloc 53
		IL_0585: br IL_058a

		IL_058a: ldloc 53
		IL_058e: stloc 54
		IL_0592: br IL_05c4

		IL_0597: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_059c: stloc 48
		IL_05a0: ldloc 48
		IL_05a4: ret

		IL_05a5: ldloca 14
		IL_05a9: stloc 13
		IL_05ad: ldarg resource
		IL_05b1: stloc 13
		IL_05b5: ldstr "Method not found @325DFB3"
		IL_05ba: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_05bf: br IL_0005

		IL_05c4: ldloc 54
		IL_05c8: ret

		IL_05c9: ldloc 20
		IL_05cd: ldlen
		IL_05ce: conv.i4
		IL_05cf: ldc.i4 2
		IL_05d4: cgt
		IL_05d6: ldc.i4.0
		IL_05d7: ceq
		IL_05d9: stloc 45
		IL_05dd: ldloc 45
		IL_05e1: brtrue IL_0597

		IL_05e6: br IL_0479
	} // end of method ScenarioResourceManager::BackgroundPreviewKey

	.method private hidebysig 
		instance void QueueBackgroundPreview (
			class BackgroundResource resource,
			bool preferThumbnail,
			string key,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4382f0
		// Header size: 12
		// Code size: 587 (0x24b)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] string,
			[2] bool,
			[3] class BackgroundResource,
			[4] class ScenarioResourceManager,
			[5] native int,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] object,
			[16] bool,
			[17] object[],
			[18] object[],
			[19] bool,
			[20] native int,
```
Context at line 20064:
```il
		IL_05d4: cgt
		IL_05d6: ldc.i4.0
		IL_05d7: ceq
		IL_05d9: stloc 45
		IL_05dd: ldloc 45
		IL_05e1: brtrue IL_0597

		IL_05e6: br IL_0479
	} // end of method ScenarioResourceManager::BackgroundPreviewKey

	.method private hidebysig 
		instance void QueueBackgroundPreview (
			class BackgroundResource resource,
			bool preferThumbnail,
			string key,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x4382f0
		// Header size: 12
		// Code size: 587 (0x24b)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] string,
			[2] bool,
			[3] class BackgroundResource,
			[4] class ScenarioResourceManager,
			[5] native int,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] object,
			[13] object,
			[14] native int,
			[15] object,
			[16] bool,
			[17] object[],
			[18] object[],
			[19] bool,
			[20] native int,
			[21] class [mscorlib]System.Collections.Generic.List`1<object>,
			[22] bool,
			[23] int32,
			[24] object,
			[25] object,
			[26] object,
			[27] bool,
			[28] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[29] int32,
			[30] object,
			[31] object,
			[32] object,
			[33] class [mscorlib]System.Collections.IEnumerator,
			[34] int32,
			[35] int32
		)

		IL_0000: br IL_0005

		IL_0005: ldc.i4.0
		IL_0006: conv.i
		IL_0007: stloc 14
		IL_000b: ldarg.0
		IL_000c: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>> ScenarioResourceManager::pendingBackgroundPreviews
		IL_0011: ldarg key
		IL_0015: ldloca 15
		IL_0019: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_001e: stloc 16
		IL_0022: ldloc 16
		IL_0026: ldc.i4.0
		IL_0027: ceq
		IL_0029: stloc 19
		IL_002d: ldloc 19
		IL_0031: brtrue IL_00fa

		IL_0036: br IL_003b

		IL_003b: nop
		IL_003c: nop
		IL_003d: ldloc 15
		IL_0041: ldfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_version
		IL_0046: ldc.i4 1
		IL_004b: add
		IL_004c: stloc 34
		IL_0050: ldloc 15
		IL_0054: ldloc 34
		IL_0058: stfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_version
		IL_005d: ldloc 15
		IL_0061: ldfld !0[] class [mscorlib]System.Collections.Generic.List`1<object>::_items
		IL_0066: stloc 17
		IL_006a: br IL_006f

		IL_006f: ldloc 15
		IL_0073: ldfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_size
		IL_0078: ldloc 17
		IL_007c: ldlen
		IL_007d: conv.i4
		IL_007e: clt
		IL_0080: ldc.i4.0
		IL_0081: ceq
		IL_0083: stloc 22
		IL_0087: ldloc 22
		IL_008b: brtrue IL_01e0

```
#### AcquireBackgroundSpine: 2 occurrences
Context at line 17891:
```il
				IL_0070: brtrue IL_0029
			// end loop

			IL_0075: br IL_0005
		// end loop
	} // end of method ScenarioResourceManager::CoTryPreloadBackgroundTexture

	.method public hidebysig 
		instance class RuntimeSpineAsset AcquireBackgroundSpine (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x436c04
		// Header size: 12
		// Code size: 1634 (0x662)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] class ScenarioResourceManager,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] object,
			[12] bool,
			[13] bool,
			[14] native int,
			[15] string,
			[16] object,
			[17] object,
			[18] class RuntimeSpineAsset,
			[19] object,
			[20] object,
			[21] bool,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] bool,
			[26] native int,
			[27] object,
			[28] object,
			[29] class [mscorlib]System.InvalidOperationException,
			[30] native int,
			[31] bool,
			[32] object,
			[33] object,
			[34] object,
			[35] object,
			[36] bool,
			[37] object,
			[38] bool,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] object,
			[44] bool,
			[45] bool,
			[46] object,
			[47] object,
			[48] object,
			[49] class ['spine-csharp']Spine.SkeletonData,
			[50] object,
			[51] object,
			[52] object,
			[53] bool,
			[54] object,
			[55] int32,
			[56] bool,
			[57] object,
			[58] object,
			[59] object,
			[60] object,
			[61] object,
			[62] bool,
			[63] object,
			[64] bool,
			[65] object,
			[66] bool,
			[67] class [mscorlib]System.NullReferenceException,
			[68] object,
			[69] object,
			[70] object,
			[71] native int,
			[72] bool,
			[73] object,
			[74] native int,
			[75] class RuntimeSpineAsset,
			[76] object,
			[77] class [mscorlib]System.NullReferenceException,
			[78] class [mscorlib]System.NullReferenceException,
			[79] bool,
			[80] object,
			[81] bool,
			[82] object,
			[83] object,
			[84] class [mscorlib]System.NullReferenceException,
			[85] class RuntimeSpineAsset,
			[86] object
		)

		IL_0000: br IL_0617

```
Context at line 18498:
```il
		IL_064b: ceq
		IL_064d: ldc.i4.0
		IL_064e: ceq
		IL_0650: stloc 81
		IL_0654: ldloc 81
		IL_0658: brtrue IL_05fa

		IL_065d: br IL_05eb
	} // end of method ScenarioResourceManager::AcquireBackgroundSpine

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x437274
		// Header size: 12
		// Code size: 29 (0x1d)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[2] uint32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 8 Invalid \"Jump target not found in method: 0x3546040\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x4372a0
		// Header size: 12
		// Code size: 1139 (0x473)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] object,
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] object,
			[9] bool,
			[10] bool,
			[11] native int,
			[12] class BackgroundResource,
			[13] object,
			[14] bool,
			[15] object,
			[16] class [System]System.IO.InvalidDataException,
			[17] string,
			[18] string,
			[19] class [mscorlib]System.InvalidOperationException,
			[20] bool,
			[21] native int,
			[22] object,
			[23] bool,
			[24] native int,
			[25] object,
			[26] bool,
			[27] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[28] string,
			[29] bool,
			[30] bool,
			[31] bool,
			[32] object,
			[33] native int,
			[34] object,
			[35] class [System]System.IO.InvalidDataException,
			[36] class [System]System.IO.InvalidDataException,
			[37] bool,
			[38] native int,
			[39] object,
			[40] object,
			[41] object,
			[42] object,
			[43] object,
			[44] string,
			[45] object,
			[46] native int,
			[47] object,
			[48] bool,
			[49] bool,
			[50] string,
			[51] native int,
			[52] object,
			[53] bool,
			[54] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[55] object,
			[56] uint8[],
			[57] object,
			[58] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[59] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[60] object,
			[61] object,
			[62] bool,
```

### windows (SHA256 85fdcf5366041f3e53058a58417d296f0fa42368b8f87a61e112470e3f7e3eb9)
#### ResolveBackground: 7 occurrences
Context at line 6798:
```il
			IL_01ae: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+20]"
			IL_01b3: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_01b8: ldc.i4.0
			IL_01b9: conv.i
			IL_01ba: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+28]"
			IL_01bf: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_01c4: ldc.i4.0
			IL_01c5: conv.i
			IL_01c6: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
			IL_01cb: stloc 6
			IL_01cf: ldloc 6
			IL_01d3: pop
			IL_01d4: br IL_01d9

			IL_01d9: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+30]"
			IL_01de: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_01e3: ldc.i4.0
			IL_01e4: conv.i
			IL_01e5: ldc.i4 0
			IL_01ea: ceq
			IL_01ec: stloc 15
			IL_01f0: ldloc 0
			IL_01f4: stloc 30
			IL_01f8: ldloc 15
			IL_01fc: brtrue IL_0da8

			IL_0201: br IL_0206

			IL_0206: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+30]"
			IL_020b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0210: ldc.i4.0
			IL_0211: conv.i
			IL_0212: stloc 24
			IL_0216: ldloc 24
			IL_021a: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
			IL_021f: ldc.i4 0
			IL_0224: ceq
			IL_0226: stloc 25
			IL_022a: ldloc 0
			IL_022e: stloc 30
			IL_0232: ldloc 25
			IL_0236: brtrue IL_0da8

			IL_023b: br IL_0240

			IL_0240: ldloc 24
			IL_0244: call instance string BackgroundResource::get_FullPath()
			IL_0249: stloc 26
			IL_024d: ldloc 0
			IL_0251: stloc 30
			IL_0255: br IL_0da8

			IL_025a: ldstr "Method not found @1809B0420"
			IL_025f: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0264: ldloc 40
			IL_0268: ldc.i4 0
			IL_026d: ceq
			IL_026f: ldc.i4.0
			IL_0270: ceq
			IL_0272: stloc 46
			IL_0276: ldloc 46
			IL_027a: brtrue IL_0764

			IL_027f: br IL_0284

			IL_0284: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+30]"
			IL_0289: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_028e: ldc.i4.0
			IL_028f: conv.i
			IL_0290: stloc 35
			IL_0294: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+30]"
			IL_0299: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_029e: ldc.i4.0
			IL_029f: conv.i
			IL_02a0: ldc.i4 0
			IL_02a5: ceq
			IL_02a7: stloc 55
			IL_02ab: ldloc 59
			IL_02af: stloc 80
			IL_02b3: ldnull
			IL_02b4: stloc 64
			IL_02b8: ldnull
			IL_02b9: stloc 82
			IL_02bd: ldloc 55
			IL_02c1: brtrue IL_0c7d

			IL_02c6: br IL_02cb

			IL_02cb: ldloc 35
			IL_02cf: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
			IL_02d4: ldc.i4 1
			IL_02d9: ceq
			IL_02db: ldc.i4.0
			IL_02dc: ceq
			IL_02de: stloc 36
			IL_02e2: ldnull
			IL_02e3: stloc 100
			IL_02e7: ldloc 36
			IL_02eb: brtrue IL_0dd2

			IL_02f0: br IL_02f5

			IL_02f5: ldloc 35
			IL_02f9: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
			IL_02fe: ldc.i4 0
			IL_0303: ceq
			IL_0305: stloc 74
			IL_0309: ldloc 74
```
Context at line 12338:
```il
	.field private initonly class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.AudioModule]UnityEngine.AudioClip> _loadedCustomVoices
	.field private initonly class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.AudioModule]UnityEngine.AudioClip> _loadedCustomSounds
	.field private bool _isManualInstalling
	.field private static literal uint64 ResStructureVersion = uint64(11)
	.field private string _manualInstallPath

	// Methods
	.method public hidebysig 
		instance class BackgroundResource ResolveBackground (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x31de54
		// Header size: 12
		// Code size: 1163 (0x48b)
		.maxstack 0
		.locals init (
			[0] uint32,
			[1] object,
			[2] object,
			[3] object,
			[4] class ScenarioResourceManager/'<>c__DisplayClass9_0',
			[5] bool,
			[6] bool,
			[7] class [mscorlib]System.Reflection.MethodBase,
			[8] class [System.Core]System.Linq.Expressions.ParameterExpression[],
			[9] bool,
			[10] class Studio.Scripts.Databases.ScenarioBgNameDbSchema,
			[11] bool,
			[12] bool,
			[13] bool,
			[14] bool,
			[15] class BackgroundResource,
			[16] native int,
			[17] object,
			[18] class ScenarioResourceManager/'<>c__DisplayClass9_0',
			[19] bool,
			[20] object,
			[21] class [mscorlib]System.Reflection.MethodBase,
			[22] native int,
			[23] native int,
			[24] class BackgroundResource,
			[25] class [mscorlib]System.Type,
			[26] class [System.Core]System.Linq.Expressions.ParameterExpression,
			[27] native int,
			[28] object,
			[29] object,
			[30] bool,
			[31] bool,
			[32] class [mscorlib]System.Reflection.MethodBase,
			[33] bool,
			[34] class [System.Core]System.Linq.Expressions.MemberExpression,
			[35] class [mscorlib]System.Type,
			[36] class [System.Core]System.Linq.Expressions.ConstantExpression,
			[37] class [mscorlib]System.Reflection.FieldInfo,
			[38] class [System.Core]System.Linq.Expressions.MemberExpression,
			[39] class [System.Core]System.Linq.Expressions.BinaryExpression,
			[40] bool,
			[41] native int,
			[42] class [System.Core]System.Linq.Expressions.Expression`1<class [mscorlib]System.Func`2<class Studio.Scripts.Databases.ScenarioBgNameDbSchema, bool>>,
			[43] valuetype FlatData.ScenarioBGNameExcel,
			[44] string,
			[45] string,
			[46] valuetype ScenarioBGType,
			[47] object,
			[48] valuetype ScenarioResourceManager/ResourceType,
			[49] string,
			[50] string,
			[51] class ResourceOverrideData/BgSpineOverrideData,
			[52] class ScenarioResourceManager,
			[53] valuetype FlatData.ScenarioBGNameExcel
		)

		IL_0000: br IL_041d
		// loop start (head: IL_041d)
			IL_0005: ldloc 4
			IL_0009: ldarg id
			IL_000d: stfld uint32 ScenarioResourceManager/'<>c__DisplayClass9_0'::id
			IL_0012: ldarg id
			IL_0016: ldc.i4 0
			IL_001b: ceq
			IL_001d: stloc 6
			IL_0021: ldloc 6
			IL_0025: brtrue IL_0407

			IL_002a: br IL_002f

			IL_002f: ldarg id
			IL_0033: ldarg.0
			IL_0034: ldfld class ResourceOverrideData ScenarioResourceManager::LocalOverrideData
			IL_0039: ldarg.0
			IL_003a: ldfld class ResourceOverrideData ScenarioResourceManager::GlobalOverrideData
			IL_003f: call class BackgroundResource BackgroundResource::ResolveOverride(uint32, class ResourceOverrideData, class ResourceOverrideData)
			IL_0044: stloc 15
			IL_0048: ldloc 15
			IL_004c: ldc.i4 0
			IL_0051: ceq
			IL_0053: ldc.i4.0
			IL_0054: ceq
			IL_0056: stloc 11
			IL_005a: ldloc 11
			IL_005e: brtrue IL_0451

			IL_0063: br IL_0068

			IL_0068: ldarg.0
			IL_0069: ldfld class SQLite4Unity3d.TableQuery`1<class Studio.Scripts.Databases.ScenarioBgNameDbSchema> ScenarioResourceManager::ScenarioBgNameQuery
			IL_006e: ldc.i4 0
```
Context at line 12751:
```il
		IL_0476: stloc 23
		IL_047a: br IL_047f

		IL_047f: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_0484: throw

		IL_0485: nop
		IL_0486: br IL_0456
	} // end of method ScenarioResourceManager::ResolveBackground

	.method public hidebysig 
		instance class [mscorlib]System.Collections.Generic.IEnumerable`1<class BackgroundResource> EnumerateBackgrounds () cil managed 
	{
		// Method begins at RVA 0x31e2ec
		// Header size: 12
		// Code size: 82 (0x52)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] class ScenarioResourceManager/'<EnumerateBackgrounds>d__10',
			[2] bool,
			[3] class [mscorlib]System.NullReferenceException
		)

		IL_0000: br IL_0022
		// loop start (head: IL_0022)
			IL_0005: ldloc 1
			IL_0009: ldarg.0
			IL_000a: stfld class ScenarioResourceManager ScenarioResourceManager/'<EnumerateBackgrounds>d__10'::'<>4__this'
			IL_000f: ldloc 1
			IL_0013: ret
			// loop start (head: IL_0022)
				IL_0014: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
				IL_0019: stloc 3
				IL_001d: ldloc 3
				IL_0021: ret

				IL_0022: ldc.i8 4294967294
				IL_002b: newobj instance void ScenarioResourceManager/'<EnumerateBackgrounds>d__10'::.ctor(int32)
				IL_0030: stloc 1
				IL_0034: nop
				IL_0035: ldloc 1
				IL_0039: ldc.i4 0
				IL_003e: ceq
				IL_0040: stloc 2
				IL_0044: ldloc 2
				IL_0048: brtrue IL_0014
			// end loop

			IL_004d: br IL_0005
		// end loop
	} // end of method ScenarioResourceManager::EnumerateBackgrounds

	.method public hidebysig 
		instance bool TryGetBackgroundOverridePath (
			uint32 id,
			[out] string& path
		) cil managed 
	{
		// Method begins at RVA 0x31e34c
		// Header size: 12
		// Code size: 222 (0xde)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] class ScenarioResourceManager,
			[2] uint32,
			[3] bool,
			[4] bool,
			[5] bool,
			[6] string,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] string&
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 0
		IL_000e: ldloc 0
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 3
		IL_001d: ldloc 3
		IL_0021: brtrue IL_0084

		IL_0026: br IL_002b

		IL_002b: ldloc 0
		IL_002f: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0034: ldc.i4 0
		IL_0039: ceq
		IL_003b: stloc 4
		IL_003f: ldloc 4
		IL_0043: brtrue IL_0084

		IL_0048: br IL_004d

		IL_004d: ldloc 0
		IL_0051: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_0056: ldc.i4 1
		IL_005b: ceq
		IL_005d: ldc.i4.0
		IL_005e: ceq
```
Context at line 12824:
```il
			[9] bool,
			[10] bool,
			[11] bool,
			[12] string&
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 0
		IL_000e: ldloc 0
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 3
		IL_001d: ldloc 3
		IL_0021: brtrue IL_0084

		IL_0026: br IL_002b

		IL_002b: ldloc 0
		IL_002f: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0034: ldc.i4 0
		IL_0039: ceq
		IL_003b: stloc 4
		IL_003f: ldloc 4
		IL_0043: brtrue IL_0084

		IL_0048: br IL_004d

		IL_004d: ldloc 0
		IL_0051: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_0056: ldc.i4 1
		IL_005b: ceq
		IL_005d: ldc.i4.0
		IL_005e: ceq
		IL_0060: stloc 5
		IL_0064: ldloc 5
		IL_0068: brtrue IL_0084

		IL_006d: br IL_0072

		IL_0072: ldloc 0
		IL_0076: call instance string BackgroundResource::get_FullPath()
		IL_007b: stloc 6
		IL_007f: br IL_008e

		IL_0084: ldnull
		IL_0085: stloc 6
		IL_0089: br IL_008e

		IL_008e: ldloc 6
		IL_0092: stloc 12
		IL_0096: ldarg path
		IL_009a: ldind.ref
		IL_009b: ldc.i4 0
		IL_00a0: clt
		IL_00a2: stloc 7
		IL_00a6: ldarg path
		IL_00aa: ldind.ref
		IL_00ab: ldc.i4 0
		IL_00b0: ceq
		IL_00b2: stloc 8
		IL_00b6: ldloc 7
		IL_00ba: ldc.i4.0
		IL_00bb: ceq
		IL_00bd: stloc 9
		IL_00c1: ldloc 8
		IL_00c5: ldc.i4.0
		IL_00c6: ceq
		IL_00c8: stloc 10
		IL_00cc: ldloc 10
		IL_00d0: ldloc 9
		IL_00d4: and
		IL_00d5: stloc 11
		IL_00d9: ldloc 11
		IL_00dd: ret
	} // end of method ScenarioResourceManager::TryGetBackgroundOverridePath

	.method public hidebysig 
		instance valuetype ScenarioBGType GetBackgroundType (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x31e438
		// Header size: 12
		// Code size: 59 (0x3b)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] class ScenarioResourceManager,
			[2] uint32,
			[3] bool
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 0
		IL_000e: ldloc 0
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 3
		IL_001d: ldloc 3
		IL_0021: brtrue IL_0035

		IL_0026: br IL_002b

		IL_002b: ldloc 0
```
Context at line 12912:
```il
			[0] class BackgroundResource,
			[1] class ScenarioResourceManager,
			[2] uint32,
			[3] bool
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 0
		IL_000e: ldloc 0
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 3
		IL_001d: ldloc 3
		IL_0021: brtrue IL_0035

		IL_0026: br IL_002b

		IL_002b: ldloc 0
		IL_002f: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_0034: ret

		IL_0035: ldc.i4 1
		IL_003a: ret
	} // end of method ScenarioResourceManager::GetBackgroundType

	.method public hidebysig specialname 
		instance bool get_Preloading () cil managed 
	{
		// Method begins at RVA 0x31e480
		// Header size: 12
		// Code size: 7 (0x7)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager
		)

		IL_0000: ldarg.0
		IL_0001: ldfld bool ScenarioResourceManager::'<Preloading>k__BackingField'
		IL_0006: ret
	} // end of method ScenarioResourceManager::get_Preloading

	.method private hidebysig specialname 
		instance void set_Preloading (
			bool 'value'
		) cil managed 
	{
		// Method begins at RVA 0x31e494
		// Header size: 12
		// Code size: 11 (0xb)
		.maxstack 0
		.locals init (
			[0] bool,
			[1] class ScenarioResourceManager
		)

		IL_0000: ldarg.0
		IL_0001: ldarg 'value'
		IL_0005: stfld bool ScenarioResourceManager::'<Preloading>k__BackingField'
		IL_000a: ret
	} // end of method ScenarioResourceManager::set_Preloading

	.method public hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator CoTryPreloadBackgroundTexture (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x31e4ac
		// Header size: 12
		// Code size: 91 (0x5b)
		.maxstack 0
		.locals init (
			[0] uint32,
			[1] class ScenarioResourceManager,
			[2] class ScenarioResourceManager/'<CoTryPreloadBackgroundTexture>d__17',
			[3] bool,
			[4] class [mscorlib]System.NullReferenceException
		)

		IL_0000: br IL_002f
		// loop start (head: IL_002f)
			IL_0005: ldloc 2
			IL_0009: ldarg.0
			IL_000a: stfld class ScenarioResourceManager ScenarioResourceManager/'<CoTryPreloadBackgroundTexture>d__17'::'<>4__this'
			IL_000f: ldloc 2
			IL_0013: ldarg id
			IL_0017: stfld uint32 ScenarioResourceManager/'<CoTryPreloadBackgroundTexture>d__17'::id
			IL_001c: ldloc 2
			IL_0020: ret
			// loop start (head: IL_002f)
				IL_0021: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
				IL_0026: stloc 4
				IL_002a: ldloc 4
				IL_002e: ret

				IL_002f: ldc.i4 0
				IL_0034: newobj instance void ScenarioResourceManager/'<CoTryPreloadBackgroundTexture>d__17'::.ctor(int32)
				IL_0039: stloc 2
				IL_003d: nop
				IL_003e: ldloc 2
				IL_0042: ldc.i4 0
				IL_0047: ceq
				IL_0049: stloc 3
				IL_004d: ldloc 3
				IL_0051: brtrue IL_0021
			// end loop

```
Context at line 13467:
```il
		.locals init (
			[0] class ScenarioResourceManager,
			[1] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[2] uint32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 12 Invalid \"Jump target not found in method: 0x1807BC3A0\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x31ea10
		// Header size: 12
		// Code size: 1024 (0x400)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] bool,
			[2] bool,
			[3] bool,
			[4] bool,
			[5] native int,
			[6] class BackgroundResource,
			[7] bool,
			[8] class [System]System.IO.InvalidDataException,
			[9] string,
			[10] string,
			[11] class [mscorlib]System.InvalidOperationException,
			[12] bool,
			[13] object,
			[14] object,
			[15] bool,
			[16] bool,
			[17] string,
			[18] bool,
			[19] bool,
			[20] bool,
			[21] object,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] string,
			[26] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[27] uint8[],
			[28] native int,
			[29] class [UnityEngine.CoreModule]UnityEngine.Object,
			[30] class [System]System.IO.InvalidDataException,
			[31] bool,
			[32] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[33] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[34] bool,
			[35] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[36] bool,
			[37] bool,
			[38] bool,
			[39] string,
			[40] string,
			[41] bool,
			[42] class [System]System.IO.InvalidDataException,
			[43] class BackgroundResource,
			[44] class BackgroundResource,
			[45] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[46] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[47] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>
		)

		IL_0000: br IL_035c

		IL_0005: ldloc 44
		IL_0009: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_000e: ldc.i4 1
		IL_0013: ceq
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 2
		IL_001c: ldloc 2
		IL_0020: brtrue IL_0379

		IL_0025: br IL_002a

		IL_002a: ldloc 44
		IL_002e: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0033: ldc.i4 0
		IL_0038: ceq
		IL_003a: ldc.i4.0
		IL_003b: ceq
		IL_003d: stloc 4
		IL_0041: ldloc 4
		IL_0045: brtrue IL_00ff

		IL_004a: br IL_004f

		IL_004f: ldarg.0
		IL_0050: ldloc 44
		IL_0054: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_0059: call instance string ScenarioResourceManager::CorrectPath(string)
		IL_005e: stloc 9
		IL_0062: br IL_0067
```
Context at line 14003:
```il
			[30] object,
			[31] bool,
			[32] object,
			[33] int32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 3
		IL_000e: ldloc 3
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 5
		IL_001d: ldloc 5
		IL_0021: brtrue IL_022b

		IL_0026: br IL_002b

		IL_002b: br IL_0263

		IL_0030: ldloc 3
		IL_0034: ldarg preferThumbnail
		IL_0038: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
		IL_003d: stloc 14
		IL_0041: br IL_0046

		IL_0046: ldarg.0
		IL_0047: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
		IL_004c: ldloc 14
		IL_0050: ldloca 15
		IL_0054: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_0059: stloc 18
		IL_005d: ldloc 18
		IL_0061: ldc.i4.0
		IL_0062: ceq
		IL_0064: stloc 19
		IL_0068: ldloc 19
		IL_006c: brtrue IL_0248

		IL_0071: br IL_0076

		IL_0076: br IL_007b

		IL_007b: ldstr "Unmanaged memory load: [v154 @ stack_-48_v5 (System.Object)+10]"
		IL_0080: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0085: ldc.i4.0
		IL_0086: conv.i
		IL_0087: ldnull
		IL_0088: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_008d: stloc 26
		IL_0091: ldloc 26
		IL_0095: ldc.i4.0
		IL_0096: ceq
		IL_0098: stloc 24
		IL_009c: ldloc 24
		IL_00a0: brtrue IL_0248

		IL_00a5: br IL_00aa

		IL_00aa: ldstr "Unmanaged memory load: [v154 @ stack_-48_v5 (System.Object)+10]"
		IL_00af: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00b4: ldc.i4.0
		IL_00b5: conv.i
		IL_00b6: stloc 22
		IL_00ba: ldstr "Unmanaged memory load: [v154 @ stack_-48_v5 (System.Object)+10]"
		IL_00bf: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00c4: ldc.i4.0
		IL_00c5: conv.i
		IL_00c6: ldc.i4 0
		IL_00cb: ceq
		IL_00cd: stloc 28
		IL_00d1: ldc.i4.0
		IL_00d2: conv.i
		IL_00d3: stloc 16
		IL_00d7: ldloc 15
		IL_00db: stloc 29
		IL_00df: ldloc 28
		IL_00e3: brtrue IL_01f1

		IL_00e8: br IL_00ed

		IL_00ed: ldloc 22
		IL_00f1: stloc 20
		IL_00f5: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
		IL_00fa: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_00ff: stloc 21
		IL_0103: ldstr "Unmanaged memory load: [v297 @ r9_v10 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
		IL_0108: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_010d: ldc.i4.0
		IL_010e: conv.i
		IL_010f: stloc 30
		IL_0113: ldstr "Unmanaged memory load: [v295 @ r10_v8 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
		IL_0118: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_011d: ldc.i4.0
		IL_011e: conv.i
		IL_011f: ldstr "Unmanaged memory load: [v297 @ r9_v10 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
		IL_0124: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0129: ldc.i4.0
		IL_012a: conv.i
		IL_012b: clt
		IL_012d: stloc 31
		IL_0131: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
		IL_0136: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_013b: stloc 16
		IL_013f: ldloc 15
		IL_0143: stloc 29
		IL_0147: ldloc 31
```
#### GetBackgroundTexture: 9 occurrences
Context at line 13451:
```il
		IL_04b6: stloc 21
		IL_04ba: br IL_0386

		IL_04bf: ldloc 6
		IL_04c3: ret
	} // end of method ScenarioResourceManager::AcquireBackgroundSpine

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x31e9e4
		// Header size: 12
		// Code size: 29 (0x1d)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[2] uint32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 12 Invalid \"Jump target not found in method: 0x1807BC3A0\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x31ea10
		// Header size: 12
		// Code size: 1024 (0x400)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] bool,
			[2] bool,
			[3] bool,
			[4] bool,
			[5] native int,
			[6] class BackgroundResource,
			[7] bool,
			[8] class [System]System.IO.InvalidDataException,
			[9] string,
			[10] string,
			[11] class [mscorlib]System.InvalidOperationException,
			[12] bool,
			[13] object,
			[14] object,
			[15] bool,
			[16] bool,
			[17] string,
			[18] bool,
			[19] bool,
			[20] bool,
			[21] object,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] string,
			[26] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[27] uint8[],
			[28] native int,
			[29] class [UnityEngine.CoreModule]UnityEngine.Object,
			[30] class [System]System.IO.InvalidDataException,
			[31] bool,
			[32] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[33] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[34] bool,
			[35] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[36] bool,
			[37] bool,
			[38] bool,
			[39] string,
			[40] string,
			[41] bool,
			[42] class [System]System.IO.InvalidDataException,
			[43] class BackgroundResource,
			[44] class BackgroundResource,
			[45] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[46] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[47] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>
		)

		IL_0000: br IL_035c

		IL_0005: ldloc 44
		IL_0009: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_000e: ldc.i4 1
		IL_0013: ceq
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 2
		IL_001c: ldloc 2
		IL_0020: brtrue IL_0379

		IL_0025: br IL_002a

		IL_002a: ldloc 44
		IL_002e: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
```
Context at line 13473:
```il
		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 12 Invalid \"Jump target not found in method: 0x1807BC3A0\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x31ea10
		// Header size: 12
		// Code size: 1024 (0x400)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] bool,
			[2] bool,
			[3] bool,
			[4] bool,
			[5] native int,
			[6] class BackgroundResource,
			[7] bool,
			[8] class [System]System.IO.InvalidDataException,
			[9] string,
			[10] string,
			[11] class [mscorlib]System.InvalidOperationException,
			[12] bool,
			[13] object,
			[14] object,
			[15] bool,
			[16] bool,
			[17] string,
			[18] bool,
			[19] bool,
			[20] bool,
			[21] object,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] string,
			[26] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[27] uint8[],
			[28] native int,
			[29] class [UnityEngine.CoreModule]UnityEngine.Object,
			[30] class [System]System.IO.InvalidDataException,
			[31] bool,
			[32] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[33] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[34] bool,
			[35] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[36] bool,
			[37] bool,
			[38] bool,
			[39] string,
			[40] string,
			[41] bool,
			[42] class [System]System.IO.InvalidDataException,
			[43] class BackgroundResource,
			[44] class BackgroundResource,
			[45] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[46] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[47] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>
		)

		IL_0000: br IL_035c

		IL_0005: ldloc 44
		IL_0009: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_000e: ldc.i4 1
		IL_0013: ceq
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 2
		IL_001c: ldloc 2
		IL_0020: brtrue IL_0379

		IL_0025: br IL_002a

		IL_002a: ldloc 44
		IL_002e: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0033: ldc.i4 0
		IL_0038: ceq
		IL_003a: ldc.i4.0
		IL_003b: ceq
		IL_003d: stloc 4
		IL_0041: ldloc 4
		IL_0045: brtrue IL_00ff

		IL_004a: br IL_004f

		IL_004f: ldarg.0
		IL_0050: ldloc 44
		IL_0054: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_0059: call instance string ScenarioResourceManager::CorrectPath(string)
		IL_005e: stloc 9
		IL_0062: br IL_0067

		IL_0067: ldstr "Method not found @180F8FD80"
		IL_006c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0071: ldloca 46
		IL_0075: call instance !0 valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>::WaitForCompletion()
		IL_007a: stloc 21
```
Context at line 13476:
```il
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 12 Invalid \"Jump target not found in method: 0x1807BC3A0\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x31ea10
		// Header size: 12
		// Code size: 1024 (0x400)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] bool,
			[2] bool,
			[3] bool,
			[4] bool,
			[5] native int,
			[6] class BackgroundResource,
			[7] bool,
			[8] class [System]System.IO.InvalidDataException,
			[9] string,
			[10] string,
			[11] class [mscorlib]System.InvalidOperationException,
			[12] bool,
			[13] object,
			[14] object,
			[15] bool,
			[16] bool,
			[17] string,
			[18] bool,
			[19] bool,
			[20] bool,
			[21] object,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] string,
			[26] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[27] uint8[],
			[28] native int,
			[29] class [UnityEngine.CoreModule]UnityEngine.Object,
			[30] class [System]System.IO.InvalidDataException,
			[31] bool,
			[32] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[33] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[34] bool,
			[35] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[36] bool,
			[37] bool,
			[38] bool,
			[39] string,
			[40] string,
			[41] bool,
			[42] class [System]System.IO.InvalidDataException,
			[43] class BackgroundResource,
			[44] class BackgroundResource,
			[45] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[46] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[47] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>
		)

		IL_0000: br IL_035c

		IL_0005: ldloc 44
		IL_0009: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_000e: ldc.i4 1
		IL_0013: ceq
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 2
		IL_001c: ldloc 2
		IL_0020: brtrue IL_0379

		IL_0025: br IL_002a

		IL_002a: ldloc 44
		IL_002e: ldfld valuetype ScenarioResourceManager/ResourceType BackgroundResource::'<Source>k__BackingField'
		IL_0033: ldc.i4 0
		IL_0038: ceq
		IL_003a: ldc.i4.0
		IL_003b: ceq
		IL_003d: stloc 4
		IL_0041: ldloc 4
		IL_0045: brtrue IL_00ff

		IL_004a: br IL_004f

		IL_004f: ldarg.0
		IL_0050: ldloc 44
		IL_0054: ldfld string BackgroundResource::'<Path>k__BackingField'
		IL_0059: call instance string ScenarioResourceManager::CorrectPath(string)
		IL_005e: stloc 9
		IL_0062: br IL_0067

		IL_0067: ldstr "Method not found @180F8FD80"
		IL_006c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0071: ldloca 46
		IL_0075: call instance !0 valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>::WaitForCompletion()
		IL_007a: stloc 21
		IL_007e: br IL_0083

		IL_0083: br IL_0088
```
Context at line 13788:
```il
		IL_031b: nop
		IL_031c: ldnull
		IL_031d: stloc 5
		IL_0321: ldloc 43
		IL_0325: stloc 6
		IL_0329: br IL_03ef

		IL_032e: ldloc 11
		IL_0332: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0337: call instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		IL_033c: ldc.i4.0
		IL_033d: conv.i
		IL_033e: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0343: ldnull
		IL_0344: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_0349: stloc 23
		IL_034d: br IL_0352

		IL_0352: ldloc 11
		IL_0356: throw

		IL_0357: ldloc 26
		IL_035b: ret

		IL_035c: ldloc 44
		IL_0360: ldc.i4 0
		IL_0365: ceq
		IL_0367: stloc 1
		IL_036b: ldloc 1
		IL_036f: brtrue IL_0226

		IL_0374: br IL_0005

		IL_0379: ldtoken [mscorlib]System.InvalidOperationException
		IL_037e: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0383: ldloc 6
		IL_0387: ldloc 5
		IL_038b: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_0390: stloc 7
		IL_0394: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0399: newobj instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		IL_039e: stloc 11
		IL_03a2: br IL_032e

		IL_03a7: ldtoken [Unity.Addressables]UnityEngine.AddressableAssets.Addressables
		IL_03ac: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_03b1: ldloc 29
		IL_03b5: ldloc 28
		IL_03b9: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03be: stloc 31
		IL_03c2: ldloc 31
		IL_03c6: ldloc 29
		IL_03ca: ldloc 28
		IL_03ce: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03d3: stloc 34
		IL_03d7: ldc.i4.0
		IL_03d8: conv.i
		IL_03d9: ldloc 29
		IL_03dd: ldloc 28
		IL_03e1: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03e6: stloc 36
		IL_03ea: br IL_0290

		IL_03ef: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
		IL_03f4: throw

		IL_03f5: nop
		IL_03f6: br IL_0379

		IL_03fb: ldloc 33
		IL_03ff: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x31ee1c
		// Header size: 12
		// Code size: 229 (0xe5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] string,
			[11] object,
			[12] class [System]System.IO.InvalidDataException,
			[13] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[14] object,
			[15] object,
			[16] object
		)

		IL_0000: br IL_00a2

		IL_0005: ldloc 6
		IL_0009: ldarg name
		IL_000d: call instance void [UnityEngine.CoreModule]UnityEngine.Object::set_name(string)
```
Context at line 13792:
```il
		IL_0325: stloc 6
		IL_0329: br IL_03ef

		IL_032e: ldloc 11
		IL_0332: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0337: call instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		IL_033c: ldc.i4.0
		IL_033d: conv.i
		IL_033e: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0343: ldnull
		IL_0344: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_0349: stloc 23
		IL_034d: br IL_0352

		IL_0352: ldloc 11
		IL_0356: throw

		IL_0357: ldloc 26
		IL_035b: ret

		IL_035c: ldloc 44
		IL_0360: ldc.i4 0
		IL_0365: ceq
		IL_0367: stloc 1
		IL_036b: ldloc 1
		IL_036f: brtrue IL_0226

		IL_0374: br IL_0005

		IL_0379: ldtoken [mscorlib]System.InvalidOperationException
		IL_037e: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0383: ldloc 6
		IL_0387: ldloc 5
		IL_038b: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_0390: stloc 7
		IL_0394: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0399: newobj instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		IL_039e: stloc 11
		IL_03a2: br IL_032e

		IL_03a7: ldtoken [Unity.Addressables]UnityEngine.AddressableAssets.Addressables
		IL_03ac: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_03b1: ldloc 29
		IL_03b5: ldloc 28
		IL_03b9: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03be: stloc 31
		IL_03c2: ldloc 31
		IL_03c6: ldloc 29
		IL_03ca: ldloc 28
		IL_03ce: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03d3: stloc 34
		IL_03d7: ldc.i4.0
		IL_03d8: conv.i
		IL_03d9: ldloc 29
		IL_03dd: ldloc 28
		IL_03e1: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03e6: stloc 36
		IL_03ea: br IL_0290

		IL_03ef: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
		IL_03f4: throw

		IL_03f5: nop
		IL_03f6: br IL_0379

		IL_03fb: ldloc 33
		IL_03ff: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x31ee1c
		// Header size: 12
		// Code size: 229 (0xe5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] string,
			[11] object,
			[12] class [System]System.IO.InvalidDataException,
			[13] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[14] object,
			[15] object,
			[16] object
		)

		IL_0000: br IL_00a2

		IL_0005: ldloc 6
		IL_0009: ldarg name
		IL_000d: call instance void [UnityEngine.CoreModule]UnityEngine.Object::set_name(string)
		IL_0012: ldloc 6
		IL_0016: ldc.i4 1
		IL_001b: call instance void [UnityEngine.CoreModule]UnityEngine.Texture::set_wrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.TextureWrapMode)
		IL_0020: br IL_0025
```
Context at line 13819:
```il
		IL_0374: br IL_0005

		IL_0379: ldtoken [mscorlib]System.InvalidOperationException
		IL_037e: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0383: ldloc 6
		IL_0387: ldloc 5
		IL_038b: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_0390: stloc 7
		IL_0394: ldstr "Use a background presenter for Spine playback, or GetBackgroundTextureAsync for a static preview."
		IL_0399: newobj instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		IL_039e: stloc 11
		IL_03a2: br IL_032e

		IL_03a7: ldtoken [Unity.Addressables]UnityEngine.AddressableAssets.Addressables
		IL_03ac: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_03b1: ldloc 29
		IL_03b5: ldloc 28
		IL_03b9: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03be: stloc 31
		IL_03c2: ldloc 31
		IL_03c6: ldloc 29
		IL_03ca: ldloc 28
		IL_03ce: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03d3: stloc 34
		IL_03d7: ldc.i4.0
		IL_03d8: conv.i
		IL_03d9: ldloc 29
		IL_03dd: ldloc 28
		IL_03e1: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_03e6: stloc 36
		IL_03ea: br IL_0290

		IL_03ef: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
		IL_03f4: throw

		IL_03f5: nop
		IL_03f6: br IL_0379

		IL_03fb: ldloc 33
		IL_03ff: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x31ee1c
		// Header size: 12
		// Code size: 229 (0xe5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] string,
			[11] object,
			[12] class [System]System.IO.InvalidDataException,
			[13] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[14] object,
			[15] object,
			[16] object
		)

		IL_0000: br IL_00a2

		IL_0005: ldloc 6
		IL_0009: ldarg name
		IL_000d: call instance void [UnityEngine.CoreModule]UnityEngine.Object::set_name(string)
		IL_0012: ldloc 6
		IL_0016: ldc.i4 1
		IL_001b: call instance void [UnityEngine.CoreModule]UnityEngine.Texture::set_wrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.TextureWrapMode)
		IL_0020: br IL_0025

		IL_0025: ldloc 6
		IL_0029: ldarg bytes
		IL_002d: ldc.i4 1
		IL_0032: call bool [UnityEngine.ImageConversionModule]UnityEngine.ImageConversion::LoadImage(class [UnityEngine.CoreModule]UnityEngine.Texture2D, uint8[], bool)
		IL_0037: stloc 8
		IL_003b: ldloc 8
		IL_003f: ldc.i4.0
		IL_0040: ceq
		IL_0042: stloc 9
		IL_0046: ldloc 9
		IL_004a: brtrue IL_00c4

		IL_004f: br IL_0054

		IL_0054: ldloc 6
		IL_0058: ret

		IL_0059: ldloc 10
		IL_005d: newobj instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_0062: stloc 12
		IL_0066: nop
		IL_0067: br IL_006c

		IL_006c: ldstr "Method not found @1804EC860"
		IL_0071: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0076: ldloc 12
```
Context at line 13851:
```il
		IL_03ef: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
		IL_03f4: throw

		IL_03f5: nop
		IL_03f6: br IL_0379

		IL_03fb: ldloc 33
		IL_03ff: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x31ee1c
		// Header size: 12
		// Code size: 229 (0xe5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] string,
			[11] object,
			[12] class [System]System.IO.InvalidDataException,
			[13] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[14] object,
			[15] object,
			[16] object
		)

		IL_0000: br IL_00a2

		IL_0005: ldloc 6
		IL_0009: ldarg name
		IL_000d: call instance void [UnityEngine.CoreModule]UnityEngine.Object::set_name(string)
		IL_0012: ldloc 6
		IL_0016: ldc.i4 1
		IL_001b: call instance void [UnityEngine.CoreModule]UnityEngine.Texture::set_wrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.TextureWrapMode)
		IL_0020: br IL_0025

		IL_0025: ldloc 6
		IL_0029: ldarg bytes
		IL_002d: ldc.i4 1
		IL_0032: call bool [UnityEngine.ImageConversionModule]UnityEngine.ImageConversion::LoadImage(class [UnityEngine.CoreModule]UnityEngine.Texture2D, uint8[], bool)
		IL_0037: stloc 8
		IL_003b: ldloc 8
		IL_003f: ldc.i4.0
		IL_0040: ceq
		IL_0042: stloc 9
		IL_0046: ldloc 9
		IL_004a: brtrue IL_00c4

		IL_004f: br IL_0054

		IL_0054: ldloc 6
		IL_0058: ret

		IL_0059: ldloc 10
		IL_005d: newobj instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_0062: stloc 12
		IL_0066: nop
		IL_0067: br IL_006c

		IL_006c: ldstr "Method not found @1804EC860"
		IL_0071: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0076: ldloc 12
		IL_007a: throw

		IL_007b: br IL_0080

		IL_0080: ldloc 6
		IL_0084: call void RuntimeSpineLoader::DestroyOwned(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0089: ldstr "Method not found @1800025C0"
		IL_008e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0093: br IL_0098

		IL_0098: ldloc 15
		IL_009c: throw

		IL_009d: ldloc 13
		IL_00a1: ret

		IL_00a2: ldc.i4 2
		IL_00a7: ldc.i4 2
		IL_00ac: ldc.i4 4
		IL_00b1: ldloc 7
		IL_00b5: newobj instance void [UnityEngine.CoreModule]UnityEngine.Texture2D::.ctor(int32, int32, valuetype [UnityEngine.CoreModule]UnityEngine.TextureFormat, bool)
		IL_00ba: stloc 6
		IL_00be: nop
		IL_00bf: br IL_0005

		IL_00c4: ldstr "Cannot decode background image: "
		IL_00c9: ldarg name
		IL_00cd: call string [mscorlib]System.String::Concat(string, string)
		IL_00d2: stloc 10
		IL_00d6: ldstr "Method not found @1804EC860"
		IL_00db: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00e0: br IL_0059
```
Context at line 13954:
```il
		IL_00cd: call string [mscorlib]System.String::Concat(string, string)
		IL_00d2: stloc 10
		IL_00d6: ldstr "Method not found @1804EC860"
		IL_00db: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00e0: br IL_0059
	} // end of method ScenarioResourceManager::DecodeBackgroundImage

	.method public hidebysig 
		instance void GetBackgroundTextureAsync (
			uint32 id,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x31ef10
		// Header size: 12
		// Code size: 671 (0x29f)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class ScenarioResourceManager,
			[3] class BackgroundResource,
			[4] uint32,
			[5] bool,
			[6] bool,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] bool,
			[12] object,
			[13] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[14] string,
			[15] object,
			[16] native int,
			[17] object,
			[18] bool,
			[19] bool,
			[20] native int,
			[21] native int,
			[22] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[23] bool,
			[24] bool,
			[25] bool,
			[26] bool,
			[27] bool,
			[28] bool,
			[29] object,
			[30] object,
			[31] bool,
			[32] object,
			[33] int32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 3
		IL_000e: ldloc 3
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 5
		IL_001d: ldloc 5
		IL_0021: brtrue IL_022b

		IL_0026: br IL_002b

		IL_002b: br IL_0263

		IL_0030: ldloc 3
		IL_0034: ldarg preferThumbnail
		IL_0038: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
		IL_003d: stloc 14
		IL_0041: br IL_0046

		IL_0046: ldarg.0
		IL_0047: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
		IL_004c: ldloc 14
		IL_0050: ldloca 15
		IL_0054: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_0059: stloc 18
		IL_005d: ldloc 18
		IL_0061: ldc.i4.0
		IL_0062: ceq
		IL_0064: stloc 19
		IL_0068: ldloc 19
		IL_006c: brtrue IL_0248

		IL_0071: br IL_0076

		IL_0076: br IL_007b

		IL_007b: ldstr "Unmanaged memory load: [v154 @ stack_-48_v5 (System.Object)+10]"
		IL_0080: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0085: ldc.i4.0
		IL_0086: conv.i
		IL_0087: ldnull
		IL_0088: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_008d: stloc 26
		IL_0091: ldloc 26
		IL_0095: ldc.i4.0
		IL_0096: ceq
		IL_0098: stloc 24
		IL_009c: ldloc 24
		IL_00a0: brtrue IL_0248

		IL_00a5: br IL_00aa
```
#### DecodeBackgroundImage: 4 occurrences
Context at line 7158:
```il
			IL_0629: ldc.i4.0
			IL_062a: conv.i
			IL_062b: stloc 65
			IL_062f: ldloc 98
			IL_0633: ldstr "Unmanaged memory load: [v1391 @ rdx_v33 (System.String)+18]"
			IL_0638: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_063d: ldc.i4.0
			IL_063e: conv.i
			IL_063f: call class [UnityEngine.CoreModule]UnityEngine.Texture2D ScenarioResourceManager::DecodeBackgroundImage(uint8[], string)
			IL_0644: stloc 60
			IL_0648: ldloc 1
			IL_064c: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture> ScenarioResourceManager::_loadedCustomBackgroundTextures
			IL_0651: ldc.i4 0
			IL_0656: ceq
			IL_0658: stloc 61
			IL_065c: ldnull
			IL_065d: stloc 100
			IL_0661: ldloc 61
			IL_0665: brtrue IL_0e07

			IL_066a: br IL_066f

			IL_066f: ldloc 1
			IL_0673: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture> ScenarioResourceManager::_loadedCustomBackgroundTextures
			IL_0678: ldloc 92
			IL_067c: ldloc 60
			IL_0680: call instance void class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::Add(!0, !1)
			IL_0685: ldc.i4.0
			IL_0686: conv.i
			IL_0687: stloc 59
			IL_068b: br IL_0e02

			IL_0690: ldloc 35
			IL_0694: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
			IL_0699: ldc.i4 1
			IL_069e: ceq
			IL_06a0: ldc.i4.0
			IL_06a1: ceq
			IL_06a3: stloc 37
			IL_06a7: ldnull
			IL_06a8: stloc 100
			IL_06ac: ldloc 37
			IL_06b0: brtrue IL_0dd2

			IL_06b5: br IL_06ba

			IL_06ba: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+30]"
			IL_06bf: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_06c4: ldc.i4.0
			IL_06c5: conv.i
			IL_06c6: ldc.i4 0
			IL_06cb: ceq
			IL_06cd: stloc 68
			IL_06d1: ldnull
			IL_06d2: stloc 64
			IL_06d6: ldnull
			IL_06d7: stloc 65
			IL_06db: ldloc 68
			IL_06df: brtrue IL_0c62

			IL_06e4: br IL_06e9

			IL_06e9: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+20]"
			IL_06ee: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_06f3: ldc.i4.0
			IL_06f4: conv.i
			IL_06f5: ldloc 35
			IL_06f9: ldfld string BackgroundResource::'<Path>k__BackingField'
			IL_06fe: call instance string ScenarioResourceManager::CorrectPath(string)
			IL_0703: stloc 90
			IL_0707: br IL_070c

			IL_070c: ldloc 90
			IL_0710: ldc.i4 0
			IL_0715: call valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle [Unity.Addressables]UnityEngine.AddressableAssets.Addressables::DownloadDependenciesAsync(object, bool)
			IL_071a: stloc 93
			IL_071e: ldloc 93
			IL_0722: ldfld class [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.IAsyncOperation [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle::m_InternalOp
			IL_0727: pop
			IL_0728: ldloc 93
			IL_072c: ldfld string [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle::m_LocationName
			IL_0731: pop
			IL_0732: br IL_0737

			IL_0737: nop
			IL_0738: nop
			IL_0739: ldloc 103
			IL_073d: box [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle
			IL_0742: stloc 97
			IL_0746: ldloc 97
			IL_074a: pop
			IL_074b: br IL_0750

			IL_0750: ldc.i4 4
			IL_0755: pop
			IL_0756: ldc.i4 1
			IL_075b: stloc 28
			IL_075f: br IL_0d89

			IL_0764: ldstr "Unmanaged memory load: [v45 @ stack_8_v2 (System.Threading.Tasks.Task`1<RuntimeSpineLoader+Files>)+30]"
			IL_0769: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_076e: ldc.i4.0
			IL_076f: conv.i
			IL_0770: ldc.i4 0
			IL_0775: ceq
			IL_0777: stloc 56
			IL_077b: ldnull
			IL_077c: stloc 7
```
Context at line 13669:
```il
		IL_0196: ldloc 25
		IL_019a: call uint8[] [mscorlib]System.IO.File::ReadAllBytes(string)
		IL_019f: stloc 27
		IL_01a3: br IL_01a8

		IL_01a8: ldloc 27
		IL_01ac: ldloc 44
		IL_01b0: ldfld string BackgroundResource::'<Name>k__BackingField'
		IL_01b5: call class [UnityEngine.CoreModule]UnityEngine.Texture2D ScenarioResourceManager::DecodeBackgroundImage(uint8[], string)
		IL_01ba: stloc 32
		IL_01be: ldloc 44
		IL_01c2: call instance string BackgroundResource::get_FullPath()
		IL_01c7: stloc 17
		IL_01cb: br IL_01d0

		IL_01d0: ldarg.0
		IL_01d1: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture> ScenarioResourceManager::_loadedCustomBackgroundTextures
		IL_01d6: ldc.i4 0
		IL_01db: ceq
		IL_01dd: stloc 19
		IL_01e1: ldnull
		IL_01e2: stloc 5
		IL_01e6: ldnull
		IL_01e7: stloc 6
		IL_01eb: ldloc 19
		IL_01ef: brtrue IL_03ef

		IL_01f4: br IL_01f9

		IL_01f9: ldarg.0
		IL_01fa: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture> ScenarioResourceManager::_loadedCustomBackgroundTextures
		IL_01ff: ldloc 17
		IL_0203: ldloc 32
		IL_0207: call instance void class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::set_Item(!0, !1)
		IL_020c: ldloc 32
		IL_0210: stloc 35
		IL_0214: br IL_0219

		IL_0219: ldloc 35
		IL_021d: stloc 33
		IL_0221: br IL_03fb

		IL_0226: ldtoken [System]System.IO.InvalidDataException
		IL_022b: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0230: ldloc 44
		IL_0234: ldloc 5
		IL_0238: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_023d: stloc 3
		IL_0241: ldstr "Background resource was not found."
		IL_0246: newobj instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_024b: stloc 8
		IL_024f: br IL_0254

		IL_0254: nop
		IL_0255: br IL_025a

		IL_025a: ldc.i4.0
		IL_025b: conv.i
		IL_025c: ldstr "Background resource was not found."
		IL_0261: ldnull
		IL_0262: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_0267: stloc 20
		IL_026b: ldloc 8
		IL_026f: throw

		IL_0270: br IL_0275

		IL_0275: nop
		IL_0276: ldnull
		IL_0277: stloc 28
		IL_027b: ldloc 20
		IL_027f: stloc 29
		IL_0283: ldloc 8
		IL_0287: stloc 30
		IL_028b: br IL_03a7

		IL_0290: ldloca 47
		IL_0294: ldloc 36
		IL_0298: ldloc 28
		IL_029c: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_02a1: stloc 38
		IL_02a5: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
		IL_02aa: throw

		IL_02ab: br IL_02b0

		IL_02b0: ldstr "Method not found @1805DCF90"
		IL_02b5: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_02ba: br IL_02bf

		IL_02bf: ldstr "Cannot load background: "
		IL_02c4: ldloc 39
		IL_02c8: call string [mscorlib]System.String::Concat(string, string)
		IL_02cd: stloc 40
		IL_02d1: ldtoken [System]System.IO.InvalidDataException
		IL_02d6: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_02db: ldloc 39
		IL_02df: ldnull
		IL_02e0: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [UnityEngine.CoreModule]UnityEngine.Texture>::TryGetValue(!0, !1&)
		IL_02e5: stloc 41
		IL_02e9: br IL_02ee

		IL_02ee: ldloc 40
		IL_02f2: newobj instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_02f7: stloc 42
		IL_02fb: nop
		IL_02fc: br IL_0301

```
Context at line 13854:
```il
		IL_03f5: nop
		IL_03f6: br IL_0379

		IL_03fb: ldloc 33
		IL_03ff: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method private hidebysig static 
		class [UnityEngine.CoreModule]UnityEngine.Texture2D DecodeBackgroundImage (
			uint8[] bytes,
			string name
		) cil managed 
	{
		// Method begins at RVA 0x31ee1c
		// Header size: 12
		// Code size: 229 (0xe5)
		.maxstack 0
		.locals init (
			[0] string,
			[1] uint8[],
			[2] object,
			[3] object,
			[4] object,
			[5] object,
			[6] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] string,
			[11] object,
			[12] class [System]System.IO.InvalidDataException,
			[13] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[14] object,
			[15] object,
			[16] object
		)

		IL_0000: br IL_00a2

		IL_0005: ldloc 6
		IL_0009: ldarg name
		IL_000d: call instance void [UnityEngine.CoreModule]UnityEngine.Object::set_name(string)
		IL_0012: ldloc 6
		IL_0016: ldc.i4 1
		IL_001b: call instance void [UnityEngine.CoreModule]UnityEngine.Texture::set_wrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.TextureWrapMode)
		IL_0020: br IL_0025

		IL_0025: ldloc 6
		IL_0029: ldarg bytes
		IL_002d: ldc.i4 1
		IL_0032: call bool [UnityEngine.ImageConversionModule]UnityEngine.ImageConversion::LoadImage(class [UnityEngine.CoreModule]UnityEngine.Texture2D, uint8[], bool)
		IL_0037: stloc 8
		IL_003b: ldloc 8
		IL_003f: ldc.i4.0
		IL_0040: ceq
		IL_0042: stloc 9
		IL_0046: ldloc 9
		IL_004a: brtrue IL_00c4

		IL_004f: br IL_0054

		IL_0054: ldloc 6
		IL_0058: ret

		IL_0059: ldloc 10
		IL_005d: newobj instance void [System]System.IO.InvalidDataException::.ctor(string)
		IL_0062: stloc 12
		IL_0066: nop
		IL_0067: br IL_006c

		IL_006c: ldstr "Method not found @1804EC860"
		IL_0071: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0076: ldloc 12
		IL_007a: throw

		IL_007b: br IL_0080

		IL_0080: ldloc 6
		IL_0084: call void RuntimeSpineLoader::DestroyOwned(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0089: ldstr "Method not found @1800025C0"
		IL_008e: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0093: br IL_0098

		IL_0098: ldloc 15
		IL_009c: throw

		IL_009d: ldloc 13
		IL_00a1: ret

		IL_00a2: ldc.i4 2
		IL_00a7: ldc.i4 2
		IL_00ac: ldc.i4 4
		IL_00b1: ldloc 7
		IL_00b5: newobj instance void [UnityEngine.CoreModule]UnityEngine.Texture2D::.ctor(int32, int32, valuetype [UnityEngine.CoreModule]UnityEngine.TextureFormat, bool)
		IL_00ba: stloc 6
		IL_00be: nop
		IL_00bf: br IL_0005

		IL_00c4: ldstr "Cannot decode background image: "
		IL_00c9: ldarg name
		IL_00cd: call string [mscorlib]System.String::Concat(string, string)
		IL_00d2: stloc 10
		IL_00d6: ldstr "Method not found @1804EC860"
		IL_00db: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00e0: br IL_0059
	} // end of method ScenarioResourceManager::DecodeBackgroundImage

	.method public hidebysig 
```
Context at line 13951:
```il

		IL_00c4: ldstr "Cannot decode background image: "
		IL_00c9: ldarg name
		IL_00cd: call string [mscorlib]System.String::Concat(string, string)
		IL_00d2: stloc 10
		IL_00d6: ldstr "Method not found @1804EC860"
		IL_00db: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00e0: br IL_0059
	} // end of method ScenarioResourceManager::DecodeBackgroundImage

	.method public hidebysig 
		instance void GetBackgroundTextureAsync (
			uint32 id,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x31ef10
		// Header size: 12
		// Code size: 671 (0x29f)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class ScenarioResourceManager,
			[3] class BackgroundResource,
			[4] uint32,
			[5] bool,
			[6] bool,
			[7] object,
			[8] object,
			[9] object,
			[10] object,
			[11] bool,
			[12] object,
			[13] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[14] string,
			[15] object,
			[16] native int,
			[17] object,
			[18] bool,
			[19] bool,
			[20] native int,
			[21] native int,
			[22] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[23] bool,
			[24] bool,
			[25] bool,
			[26] bool,
			[27] bool,
			[28] bool,
			[29] object,
			[30] object,
			[31] bool,
			[32] object,
			[33] int32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 3
		IL_000e: ldloc 3
		IL_0012: ldc.i4 0
		IL_0017: ceq
		IL_0019: stloc 5
		IL_001d: ldloc 5
		IL_0021: brtrue IL_022b

		IL_0026: br IL_002b

		IL_002b: br IL_0263

		IL_0030: ldloc 3
		IL_0034: ldarg preferThumbnail
		IL_0038: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
		IL_003d: stloc 14
		IL_0041: br IL_0046

		IL_0046: ldarg.0
		IL_0047: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
		IL_004c: ldloc 14
		IL_0050: ldloca 15
		IL_0054: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_0059: stloc 18
		IL_005d: ldloc 18
		IL_0061: ldc.i4.0
		IL_0062: ceq
		IL_0064: stloc 19
		IL_0068: ldloc 19
		IL_006c: brtrue IL_0248

		IL_0071: br IL_0076

		IL_0076: br IL_007b

		IL_007b: ldstr "Unmanaged memory load: [v154 @ stack_-48_v5 (System.Object)+10]"
		IL_0080: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0085: ldc.i4.0
		IL_0086: conv.i
		IL_0087: ldnull
		IL_0088: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_008d: stloc 26
		IL_0091: ldloc 26
		IL_0095: ldc.i4.0
		IL_0096: ceq
		IL_0098: stloc 24
		IL_009c: ldloc 24
```
#### GetBackgroundPreviewAsync: 2 occurrences
Context at line 14212:
```il
		IL_028b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0290: br IL_0280

		IL_0295: ldstr "Warning: Method ends with non empty stack (-78), the output could be wrong!"
		IL_029a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundTextureAsync

	.method public hidebysig 
		instance void GetBackgroundPreviewAsync (
			class BackgroundResource resource,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x31f1bc
		// Header size: 12
		// Code size: 619 (0x26b)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] bool,
			[9] bool,
			[10] object,
			[11] string,
			[12] object,
			[13] bool,
			[14] native int,
			[15] object,
			[16] bool,
			[17] int32,
			[18] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[19] bool,
			[20] native int,
			[21] native int,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] bool,
			[26] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[27] object,
			[28] bool,
			[29] object,
			[30] bool,
			[31] object
		)

		IL_0000: br IL_0243
		// loop start (head: IL_0243)
			IL_0005: ldarg resource
			IL_0009: ldc.i4 0
			IL_000e: ceq
			IL_0010: stloc 9
			IL_0014: ldloc 9
			IL_0018: brtrue IL_0238

			IL_001d: br IL_0022

			IL_0022: ldarg resource
			IL_0026: ldarg preferThumbnail
			IL_002a: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
			IL_002f: stloc 11
			IL_0033: br IL_0038

			IL_0038: ldarg.0
			IL_0039: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
			IL_003e: ldloc 11
			IL_0042: ldloca 10
			IL_0046: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
			IL_004b: stloc 13
			IL_004f: ldloc 13
			IL_0053: ldc.i4.0
			IL_0054: ceq
			IL_0056: stloc 19
			IL_005a: ldloc 19
			IL_005e: brtrue IL_021d

			IL_0063: br IL_0068

			IL_0068: br IL_006d

			IL_006d: ldstr "Unmanaged memory load: [v77 @ stack_20_v4 (System.Object)+10]"
			IL_0072: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0077: ldc.i4.0
			IL_0078: conv.i
			IL_0079: ldnull
			IL_007a: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_007f: stloc 16
			IL_0083: ldloc 16
			IL_0087: ldc.i4.0
			IL_0088: ceq
			IL_008a: stloc 24
			IL_008e: ldloc 24
			IL_0092: brtrue IL_021d

			IL_0097: br IL_009c

			IL_009c: ldstr "Unmanaged memory load: [v77 @ stack_20_v4 (System.Object)+10]"
			IL_00a1: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00a6: ldc.i4.0
			IL_00a7: conv.i
			IL_00a8: stloc 18
```
Context at line 14451:
```il

			IL_025b: br IL_0005
		// end loop

		IL_0260: ret

		IL_0261: ldstr "Warning: Method ends with non empty stack (-48), the output could be wrong!"
		IL_0266: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundPreviewAsync

	.method private hidebysig static 
		string BackgroundPreviewKey (
			class BackgroundResource resource,
			bool preferThumbnail
		) cil managed 
	{
		// Method begins at RVA 0x31f434
		// Header size: 12
		// Code size: 1109 (0x455)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] string,
			[2] string[],
			[3] string[],
			[4] object,
			[5] object,
			[6] object,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] string,
			[13] string,
			[14] string,
			[15] string,
			[16] string,
			[17] bool,
			[18] bool,
			[19] bool,
			[20] bool,
			[21] bool,
			[22] class [mscorlib]System.IndexOutOfRangeException,
			[23] string,
			[24] string,
			[25] bool,
			[26] bool,
			[27] string,
			[28] string,
			[29] string,
			[30] string,
			[31] bool,
			[32] bool,
			[33] string,
			[34] bool,
			[35] valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'
		)

		IL_0000: br IL_0005

		IL_0005: ldarg resource
		IL_0009: call instance string BackgroundResource::get_CacheKey()
		IL_000e: stloc 1
		IL_0012: br IL_0017

		IL_0017: ldloca 34
		IL_001b: call instance string [mscorlib]System.Boolean::ToString()
		IL_0020: stloc 12
		IL_0024: ldloc 1
		IL_0028: ldstr "\n"
		IL_002d: ldloc 12
		IL_0031: call string [mscorlib]System.String::Concat(string, string, string)
		IL_0036: stloc 23
		IL_003a: br IL_003f

		IL_003f: ldstr "Unmanaged memory load: [resource @ rcx (BackgroundResource)+24]"
		IL_0044: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0049: ldc.i4.0
		IL_004a: conv.i
		IL_004b: ldc.i4 0
		IL_0050: ceq
		IL_0052: stloc 25
		IL_0056: ldloc 25
		IL_005a: brtrue IL_0409

		IL_005f: br IL_0064

		IL_0064: ldstr "Method not found @1809B0420"
		IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_006e: ldloc 4
		IL_0072: ldc.i4 0
		IL_0077: ceq
		IL_0079: stloc 26
		IL_007d: ldloc 26
		IL_0081: brtrue IL_040e

		IL_0086: br IL_008b

		IL_008b: ldc.i4 5
		IL_0090: newarr [mscorlib]System.String
		IL_0095: stloc 2
		IL_0099: br IL_009e

		IL_009e: ldloc 2
		IL_00a2: ldlen
		IL_00a3: conv.i4
		IL_00a4: ldc.i4 0
```
#### QueueBackgroundPreview: 5 occurrences
Context at line 14187:
```il

		IL_0243: br IL_0286

		IL_0248: ldarg.0
		IL_0249: ldloc 3
		IL_024d: ldarg preferThumbnail
		IL_0251: ldloc 14
		IL_0255: ldloc 13
		IL_0259: call instance void ScenarioResourceManager::QueueBackgroundPreview(class BackgroundResource, bool, string, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
		IL_025e: br IL_0281

		IL_0263: ldarg callback
		IL_0267: ldc.i4 0
		IL_026c: ceq
		IL_026e: stloc 11
		IL_0272: ldloc 11
		IL_0276: brtrue IL_0281

		IL_027b: br IL_0030

		IL_0280: ret

		IL_0281: br IL_0280

		IL_0286: ldstr "Indirect call: [callback @ r9 (System.Action`1<UnityEngine.Texture>)+18] (should have been resolved before IL gen)"
		IL_028b: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0290: br IL_0280

		IL_0295: ldstr "Warning: Method ends with non empty stack (-78), the output could be wrong!"
		IL_029a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundTextureAsync

	.method public hidebysig 
		instance void GetBackgroundPreviewAsync (
			class BackgroundResource resource,
			bool preferThumbnail,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x31f1bc
		// Header size: 12
		// Code size: 619 (0x26b)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] object,
			[5] object,
			[6] object,
			[7] object,
			[8] bool,
			[9] bool,
			[10] object,
			[11] string,
			[12] object,
			[13] bool,
			[14] native int,
			[15] object,
			[16] bool,
			[17] int32,
			[18] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[19] bool,
			[20] native int,
			[21] native int,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] bool,
			[26] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>,
			[27] object,
			[28] bool,
			[29] object,
			[30] bool,
			[31] object
		)

		IL_0000: br IL_0243
		// loop start (head: IL_0243)
			IL_0005: ldarg resource
			IL_0009: ldc.i4 0
			IL_000e: ceq
			IL_0010: stloc 9
			IL_0014: ldloc 9
			IL_0018: brtrue IL_0238

			IL_001d: br IL_0022

			IL_0022: ldarg resource
			IL_0026: ldarg preferThumbnail
			IL_002a: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
			IL_002f: stloc 11
			IL_0033: br IL_0038

			IL_0038: ldarg.0
			IL_0039: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
			IL_003e: ldloc 11
			IL_0042: ldloca 10
			IL_0046: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
			IL_004b: stloc 13
			IL_004f: ldloc 13
			IL_0053: ldc.i4.0
			IL_0054: ceq
			IL_0056: stloc 19
			IL_005a: ldloc 19
			IL_005e: brtrue IL_021d

```
Context at line 14430:
```il
			IL_0213: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0218: br IL_0260

			IL_021d: ldarg.0
			IL_021e: ldarg resource
			IL_0222: ldarg preferThumbnail
			IL_0226: ldloc 11
			IL_022a: ldloc 26
			IL_022e: call instance void ScenarioResourceManager::QueueBackgroundPreview(class BackgroundResource, bool, string, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
			IL_0233: br IL_0260

			IL_0238: ldstr "Indirect call: [callback @ r9 (System.Action`1<UnityEngine.Texture>)+18] (should have been resolved before IL gen)"
			IL_023d: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0242: ret

			IL_0243: ldarg callback
			IL_0247: ldc.i4 0
			IL_024c: ceq
			IL_024e: stloc 8
			IL_0252: ldloc 8
			IL_0256: brtrue IL_0260

			IL_025b: br IL_0005
		// end loop

		IL_0260: ret

		IL_0261: ldstr "Warning: Method ends with non empty stack (-48), the output could be wrong!"
		IL_0266: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundPreviewAsync

	.method private hidebysig static 
		string BackgroundPreviewKey (
			class BackgroundResource resource,
			bool preferThumbnail
		) cil managed 
	{
		// Method begins at RVA 0x31f434
		// Header size: 12
		// Code size: 1109 (0x455)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] string,
			[2] string[],
			[3] string[],
			[4] object,
			[5] object,
			[6] object,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] string,
			[13] string,
			[14] string,
			[15] string,
			[16] string,
			[17] bool,
			[18] bool,
			[19] bool,
			[20] bool,
			[21] bool,
			[22] class [mscorlib]System.IndexOutOfRangeException,
			[23] string,
			[24] string,
			[25] bool,
			[26] bool,
			[27] string,
			[28] string,
			[29] string,
			[30] string,
			[31] bool,
			[32] bool,
			[33] string,
			[34] bool,
			[35] valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'
		)

		IL_0000: br IL_0005

		IL_0005: ldarg resource
		IL_0009: call instance string BackgroundResource::get_CacheKey()
		IL_000e: stloc 1
		IL_0012: br IL_0017

		IL_0017: ldloca 34
		IL_001b: call instance string [mscorlib]System.Boolean::ToString()
		IL_0020: stloc 12
		IL_0024: ldloc 1
		IL_0028: ldstr "\n"
		IL_002d: ldloc 12
		IL_0031: call string [mscorlib]System.String::Concat(string, string, string)
		IL_0036: stloc 23
		IL_003a: br IL_003f

		IL_003f: ldstr "Unmanaged memory load: [resource @ rcx (BackgroundResource)+24]"
		IL_0044: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0049: ldc.i4.0
		IL_004a: conv.i
		IL_004b: ldc.i4 0
		IL_0050: ceq
		IL_0052: stloc 25
		IL_0056: ldloc 25
		IL_005a: brtrue IL_0409

		IL_005f: br IL_0064
```
Context at line 14848:
```il

		IL_0447: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_044c: stloc 22
		IL_0450: ldloc 22
		IL_0454: ret
	} // end of method ScenarioResourceManager::BackgroundPreviewKey

	.method private hidebysig 
		instance void QueueBackgroundPreview (
			class BackgroundResource resource,
			bool preferThumbnail,
			string key,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x31f898
		// Header size: 12
		// Code size: 361 (0x169)
		.maxstack 0
		.locals init (
			[0] string,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] object,
			[5] bool,
			[6] object[],
			[7] bool,
			[8] class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>,
			[9] int32,
			[10] bool,
			[11] string,
			[12] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[13] class ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28',
			[14] int32,
			[15] int32
		)

		IL_0000: br IL_0005

		IL_0005: ldarg.0
		IL_0006: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>> ScenarioResourceManager::pendingBackgroundPreviews
		IL_000b: ldarg key
		IL_000f: ldloca 4
		IL_0013: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_0018: stloc 5
		IL_001c: ldloc 5
		IL_0020: brtrue IL_00b3

		IL_0025: br IL_002a

		IL_002a: newobj instance void class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>::.ctor()
		IL_002f: stloc 8
		IL_0033: nop
		IL_0034: br IL_0039

		IL_0039: ldloc 8
		IL_003d: ldloc 11
		IL_0041: ldc.i4.0
		IL_0042: conv.i
		IL_0043: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>>::TryGetValue(!0, !1&)
		IL_0048: stloc 7
		IL_004c: br IL_0051

		IL_0051: ldarg.0
		IL_0052: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>> ScenarioResourceManager::pendingBackgroundPreviews
		IL_0057: ldarg key
		IL_005b: ldloc 8
		IL_005f: call instance void class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::Add(!0, !1)
		IL_0064: br IL_0155

		IL_0069: ldloc 13
		IL_006d: ldarg.0
		IL_006e: stfld class ScenarioResourceManager ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::'<>4__this'
		IL_0073: ldloc 13
		IL_0077: ldarg resource
		IL_007b: stfld class BackgroundResource ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::resource
		IL_0080: br IL_0085

		IL_0085: ldloc 13
		IL_0089: ldarg preferThumbnail
		IL_008d: stfld bool ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::preferThumbnail
		IL_0092: ldloc 13
		IL_0096: ldarg key
		IL_009a: stfld string ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::key
		IL_009f: ldarg.0
		IL_00a0: ldloc 13
		IL_00a4: call instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_00a9: stloc 12
		IL_00ad: br IL_00b2

		IL_00b2: ret

		IL_00b3: ldloc 4
		IL_00b7: ldfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_version
		IL_00bc: ldc.i4 1
		IL_00c1: add
		IL_00c2: stloc 15
		IL_00c6: ldloc 4
		IL_00ca: ldloc 15
		IL_00ce: stfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_version
		IL_00d3: ldloc 4
		IL_00d7: ldfld !0[] class [mscorlib]System.Collections.Generic.List`1<object>::_items
		IL_00dc: stloc 6
		IL_00e0: br IL_00e5

		IL_00e5: ldloc 4
		IL_00e9: ldfld int32 class [mscorlib]System.Collections.Generic.List`1<object>::_size
```
Context at line 14984:
```il
		IL_014b: stelem [mscorlib]System.Object
		IL_0150: br IL_00b2

		IL_0155: ldc.i4 0
		IL_015a: newobj instance void ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::.ctor(int32)
		IL_015f: stloc 13
		IL_0163: nop
		IL_0164: br IL_0069
	} // end of method ScenarioResourceManager::QueueBackgroundPreview

	.method private hidebysig static 
		string BackgroundPreviewCachePath (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x31fa10
		// Header size: 12
		// Code size: 135 (0x87)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] bool,
			[2] bool,
			[3] string,
			[4] class [mscorlib]System.NullReferenceException,
			[5] string
		)

		IL_0000: ldstr "Unmanaged memory load: [18498B8FD]"
		IL_0005: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_000a: ldc.i4.0
		IL_000b: conv.i
		IL_000c: ldc.i4 0
		IL_0011: ceq
		IL_0013: ldc.i4.0
		IL_0014: ceq
		IL_0016: stloc 1
		IL_001a: ldloc 1
		IL_001e: brtrue IL_006a

		IL_0023: br IL_0028

		IL_0028: ldc.i4 1
		IL_002d: pop
		IL_002e: br IL_006a
		// loop start (head: IL_006a)
			IL_0033: ldarg resource
			IL_0037: call instance string BackgroundResource::get_FullPath()
			IL_003c: stloc 3
			IL_0040: ldloc 3
			IL_0044: ldstr ".skel.preview.png"
			IL_0049: call string [mscorlib]System.String::Concat(string, string)
			IL_004e: stloc 5
			IL_0052: br IL_0057

			IL_0057: ldloc 5
			IL_005b: ret
			// loop start (head: IL_006a)
				IL_005c: newobj instance void [mscorlib]System.NullReferenceException::.ctor()
				IL_0061: stloc 4
				IL_0065: ldloc 4
				IL_0069: ret

				IL_006a: ldarg resource
				IL_006e: ldc.i4 0
				IL_0073: ceq
				IL_0075: stloc 2
				IL_0079: ldloc 2
				IL_007d: brtrue IL_005c
			// end loop

			IL_0082: br IL_0033
		// end loop
	} // end of method ScenarioResourceManager::BackgroundPreviewCachePath

	.method private hidebysig static 
		void SaveBackgroundPreview (
			class [UnityEngine.CoreModule]UnityEngine.RenderTexture texture,
			string path
		) cil managed 
	{
		// Method begins at RVA 0x31faa4
		// Header size: 12
		// Code size: 475 (0x1db)
		.maxstack 0
		.locals init (
			[0] string,
			[1] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[2] object,
			[3] class [UnityEngine.CoreModule]UnityEngine.RenderTexture,
			[4] valuetype [mscorlib]System.Guid,
			[5] string,
			[6] string,
			[7] bool,
			[8] string,
			[9] bool,
			[10] int32,
			[11] int32,
			[12] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[13] int32,
			[14] int32,
			[15] bool,
			[16] object,
			[17] object,
			[18] string,
			[19] class [mscorlib]System.IO.DirectoryInfo,
			[20] uint8[],
			[21] bool,
```
Context at line 16685:
```il

		IL_017e: ldarg.0
		IL_017f: ldloc 10
		IL_0183: ldfld class BackgroundResource ScenarioResourceManager/BackgroundPreview::Resource
		IL_0188: ldloc 10
		IL_018c: ldfld bool ScenarioResourceManager/BackgroundPreview::PreferThumbnail
		IL_0191: ldloc 8
		IL_0195: ldloc 21
		IL_0199: call instance void ScenarioResourceManager::QueueBackgroundPreview(class BackgroundResource, bool, string, class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>)
		IL_019e: ldloc 9
		IL_01a2: ldc.i4 1
		IL_01a7: add
		IL_01a8: stloc 9
		IL_01ac: ldloc 8
		IL_01b0: ldc.i4 16
		IL_01b5: add
		IL_01b6: stloc 8
		IL_01ba: ldloc 11
		IL_01be: stloc 22
		IL_01c2: ldloc 9
		IL_01c6: stloc 4
		IL_01ca: br IL_0240

		IL_01cf: ldloc 10
		IL_01d3: call void ScenarioResourceManager::DestroyBackgroundPreview(class ScenarioResourceManager/BackgroundPreview)
		IL_01d8: br IL_01dd

		IL_01dd: ldarg.0
		IL_01de: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
		IL_01e3: ldloc 8
		IL_01e7: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::Remove(!0)
		IL_01ec: stloc 16
		IL_01f0: nop
		IL_01f1: ldloc 16
		IL_01f5: stloc 22
		IL_01f9: br IL_01fe

		IL_01fe: ldloc 9
		IL_0202: ldc.i4 1
		IL_0207: add
		IL_0208: stloc 9
		IL_020c: ldloc 8
		IL_0210: ldc.i4 16
		IL_0215: add
		IL_0216: stloc 8
		IL_021a: ldloc 9
		IL_021e: stloc 4
		IL_0222: br IL_0240

		IL_0227: ldarg focused
		IL_022b: ldc.i4.0
		IL_022c: ceq
		IL_022e: stloc 2
		IL_0232: ldloc 2
		IL_0236: brtrue IL_0261

		IL_023b: br IL_0005

		IL_0240: ldloc 4
		IL_0244: ldloc 3
		IL_0248: ldlen
		IL_0249: conv.i4
		IL_024a: clt
		IL_024c: ldc.i4.0
		IL_024d: ceq
		IL_024f: stloc 5
		IL_0253: ldloc 5
		IL_0257: brtrue IL_0261

		IL_025c: br IL_0047

		IL_0261: ret

		IL_0262: ldstr "Warning: Method ends with non empty stack (-58), the output could be wrong!"
		IL_0267: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::OnApplicationFocus

	.method private hidebysig static 
		void ReportBackgroundError (
			class [mscorlib]System.Exception 'error'
		) cil managed 
	{
		// Method begins at RVA 0x320cc0
		// Header size: 12
		// Code size: 185 (0xb9)
		.maxstack 0
		.locals init (
			[0] class [mscorlib]System.Exception,
			[1] bool,
			[2] bool,
			[3] string,
			[4] string,
			[5] bool,
			[6] native int
		)

		IL_0000: br IL_0085

		IL_0005: ldarg 'error'
		IL_0009: call instance string [mscorlib]System.Exception::get_Message()
		IL_000e: stloc 3
		IL_0012: ldloc 3
		IL_0016: ldc.i4 0
		IL_001b: ceq
		IL_001d: ldc.i4.0
		IL_001e: ceq
		IL_0020: stloc 2
		IL_0024: ldloc 2
```
#### BackgroundPreviewKey: 11 occurrences
Context at line 14018:
```il
		IL_0021: brtrue IL_022b

		IL_0026: br IL_002b

		IL_002b: br IL_0263

		IL_0030: ldloc 3
		IL_0034: ldarg preferThumbnail
		IL_0038: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
		IL_003d: stloc 14
		IL_0041: br IL_0046

		IL_0046: ldarg.0
		IL_0047: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
		IL_004c: ldloc 14
		IL_0050: ldloca 15
		IL_0054: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_0059: stloc 18
		IL_005d: ldloc 18
		IL_0061: ldc.i4.0
		IL_0062: ceq
		IL_0064: stloc 19
		IL_0068: ldloc 19
		IL_006c: brtrue IL_0248

		IL_0071: br IL_0076

		IL_0076: br IL_007b

		IL_007b: ldstr "Unmanaged memory load: [v154 @ stack_-48_v5 (System.Object)+10]"
		IL_0080: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0085: ldc.i4.0
		IL_0086: conv.i
		IL_0087: ldnull
		IL_0088: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_008d: stloc 26
		IL_0091: ldloc 26
		IL_0095: ldc.i4.0
		IL_0096: ceq
		IL_0098: stloc 24
		IL_009c: ldloc 24
		IL_00a0: brtrue IL_0248

		IL_00a5: br IL_00aa

		IL_00aa: ldstr "Unmanaged memory load: [v154 @ stack_-48_v5 (System.Object)+10]"
		IL_00af: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00b4: ldc.i4.0
		IL_00b5: conv.i
		IL_00b6: stloc 22
		IL_00ba: ldstr "Unmanaged memory load: [v154 @ stack_-48_v5 (System.Object)+10]"
		IL_00bf: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_00c4: ldc.i4.0
		IL_00c5: conv.i
		IL_00c6: ldc.i4 0
		IL_00cb: ceq
		IL_00cd: stloc 28
		IL_00d1: ldc.i4.0
		IL_00d2: conv.i
		IL_00d3: stloc 16
		IL_00d7: ldloc 15
		IL_00db: stloc 29
		IL_00df: ldloc 28
		IL_00e3: brtrue IL_01f1

		IL_00e8: br IL_00ed

		IL_00ed: ldloc 22
		IL_00f1: stloc 20
		IL_00f5: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
		IL_00fa: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_00ff: stloc 21
		IL_0103: ldstr "Unmanaged memory load: [v297 @ r9_v10 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
		IL_0108: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_010d: ldc.i4.0
		IL_010e: conv.i
		IL_010f: stloc 30
		IL_0113: ldstr "Unmanaged memory load: [v295 @ r10_v8 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
		IL_0118: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_011d: ldc.i4.0
		IL_011e: conv.i
		IL_011f: ldstr "Unmanaged memory load: [v297 @ r9_v10 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
		IL_0124: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0129: ldc.i4.0
		IL_012a: conv.i
		IL_012b: clt
		IL_012d: stloc 31
		IL_0131: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
		IL_0136: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_013b: stloc 16
		IL_013f: ldloc 15
		IL_0143: stloc 29
		IL_0147: ldloc 31
		IL_014b: brtrue IL_01f1

		IL_0150: br IL_0155

		IL_0155: ldstr "Unmanaged memory load: [v295 @ r10_v8 (Il2CppClass<UnityEngine.RenderTexture>)+C8]"
		IL_015a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_015f: ldc.i4.0
		IL_0160: conv.i
		IL_0161: stloc 32
		IL_0165: ldstr "Unmanaged memory load: [v436 @ rax_v30+FFFFFFF8+v400 @ rax_v29*8]"
		IL_016a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_016f: ldc.i4.0
		IL_0170: conv.i
		IL_0171: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
		IL_0176: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
```
Context at line 14270:
```il
			IL_0010: stloc 9
			IL_0014: ldloc 9
			IL_0018: brtrue IL_0238

			IL_001d: br IL_0022

			IL_0022: ldarg resource
			IL_0026: ldarg preferThumbnail
			IL_002a: call string ScenarioResourceManager::BackgroundPreviewKey(class BackgroundResource, bool)
			IL_002f: stloc 11
			IL_0033: br IL_0038

			IL_0038: ldarg.0
			IL_0039: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class ScenarioResourceManager/BackgroundPreview> ScenarioResourceManager::backgroundPreviews
			IL_003e: ldloc 11
			IL_0042: ldloca 10
			IL_0046: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
			IL_004b: stloc 13
			IL_004f: ldloc 13
			IL_0053: ldc.i4.0
			IL_0054: ceq
			IL_0056: stloc 19
			IL_005a: ldloc 19
			IL_005e: brtrue IL_021d

			IL_0063: br IL_0068

			IL_0068: br IL_006d

			IL_006d: ldstr "Unmanaged memory load: [v77 @ stack_20_v4 (System.Object)+10]"
			IL_0072: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0077: ldc.i4.0
			IL_0078: conv.i
			IL_0079: ldnull
			IL_007a: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_007f: stloc 16
			IL_0083: ldloc 16
			IL_0087: ldc.i4.0
			IL_0088: ceq
			IL_008a: stloc 24
			IL_008e: ldloc 24
			IL_0092: brtrue IL_021d

			IL_0097: br IL_009c

			IL_009c: ldstr "Unmanaged memory load: [v77 @ stack_20_v4 (System.Object)+10]"
			IL_00a1: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00a6: ldc.i4.0
			IL_00a7: conv.i
			IL_00a8: stloc 18
			IL_00ac: ldstr "Unmanaged memory load: [v77 @ stack_20_v4 (System.Object)+10]"
			IL_00b1: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00b6: ldc.i4.0
			IL_00b7: conv.i
			IL_00b8: ldc.i4 0
			IL_00bd: ceq
			IL_00bf: stloc 28
			IL_00c3: ldc.i4.0
			IL_00c4: conv.i
			IL_00c5: stloc 14
			IL_00c9: ldloc 10
			IL_00cd: stloc 15
			IL_00d1: ldloc 28
			IL_00d5: brtrue IL_01e3

			IL_00da: br IL_00df

			IL_00df: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
			IL_00e4: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			IL_00e9: stloc 21
			IL_00ed: ldloc 18
			IL_00f1: stloc 20
			IL_00f5: ldstr "Unmanaged memory load: [v285 @ r9_v8 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
			IL_00fa: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_00ff: ldc.i4.0
			IL_0100: conv.i
			IL_0101: stloc 29
			IL_0105: ldstr "Unmanaged memory load: [v281 @ r10_v6 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
			IL_010a: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_010f: ldc.i4.0
			IL_0110: conv.i
			IL_0111: ldstr "Unmanaged memory load: [v285 @ r9_v8 (Il2CppClass<UnityEngine.RenderTexture>)+130]"
			IL_0116: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_011b: ldc.i4.0
			IL_011c: conv.i
			IL_011d: clt
			IL_011f: stloc 30
			IL_0123: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
			IL_0128: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			IL_012d: stloc 14
			IL_0131: ldloc 10
			IL_0135: stloc 15
			IL_0139: ldloc 30
			IL_013d: brtrue IL_01e3

			IL_0142: br IL_0147

			IL_0147: ldstr "Unmanaged memory load: [v281 @ r10_v6 (Il2CppClass<UnityEngine.RenderTexture>)+C8]"
			IL_014c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0151: ldc.i4.0
			IL_0152: conv.i
			IL_0153: stloc 31
			IL_0157: ldstr "Unmanaged memory load: [v377 @ rax_v20+FFFFFFF8+v362 @ rax_v19*8]"
			IL_015c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
			IL_0161: ldc.i4.0
			IL_0162: conv.i
			IL_0163: ldtoken [UnityEngine.CoreModule]UnityEngine.RenderTexture
			IL_0168: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
```
Context at line 14454:
```il

		IL_0260: ret

		IL_0261: ldstr "Warning: Method ends with non empty stack (-48), the output could be wrong!"
		IL_0266: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
	} // end of method ScenarioResourceManager::GetBackgroundPreviewAsync

	.method private hidebysig static 
		string BackgroundPreviewKey (
			class BackgroundResource resource,
			bool preferThumbnail
		) cil managed 
	{
		// Method begins at RVA 0x31f434
		// Header size: 12
		// Code size: 1109 (0x455)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] string,
			[2] string[],
			[3] string[],
			[4] object,
			[5] object,
			[6] object,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] string,
			[13] string,
			[14] string,
			[15] string,
			[16] string,
			[17] bool,
			[18] bool,
			[19] bool,
			[20] bool,
			[21] bool,
			[22] class [mscorlib]System.IndexOutOfRangeException,
			[23] string,
			[24] string,
			[25] bool,
			[26] bool,
			[27] string,
			[28] string,
			[29] string,
			[30] string,
			[31] bool,
			[32] bool,
			[33] string,
			[34] bool,
			[35] valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'
		)

		IL_0000: br IL_0005

		IL_0005: ldarg resource
		IL_0009: call instance string BackgroundResource::get_CacheKey()
		IL_000e: stloc 1
		IL_0012: br IL_0017

		IL_0017: ldloca 34
		IL_001b: call instance string [mscorlib]System.Boolean::ToString()
		IL_0020: stloc 12
		IL_0024: ldloc 1
		IL_0028: ldstr "\n"
		IL_002d: ldloc 12
		IL_0031: call string [mscorlib]System.String::Concat(string, string, string)
		IL_0036: stloc 23
		IL_003a: br IL_003f

		IL_003f: ldstr "Unmanaged memory load: [resource @ rcx (BackgroundResource)+24]"
		IL_0044: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0049: ldc.i4.0
		IL_004a: conv.i
		IL_004b: ldc.i4 0
		IL_0050: ceq
		IL_0052: stloc 25
		IL_0056: ldloc 25
		IL_005a: brtrue IL_0409

		IL_005f: br IL_0064

		IL_0064: ldstr "Method not found @1809B0420"
		IL_0069: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_006e: ldloc 4
		IL_0072: ldc.i4 0
		IL_0077: ceq
		IL_0079: stloc 26
		IL_007d: ldloc 26
		IL_0081: brtrue IL_040e

		IL_0086: br IL_008b

		IL_008b: ldc.i4 5
		IL_0090: newarr [mscorlib]System.String
		IL_0095: stloc 2
		IL_0099: br IL_009e

		IL_009e: ldloc 2
		IL_00a2: ldlen
		IL_00a3: conv.i4
		IL_00a4: ldc.i4 0
		IL_00a9: cgt
		IL_00ab: ldc.i4.0
		IL_00ac: ceq
```
Context at line 14592:
```il
		IL_0111: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0116: ldc.i4.0
		IL_0117: conv.i
		IL_0118: ldstr ".skel"
		IL_011d: call string [mscorlib]System.String::Concat(string, string)
		IL_0122: stloc 28
		IL_0126: ldloc 28
		IL_012a: ldloca 35
		IL_012e: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0133: stloc 13
		IL_0137: br IL_013c

		IL_013c: ldloc 2
		IL_0140: ldlen
		IL_0141: conv.i4
		IL_0142: ldc.i4 2
		IL_0147: cgt
		IL_0149: ldc.i4.0
		IL_014a: ceq
		IL_014c: stloc 18
		IL_0150: ldloc 18
		IL_0154: brtrue IL_0447

		IL_0159: br IL_015e

		IL_015e: ldloc 2
		IL_0162: ldc.i4 2
		IL_0167: ldloc 13
		IL_016b: stelem [mscorlib]System.String
		IL_0170: ldloc 2
		IL_0174: ldlen
		IL_0175: conv.i4
		IL_0176: ldc.i4 3
		IL_017b: cgt
		IL_017d: ldc.i4.0
		IL_017e: ceq
		IL_0180: stloc 8
		IL_0184: ldloc 8
		IL_0188: brtrue IL_0447

		IL_018d: br IL_0192

		IL_0192: ldloc 2
		IL_0196: ldc.i4 3
		IL_019b: ldstr "\n"
		IL_01a0: stelem [mscorlib]System.String
		IL_01a5: br IL_01aa

		IL_01aa: ldstr "Unmanaged memory load: [v35 @ stack_8_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+30]"
		IL_01af: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_01b4: ldc.i4.0
		IL_01b5: conv.i
		IL_01b6: ldstr ".atlas"
		IL_01bb: call string [mscorlib]System.String::Concat(string, string)
		IL_01c0: stloc 29
		IL_01c4: ldloc 29
		IL_01c8: ldloca 35
		IL_01cc: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_01d1: stloc 14
		IL_01d5: br IL_01da

		IL_01da: ldloc 2
		IL_01de: ldlen
		IL_01df: conv.i4
		IL_01e0: ldc.i4 4
		IL_01e5: cgt
		IL_01e7: ldc.i4.0
		IL_01e8: ceq
		IL_01ea: stloc 9
		IL_01ee: ldloc 9
		IL_01f2: brtrue IL_0447

		IL_01f7: br IL_01fc

		IL_01fc: ldloc 2
		IL_0200: ldc.i4 4
		IL_0205: ldloc 14
		IL_0209: stelem [mscorlib]System.String
		IL_020e: ldloc 2
		IL_0212: call string [mscorlib]System.String::Concat(string[])
		IL_0217: stloc 30
		IL_021b: br IL_0220

		IL_0220: ldc.i4 5
		IL_0225: newarr [mscorlib]System.String
		IL_022a: stloc 3
		IL_022e: br IL_0233

		IL_0233: ldloc 3
		IL_0237: ldlen
		IL_0238: conv.i4
		IL_0239: ldc.i4 0
		IL_023e: cgt
		IL_0240: ldc.i4.0
		IL_0241: ceq
		IL_0243: stloc 19
		IL_0247: ldloc 19
		IL_024b: brtrue IL_0447

		IL_0250: br IL_0255

		IL_0255: ldloc 3
		IL_0259: ldc.i4 0
		IL_025e: ldloc 30
		IL_0262: stelem [mscorlib]System.String
		IL_0267: ldloc 3
		IL_026b: ldlen
		IL_026c: conv.i4
```
Context at line 14641:
```il
		IL_01af: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_01b4: ldc.i4.0
		IL_01b5: conv.i
		IL_01b6: ldstr ".atlas"
		IL_01bb: call string [mscorlib]System.String::Concat(string, string)
		IL_01c0: stloc 29
		IL_01c4: ldloc 29
		IL_01c8: ldloca 35
		IL_01cc: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_01d1: stloc 14
		IL_01d5: br IL_01da

		IL_01da: ldloc 2
		IL_01de: ldlen
		IL_01df: conv.i4
		IL_01e0: ldc.i4 4
		IL_01e5: cgt
		IL_01e7: ldc.i4.0
		IL_01e8: ceq
		IL_01ea: stloc 9
		IL_01ee: ldloc 9
		IL_01f2: brtrue IL_0447

		IL_01f7: br IL_01fc

		IL_01fc: ldloc 2
		IL_0200: ldc.i4 4
		IL_0205: ldloc 14
		IL_0209: stelem [mscorlib]System.String
		IL_020e: ldloc 2
		IL_0212: call string [mscorlib]System.String::Concat(string[])
		IL_0217: stloc 30
		IL_021b: br IL_0220

		IL_0220: ldc.i4 5
		IL_0225: newarr [mscorlib]System.String
		IL_022a: stloc 3
		IL_022e: br IL_0233

		IL_0233: ldloc 3
		IL_0237: ldlen
		IL_0238: conv.i4
		IL_0239: ldc.i4 0
		IL_023e: cgt
		IL_0240: ldc.i4.0
		IL_0241: ceq
		IL_0243: stloc 19
		IL_0247: ldloc 19
		IL_024b: brtrue IL_0447

		IL_0250: br IL_0255

		IL_0255: ldloc 3
		IL_0259: ldc.i4 0
		IL_025e: ldloc 30
		IL_0262: stelem [mscorlib]System.String
		IL_0267: ldloc 3
		IL_026b: ldlen
		IL_026c: conv.i4
		IL_026d: ldc.i4 1
		IL_0272: cgt
		IL_0274: ldc.i4.0
		IL_0275: ceq
		IL_0277: stloc 10
		IL_027b: ldloc 10
		IL_027f: brtrue IL_0447

		IL_0284: br IL_0289

		IL_0289: ldloc 3
		IL_028d: ldc.i4 1
		IL_0292: ldstr "\n"
		IL_0297: stelem [mscorlib]System.String
		IL_029c: br IL_02a1

		IL_02a1: ldstr "Unmanaged memory load: [v35 @ stack_8_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+38]"
		IL_02a6: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_02ab: ldc.i4.0
		IL_02ac: conv.i
		IL_02ad: stloc 5
		IL_02b1: br IL_02b6

		IL_02b6: ldstr "Unmanaged memory load: [v95 @ rcx_v30+38]"
		IL_02bb: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_02c0: ldc.i4.0
		IL_02c1: conv.i
		IL_02c2: call bool [mscorlib]System.String::IsNullOrWhiteSpace(string)
		IL_02c7: stloc 31
		IL_02cb: ldloc 31
		IL_02cf: ldc.i4.0
		IL_02d0: ceq
		IL_02d2: stloc 32
		IL_02d6: ldloc 32
		IL_02da: brtrue IL_02f2

		IL_02df: br IL_02e4

		IL_02e4: ldstr "no-preview"
		IL_02e9: stloc 15
		IL_02ed: br IL_0325

		IL_02f2: ldstr "Unmanaged memory load: [v35 @ stack_8_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+38]"
		IL_02f7: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_02fc: ldc.i4.0
		IL_02fd: conv.i
		IL_02fe: stloc 6
		IL_0302: br IL_0307

```
Context at line 14746:
```il
		IL_02fe: stloc 6
		IL_0302: br IL_0307

		IL_0307: ldstr "Unmanaged memory load: [v97 @ rcx_v40+38]"
		IL_030c: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0311: ldc.i4.0
		IL_0312: conv.i
		IL_0313: ldloca 35
		IL_0317: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_031c: stloc 15
		IL_0320: br IL_0325

		IL_0325: ldloc 3
		IL_0329: ldlen
		IL_032a: conv.i4
		IL_032b: ldc.i4 2
		IL_0330: cgt
		IL_0332: ldc.i4.0
		IL_0333: ceq
		IL_0335: stloc 20
		IL_0339: ldloc 20
		IL_033d: brtrue IL_0447

		IL_0342: br IL_0347

		IL_0347: ldloc 3
		IL_034b: ldc.i4 2
		IL_0350: ldloc 15
		IL_0354: stelem [mscorlib]System.String
		IL_0359: ldloc 3
		IL_035d: ldlen
		IL_035e: conv.i4
		IL_035f: ldc.i4 3
		IL_0364: cgt
		IL_0366: ldc.i4.0
		IL_0367: ceq
		IL_0369: stloc 11
		IL_036d: ldloc 11
		IL_0371: brtrue IL_0447

		IL_0376: br IL_037b

		IL_037b: ldloc 3
		IL_037f: ldc.i4 3
		IL_0384: ldstr "\n"
		IL_0389: stelem [mscorlib]System.String
		IL_038e: br IL_0393

		IL_0393: ldstr "Unmanaged memory load: [v35 @ stack_8_v1 (ScenarioResourceManager+<>c__DisplayClass24_0)+30]"
		IL_0398: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_039d: ldc.i4.0
		IL_039e: conv.i
		IL_039f: ldstr ".skel.preview.png"
		IL_03a4: call string [mscorlib]System.String::Concat(string, string)
		IL_03a9: stloc 33
		IL_03ad: ldloc 33
		IL_03b1: ldloca 35
		IL_03b5: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_03ba: stloc 16
		IL_03be: br IL_03c3

		IL_03c3: ldloc 3
		IL_03c7: ldlen
		IL_03c8: conv.i4
		IL_03c9: ldc.i4 4
		IL_03ce: cgt
		IL_03d0: ldc.i4.0
		IL_03d1: ceq
		IL_03d3: stloc 21
		IL_03d7: ldloc 21
		IL_03db: brtrue IL_0447

		IL_03e0: br IL_03e5

		IL_03e5: ldloc 3
		IL_03e9: ldc.i4 4
		IL_03ee: ldloc 16
		IL_03f2: stelem [mscorlib]System.String
		IL_03f7: ldloc 3
		IL_03fb: call string [mscorlib]System.String::Concat(string[])
		IL_0400: stloc 23
		IL_0404: br IL_0409

		IL_0409: ldloc 23
		IL_040d: ret

		IL_040e: ldstr "Unmanaged memory load: [resource @ rcx (BackgroundResource)+30]"
		IL_0413: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0418: ldc.i4.0
		IL_0419: conv.i
		IL_041a: ldloca 35
		IL_041e: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0423: stloc 27
		IL_0427: ldloc 23
		IL_042b: ldstr "\n"
		IL_0430: ldloc 27
		IL_0434: call string [mscorlib]System.String::Concat(string, string, string)
		IL_0439: stloc 24
		IL_043d: br IL_0442

		IL_0442: ldloc 24
		IL_0446: ret

		IL_0447: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_044c: stloc 22
		IL_0450: ldloc 22
		IL_0454: ret
	} // end of method ScenarioResourceManager::BackgroundPreviewKey
```
Context at line 14795:
```il
		IL_0398: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_039d: ldc.i4.0
		IL_039e: conv.i
		IL_039f: ldstr ".skel.preview.png"
		IL_03a4: call string [mscorlib]System.String::Concat(string, string)
		IL_03a9: stloc 33
		IL_03ad: ldloc 33
		IL_03b1: ldloca 35
		IL_03b5: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_03ba: stloc 16
		IL_03be: br IL_03c3

		IL_03c3: ldloc 3
		IL_03c7: ldlen
		IL_03c8: conv.i4
		IL_03c9: ldc.i4 4
		IL_03ce: cgt
		IL_03d0: ldc.i4.0
		IL_03d1: ceq
		IL_03d3: stloc 21
		IL_03d7: ldloc 21
		IL_03db: brtrue IL_0447

		IL_03e0: br IL_03e5

		IL_03e5: ldloc 3
		IL_03e9: ldc.i4 4
		IL_03ee: ldloc 16
		IL_03f2: stelem [mscorlib]System.String
		IL_03f7: ldloc 3
		IL_03fb: call string [mscorlib]System.String::Concat(string[])
		IL_0400: stloc 23
		IL_0404: br IL_0409

		IL_0409: ldloc 23
		IL_040d: ret

		IL_040e: ldstr "Unmanaged memory load: [resource @ rcx (BackgroundResource)+30]"
		IL_0413: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0418: ldc.i4.0
		IL_0419: conv.i
		IL_041a: ldloca 35
		IL_041e: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0423: stloc 27
		IL_0427: ldloc 23
		IL_042b: ldstr "\n"
		IL_0430: ldloc 27
		IL_0434: call string [mscorlib]System.String::Concat(string, string, string)
		IL_0439: stloc 24
		IL_043d: br IL_0442

		IL_0442: ldloc 24
		IL_0446: ret

		IL_0447: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_044c: stloc 22
		IL_0450: ldloc 22
		IL_0454: ret
	} // end of method ScenarioResourceManager::BackgroundPreviewKey

	.method private hidebysig 
		instance void QueueBackgroundPreview (
			class BackgroundResource resource,
			bool preferThumbnail,
			string key,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x31f898
		// Header size: 12
		// Code size: 361 (0x169)
		.maxstack 0
		.locals init (
			[0] string,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] object,
			[5] bool,
			[6] object[],
			[7] bool,
			[8] class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>,
			[9] int32,
			[10] bool,
			[11] string,
			[12] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[13] class ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28',
			[14] int32,
			[15] int32
		)

		IL_0000: br IL_0005

		IL_0005: ldarg.0
		IL_0006: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>> ScenarioResourceManager::pendingBackgroundPreviews
		IL_000b: ldarg key
		IL_000f: ldloca 4
		IL_0013: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_0018: stloc 5
		IL_001c: ldloc 5
		IL_0020: brtrue IL_00b3

		IL_0025: br IL_002a

		IL_002a: newobj instance void class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>::.ctor()
		IL_002f: stloc 8
		IL_0033: nop
		IL_0034: br IL_0039
```
Context at line 14829:
```il
		IL_0409: ldloc 23
		IL_040d: ret

		IL_040e: ldstr "Unmanaged memory load: [resource @ rcx (BackgroundResource)+30]"
		IL_0413: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0418: ldc.i4.0
		IL_0419: conv.i
		IL_041a: ldloca 35
		IL_041e: call string ScenarioResourceManager::'<BackgroundPreviewKey>g__Stamp|24_0'(string, valuetype ScenarioResourceManager/'<>c__DisplayClass24_0'&)
		IL_0423: stloc 27
		IL_0427: ldloc 23
		IL_042b: ldstr "\n"
		IL_0430: ldloc 27
		IL_0434: call string [mscorlib]System.String::Concat(string, string, string)
		IL_0439: stloc 24
		IL_043d: br IL_0442

		IL_0442: ldloc 24
		IL_0446: ret

		IL_0447: newobj instance void [mscorlib]System.IndexOutOfRangeException::.ctor()
		IL_044c: stloc 22
		IL_0450: ldloc 22
		IL_0454: ret
	} // end of method ScenarioResourceManager::BackgroundPreviewKey

	.method private hidebysig 
		instance void QueueBackgroundPreview (
			class BackgroundResource resource,
			bool preferThumbnail,
			string key,
			class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture> callback
		) cil managed 
	{
		// Method begins at RVA 0x31f898
		// Header size: 12
		// Code size: 361 (0x169)
		.maxstack 0
		.locals init (
			[0] string,
			[1] bool,
			[2] class BackgroundResource,
			[3] class ScenarioResourceManager,
			[4] object,
			[5] bool,
			[6] object[],
			[7] bool,
			[8] class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>,
			[9] int32,
			[10] bool,
			[11] string,
			[12] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[13] class ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28',
			[14] int32,
			[15] int32
		)

		IL_0000: br IL_0005

		IL_0005: ldarg.0
		IL_0006: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>> ScenarioResourceManager::pendingBackgroundPreviews
		IL_000b: ldarg key
		IL_000f: ldloca 4
		IL_0013: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_0018: stloc 5
		IL_001c: ldloc 5
		IL_0020: brtrue IL_00b3

		IL_0025: br IL_002a

		IL_002a: newobj instance void class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>::.ctor()
		IL_002f: stloc 8
		IL_0033: nop
		IL_0034: br IL_0039

		IL_0039: ldloc 8
		IL_003d: ldloc 11
		IL_0041: ldc.i4.0
		IL_0042: conv.i
		IL_0043: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>>::TryGetValue(!0, !1&)
		IL_0048: stloc 7
		IL_004c: br IL_0051

		IL_0051: ldarg.0
		IL_0052: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class [mscorlib]System.Collections.Generic.List`1<class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Texture>>> ScenarioResourceManager::pendingBackgroundPreviews
		IL_0057: ldarg key
		IL_005b: ldloc 8
		IL_005f: call instance void class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::Add(!0, !1)
		IL_0064: br IL_0155

		IL_0069: ldloc 13
		IL_006d: ldarg.0
		IL_006e: stfld class ScenarioResourceManager ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::'<>4__this'
		IL_0073: ldloc 13
		IL_0077: ldarg resource
		IL_007b: stfld class BackgroundResource ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::resource
		IL_0080: br IL_0085

		IL_0085: ldloc 13
		IL_0089: ldarg preferThumbnail
		IL_008d: stfld bool ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::preferThumbnail
		IL_0092: ldloc 13
		IL_0096: ldarg key
		IL_009a: stfld string ScenarioResourceManager/'<CoLoadBackgroundPreview>d__28'::key
		IL_009f: ldarg.0
		IL_00a0: ldloc 13
		IL_00a4: call instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_00a9: stloc 12
```
#### AcquireBackgroundSpine: 2 occurrences
Context at line 13017:
```il
				IL_0051: brtrue IL_0021
			// end loop

			IL_0056: br IL_0005
		// end loop
	} // end of method ScenarioResourceManager::CoTryPreloadBackgroundTexture

	.method public hidebysig 
		instance class RuntimeSpineAsset AcquireBackgroundSpine (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x31e514
		// Header size: 12
		// Code size: 1220 (0x4c4)
		.maxstack 0
		.locals init (
			[0] class BackgroundResource,
			[1] bool,
			[2] bool,
			[3] string,
			[4] bool,
			[5] bool,
			[6] class RuntimeSpineAsset,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] int32,
			[13] object,
			[14] bool,
			[15] class ScenarioResourceManager,
			[16] object,
			[17] native int,
			[18] native int,
			[19] string,
			[20] bool,
			[21] class [mscorlib]System.ObjectDisposedException,
			[22] class [mscorlib]System.InvalidOperationException,
			[23] bool,
			[24] object,
			[25] object,
			[26] native int,
			[27] bool,
			[28] bool,
			[29] bool,
			[30] object,
			[31] class RuntimeSpineAsset,
			[32] int32,
			[33] native int,
			[34] bool,
			[35] class ['spine-csharp']Spine.SkeletonData,
			[36] int32,
			[37] bool,
			[38] bool,
			[39] class RuntimeSpineLoader/Files,
			[40] class ScenarioResourceManager,
			[41] object,
			[42] class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>
		)

		IL_0000: br IL_0442

		IL_0005: ldarg resource
		IL_0009: call instance bool BackgroundResource::get_IsCustomSpine()
		IL_000e: stloc 2
		IL_0012: ldloc 2
		IL_0016: ldc.i4.0
		IL_0017: ceq
		IL_0019: stloc 11
		IL_001d: ldnull
		IL_001e: stloc 19
		IL_0022: ldloc 11
		IL_0026: brtrue IL_0467

		IL_002b: br IL_0030

		IL_0030: ldarg resource
		IL_0034: call instance string BackgroundResource::get_FullPath()
		IL_0039: stloc 3
		IL_003d: ldarg.0
		IL_003e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class RuntimeSpineAsset> ScenarioResourceManager::backgroundSkeletons
		IL_0043: ldc.i4 0
		IL_0048: ceq
		IL_004a: stloc 7
		IL_004e: ldnull
		IL_004f: stloc 13
		IL_0053: ldloc 7
		IL_0057: brtrue IL_036b

		IL_005c: br IL_0061

		IL_0061: ldarg.0
		IL_0062: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class RuntimeSpineAsset> ScenarioResourceManager::backgroundSkeletons
		IL_0067: ldloc 3
		IL_006b: ldloca 41
		IL_006f: call instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<object, object>::TryGetValue(!0, !1&)
		IL_0074: stloc 23
		IL_0078: ldloca 41
		IL_007c: stloc 25
		IL_0080: ldc.i4.0
		IL_0081: conv.i
		IL_0082: stloc 26
		IL_0086: ldloc 3
		IL_008a: stloc 30
		IL_008e: ldarg.0
		IL_008f: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<string, class RuntimeSpineAsset> ScenarioResourceManager::backgroundSkeletons
```
Context at line 13448:
```il
		IL_04a7: call instance void class [mscorlib]System.Collections.Generic.Dictionary`2<string, int32>::set_Item(!0, !1)
		IL_04ac: ldstr "RuntimeSpineAsset"
		IL_04b1: newobj instance void [mscorlib]System.ObjectDisposedException::.ctor(string)
		IL_04b6: stloc 21
		IL_04ba: br IL_0386

		IL_04bf: ldloc 6
		IL_04c3: ret
	} // end of method ScenarioResourceManager::AcquireBackgroundSpine

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			uint32 id
		) cil managed 
	{
		// Method begins at RVA 0x31e9e4
		// Header size: 12
		// Code size: 29 (0x1d)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[2] uint32
		)

		IL_0000: ldarg.0
		IL_0001: ldarg id
		IL_0005: call instance class BackgroundResource ScenarioResourceManager::ResolveBackground(uint32)
		IL_000a: stloc 1
		IL_000e: ldstr "Invalid instruction: 12 Invalid \"Jump target not found in method: 0x1807BC3A0\""
		IL_0013: call void Cpp2ILInjected.Cpp2ILHelpers::NoteDecompilerIssue(string)
		IL_0018: ldloc 1
		IL_001c: ret
	} // end of method ScenarioResourceManager::GetBackgroundTexture

	.method public hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.Texture GetBackgroundTexture (
			class BackgroundResource resource
		) cil managed 
	{
		// Method begins at RVA 0x31ea10
		// Header size: 12
		// Code size: 1024 (0x400)
		.maxstack 0
		.locals init (
			[0] class ScenarioResourceManager,
			[1] bool,
			[2] bool,
			[3] bool,
			[4] bool,
			[5] native int,
			[6] class BackgroundResource,
			[7] bool,
			[8] class [System]System.IO.InvalidDataException,
			[9] string,
			[10] string,
			[11] class [mscorlib]System.InvalidOperationException,
			[12] bool,
			[13] object,
			[14] object,
			[15] bool,
			[16] bool,
			[17] string,
			[18] bool,
			[19] bool,
			[20] bool,
			[21] object,
			[22] bool,
			[23] bool,
			[24] bool,
			[25] string,
			[26] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[27] uint8[],
			[28] native int,
			[29] class [UnityEngine.CoreModule]UnityEngine.Object,
			[30] class [System]System.IO.InvalidDataException,
			[31] bool,
			[32] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[33] class [UnityEngine.CoreModule]UnityEngine.Texture,
			[34] bool,
			[35] class [UnityEngine.CoreModule]UnityEngine.Texture2D,
			[36] bool,
			[37] bool,
			[38] bool,
			[39] string,
			[40] string,
			[41] bool,
			[42] class [System]System.IO.InvalidDataException,
			[43] class BackgroundResource,
			[44] class BackgroundResource,
			[45] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[46] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>,
			[47] valuetype [Unity.ResourceManager]UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle`1<object>
		)

		IL_0000: br IL_035c

		IL_0005: ldloc 44
		IL_0009: ldfld valuetype ScenarioBGType BackgroundResource::'<Type>k__BackingField'
		IL_000e: ldc.i4 1
		IL_0013: ceq
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: stloc 2
		IL_001c: ldloc 2
		IL_0020: brtrue IL_0379

		IL_0025: br IL_002a
```

## Guardrails
- A recovered IL call is evidence of the call in that recovered method body, not proof that every runtime path succeeds.
- If the IL contains NoteDecompilerIssue/invalid instruction/unmanaged memory load, treat affected branches as unresolved.
- Android and Windows are used as cross-checks; disagreement must not be silently merged.
- No C# compatibility code or IPA is built by this workflow.
