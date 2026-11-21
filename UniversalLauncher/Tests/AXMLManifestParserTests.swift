import XCTest
@testable import UniversalLauncher

final class AXMLManifestParserTests: XCTestCase {
    func testParsesManifestMetadataFromUTF8StringPool() throws {
        let info = try AXMLManifestParser.parse(AXMLFixture.make(utf8: true))
        XCTAssertEqual(info.packageName, "com.example.test")
        XCTAssertEqual(info.minSDK, "21")
        XCTAssertEqual(info.targetSDK, "35")
        XCTAssertEqual(info.permissions, ["android.permission.INTERNET"])
        XCTAssertEqual(info.activities, [".MainActivity"])
        XCTAssertEqual(info.launcherActivities, [".MainActivity"])
    }

    func testParsesManifestMetadataFromUTF16StringPool() throws {
        let info = try AXMLManifestParser.parse(AXMLFixture.make(utf8: false))
        XCTAssertEqual(info.packageName, "com.example.test")
        XCTAssertEqual(info.launcherActivities, [".MainActivity"])
    }

    func testDoesNotCombineMainAndLauncherAcrossDifferentIntentFilters() throws {
        let info = try AXMLManifestParser.parse(AXMLFixture.make(utf8: true, splitLauncherFilter: true))
        XCTAssertTrue(info.launcherActivities.isEmpty)
    }

    func testRejectsTruncatedDocument() {
        XCTAssertThrowsError(try AXMLManifestParser.parse(Data([0x03, 0x00, 0x08])))
    }
}

private enum AXMLFixture {
    private static let strings = [
        "manifest", "package", "com.example.test",
        "uses-sdk", "minSdkVersion", "targetSdkVersion",
        "uses-permission", "name", "android.permission.INTERNET",
        "activity", ".MainActivity", "intent-filter", "action",
        "android.intent.action.MAIN", "category", "android.intent.category.LAUNCHER"
    ]

    static func make(utf8: Bool, splitLauncherFilter: Bool = false) -> Data {
        var pool = Data()
        var encodedStrings = Data()
        var offsets: [UInt32] = []

        for value in strings {
            offsets.append(UInt32(encodedStrings.count))
            if utf8 {
                let bytes = Array(value.utf8)
                appendLength8(UInt8(bytes.count), to: &encodedStrings)
                appendLength8(UInt8(value.utf16.count), to: &encodedStrings)
                encodedStrings.append(contentsOf: bytes)
                encodedStrings.append(0)
            } else {
                let units = Array(value.utf16)
                appendLE(UInt16(units.count), to: &encodedStrings)
                for unit in units { appendLE(unit, to: &encodedStrings) }
                appendLE(UInt16(0), to: &encodedStrings)
            }
        }

        let headerSize: UInt32 = 28
        let stringsStart = headerSize + UInt32(strings.count * 4)
        let poolSize = stringsStart + UInt32(encodedStrings.count)
        appendLE(UInt16(0x0001), to: &pool)
        appendLE(UInt16(28), to: &pool)
        appendLE(poolSize, to: &pool)
        appendLE(UInt32(strings.count), to: &pool)
        appendLE(UInt32(0), to: &pool)
        appendLE(utf8 ? UInt32(0x100) : UInt32(0), to: &pool)
        appendLE(stringsStart, to: &pool)
        appendLE(UInt32(0), to: &pool)
        for offset in offsets { appendLE(offset, to: &pool) }
        pool.append(encodedStrings)

        var nodes = Data()
        nodes.append(start("manifest", attributes: [("package", .string("com.example.test"))]))
        nodes.append(start("uses-sdk", attributes: [
            ("minSdkVersion", .integer(21)), ("targetSdkVersion", .integer(35))
        ]))
        nodes.append(end("uses-sdk"))
        nodes.append(start("uses-permission", attributes: [("name", .string("android.permission.INTERNET"))]))
        nodes.append(end("uses-permission"))
        nodes.append(start("activity", attributes: [("name", .string(".MainActivity"))]))

        if splitLauncherFilter {
            nodes.append(start("intent-filter"))
            nodes.append(start("action", attributes: [("name", .string("android.intent.action.MAIN"))]))
            nodes.append(end("action"))
            nodes.append(end("intent-filter"))
            nodes.append(start("intent-filter"))
            nodes.append(start("category", attributes: [("name", .string("android.intent.category.LAUNCHER"))]))
            nodes.append(end("category"))
            nodes.append(end("intent-filter"))
        } else {
            nodes.append(start("intent-filter"))
            nodes.append(start("action", attributes: [("name", .string("android.intent.action.MAIN"))]))
            nodes.append(end("action"))
            nodes.append(start("category", attributes: [("name", .string("android.intent.category.LAUNCHER"))]))
            nodes.append(end("category"))
            nodes.append(end("intent-filter"))
        }

        nodes.append(end("activity"))
        nodes.append(end("manifest"))

        var document = Data()
        appendLE(UInt16(0x0003), to: &document)
        appendLE(UInt16(8), to: &document)
        appendLE(UInt32(8 + pool.count + nodes.count), to: &document)
        document.append(pool)
        document.append(nodes)
        return document
    }

    private enum Value {
        case string(String)
        case integer(UInt32)
    }

    private static func start(_ element: String, attributes: [(String, Value)] = []) -> Data {
        var chunk = Data()
        appendLE(UInt16(0x0102), to: &chunk)
        appendLE(UInt16(16), to: &chunk)
        appendLE(UInt32(36 + attributes.count * 20), to: &chunk)
        appendLE(UInt32(1), to: &chunk) // line number
        appendLE(UInt32.max, to: &chunk) // comment
        appendLE(UInt32.max, to: &chunk) // namespace
        appendLE(index(element), to: &chunk)
        appendLE(UInt16(20), to: &chunk) // attributeStart
        appendLE(UInt16(20), to: &chunk) // attributeSize
        appendLE(UInt16(attributes.count), to: &chunk)
        appendLE(UInt16(0), to: &chunk) // id index
        appendLE(UInt16(0), to: &chunk) // class index
        appendLE(UInt16(0), to: &chunk) // style index

        for (name, value) in attributes {
            appendLE(UInt32.max, to: &chunk)
            appendLE(index(name), to: &chunk)
            switch value {
            case .string(let string):
                appendLE(index(string), to: &chunk)
                appendLE(UInt16(8), to: &chunk)
                chunk.append(0)
                chunk.append(0x03)
                appendLE(index(string), to: &chunk)
            case .integer(let integer):
                appendLE(UInt32.max, to: &chunk)
                appendLE(UInt16(8), to: &chunk)
                chunk.append(0)
                chunk.append(0x10)
                appendLE(integer, to: &chunk)
            }
        }
        return chunk
    }

    private static func end(_ element: String) -> Data {
        var chunk = Data()
        appendLE(UInt16(0x0103), to: &chunk)
        appendLE(UInt16(16), to: &chunk)
        appendLE(UInt32(24), to: &chunk)
        appendLE(UInt32(1), to: &chunk)
        appendLE(UInt32.max, to: &chunk)
        appendLE(UInt32.max, to: &chunk)
        appendLE(index(element), to: &chunk)
        return chunk
    }

    private static func index(_ value: String) -> UInt32 {
        UInt32(strings.firstIndex(of: value)!)
    }

    private static func appendLength8(_ value: UInt8, to data: inout Data) {
        data.append(value)
    }

    private static func appendLE<T: FixedWidthInteger>(_ value: T, to data: inout Data) {
        var littleEndian = value.littleEndian
        withUnsafeBytes(of: &littleEndian) { data.append(contentsOf: $0) }
    }
}
