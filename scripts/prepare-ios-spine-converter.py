#!/usr/bin/env python3
from pathlib import Path
root = Path(__file__).resolve().parents[1] / "external" / "SpineSkeletonDataConverter"
main = root / "src" / "main.cpp"
cmake = root / "CMakeLists.txt"
if not main.is_file() or not cmake.is_file():
    raise SystemExit("Pinned Spine converter source tree is incomplete")
text = main.read_text(encoding="utf-8")
entry = "int main(int argc, char* argv[]) {"
if text.count(entry) != 1:
    raise SystemExit("Expected exactly one CLI main() in src/main.cpp")
if "SPINE_CONVERTER_NO_MAIN" not in text:
    text = text.replace(entry, "#ifndef SPINE_CONVERTER_NO_MAIN\n" + entry, 1)
    text = text.rstrip() + "\n#endif // SPINE_CONVERTER_NO_MAIN\n"
    main.write_text(text, encoding="utf-8")
abi = r'''#define SPINE_CONVERTER_NO_MAIN
#include "main.cpp"
#include <cstdint>
#include <cstdlib>
#include <cstring>
#include <filesystem>
#include <fstream>
#include <string>
#include <vector>
#include <unistd.h>
#include "azurearchive_spine_converter_abi.h"
namespace {
int convertBuffer(const uint8_t* input, int32_t inputLength, int32_t dstVersion,
                  bool inputIsJson, uint8_t** output, int32_t* outputLength) noexcept {
    if (!output || !outputLength) return -1;
    *output = nullptr; *outputLength = 0;
    if (!input || inputLength <= 0) return -1;
    if (dstVersion < -1 || dstVersion > 6) return -2;
    try {
        std::string pattern = (std::filesystem::temp_directory_path() / "azspine-XXXXXX").string();
        std::vector<char> temp(pattern.begin(), pattern.end()); temp.push_back('\0');
        char* created = mkdtemp(temp.data());
        if (!created) return -3;
        std::filesystem::path dir(created);
        const auto inputPath = dir / (inputIsJson ? "input.json" : "input.skel");
        const auto outputPath = dir / "output.skel";
        {
            std::ofstream stream(inputPath, std::ios::binary);
            if (!stream) { std::filesystem::remove_all(dir); return -4; }
            stream.write(reinterpret_cast<const char*>(input), inputLength);
            if (!stream.good()) { std::filesystem::remove_all(dir); return -5; }
        }
        const SpineVersion inputVersion = detectSpineVersion(inputPath.string());
        if (inputVersion == SpineVersion::Invalid) { std::filesystem::remove_all(dir); return -6; }
        const SpineVersion outputVersion = dstVersion == -1 ? inputVersion : static_cast<SpineVersion>(dstVersion);
        const FileFormat inputFormat = inputIsJson ? FileFormat::Json : FileFormat::Skel;
        if (!convertFile(inputPath.string(), outputPath.string(), inputFormat, FileFormat::Skel,
                         inputVersion, outputVersion, std::string(), false)) {
            std::filesystem::remove_all(dir); return -7;
        }
        std::ifstream stream(outputPath, std::ios::binary);
        if (!stream) { std::filesystem::remove_all(dir); return -8; }
        std::vector<uint8_t> bytes((std::istreambuf_iterator<char>(stream)), std::istreambuf_iterator<char>());
        std::filesystem::remove_all(dir);
        if (bytes.empty() || bytes.size() > static_cast<size_t>(INT32_MAX)) return -9;
        auto* result = static_cast<uint8_t*>(std::malloc(bytes.size()));
        if (!result) return -10;
        std::memcpy(result, bytes.data(), bytes.size());
        *output = result; *outputLength = static_cast<int32_t>(bytes.size());
        return 0;
    } catch (...) { return -99; }
}
}
extern "C" int AzureArchiveSpineConvertBuffer(const uint8_t* in_buf, int32_t in_len,
                                               int32_t dst_version, uint8_t** out_buf, int32_t* out_len) {
    return convertBuffer(in_buf, in_len, dst_version, false, out_buf, out_len);
}
extern "C" int AzureArchiveSpineJsonToBinary(const char* json_utf8, int32_t json_len,
                                              uint8_t** out_buf, int32_t* out_len) {
    return convertBuffer(reinterpret_cast<const uint8_t*>(json_utf8), json_len, 6, true, out_buf, out_len);
}
extern "C" void AzureArchiveSpineFreeBuffer(void* buffer) { std::free(buffer); }
'''
(root / "src" / "ios_abi.cpp").write_text(abi, encoding="utf-8")
header = Path(__file__).resolve().parents[1] / "scripts" / "azurearchive_spine_converter_abi.h"
(root / "include" / "azurearchive_spine_converter_abi.h").write_text(header.read_text(encoding="utf-8"), encoding="utf-8")
common = root / "src" / "common.cpp"
common_text = common.read_text(encoding="utf-8")
required_includes = ["#include <sstream>", "#include <cstring>", "#include <cstdlib>", "#include <cstdio>", "#include <cmath>", "#include <iomanip>"]
for include in required_includes:
    if include not in common_text:
        common_text = common_text.replace('#include "SkeletonData.h"', '#include "SkeletonData.h"\n' + include, 1)
common.write_text(common_text, encoding="utf-8")

cmake_text = cmake.read_text(encoding="utf-8")
marker = "add_executable(SpineAtlasDowngrade"
if "azurearchive_spine_converter_ios" not in cmake_text:
    dollar = chr(36)
    block = """
# iOS ARM64 static library: the CLI entry point is compiled out in ios_abi.cpp.
file(GLOB SPINE_CONVERTER_CORE_SOURCES CONFIGURE_DEPENDS "@DOLLAR@{CMAKE_CURRENT_SOURCE_DIR}/src/*.cpp")
list(REMOVE_ITEM SPINE_CONVERTER_CORE_SOURCES
    "@DOLLAR@{CMAKE_CURRENT_SOURCE_DIR}/src/main.cpp"
    "@DOLLAR@{CMAKE_CURRENT_SOURCE_DIR}/src/SpineAtlasDowngrade.cpp")
add_library(azurearchive_spine_converter_ios STATIC
    "@DOLLAR@{CMAKE_CURRENT_SOURCE_DIR}/src/ios_abi.cpp"
    @DOLLAR@{SPINE_CONVERTER_CORE_SOURCES})
target_include_directories(azurearchive_spine_converter_ios PRIVATE "@DOLLAR@{CMAKE_CURRENT_SOURCE_DIR}/include")
set_target_properties(azurearchive_spine_converter_ios PROPERTIES POSITION_INDEPENDENT_CODE ON OUTPUT_NAME "azurearchive_spine_converter_ios")
""".replace("@DOLLAR@", dollar)
    if marker not in cmake_text:
        raise SystemExit("Could not locate CMake target insertion point")
    cmake.write_text(cmake_text.replace(marker, block + "\n" + marker, 1), encoding="utf-8")
print("Prepared iOS static-library target and custom buffer C ABI.")
