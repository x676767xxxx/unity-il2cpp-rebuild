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
    static func isSafeRelativePath(_ path: String) -> Bool {
        guard !path.isEmpty,
              !path.hasPrefix("/"),
              !path.unicodeScalars.contains(where: { $0.value == 0x5C || $0.value == 0 }) else {
            return false
        }

        let components = path.split(separator: "/", omittingEmptySubsequences: true)
        return !components.isEmpty && components.allSatisfy { $0 != "." && $0 != ".." }
    }

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
        var extractionSucceeded = false
        defer {
            if !extractionSucceeded {
                try? manager.removeItem(at: root)
            }
        }

        let archive = try Archive(url: archiveURL, accessMode: .read)
        let rootPath = root.standardizedFileURL.path + "/"

        for entry in archive {
            let relativePath = entry.path
            guard isSafeRelativePath(relativePath) else {
                throw ArchiveExtractionError.unsafeEntry(relativePath)
            }
            guard entry.type != .symlink else {
                throw ArchiveExtractionError.symbolicLink(relativePath)
            }

            let destination = root.appendingPathComponent(relativePath).standardizedFileURL
            guard destination.path.hasPrefix(rootPath) else {
                throw ArchiveExtractionError.unsafeEntry(relativePath)
            }

            try archive.extract(entry, to: destination)
        }

        extractionSucceeded = true
        return root
    }
}
