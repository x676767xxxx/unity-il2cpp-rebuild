#!/usr/bin/env python3
"""Extract UnityEngine.Texture IL2CPP vtable entries from metadata v29.

The parser is deliberately narrow: it validates the v29 header and record sizes,
then cross-references the raw vtable method indices with REA's decoded method
and type tables. It does not infer runtime Il2CppClass layout offsets.
"""
import argparse
import json
import pathlib
import struct
import sys

HEADER_MAGIC = 0xFAB11BAF
HEADER_SIZE_V29 = 256
TYPE_DEF_SIZE_V29 = 88
METHOD_DEF_SIZE_V29 = 32

def fail(message):
    raise SystemExit("ERROR: " + message)

def read_cstr(blob, base, index):
    start = base + index
    if index < 0 or start >= len(blob):
        return ""
    end = blob.find(b"\0", start)
    if end < 0:
        return ""
    return blob[start:end].decode("utf-8", errors="replace")

def decode_compressed_int32(data, offset, end):
    encoded = 0
    shift = 0
    raw = bytearray()
    for _ in range(5):
        if offset + len(raw) >= end:
            fail("compressed int32 runs past field-default data section")
        byte = data[offset + len(raw)]
        raw.append(byte)
        encoded |= (byte & 0x7f) << shift
        if (byte & 0x80) == 0:
            # IL2CPP v29 compressed signed integers use zig-zag decoding.
            value = (encoded >> 1) ^ -(encoded & 1)
            return value, bytes(raw).hex()
        shift += 7
    fail("invalid compressed int32 in field-default data")


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--metadata", required=True)
    ap.add_argument("--linked-evidence", required=True)
    ap.add_argument("--out-dir", required=True)
    args = ap.parse_args()

    data = pathlib.Path(args.metadata).read_bytes()
    if len(data) < HEADER_SIZE_V29:
        fail("metadata shorter than v29 header")
    magic, version = struct.unpack_from("<Ii", data, 0)
    if magic != HEADER_MAGIC or version != 29:
        fail(f"expected metadata v29 magic={HEADER_MAGIC:#x}, got magic={magic:#x}, version={version}")

    pairs = struct.unpack_from("<62i", data, 8)
    sections = [(pairs[i*2], pairs[i*2+1]) for i in range(31)]
    # Header pair order is the v29 order documented by Il2CppDumper:
    # 2=strings, 5=methods, 17=vtable methods, 19=type definitions.
    string_off, string_size = sections[2]
    method_off, method_size = sections[5]
    vtable_off, vtable_size = sections[17]
    typedef_off, typedef_size = sections[19]
    for name, off, size in [
        ("strings", string_off, string_size),
        ("methods", method_off, method_size),
        ("vtable_methods", vtable_off, vtable_size),
        ("type_definitions", typedef_off, typedef_size),
    ]:
        if off < HEADER_SIZE_V29 or size < 0 or off + size > len(data):
            fail(f"invalid {name} section range: {off}, {size}, file={len(data)}")
    if typedef_size % TYPE_DEF_SIZE_V29:
        fail(f"type definition section size {typedef_size} is not divisible by {TYPE_DEF_SIZE_V29}")
    if method_size % METHOD_DEF_SIZE_V29:
        fail(f"method section size {method_size} is not divisible by {METHOD_DEF_SIZE_V29}")
    if vtable_size % 4:
        fail("vtable method section size is not divisible by 4")

    evidence = json.loads(pathlib.Path(args.linked_evidence).read_text())
    norm = evidence["records"][0]["normalized_result"]
    methods = norm["methods"]
    types = norm["types"]
    if len(methods) != method_size // METHOD_DEF_SIZE_V29:
        fail(f"REA methods count {len(methods)} != metadata methods count {method_size // METHOD_DEF_SIZE_V29}")
    if len(types) != typedef_size // TYPE_DEF_SIZE_V29:
        fail(f"REA types count {len(types)} != metadata type definitions count {typedef_size // TYPE_DEF_SIZE_V29}")

    method_records = []
    for idx in range(method_size // METHOD_DEF_SIZE_V29):
        # v29 Il2CppMethodDefinition: name, declaring type, return type,
        # parameter start, generic container, token, flags, iflags, slot, argc.
        rec = struct.unpack_from("<5iI4H", data, method_off + idx * METHOD_DEF_SIZE_V29)
        name_idx, declaring_type, _ret, _param_start, _generic, token, flags, iflags, slot, argc = rec
        method_records.append({
            "name": read_cstr(data, string_off, name_idx),
            "declaring_type": declaring_type,
            "slot": slot,
            "token": token,
            "flags": flags,
            "iflags": iflags,
            "parameter_count": argc,
        })

    # v29 field/default-value sections: field definitions are 12 bytes,
    # field-default records are (fieldIndex, typeIndex, dataIndex), each 12 bytes.
    field_off, field_size = sections[11]
    field_default_off, field_default_size = sections[7]
    field_data_off, field_data_size = sections[8]
    if field_size % 12 or field_default_size % 12:
        fail("field/default-value section size is not divisible by 12")
    for name, off, size in [
        ("fields", field_off, field_size),
        ("field_default_values", field_default_off, field_default_size),
        ("field_default_data", field_data_off, field_data_size),
    ]:
        if off < HEADER_SIZE_V29 or size < 0 or off + size > len(data):
            fail(f"invalid {name} section range: {off}, {size}, file={len(data)}")
    field_records = []
    for idx in range(field_size // 12):
        name_idx, type_index, token = struct.unpack_from("<IiI", data, field_off + idx * 12)
        field_records.append({"name": read_cstr(data, string_off, name_idx),
                              "type_index": type_index, "token": token})
    field_defaults = {}
    for idx in range(field_default_size // 12):
        field_index, type_index, data_index = struct.unpack_from("<3i", data, field_default_off + idx * 12)
        if 0 <= data_index < field_data_size:
            # Keep opaque payload offsets here. Other fields may store floats,
            # structs or other encodings; decode only TextureFormat enum
            # constants below after their declaring type has been identified.
            field_defaults[field_index] = {"type_index": type_index, "data_index": data_index}

    vtable_values = struct.unpack_from("<" + "I" * (vtable_size // 4), data, vtable_off)
    type_records = []
    for idx in range(typedef_size // TYPE_DEF_SIZE_V29):
        rec = struct.unpack_from("<8I8i8H2I", data, typedef_off + idx * TYPE_DEF_SIZE_V29)
        (name_idx, namespace_idx, byval, declaring, parent, element, generic, flags,
         field_start, method_start, event_start, property_start, nested_start,
         interfaces_start, vtable_start, interface_offsets_start,
         method_count, property_count, field_count, event_count, nested_count,
         vtable_count, interfaces_count, interface_offsets_count, bitfield, token) = rec
        type_records.append({
            "index": idx,
            "name": read_cstr(data, string_off, name_idx),
            "namespace": read_cstr(data, string_off, namespace_idx),
            "parent": parent,
            "vtable_start": vtable_start,
            "vtable_count": vtable_count,
            "method_start": method_start,
            "method_count": method_count,
            "field_start": field_start,
            "field_count": field_count,
            "token": token,
        })

    by_index = {t["index"]: t for t in type_records}
    wanted = [t for t in type_records if t["namespace"] == "UnityEngine" and t["name"] in {
        "Object", "Texture", "Texture2D", "Texture3D", "RenderTexture"
    }]
    if not any(t["name"] == "Texture" for t in wanted):
        fail("UnityEngine.Texture type not found")
    texture_format_type = next((t for t in type_records
                                 if t["namespace"] == "UnityEngine" and t["name"] == "TextureFormat"), None)
    if texture_format_type is None:
        fail("UnityEngine.TextureFormat enum type not found")
    expected_enum_values = {"RGB24": 3, "DXT1": 10, "DXT5": 12}
    enum_values = []
    for field_index in range(texture_format_type["field_start"],
                             texture_format_type["field_start"] + texture_format_type["field_count"]):
        field = field_records[field_index]
        default = field_defaults.get(field_index)
        if default is not None:
            value, raw_hex = decode_compressed_int32(
                data, field_data_off + default["data_index"], field_data_off + field_data_size)
            enum_values.append({
                "field_index": field_index,
                "name": field["name"],
                "type_index": default["type_index"],
                "data_index": default["data_index"],
                "value": value,
                "raw_hex": raw_hex,
                "field_type_name": norm["field_types"][field_index].get("name"),
            })
    actual_enum_values = {item["name"]: item["value"] for item in enum_values}
    enum_value_checks = {name: {"expected": expected, "actual": actual_enum_values.get(name),
                                "matches": actual_enum_values.get(name) == expected}
                         for name, expected in expected_enum_values.items()}

    rows = []
    for t in wanted:
        start, count = t["vtable_start"], t["vtable_count"]
        if start < 0 or count < 0 or start + count > len(vtable_values):
            fail(f"invalid vtable range for {t['namespace']}.{t['name']}: {start}+{count}")
        for slot_index in range(count):
            raw = vtable_values[start + slot_index]
            # Since v27, encoded method indices use bit 0 to distinguish
            # MethodSpec and store the index shifted left by one.
            decoded = (raw & 0x1FFFFFFE) >> 1
            method_name = methods[decoded]["name"] if decoded < len(methods) else "<out-of-range>"
            method_owner_index = methods[decoded].get("declaring_type") if decoded < len(methods) else None
            owner = types[method_owner_index] if isinstance(method_owner_index, int) and 0 <= method_owner_index < len(types) else {}
            meta = method_records[decoded] if decoded < len(method_records) else {}
            rows.append({
                "type": f'{t["namespace"]}.{t["name"]}',
                "type_index": t["index"],
                "parent_index": t["parent"],
                "vtable_slot": slot_index,
                "raw_encoded": f"0x{raw:08x}",
                "decoded_method_index": decoded,
                "encoded_usage_tag": (raw >> 29) & 0x7,
                "encoded_low_bit_marker": bool(raw & 1),
                "method": method_name,
                "declaring_type": (owner.get("namespace","") + "." if owner.get("namespace") else "") + owner.get("name",""),
                "method_definition_slot": meta.get("slot"),
                "method_token": f'0x{meta.get("token",0):08x}' if meta else None,
            })

    out = pathlib.Path(args.out_dir)
    out.mkdir(parents=True, exist_ok=True)
    enum_tsv = out / "unity-texture-format-enum-values.tsv"
    with enum_tsv.open("w", encoding="utf-8") as f:
        f.write("field_index\tname\tfield_type_name\ttype_index\tdata_index\tvalue\traw_hex\n")
        for item in enum_values:
            f.write("\t".join(str(item.get(k, "")) for k in
                              ["field_index","name","field_type_name","type_index","data_index","value","raw_hex"]) + "\n")
    tsv = out / "unity-texture-vtable-slots.tsv"
    fields = ["type","type_index","parent_index","vtable_slot","raw_encoded","decoded_method_index",
              "encoded_usage_tag","encoded_low_bit_marker","method","declaring_type","method_definition_slot","method_token"]
    with tsv.open("w", encoding="utf-8") as f:
        f.write("\t".join(fields) + "\n")
        for row in rows:
            f.write("\t".join(str(row.get(k, "")) for k in fields) + "\n")
    report = {
        "format": "il2cpp-v29-texture-vtable-map-v1",
        "metadata_version": version,
        "metadata_bytes": len(data),
        "metadata_sections": {
            "strings": {"offset": string_off, "size": string_size},
            "methods": {"offset": method_off, "size": method_size, "count": len(method_records)},
            "vtable_methods": {"offset": vtable_off, "size": vtable_size, "count": len(vtable_values)},
            "type_definitions": {"offset": typedef_off, "size": typedef_size, "count": len(type_records)},
        },
        "types": wanted,
        "texture_format_enum_values": enum_values,
        "texture_format_enum_value_checks": enum_value_checks,
        "field_data_section": {"offset": field_data_off, "size": field_data_size},
        "entries": rows,
        "limits": [
            "This is the serialized metadata vtable, not a dump of runtime Il2CppClass memory.",
            "The runtime byte offsets in the ARM64 calls must be matched to the exact Unity 2023.2.22f1 Il2CppClass layout before equating them to vtable slot numbers.",
            "The encoded usage tag and low-bit marker are preserved as raw encoding metadata; the low bit is not interpreted as a generic MethodSpec flag."
        ]
    }
    (out / "unity-texture-vtable-slots.json").write_text(json.dumps(report, indent=2) + "\n")
    print(json.dumps({
        "metadata_version": version,
        "type_count": len(type_records),
        "method_count": len(method_records),
        "vtable_count": len(vtable_values),
        "selected_types": wanted,
        "texture_format_enum_values": enum_values,
        "texture_format_enum_value_checks": enum_value_checks,
        "selected_slots": [r for r in rows if r["vtable_slot"] in (0,2,4,5,6,7,11,12)],
        "output": str(tsv),
    }, indent=2))

if __name__ == "__main__":
    main()
