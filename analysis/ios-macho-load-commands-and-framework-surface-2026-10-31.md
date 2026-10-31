# iOS 0.7.9.2 Mach-O dependencies and framework surface

Source: XR1 release IPA; Actions workflow inspect-ios-mach-o-surface.

This is static binary metadata. A symbol/string is not proof of a usable managed API or runtime compatibility.

## App metadata and framework inventory
```text
CFBundleIdentifier: com.foxxlight.AzureArchive
CFBundleExecutable: AzureArchive
CFBundleShortVersionString: 0.7.9.2
CFBundleVersion: 0
MinimumOSVersion: 13.0
DTPlatformName: iphoneos
DTSDKName: iphoneos16.4
DTXcode: 1431
DTXcodeBuild: 14E300c
LSRequiresIPhoneOS: True

Frameworks:
UnityFramework.framework dir

Embedded frameworks/dylibs:

```

## UnityFramework Mach-O inspection
```text
## UnityFramework file
work/app/Payload/AzureArchive.app/Frameworks/UnityFramework.framework/UnityFramework: Mach-O universal binary with 1 architecture: [arm64:\012- Mach-O 64-bit arm64 dynamically linked shared library, flags:<NOUNDEFS|DYLDLINK|TWOLEVEL|WEAK_DEFINES|BINDS_TO_WEAK|NO_REEXPORTED_DYLIBS|HAS_TLV_DESCRIPTORS>]
## Mach-O load commands
      cmd LC_SEGMENT_64
  segname __TEXT
  sectname __text
   segname __TEXT
  sectname __stubs
   segname __TEXT
  sectname __stub_helper
   segname __TEXT
  sectname __gcc_except_tab
   segname __TEXT
  sectname __literal8
   segname __TEXT
  sectname __objc_methname
   segname __TEXT
  sectname __cstring
   segname __TEXT
  sectname __const
   segname __TEXT
  sectname __literal4
   segname __TEXT
  sectname __literal16
   segname __TEXT
  sectname __textcoal_nt
   segname __TEXT
  sectname __const_coal
   segname __TEXT
  sectname il2cpp
   segname __TEXT
  sectname __unwind_info
   segname __TEXT
  sectname __eh_frame
   segname __TEXT
      cmd LC_SEGMENT_64
  segname __DATA_CONST
  sectname __got
   segname __DATA_CONST
  sectname __const
   segname __DATA_CONST
  sectname __objc_protolist
   segname __DATA_CONST
  sectname __objc_classlist
   segname __DATA_CONST
  sectname __objc_catlist
   segname __DATA_CONST
  sectname __cfstring
   segname __DATA_CONST
  sectname __mod_init_func
   segname __DATA_CONST
  sectname __objc_imageinfo
   segname __DATA_CONST
      cmd LC_SEGMENT_64
  segname __DATA
  sectname __la_symbol_ptr
   segname __DATA
  sectname __data
   segname __DATA
  sectname __objc_selrefs
   segname __DATA
  sectname __objc_data
   segname __DATA
  sectname __objc_superrefs
   segname __DATA
  sectname __objc_ivar
   segname __DATA
  sectname __objc_const
   segname __DATA
  sectname __objc_classrefs
   segname __DATA
  sectname __thread_vars
   segname __DATA
  sectname __objc_protorefs
   segname __DATA
  sectname __datacoal_nt
   segname __DATA
  sectname __thread_bss
   segname __DATA
  sectname __common
   segname __DATA
  sectname __bss
   segname __DATA
      cmd LC_SEGMENT_64
  segname __LINKEDIT
            cmd LC_DYLD_INFO_ONLY
     cmd LC_SYMTAB
            cmd LC_DYSYMTAB
          cmd LC_ENCRYPTION_INFO_64
          cmd LC_ID_DYLIB
         name @executable_path/Frameworks/UnityFramework.framework/UnityFramework (offset 24)
      current version 0.0.0
compatibility version 0.0.0
     cmd LC_UUID
       cmd LC_BUILD_VERSION
  platform ios
       sdk 16.4
     minos 13.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/UIKit.framework/UIKit (offset 24)
      current version 6441.1.101
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/CoreText.framework/CoreText (offset 24)
      current version 1.0.0
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/QuartzCore.framework/QuartzCore (offset 24)
      current version 1.11.0
compatibility version 1.2.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/CoreVideo.framework/CoreVideo (offset 24)
      current version 1.5.0
compatibility version 1.2.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/Metal.framework/Metal (offset 24)
      current version 306.5.16
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/Foundation.framework/Foundation (offset 24)
      current version 1971.0.0
compatibility version 300.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/CoreFoundation.framework/CoreFoundation (offset 24)
      current version 1971.0.0
compatibility version 150.0.0
          cmd LC_LOAD_DYLIB
         name /usr/lib/libobjc.A.dylib (offset 24)
      current version 228.0.0
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/CFNetwork.framework/CFNetwork (offset 24)
      current version 1406.0.4
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/Security.framework/Security (offset 24)
      current version 60420.102.1
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/CoreGraphics.framework/CoreGraphics (offset 24)
      current version 1690.5.4
compatibility version 64.0.0
          cmd LC_LOAD_WEAK_DYLIB
         name /usr/lib/libSystem.B.dylib (offset 24)
      current version 1319.100.3
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /usr/lib/libc++.1.dylib (offset 24)
      current version 1500.65.0
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /usr/lib/libiconv.2.dylib (offset 24)
      current version 7.0.0
compatibility version 7.0.0
          cmd LC_LOAD_WEAK_DYLIB
         name /System/Library/Frameworks/CoreMotion.framework/CoreMotion (offset 24)
      current version 2785.1.32
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/MediaToolbox.framework/MediaToolbox (offset 24)
      current version 1.0.0
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/AudioToolbox.framework/AudioToolbox (offset 24)
      current version 1000.0.0
compatibility version 1.0.0
          cmd LC_LOAD_WEAK_DYLIB
         name /System/Library/Frameworks/AVFoundation.framework/AVFoundation (offset 24)
      current version 2.0.0
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/AVKit.framework/AVKit (offset 24)
      current version 1.0.0
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/CoreMedia.framework/CoreMedia (offset 24)
      current version 1.0.0
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/OpenAL.framework/OpenAL (offset 24)
      current version 1.0.0
compatibility version 1.0.0
          cmd LC_LOAD_DYLIB
         name /System/Library/Frameworks/SystemConfiguration.framework/SystemConfiguration (offset 24)
      current version 1241.100.11
compatibility version 1.0.0
          cmd LC_LOAD_WEAK_DYLIB
         name /System/Library/Frameworks/GameController.framework/GameController (offset 24)
      current version 10.5.10
compatibility version 1.0.0
      cmd LC_FUNCTION_STARTS
      cmd LC_DATA_IN_CODE
          cmd LC_RPATH
      cmd LC_CODE_SIGNATURE
## Dylibs used
work/app/Payload/AzureArchive.app/Frameworks/UnityFramework.framework/UnityFramework:
	@executable_path/Frameworks/UnityFramework.framework/UnityFramework (compatibility version 0.0.0, current version 0.0.0)
	/System/Library/Frameworks/UIKit.framework/UIKit (compatibility version 1.0.0, current version 6441.1.101)
	/System/Library/Frameworks/CoreText.framework/CoreText (compatibility version 1.0.0, current version 1.0.0)
	/System/Library/Frameworks/QuartzCore.framework/QuartzCore (compatibility version 1.2.0, current version 1.11.0)
	/System/Library/Frameworks/CoreVideo.framework/CoreVideo (compatibility version 1.2.0, current version 1.5.0)
	/System/Library/Frameworks/Metal.framework/Metal (compatibility version 1.0.0, current version 306.5.16)
	/System/Library/Frameworks/Foundation.framework/Foundation (compatibility version 300.0.0, current version 1971.0.0)
	/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation (compatibility version 150.0.0, current version 1971.0.0)
	/usr/lib/libobjc.A.dylib (compatibility version 1.0.0, current version 228.0.0)
	/System/Library/Frameworks/CFNetwork.framework/CFNetwork (compatibility version 1.0.0, current version 1406.0.4)
	/System/Library/Frameworks/Security.framework/Security (compatibility version 1.0.0, current version 60420.102.1)
	/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics (compatibility version 64.0.0, current version 1690.5.4)
	/usr/lib/libSystem.B.dylib (compatibility version 1.0.0, current version 1319.100.3, weak)
	/usr/lib/libc++.1.dylib (compatibility version 1.0.0, current version 1500.65.0)
	/usr/lib/libiconv.2.dylib (compatibility version 7.0.0, current version 7.0.0)
	/System/Library/Frameworks/CoreMotion.framework/CoreMotion (compatibility version 1.0.0, current version 2785.1.32, weak)
	/System/Library/Frameworks/MediaToolbox.framework/MediaToolbox (compatibility version 1.0.0, current version 1.0.0)
	/System/Library/Frameworks/AudioToolbox.framework/AudioToolbox (compatibility version 1.0.0, current version 1000.0.0)
	/System/Library/Frameworks/AVFoundation.framework/AVFoundation (compatibility version 1.0.0, current version 2.0.0, weak)
	/System/Library/Frameworks/AVKit.framework/AVKit (compatibility version 1.0.0, current version 1.0.0)
	/System/Library/Frameworks/CoreMedia.framework/CoreMedia (compatibility version 1.0.0, current version 1.0.0)
	/System/Library/Frameworks/OpenAL.framework/OpenAL (compatibility version 1.0.0, current version 1.0.0)
	/System/Library/Frameworks/SystemConfiguration.framework/SystemConfiguration (compatibility version 1.0.0, current version 1241.100.11)
	/System/Library/Frameworks/GameController.framework/GameController (compatibility version 1.0.0, current version 10.5.10, weak)
## Exported symbols relevant to Unity/Spine/image/Addressables
work/app/Payload/AzureArchive.app/Frameworks/UnityFramework.framework/UnityFramework:
0000000004cd3c70 S _OBJC_CLASS_$_UnityFramework
0000000004cd3c98 S _OBJC_METACLASS_$_UnityFramework
000000000031d828 T _UnitySendMessage
0000000004ce2490 S __gUnityFramework
## Framework binary inventory
work/app/Payload/AzureArchive.app/Frameworks/UnityFramework.framework/Info.plist: Apple binary property list
work/app/Payload/AzureArchive.app/Frameworks/UnityFramework.framework/UnityFramework: Mach-O universal binary with 1 architecture: [arm64:\012- Mach-O 64-bit arm64 dynamically linked shared library, flags:<NOUNDEFS|DYLDLINK|TWOLEVEL|WEAK_DEFINES|BINDS_TO_WEAK|NO_REEXPORTED_DYLIBS|HAS_TLV_DESCRIPTORS>]
work/app/Payload/AzureArchive.app/Frameworks/UnityFramework.framework/_CodeSignature/CodeResources: XML 1.0 document, ASCII text
work/app/Payload/AzureArchive.app/Frameworks/UnityFramework.framework/PrivacyInfo.xcprivacy: XML 1.0 document, ASCII text

```

## Interpretation limits
- LC_LOAD_DYLIB records show linked dynamic dependencies, not all statically linked code.
- Exported symbols are only the native symbol surface; IL2CPP managed methods are usually not exported under their C# names.
- This does not prove the iOS 1.0 game can run in the old UnityFramework.
- No compatibility code or IPA is built by this inspection.
