// Audits the XML doc comments on the public API: spec links resolve, spellings are UK English, builder
// members document their parameters and return value, model properties name their compose-spec key, and
// "Requires Compose X" remarks are well formed, name a real Compose release, and agree between a definition
// and its builder.
//
// Usage: node scripts/audit-docs.mjs [--offline]
// --offline skips the checks that need the network (spec anchors and Compose release tags).
import { readFile, readdir } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

const offline = process.argv.includes("--offline");
const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const problems = [];
const report = (file, line, message) => problems.push(`${path.relative(root, file)}:${line}: ${message}`);

async function sources(dir) {
  const found = [];
  for (const entry of await readdir(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) found.push(...(await sources(full)));
    else if (entry.name.endsWith(".cs")) found.push(full);
  }
  return found;
}

// Every doc comment block with the public declaration that follows it.
function members(text, file) {
  const lines = text.split("\n");
  const found = [];
  for (let i = 0; i < lines.length; i++) {
    if (!lines[i].trim().startsWith("///")) continue;
    let j = i;
    const doc = [];
    while (j < lines.length && lines[j].trim().startsWith("///")) doc.push(lines[j++].trim().slice(3).trim());
    let k = j;
    while (k < lines.length && lines[k].trim().startsWith("[")) k++;
    found.push({ file, line: i + 1, doc: doc.join(" ").replace(/\s+/g, " "), decl: (lines[k] ?? "").trim() });
    i = j - 1;
  }
  return found;
}

const all = [];
for (const file of await sources(path.join(root, "src", "DockerComposeFluent"))) {
  all.push(...members(await readFile(file, "utf8"), file));
}
const publicMembers = all.filter((m) => m.decl.startsWith("public "));
const tag = (m, name) => new RegExp(`<${name}[ >]`).test(m.doc);
const type = (m) => path.basename(m.file, ".cs");

// 1. UK English in doc comments.
const american = /\b(serializ\w*|normaliz\w*|initializ\w*|customiz\w*|color\w*|behavior\w*|recogniz\w*|organiz\w*|analyz\w*|licens(?:e)\b|honor\w*|favor\w*|center\w*|artifact(?!s)\b)/gi;
const usOnly = /\b(serializ\w*|normaliz\w*|initializ\w*|customiz\w*|color\w*|behavior\w*|recogniz\w*|organiz\w*|analyz\w*|honor\w*|favor\w*|center\w*)\b/gi;
for (const m of all) {
  const text = m.doc.replace(/<c>.*?<\/c>/g, "").replace(/<see [^>]*>/g, "");
  for (const hit of text.match(usOnly) ?? []) report(m.file, m.line, `US spelling "${hit}"`);
}

// 2. Builder members document every parameter and a return value.
for (const m of publicMembers.filter((x) => x.file.includes(`${path.sep}Builders${path.sep}`))) {
  const method = m.decl.match(/^public (?:static )?([\w<>?,.\[\] ]+?) (\w+)\((.*)\)$/);
  if (!method || m.decl.includes(" class ") || m.decl.startsWith("public sealed")) continue;
  const [, returns, name, args] = method;
  if (!/<summary>.{12,}/.test(m.doc)) report(m.file, m.line, `${type(m)}.${name} has no real summary`);
  const names = args.trim() === "" ? [] : args.split(/,(?![^<]*>)/).map((a) => a.trim().replace(/\s*=.*$/, "").split(" ").pop().replace(/^@/, ""));
  for (const p of names) if (!m.doc.includes(`<param name="${p}">`)) report(m.file, m.line, `${type(m)}.${name} is missing <param name="${p}">`);
  if (returns !== "void" && !tag(m, "returns")) report(m.file, m.line, `${type(m)}.${name} is missing <returns>`);
}

// 3. Model properties name their compose-spec key, unless they are helpers rather than compose keys.
const helper = /^(Extensions|Is\w+|Uses\w+|CanBe\w+|Values|Variables|Shell|Arguments|ShortSyntax|Definition|Policy|MaxRetries|Single|Path|All|Devices|Services|Networks|Volumes|Secrets|Configs|Includes)$/;
for (const m of publicMembers.filter((x) => x.file.includes(`${path.sep}Models${path.sep}`))) {
  if (/\b(class|record|enum|struct|interface)\b/.test(m.decl)) continue;
  const property = m.decl.match(/^public [\w<>?,. ]+ (\w+)$/);
  if (!property || helper.test(property[1]) || property[1] === type(m)) continue;
  if (!/as specified by <c>/.test(m.doc)) report(m.file, m.line, `${type(m)}.${property[1]} does not say "as specified by <c>key</c>"`);
}

