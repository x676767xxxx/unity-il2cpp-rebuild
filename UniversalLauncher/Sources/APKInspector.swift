import Foundation
import ZIPFoundation

struct APKReport {
    let entryCount: Int
    let hasManifest: Bool
    let dexCount: Int
    let supportedABIs: [String]
    let hasNativeLibraries: Bool
    let manifestInfo: AXMLManifestParser.ManifestInfo?
    let manifestParseError: String?

    var summary: String {
        let abiText = supportedABIs.isEmpty ? "No native ABI folders detected" : supportedABIs.joined(separator: ", ")
        var lines = [
            "APK structure inspection",
            "Entries: \(entryCount)",
            "AndroidManifest.xml: \(hasManifest ? "present" : "missing")",
            "DEX files: \(dexCount)",
            "Native libraries: \(hasNativeLibraries ? "present" : "not detected")",
            "ABIs: \(abiText)"
        ]

        if let manifestInfo {
            lines += [
                "",
                "Manifest metadata",
                "Package: \(manifestInfo.packageName ?? "not found")",
                "Version code: \(manifestInfo.versionCode ?? "not found")",
                "Version name: \(manifestInfo.versionName ?? "not found")",
                "Min SDK: \(manifestInfo.minSDK ?? "not found")",
                "Target SDK: \(manifestInfo.targetSDK ?? "not found")",
                "Permissions: \(manifestInfo.permissions.isEmpty ? "none detected" : manifestInfo.permissions.joined(separator: ", "))",
                "Activities: \(manifestInfo.activities.isEmpty ? "none detected" : manifestInfo.activities.joined(separator: ", "))",
                "Launcher activities: \(manifestInfo.launcherActivities.isEmpty ? "none detected" : manifestInfo.launcherActivities.joined(separator: ", "))"
            ]
        } else if let manifestParseError {
            lines += ["", "Manifest parsing: \(manifestParseError)"]
        }

        lines += ["", "This is structural inspection only. It does not install or execute the APK."]
        return lines.joined(separator: "\n")
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
        let manifestEntry = archive["AndroidManifest.xml"]
        let dexCount = paths.filter {
            let name = URL(fileURLWithPath: $0).lastPathComponent
            return name == "classes.dex" || name.range(of: #"^classes[0-9]+\.dex$"#, options: .regularExpression) != nil
        }.count

        let abiNames = ["arm64-v8a", "armeabi-v7a", "x86_64", "x86"]
        let abis = abiNames.filter { abi in
            paths.contains { $0.hasPrefix("lib/\(abi)/") }
        }

        var manifestInfo: AXMLManifestParser.ManifestInfo?
        var manifestParseError: String?
        if let manifestEntry {
            let temporaryURL = FileManager.default.temporaryDirectory
                .appendingPathComponent("UniversalLauncher-\(UUID().uuidString)-AndroidManifest.xml")
            defer { try? FileManager.default.removeItem(at: temporaryURL) }
            do {
                _ = try archive.extract(manifestEntry, to: temporaryURL)
                let data = try Data(contentsOf: temporaryURL)
                manifestInfo = try AXMLManifestParser.parse(data)
            } catch {
                manifestParseError = error.localizedDescription
            }
        }

        return APKReport(
            entryCount: paths.count,
            hasManifest: manifestEntry != nil,
            dexCount: dexCount,
            supportedABIs: abis,
            hasNativeLibraries: paths.contains { $0.hasPrefix("lib/") && $0.hasSuffix(".so") },
            manifestInfo: manifestInfo,
            manifestParseError: manifestParseError
        )
    }
}
