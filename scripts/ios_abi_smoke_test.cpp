#include "azurearchive_spine_converter_abi.h"
#include <cstdint>
#include <fstream>
#include <iostream>
#include <iterator>
#include <vector>

int main(int argc, char** argv) {
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
