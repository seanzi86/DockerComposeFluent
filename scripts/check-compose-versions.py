#!/usr/bin/env python3
"""Checks the "Requires Compose X or later" remarks against Compose's own history.

The compose-spec text carries no version badges, so the versions come from two repositories:
  1. the compose-go JSON schema, to find the first compose-go release that lists each key, and
  2. Compose's go.mod at each release, to find the first Compose release that shipped that compose-go.
That is the first Compose release whose `docker compose config` accepts the key, a lower bound for the
feature. A remark may be higher (a release note shows the feature arrived later, for example mac_address in
2.24.0), but never lower, and a key newer than Compose 2.0.1 must have one.

Usage: scripts/check-compose-versions.py        (clones both repositories into a temporary directory;
                                                  needs git and about a minute)
"""
import json, os, re, subprocess, sys, tempfile

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
BASELINE = (2, 0, 1)  # keys already in the schema at Compose 2.0.x need no remark


def git(repo, *args):
    return subprocess.run(["git", "-C", repo, *args], capture_output=True, text=True).stdout


def vt(text):
    return tuple(int(x) for x in re.findall(r"\d+", text.split("-")[0]))


def schema_paths(schema):
    definitions, found = schema.get("definitions", {}), set()

    def walk(node, prefix, seen):
        if not isinstance(node, dict):
            return
        if "$ref" in node:
            name = node["$ref"].split("/")[-1]
            if name not in seen:
                walk(definitions.get(name, {}), prefix, seen | {name})
            return
        for combiner in ("oneOf", "anyOf", "allOf"):
            for sub in node.get(combiner, []):
                walk(sub, prefix, seen)
        for key, value in node.get("properties", {}).items():
            found.add(f"{prefix}.{key}")
            walk(value, f"{prefix}.{key}", seen)
        for value in node.get("patternProperties", {}).values():
            walk(value, prefix + ".*", seen)
        if isinstance(node.get("additionalProperties"), dict):
            walk(node["additionalProperties"], prefix + ".*", seen)
        if isinstance(node.get("items"), dict):
            walk(node["items"], prefix + "[]", seen)

    walk(schema, "", frozenset())
    return found


def first_compose_versions(work):
    compose_go, compose = os.path.join(work, "compose-go"), os.path.join(work, "compose")
    for url, target in (("https://github.com/compose-spec/compose-go.git", compose_go), ("https://github.com/docker/compose.git", compose)):
        subprocess.run(["git", "clone", "-q", "--bare", "--filter=blob:none", url, target], check=True)
    releases = sorted((t for t in git(compose, "tag").split() if re.match(r"^v(2|5)\.\d+\.\d+$", t)), key=vt)
    uses = {}
    for tag in releases:
        match = re.search(r"compose-spec/compose-go(?:/v2)? (v[\d.]+)", git(compose, "show", f"{tag}:go.mod"))
        if match:
            uses[tag] = match.group(1)
    first_go = {}
    for tag in sorted((t for t in git(compose_go, "tag").split() if re.match(r"^v\d+\.\d+\.\d+$", t)), key=vt):
        text = git(compose_go, "show", f"{tag}:schema/compose-spec.json")
        if not text:
            continue
        try:
            for path in schema_paths(json.loads(text)):
                first_go.setdefault(path, tag)
        except ValueError:
            pass
    result = {}
    for path, go_tag in first_go.items():
        for release in releases:
            if release in uses and vt(uses[release]) >= vt(go_tag):
                result[path] = release[1:]
                break
    return result


def sections():
    script = open(os.path.join(ROOT, "website", "scripts", "build-supported-properties.mjs")).read()
    return re.findall(r'\{ type: "(\w+)", title: "[^"]*", path: "`([^`]+)`', script)


def main():
    with tempfile.TemporaryDirectory() as work:
        derived = first_compose_versions(work)
    problems, checked = [], 0
    for type_name, path in sections():
        source = os.path.join(ROOT, "src", "DockerComposeFluent", "Models", type_name + ".cs")
        if not os.path.exists(source):
            continue
        text = open(source).read()
        blocks = re.finditer(r"((?:[ ]*///[^\n]*\n)+)[ ]*public [^\n{]+? (\w+)\s*\n?\s*(?:\{ get; init; \}|\{\n\s*get)", text)
        for block in blocks:
            doc = re.sub(r"\s+", " ", block.group(1))
            key = re.search(r"as specified by <c>([^<]+)</c>", doc)
            if not key:
                continue
            remark = re.search(r"Requires Compose (\d+\.\d+\.\d+) or later", doc)
            have = vt(remark.group(1)) if remark else None
            best = None
            for part in re.split(r"` and `", path):
                base = "." + part.replace("<name>", "*").replace("device_*", "device_read_bps")
                for prefix in (base, base + "[]", base + ".*", base + "[].*"):
                    for suffix in (key.group(1), key.group(1) + "[]", key.group(1) + ".*"):
                        if f"{prefix}.{suffix}" in derived and best is None:
                            best = derived[f"{prefix}.{suffix}"]
            if best is None:
                continue
            checked += 1
            want = vt(best)
            if want > BASELINE and (have is None or have < want):
                problems.append(f"{type_name}.{key.group(1)}: Compose {best} is the first release that accepts it, but the remark says {remark.group(1) if remark else 'nothing'}")
    for problem in problems:
        print(problem)
    print(f"\n{len(problems)} problem(s) in {checked} keys checked against Compose's history.")
    return 1 if problems else 0


if __name__ == "__main__":
    sys.exit(main())
