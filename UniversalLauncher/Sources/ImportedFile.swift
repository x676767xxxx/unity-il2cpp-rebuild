import Foundation

enum ImportedFileKind: String, Codable {
    case apk
    case zip
    case rar
    case sevenZip
    case other

    var title: String {
        switch self {
        case .apk: return "Android APK"
        case .zip: return "ZIP archive"
        case .rar: return "RAR archive"
        case .sevenZip: return "7Z archive"
        case .other: return "Other file"
        }
    }

    var systemImage: String {
        switch self {
        case .apk: return "app.badge"
        case .zip, .rar, .sevenZip: return "archivebox"
        case .other: return "doc"
        }
    }

    var detail: String {
        switch self {
        case .apk: return "Imported for inspection; Android execution engine is not connected."
        case .zip: return "Archive imported; extraction is the next milestone."
        case .rar, .sevenZip: return "Archive imported; decoder support will be added after ZIP."
        case .other: return "Stored in the app library."
        }
    }
}

struct ImportedFile: Identifiable, Codable {
    let id: UUID
    let name: String
    let storedURL: URL
    let kind: ImportedFileKind
    let byteCount: Int64

    var sizeDescription: String {
        ByteCountFormatter.string(fromByteCount: byteCount, countStyle: .file)
    }
}

enum FileClassifier {
    static func classify(_ url: URL) -> ImportedFileKind {
        switch url.pathExtension.lowercased() {
        case "apk": return .apk
        case "zip": return .zip
        case "rar": return .rar
        case "7z": return .sevenZip
        default: return .other
        }
    }
}
