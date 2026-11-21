import Foundation

/// Minimal, bounds-checked reader for Android's compiled binary XML (AXML).
/// It extracts manifest metadata only; it does not attempt to render arbitrary XML.
enum AXMLManifestParser {
    struct ManifestInfo {
        var packageName: String?
        var versionCode: String?
        var versionName: String?
        var minSDK: String?
        var targetSDK: String?
        var permissions: [String] = []
        var activities: [String] = []
        var launcherActivities: [String] = []
    }

    enum ParseError: LocalizedError {
        case truncated
        case invalidHeader
        case malformedChunk
        case missingStringPool
        case invalidStringIndex

        var errorDescription: String? {
            switch self {
            case .truncated: return "Manifest is truncated or contains an out-of-range offset."
            case .invalidHeader: return "AndroidManifest.xml is not a valid binary XML document."
            case .malformedChunk: return "AndroidManifest.xml contains a malformed chunk."
            case .missingStringPool: return "AXML string pool is missing."
            case .invalidStringIndex: return "AXML contains an invalid string index."
            }
        }
    }

    private struct Attribute {
        let name: String
        let value: String
    }

    static func parse(_ data: Data) throws -> ManifestInfo {
        var reader = Reader(data: data)
        guard try reader.u16(at: 0) == 0x0003,
              try reader.u16(at: 2) >= 8 else { throw ParseError.invalidHeader }
        let documentSize = Int(try reader.u32(at: 4))
        guard documentSize >= 8, documentSize <= data.count else { throw ParseError.truncated }

        var strings: [String]?
        var result = ManifestInfo()
        var elementStack: [String] = []
        var insideIntentFilter = false
        var filterHasMain = false
        var filterHasLauncher = false
        var currentActivity: String?

        var offset = 8
        while offset < documentSize {
            guard offset + 8 <= documentSize else { throw ParseError.truncated }
            let type = try reader.u16(at: offset)
            let headerSize = Int(try reader.u16(at: offset + 2))
            let chunkSize = Int(try reader.u32(at: offset + 4))
            guard headerSize >= 8, chunkSize >= headerSize,
                  offset + chunkSize <= documentSize else { throw ParseError.malformedChunk }

            if type == 0x0001 {
                strings = try parseStringPool(reader: reader, offset: offset, headerSize: headerSize, chunkSize: chunkSize)
            } else if type == 0x0102 {
                guard let pool = strings else { throw ParseError.missingStringPool }
                guard chunkSize >= headerSize + 20 else { throw ParseError.malformedChunk }
                let attributes = try parseStartElement(reader: reader, offset: offset, headerSize: headerSize, chunkSize: chunkSize, strings: pool)
                let elementName = try reader.stringIndex(at: offset + 20, strings: pool)
                elementStack.append(elementName)

                let map = Dictionary(attributes.map { ($0.name, $0.value) }, uniquingKeysWith: { first, _ in first })
                if elementName == "manifest" {
                    result.packageName = map["package"]
                    result.versionCode = map["versionCode"]
                    result.versionName = map["versionName"]
                } else if elementName == "uses-sdk" {
                    result.minSDK = map["minSdkVersion"]
                    result.targetSDK = map["targetSdkVersion"]
                } else if elementName == "uses-permission" || elementName == "uses-permission-sdk-23" {
                    if let name = map["name"] { result.permissions.append(name) }
                } else if elementName == "activity" || elementName == "activity-alias" {
                    currentActivity = map["name"]
                    if let currentActivity { result.activities.append(currentActivity) }
                } else if elementName == "intent-filter" {
                    insideIntentFilter = true
                    filterHasMain = false
                    filterHasLauncher = false
                } else if insideIntentFilter, elementName == "action", map["name"] == "android.intent.action.MAIN" {
                    filterHasMain = true
                } else if insideIntentFilter, elementName == "category", map["name"] == "android.intent.category.LAUNCHER" {
                    filterHasLauncher = true
                }
            } else if type == 0x0103 {
                guard chunkSize >= headerSize + 8 else { throw ParseError.malformedChunk }
                if let pool = strings {
                    let name = try reader.stringIndex(at: offset + 20, strings: pool)
                    if name == "intent-filter" {
                        if insideIntentFilter, filterHasMain && filterHasLauncher, let currentActivity {
                            result.launcherActivities.append(currentActivity)
                        }
                        insideIntentFilter = false
                        filterHasMain = false
                        filterHasLauncher = false
                    } else if name == "activity" || name == "activity-alias" {
                        currentActivity = nil
                        insideIntentFilter = false
                        filterHasMain = false
                        filterHasLauncher = false
                    }
                }
                if !elementStack.isEmpty { elementStack.removeLast() }
            }
            offset += chunkSize
        }

        result.permissions = Array(Set(result.permissions)).sorted()
        result.activities = Array(Set(result.activities)).sorted()
        result.launcherActivities = Array(Set(result.launcherActivities)).sorted()
        return result
    }

