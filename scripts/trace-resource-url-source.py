#!/usr/bin/env python3
"""Trace resource URL configuration references in ILSpy-decompiled C# sources."""
import json, pathlib, re, sys

TERMS = {
    "AddrLoadUrl": re.compile(r"AddrLoadUrl|set_AddrLoadUrl", re.I),
    "ScenarioResourceManager": re.compile(r"ScenarioResourceManager", re.I),
    "URL/config": re.compile(r"base.?url|remote.?load|resource.?url|catalog.?url|https?://|endpoint|addressables", re.I),
}
def main():
    if len(sys.argv) != 3:
        raise SystemExit("usage: trace-resource-url-source.py DECOMPILED_DIR OUTPUT_JSON")
    root, out = pathlib.Path(sys.argv[1]), pathlib.Path(sys.argv[2])
    report = {"source_root": str(root), "term_counts": {}, "matches": []}
    for path in root.rglob("*.cs"):
        try:
            lines = path.read_text(errors="replace").splitlines()
        except OSError:
            continue
        rel = str(path.relative_to(root))
        for term, pattern in TERMS.items():
            for i, line in enumerate(lines):
                if pattern.search(line):
                    report["term_counts"][term] = report["term_counts"].get(term, 0) + 1
                    report["matches"].append({
                        "term": term, "file": rel, "line": i + 1,
                        "context": lines[max(0, i-12):min(len(lines), i+13)]
                    })
    report["matches"].sort(key=lambda x: (x["term"], x["file"], x["line"]))
    out.write_text(json.dumps(report, indent=2))
    md = ["# Resource URL source trace", "",
          "Static decompilation references only; a match is not proof of runtime assignment.", ""]
    for term in TERMS:
        md += [f"## {term} ({report['term_counts'].get(term, 0)} matches)", ""]
        for hit in [x for x in report["matches"] if x["term"] == term]:
            md += [f"### {hit['file']}:{hit['line']}", "```csharp",
                   *hit["context"], "```", ""]
    out.with_suffix(".md").write_text("\n".join(md))
    print(json.dumps(report["term_counts"]), flush=True)
if __name__ == "__main__":
    main()
