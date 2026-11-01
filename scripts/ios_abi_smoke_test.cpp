#include "azurearchive_spine_converter_abi.h"
#include <algorithm>
#include <cstdint>
#include <filesystem>
#include <fstream>
#include <iostream>
#include <iterator>
#include <string>
#include <vector>

namespace {
bool convertFixture(const std::filesystem::path& path) {
    std::ifstream input(path, std::ios::binary);
    if (!input) {
        std::cerr << "Cannot open fixture: " << path << "\\n";
        return false;
    }
    std::vector<uint8_t> json((std::istreambuf_iterator<char>(input)),
                              std::istreambuf_iterator<char>());
    if (json.empty() || json.size() > static_cast<size_t>(INT32_MAX)) {
        std::cerr << "Invalid fixture length: " << path << "\\n";
        return false;
    }

    uint8_t* binary = nullptr;
    int32_t binaryLength = 0;
    int rc = AzureArchiveSpineJsonToBinary(
        reinterpret_cast<const char*>(json.data()),
        static_cast<int32_t>(json.size()), &binary, &binaryLength);
    if (rc != 0 || !binary || binaryLength <= 0) {
        std::cerr << "JSON-to-SKEL failed for " << path << ": " << rc << "\\n";
        AzureArchiveSpineFreeBuffer(binary);
        return false;
    }

    uint8_t* roundTrip = nullptr;
    int32_t roundTripLength = 0;
    rc = AzureArchiveSpineConvertBuffer(
        binary, binaryLength, 6, &roundTrip, &roundTripLength);
    AzureArchiveSpineFreeBuffer(binary);
    if (rc != 0 || !roundTrip || roundTripLength <= 0) {
        std::cerr << "SKEL-to-SKEL Spine 4.2 validation failed for "
                  << path << ": " << rc << "\\n";
        AzureArchiveSpineFreeBuffer(roundTrip);
        return false;
    }

    std::cout << "PASS " << path.filename().string() << " (JSON "
              << json.size() << " -> SKEL " << binaryLength
              << " -> SKEL " << roundTripLength << " bytes)\\n";
    AzureArchiveSpineFreeBuffer(roundTrip);
    return true;
}
}

int main(int argc, char** argv) {
    // Invalid inputs must fail cleanly and clear output parameters.
    uint8_t* invalidOutput = reinterpret_cast<uint8_t*>(1);
    int32_t invalidLength = 99;
    int invalidRc = AzureArchiveSpineJsonToBinary(nullptr, 1, &invalidOutput, &invalidLength);
    if (invalidRc != -1 || invalidOutput != nullptr || invalidLength != 0) {
        std::cerr << "Null JSON input contract failed\\n";
        return 10;
    }
    invalidOutput = reinterpret_cast<uint8_t*>(1);
    invalidLength = 99;
    const char emptyJson[] = "";
    invalidRc = AzureArchiveSpineJsonToBinary(emptyJson, 0, &invalidOutput, &invalidLength);
    if (invalidRc != -1 || invalidOutput != nullptr || invalidLength != 0) {
        std::cerr << "Empty JSON input contract failed\\n";
        return 11;
    }
    uint8_t dummy = 0;
    invalidOutput = nullptr;
    invalidLength = 99;
    invalidRc = AzureArchiveSpineConvertBuffer(&dummy, 1, 99, &invalidOutput, &invalidLength);
    if (invalidRc != -2 || invalidOutput != nullptr || invalidLength != 0) {
        std::cerr << "Invalid destination version contract failed\\n";
        return 12;
    }
    if (AzureArchiveSpineJsonToBinary(emptyJson, 0, nullptr, &invalidLength) != -1) {
        std::cerr << "Null output pointer contract failed\\n";
        return 13;
    }
    std::cout << "PASS: 4 invalid-input and output-parameter checks\\n";

    if (argc != 2) {
        std::cerr << "Usage: abi_smoke <Spine 4.2 JSON file or fixture directory>\\n";
        return 2;
    }

    const std::filesystem::path target(argv[1]);
    std::vector<std::filesystem::path> fixtures;
    if (std::filesystem::is_directory(target)) {
        for (const auto& entry : std::filesystem::recursive_directory_iterator(target)) {
            if (entry.is_regular_file() && entry.path().extension() == ".json")
                fixtures.push_back(entry.path());
        }
        std::sort(fixtures.begin(), fixtures.end());
    } else if (std::filesystem::is_regular_file(target)) {
        fixtures.push_back(target);
    } else {
        std::cerr << "Fixture path does not exist: " << target << "\\n";
        return 3;
    }

    if (fixtures.empty()) {
        std::cerr << "No Spine JSON fixtures found under: " << target << "\\n";
        return 4;
    }

    size_t passed = 0;
    for (const auto& fixture : fixtures) {
        if (!convertFixture(fixture)) return 5;
        ++passed;
    }
    std::cout << "PASS: " << passed
              << " Spine 4.2 JSON -> SKEL -> SKEL ABI fixtures\\n";
    return 0;
}
