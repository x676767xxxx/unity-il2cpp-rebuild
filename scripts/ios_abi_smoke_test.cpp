#include "azurearchive_spine_converter_abi.h"
#include <cstdint>
#include <fstream>
#include <iostream>
#include <iterator>
#include <vector>

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
 {
    if (argc != 2) return 2;
    std::ifstream input(argv[1], std::ios::binary);
    if (!input) return 3;
    std::vector<uint8_t> json((std::istreambuf_iterator<char>(input)), std::istreambuf_iterator<char>());
    if (json.empty()) return 4;

    uint8_t* binary = nullptr;
    int32_t binaryLength = 0;
    int rc = AzureArchiveSpineJsonToBinary(reinterpret_cast<const char*>(json.data()),
                                            static_cast<int32_t>(json.size()),
                                            &binary, &binaryLength);
    if (rc != 0 || !binary || binaryLength <= 0) {
        std::cerr << "JSON-to-binary failed: " << rc << "\n";
        AzureArchiveSpineFreeBuffer(binary);
        return 5;
    }

    uint8_t* roundTrip = nullptr;
    int32_t roundTripLength = 0;
    rc = AzureArchiveSpineConvertBuffer(binary, binaryLength, 6, &roundTrip, &roundTripLength);
    AzureArchiveSpineFreeBuffer(binary);
    if (rc != 0 || !roundTrip || roundTripLength <= 0) {
        std::cerr << "Binary round-trip failed: " << rc << "\n";
        AzureArchiveSpineFreeBuffer(roundTrip);
        return 6;
    }
    AzureArchiveSpineFreeBuffer(roundTrip);
    std::cout << "C ABI JSON -> SKEL -> SKEL round-trip passed (" << binaryLength
              << " -> " << roundTripLength << " bytes)\n";
    return 0;
}