    private static func parseStringPool(reader: Reader, offset: Int, headerSize: Int, chunkSize: Int) throws -> [String] {
        guard headerSize >= 28, chunkSize >= headerSize else { throw ParseError.malformedChunk }
        let count = Int(try reader.u32(at: offset + 8))
        let flags = try reader.u32(at: offset + 16)
        let stringsStart = Int(try reader.u32(at: offset + 20))
        guard count >= 0, count <= 100_000,
              offset + headerSize + count * 4 <= offset + chunkSize,
              stringsStart >= headerSize, stringsStart <= chunkSize else { throw ParseError.malformedChunk }

        let utf8 = (flags & 0x100) != 0
        var values: [String] = []
        values.reserveCapacity(count)
        for index in 0..<count {
            let relative = Int(try reader.u32(at: offset + headerSize + index * 4))
            let start = offset + stringsStart + relative
            guard start >= offset, start < offset + chunkSize else { throw ParseError.truncated }
            if utf8 {
                var cursor = start
                let chunkEnd = offset + chunkSize
                _ = try reader.length8(at: &cursor, limit: chunkEnd)
                let byteLength = try reader.length8(at: &cursor, limit: chunkEnd)
                guard cursor <= chunkEnd, byteLength <= chunkEnd - cursor else { throw ParseError.truncated }
                let bytes = try reader.bytes(at: cursor, count: byteLength)
                guard let string = String(data: bytes, encoding: .utf8) else { throw ParseError.malformedChunk }
                values.append(string)
            } else {
                var cursor = start
                let chunkEnd = offset + chunkSize
                let unitLength = try reader.length16(at: &cursor, limit: chunkEnd)
                guard cursor <= chunkEnd, unitLength <= (chunkEnd - cursor) / 2 else { throw ParseError.truncated }
                let byteLength = unitLength * 2
                let bytes = try reader.bytes(at: cursor, count: byteLength)
                guard let string = String(data: bytes, encoding: .utf16LittleEndian) else { throw ParseError.malformedChunk }
                values.append(string)
            }
        }
        return values
    }

    private static func parseStartElement(reader: Reader, offset: Int, headerSize: Int, chunkSize: Int, strings: [String]) throws -> [Attribute] {
        let extensionOffset = offset + headerSize
        guard chunkSize >= headerSize + 20 else { throw ParseError.malformedChunk }
        let attributeStart = Int(try reader.u16(at: extensionOffset + 8))
        let attributeSize = Int(try reader.u16(at: extensionOffset + 10))
        let attributeCount = Int(try reader.u16(at: extensionOffset + 12))
        guard attributeSize >= 20, attributeCount <= 4096 else { throw ParseError.malformedChunk }
        let firstAttribute = extensionOffset + attributeStart
        guard firstAttribute >= offset, firstAttribute + attributeCount * attributeSize <= offset + chunkSize else { throw ParseError.truncated }

        var attributes: [Attribute] = []
        for index in 0..<attributeCount {
            let base = firstAttribute + index * attributeSize
            let nameIndex = Int(try reader.u32(at: base + 4))
            let rawValueIndex = try reader.u32(at: base + 8)
            let valueType = try reader.u8(at: base + 15)
            let valueData = try reader.u32(at: base + 16)
            guard nameIndex < strings.count else { throw ParseError.invalidStringIndex }
            let name = strings[nameIndex]
            let value: String
            if rawValueIndex != 0xFFFFFFFF {
                guard Int(rawValueIndex) < strings.count else { throw ParseError.invalidStringIndex }
                value = strings[Int(rawValueIndex)]
            } else if valueType == 0x03 {
                guard Int(valueData) < strings.count else { throw ParseError.invalidStringIndex }
                value = strings[Int(valueData)]
            } else if valueType == 0x12 {
                value = valueData == 0 ? "false" : "true"
            } else if valueType == 0x10 || valueType == 0x11 {
                value = String(valueData)
            } else {
                value = String(valueData)
            }
            attributes.append(Attribute(name: name, value: value))
        }
        return attributes
    }

    private struct Reader {
        let data: Data

        func bytes(at offset: Int, count: Int) throws -> Data {
            guard offset >= 0, count >= 0, offset <= data.count, count <= data.count - offset else {
                throw ParseError.truncated
            }
            return data.subdata(in: offset..<(offset + count))
        }

        func u8(at offset: Int) throws -> UInt8 {
            guard offset >= 0, offset < data.count else { throw ParseError.truncated }
            return data[offset]
        }

        func u16(at offset: Int) throws -> UInt16 {
            let b = try bytes(at: offset, count: 2)
            return UInt16(b[b.startIndex]) | (UInt16(b[b.startIndex + 1]) << 8)
        }

        func u32(at offset: Int) throws -> UInt32 {
            let b = try bytes(at: offset, count: 4)
            return UInt32(b[b.startIndex]) |
                (UInt32(b[b.startIndex + 1]) << 8) |
                (UInt32(b[b.startIndex + 2]) << 16) |
                (UInt32(b[b.startIndex + 3]) << 24)
        }

        func stringIndex(at offset: Int, strings: [String]) throws -> String {
            let index = try u32(at: offset)
            guard index != 0xFFFFFFFF, Int(index) < strings.count else { throw ParseError.invalidStringIndex }
            return strings[Int(index)]
        }

        func length8(at offset: inout Int, limit: Int) throws -> Int {
            guard offset >= 0, offset < limit else { throw ParseError.truncated }
            let first = Int(try u8(at: offset))
            offset += 1
            if first & 0x80 == 0 { return first }
            guard offset < limit else { throw ParseError.truncated }
            let second = Int(try u8(at: offset))
            offset += 1
            return ((first & 0x7F) << 8) | second
        }

        func length16(at offset: inout Int, limit: Int) throws -> Int {
            guard offset >= 0, offset <= limit - 2 else { throw ParseError.truncated }
            let first = Int(try u16(at: offset))
            offset += 2
            if first & 0x8000 == 0 { return first }
            guard offset <= limit - 2 else { throw ParseError.truncated }
            let second = Int(try u16(at: offset))
            offset += 2
            return ((first & 0x7FFF) << 16) | second
        }
    }
}
