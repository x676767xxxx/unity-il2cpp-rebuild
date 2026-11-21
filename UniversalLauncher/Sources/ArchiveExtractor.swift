import Foundation
import ZIPFoundation

enum ArchiveExtractionError: LocalizedError {
    case unsupportedFormat
    case unsafeEntry(String)
    case symbolicLink(String)

    var errorDescription: String? {
        switch self {
        case .unsupportedFormat:
            return "Only ZIP archives can be extracted in this version."
        case .unsafeEntry(let path):
            return "The archive contains an unsafe path: \(path)"
        case .symbolicLink(let path):
            return "Symbolic links are not extracted: \(path)"
        }
    }
}

enum ArchiveExtractor {
    static func extractZIP(at archiveURL: URL) throws -> URL {
        guard archiveURL.pathExtension.lowercased() == "zip" else {
            throw ArchiveExtractionError.unsupportedFormat
        }

        let manager = FileManager.default
        let documents = try manager.url(
            for: .documentDirectory,
            in: .userDomainMask,
            appropriateFor: nil,
            create: true
        )
        let root = documents
            .appendingPathComponent("Extracted", isDirectory: true)
            .appendingPathComponent(UUID().uuidString, isDirectory: true)
        try manager.createDirectory(at: root, withIntermediateDirectories: true)

        let archive = try Archive(url: archiveURL, accessMode: .read)
        let rootPath = root.standardizedFileURL.path + "/"

        for entry in archive {
            let relativePath = entry.path
            let destination = root.appendingPathComponent(relativePath).standardizedFileURL

            guard destination.path.hasPrefix(rootPath) else {
                throw ArchiveExtractionError.unsafeEntry(relativePath)
            }
            guard entry.type != .symlink else {
                throw ArchiveExtractionError.symbolicLink(relativePath)
            }

            try archive.extract(entry, to: destination)
        }

        return root
    }
}
