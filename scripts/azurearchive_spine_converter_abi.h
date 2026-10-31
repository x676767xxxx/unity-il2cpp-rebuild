#ifndef AZUREARCHIVE_SPINE_CONVERTER_ABI_H
#define AZUREARCHIVE_SPINE_CONVERTER_ABI_H
#include <stdint.h>
#ifdef __cplusplus
extern "C" {
#endif
int AzureArchiveSpineConvertBuffer(const uint8_t *in_buf, int32_t in_len, int32_t dst_version, uint8_t **out_buf, int32_t *out_len);
int AzureArchiveSpineJsonToBinary(const char *json_utf8, int32_t json_len, uint8_t **out_buf, int32_t *out_len);
void AzureArchiveSpineFreeBuffer(void *buffer);
#ifdef __cplusplus
}
#endif
#endif
