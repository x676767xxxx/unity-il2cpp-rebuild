#!/usr/bin/env python3
"""Apply the pinned Husk 3.4 Unity-version detection correction.

The upstream 3.4 scanner calls an APK Unity 6 whenever libunity.so contains
UnityPlayerForActivityOrService. AzureArchive's Unity 2023.2.22f1 package also
contains that symbol. Prefer the UnityFS engine-version string when available.
The script is intentionally strict and only targets the pinned 3.4 source layout.
"""
from pathlib import Path

ROOT = Path("Husk/src/translation-layer")


def replace_once(path: Path, old: str, new: str) -> None:
    source = path.read_text()
    count = source.count(old)
    if count != 1:
        raise SystemExit(f"{path}: expected exactly one patch anchor, found {count}")
    path.write_text(source.replace(old, new, 1))


internal = ROOT / "husk-tl-internal.h"
replace_once(
    internal,
    """bool tl_zip_data(const tl_zip *z, const tl_zip_entry *e, size_t limit,
                 const uint8_t **out, size_t *out_len, bool *owned,
                 char *err, size_t errlen);
""",
    """bool tl_zip_data(const tl_zip *z, const tl_zip_entry *e, size_t limit,
                 const uint8_t **out, size_t *out_len, bool *owned,
                 char *err, size_t errlen);
bool tl_zip_prefix(const tl_zip *z, const tl_zip_entry *e, uint8_t *out,
                   size_t out_cap, size_t *out_len, char *err, size_t errlen);
""",
)

zip_c = ROOT / "husk-tl-zip.c"
replace_once(
    zip_c,
    """#include <fcntl.h>
#include <stdarg.h>
""",
    """#include <fcntl.h>
#include <limits.h>
#include <stdarg.h>
""",
)
replace_once(
    zip_c,
    """    *owned = true;
    return true;
}

/* ------------------------------------------------------------------ extraction (web apps) */
""",
    """    *owned = true;
    return true;
}

/* Read only the beginning of an entry. Unity's UnityFS header carries the
 * engine version near the start of data.unity3d; inflating the whole 160+ MiB
 * bundle just to read that string would be wasteful on a phone. */
bool tl_zip_prefix(const tl_zip *z, const tl_zip_entry *e, uint8_t *out,
                   size_t out_cap, size_t *out_len, char *err, size_t errlen)
{
    if (out_len) *out_len = 0;
    if (!z || !e || !out || !out_len || out_cap == 0 || out_cap > UINT_MAX) {
        fail(err, errlen, "invalid prefix-read arguments");
        return false;
    }
    if (e->flags & 1) {
        fail(err, errlen, "%s is encrypted", e->name);
        return false;
    }
    if (!in_file(z, e->local_offset, 30) || rd32(z->map + e->local_offset) != SIG_LOCAL) {
        fail(err, errlen, "%s: damaged local header", e->name);
        return false;
    }
    const uint8_t *l = z->map + e->local_offset;
    uint64_t data_off = e->local_offset + 30ull + rd16(l + 26) + rd16(l + 28);
    if (!in_file(z, data_off, e->csize)) {
        fail(err, errlen, "%s runs past the end of the file", e->name);
        return false;
    }
    const uint8_t *data = z->map + data_off;
    if (e->method == 0) {
        if (e->csize != e->usize) {
            fail(err, errlen, "%s: stored entry with mismatched sizes", e->name);
            return false;
        }
        size_t n = e->usize < out_cap ? (size_t)e->usize : out_cap;
        memcpy(out, data, n);
        *out_len = n;
        return true;
    }
    if (e->method != 8) {
        fail(err, errlen, "%s uses compression method %u", e->name, e->method);
        return false;
    }

    z_stream zs;
    memset(&zs, 0, sizeof(zs));
    if (inflateInit2(&zs, -MAX_WBITS) != Z_OK) {
        fail(err, errlen, "inflate failed to start");
        return false;
    }
    zs.next_out = out;
    zs.avail_out = (uInt)out_cap;
    const uint8_t *in = data;
    uint64_t in_left = e->csize;
    int rc = Z_OK;
    while (rc == Z_OK && zs.avail_out > 0) {
        if (zs.avail_in == 0 && in_left > 0) {
            uInt n = in_left > UINT_MAX ? UINT_MAX : (uInt)in_left;
            zs.next_in = (Bytef *)in;
            zs.avail_in = n;
            in += n;
            in_left -= n;
        }
        if (zs.avail_in == 0) break;
        rc = inflate(&zs, Z_NO_FLUSH);
    }
    size_t produced = (size_t)zs.total_out;
    bool enough = produced == out_cap;
    bool ended = rc == Z_STREAM_END;
    inflateEnd(&zs);
    if (!enough && !ended) {
        fail(err, errlen, "%s could not provide the requested prefix", e->name);
        return false;
    }
    *out_len = produced;
    return true;
}

/* ------------------------------------------------------------------ extraction (web apps) */
""",
)

scan_c = ROOT / "husk-tl-scan.c"
replace_once(
    scan_c,
    """/* Whether the app really starts through a NativeActivity: its manifest names android.app.NativeActivity, or a class in its dex
""",
    """/* Unity 6 is version 6000.x. A class-name marker in libunity.so is not
 * sufficient: older Unity 2023 builds can ship UnityPlayerForActivityOrService
 * too. Prefer the engine-version field in the UnityFS header when present. */
static bool unity_engine_version_is_6(const char *const *paths, int count, bool *known)
{
    static const char *const entries[] = {
        "assets/bin/Data/data.unity3d",
        "assets/bin/Data/globalgamemanagers",
    };
    *known = false;
    for (int p = 0; p < count; p++) {
        tl_zip z;
        char err[160] = "";
        if (!tl_zip_open(&z, paths[p], err, sizeof(err))) continue;
        for (size_t i = 0; i < sizeof(entries) / sizeof(entries[0]); i++) {
            const tl_zip_entry *e = tl_zip_find(&z, entries[i]);
            if (!e) continue;
            uint8_t header[512];
            size_t n = 0;
            if (!tl_zip_prefix(&z, e, header, sizeof(header), &n, err, sizeof(err))) continue;
            if (n < 13 || memcmp(header, "UnityFS\\0", 8) != 0) continue;
            size_t at = 12; /* signature + format version */
            while (at < n && header[at] != 0) at++; /* player version */
            if (at >= n) continue;
            at++;
            size_t start = at;
            while (at < n && header[at] != 0) at++; /* engine version */
            if (at == start || at >= n) continue;
            *known = true;
            bool is6 = at - start >= 5 && memcmp(header + start, "6000.", 5) == 0;
            tl_zip_close(&z);
            return is6;
        }
        tl_zip_close(&z);
    }
    return false;
}

/* Whether the app really starts through a NativeActivity: its manifest names android.app.NativeActivity, or a class in its dex
""",
)
replace_once(
    scan_c,
    """    bool na_manifest = false;
    if (eng.native_activity && !starts_native_activity(paths, count, &na_manifest)) eng.native_activity = false;
""",
    """    bool unity_version_known = false;
    bool unity_version_is_6 = unity_engine_version_is_6(paths, count, &unity_version_known);
    if (unity_version_known && (eng.il2cpp || eng.unity)) eng.unity6 = unity_version_is_6;

    bool na_manifest = false;
    if (eng.native_activity && !starts_native_activity(paths, count, &na_manifest)) eng.native_activity = false;
""",
)

print("Applied pinned Husk 3.4 Unity version-classification correction.")
