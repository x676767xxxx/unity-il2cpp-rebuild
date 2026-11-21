import Foundation
import ZIPFoundation

struct APKReport {
    let entryCount: Int
    let hasManifest: Bool
    let dexCount: Int
    let supportedABIs: [String]
    let hasNativeLibraries: Bool

    var summary: String {
        let abiText = supportedABIs.isEmpty ? "No native ABI folders detected" : supportedABIs.joined(separator: ", ")
        return """
        APK structure inspection
        Entries: \(entryCount)
        AndroidManifest.xml: \(hasManifest ? "present" : "missing")
        DEX files: \(dexCount)
        Native libraries: \(hasNativeLibraries ? "present" : "not detected")
        ABIs: \(abiText)

        This is structural inspection only. It does not install or execute the APK.
        """
    }
}

enum APKInspector {
    static func inspect(_ url: URL) throws -> APKReport {
        guard url.pathExtension.lowercased() == "apk" else {
            throw NSError(
                domain: "UniversalLauncher.APKInspector",
                code: 1,
                userInfo: [NSLocalizedDescriptionKey: "Selected file is not an APK."]
            )
        }

        let archive = try Archive(url: url, accessMode: .read)
        let paths = archive.map { $0.path }
        let hasManifest = paths.contains { $0 == "AndroidManifest.xml" }
        let dexCount = paths.filter {
            let name = URL(fileURLWithPath: $0).lastPathComponent
            return name == "classes.dex" || name.range(of: #"^classes[0-9]+\.dex$"#, options: .regularExpression) != nil
        }.count

        let abiNames = ["arm64-v8a", "armeabi-v7a", "x86_64", "x86"]
        let abis = abiNames.filter { abi in
            paths.contains { $0.hasPrefix("lib/\(abi)/") }
        }

        return APKReport(
            entryCount: paths.count,
            hasManifest: hasManifest,
            dexCount: dexCount,
            supportedABIs: abis,
            hasNativeLibraries: paths.contains { $0.hasPrefix("lib/") && $0.hasSuffix(".so") }
        )
    }
}
