#!/usr/bin/env python3
"""Find URL/domain and Addressables base-URL clues in APK and Windows ZIP inputs."""
import json
import pathlib
import re
import sys
import zipfile

URL_RE = re.compile(rb'https?://[^\x00-\x20\"\'<>]{4,512}', re.I)
DOMAIN_RE = re.compile(rb'(?<![A-Za-z0-9_])(?:[A-Za-z0-9-]+\.)+[A-Za-z]{2,}(?::[0-9]{2,5})?(?:/[^\x00-\x20\"\'<>]{0,200})?', re.I)
TOKEN_RE = re.compile(rb'.{0,100}(?:AddrLoadUrl|ScenarioResourceManager|Addressables|cdn|assetbundle|resource.?url).{0,160}', re.I)
INTERESTING = ('.dex', '.so', '.dll', '.exe', '.json', '.xml', '.config', '.txt', '.bytes', '.dat', '.asset', '.bin')

def printable(data):
    found = set()
    for rx in (URL_RE, DOMAIN_RE):
        for m in rx.finditer(data):
            value = m.group(0).decode('utf-8', 'replace').strip('.,);]')
            if value:
                found.add(value)
    tokens = set()
    for m in TOKEN_RE.finditer(data):
        value = re.sub(rb'[^\x20-\x7e]', b' ', m.group(0)).decode('ascii', 'ignore').strip()
        if value:
            tokens.add(value)
    return sorted(found), sorted(tokens)

def scan_zip(path, label):
    urls, tokens, scanned = set(), set(), 0
    with zipfile.ZipFile(path) as z:
        for info in z.infolist():
            name = info.filename.lower()
            if not name.endswith(INTERESTING) or info.file_size > 64 * 1024 * 1024:
                continue
            try:
                data = z.read(info)
            except Exception:
                continue
            scanned += 1
            u, t = printable(data)
            urls.update((label + ':' + info.filename, x) for x in u)
            tokens.update((label + ':' + info.filename, x) for x in t)
    return {'archive': label, 'files_scanned': scanned,
            'url_or_domain_candidates': [{'file': f, 'value': v} for f, v in sorted(urls)],
            'endpoint_related_strings': [{'file': f, 'value': v} for f, v in sorted(tokens)]}

def main():
    apk, windows, out = map(pathlib.Path, sys.argv[1:4])
    result = {'note': 'Static string candidates only; values may be unused, obsolete, or environment-specific.',
              'inputs': [scan_zip(apk, 'android-apk'), scan_zip(windows, 'windows-1.0.1')]}
    out.parent.mkdir(parents=True, exist_ok=True)
    out.write_text(json.dumps(result, indent=2, ensure_ascii=False) + '\\n', encoding='utf-8')
    lines = ['# Static endpoint and AddrLoadUrl string scan', '',
             'Static strings are leads only; they do not prove a live endpoint or the runtime-selected value.', '']
    for item in result['inputs']:
        lines += [f"## {item['archive']}", f"- Files scanned: {item['files_scanned']}", '- URL/domain candidates:']
        lines += [f"  - `{x['file']}`: `{x['value']}`" for x in item['url_or_domain_candidates']] or ['  - None found']
        lines += ['- Endpoint-related strings:']
        lines += [f"  - `{x['file']}`: `{x['value']}`" for x in item['endpoint_related_strings']] or ['  - None found']
        lines += ['']
    out.with_name('endpoint-string-scan.md').write_text('\\n'.join(lines), encoding='utf-8')
    print(json.dumps({x['archive']: {'files_scanned': x['files_scanned'],
          'url_candidates': len(x['url_or_domain_candidates']),
          'endpoint_strings': len(x['endpoint_related_strings'])} for x in result['inputs']}))

if __name__ == '__main__':
    main()
