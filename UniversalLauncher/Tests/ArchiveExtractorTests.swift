import XCTest
@testable import UniversalLauncher

final class ArchiveExtractorTests: XCTestCase {
    func testAcceptsNormalRelativePaths() {
        XCTAssertTrue(ArchiveExtractor.isSafeRelativePath("file.txt"))
        XCTAssertTrue(ArchiveExtractor.isSafeRelativePath("assets/images/icon.png"))
        XCTAssertTrue(ArchiveExtractor.isSafeRelativePath("assets/"))
    }

    func testRejectsEmptyAndAbsolutePaths() {
        XCTAssertFalse(ArchiveExtractor.isSafeRelativePath(""))
        XCTAssertFalse(ArchiveExtractor.isSafeRelativePath("/private/file.txt"))
    }

    func testRejectsParentAndCurrentDirectoryTraversal() {
        XCTAssertFalse(ArchiveExtractor.isSafeRelativePath("../outside.txt"))
        XCTAssertFalse(ArchiveExtractor.isSafeRelativePath("assets/../../outside.txt"))
        XCTAssertFalse(ArchiveExtractor.isSafeRelativePath("./file.txt"))
    }

    func testRejectsWindowsSeparatorsAndNUL() {
        XCTAssertFalse(ArchiveExtractor.isSafeRelativePath("..\\\\outside.txt"))
        XCTAssertFalse(ArchiveExtractor.isSafeRelativePath("folder\\\\file.txt"))
        XCTAssertFalse(ArchiveExtractor.isSafeRelativePath("folder/\0file.txt"))
    }
}