// 4. Version remarks: exact wording, one per member, and the same versions on a definition and its builder.
const remark = /<remarks>Requires Compose (\d+\.\d+\.\d+) or later\.<\/remarks>/;
const versionsOf = new Map();
for (const m of all.filter((x) => x.doc.includes("Requires Compose"))) {
  const hit = m.doc.match(remark);
  if (!hit) {
    report(m.file, m.line, `version remark is not exactly "<remarks>Requires Compose X.Y.Z or later.</remarks>"`);
    continue;
  }
  if (!versionsOf.has(type(m))) versionsOf.set(type(m), new Set());
  versionsOf.get(type(m)).add(hit[1]);
}
for (const [name, versions] of versionsOf) {
  if (!name.endsWith("Builder") || name === "ServiceBuilder") continue;
  const definition = versionsOf.get(name.replace(/Builder$/, "Definition")) ?? versionsOf.get(name.replace(/Builder$/, ""));
  if (!definition) continue;
  if (name !== "ServiceBuilder") for (const v of versions) if (!definition.has(v)) report(path.join(root, "src"), 0, `${name} has a ${v} remark that its definition lacks`);
  for (const v of definition) if (!versions.has(v)) report(path.join(root, "src"), 0, `${name} has no ${v} remark although its definition does`);
}

// 5. Network checks.
if (!offline) {
  const spec = new Map();
  const specAnchors = async (file) => {
    if (!spec.has(file)) {
      const response = await fetch(`https://raw.githubusercontent.com/compose-spec/compose-spec/main/${file}`);
      if (response.status === 404) { spec.set(file, null); } else if (!response.ok) { throw new Error(`GitHub answered ${response.status} for ${file}; try again.`); } else {
        const slugs = new Set();
        const counts = new Map();
        let fenced = false;
        for (const line of (await response.text()).split("\n")) {
          if (line.startsWith("```")) fenced = !fenced;
          const heading = !fenced && line.match(/^#{1,6}\s+(.*?)\s*#*$/);
          if (!heading) continue;
          let slug = heading[1].toLowerCase().replace(/<[^>]+>/g, "").replace(/[`*]/g, "").replace(/[^\p{L}\p{N}\s_-]/gu, "").trim().replace(/\s/g, "-");
          const n = counts.get(slug) ?? 0;
          counts.set(slug, n + 1);
          slugs.add(n === 0 ? slug : `${slug}-${n}`);
        }
        spec.set(file, slugs);
      }
    }
    return spec.get(file);
  };
  const seen = new Set();
  for (const m of all) {
    for (const [, file, anchor] of m.doc.matchAll(/href="https:\/\/github\.com\/compose-spec\/compose-spec\/blob\/main\/([\w.-]+\.md)(?:#([^"]*))?"/g)) {
      const key = `${file}#${anchor ?? ""}`;
      if (seen.has(key)) continue;
      seen.add(key);
      const slugs = await specAnchors(file);
      if (!slugs) report(m.file, m.line, `spec file ${file} does not exist`);
      else if (anchor && !slugs.has(anchor)) report(m.file, m.line, `spec anchor ${key} does not exist`);
    }
  }

  const headers = process.env.GITHUB_TOKEN ? { Authorization: `Bearer ${process.env.GITHUB_TOKEN}` } : {};
  const tags = new Set();
  for (let page = 1; page <= 10; page++) {
    const response = await fetch(`https://api.github.com/repos/docker/compose/tags?per_page=100&page=${page}`, { headers });
    if (!response.ok) { console.warn(`Skipping the release check: GitHub answered ${response.status}.`); tags.clear(); break; }
    const batch = await response.json();
    for (const t of batch) tags.add(t.name.replace(/^v/, ""));
    if (batch.length < 100) break;
  }
  if (tags.size > 0) {
    for (const m of all) {
      const hit = m.doc.match(remark);
      if (hit && !tags.has(hit[1])) report(m.file, m.line, `Compose ${hit[1]} is not a released version`);
    }
  }
}

const unique = [...new Set(problems)];
for (const p of unique) console.log(p);
console.log(`\n${unique.length} problem(s) in ${all.length} doc comments.`);
process.exit(unique.length === 0 ? 0 : 1);
