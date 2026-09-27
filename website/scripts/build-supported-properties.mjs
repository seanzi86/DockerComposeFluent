// Regenerates the property tables in website/docs/supported-properties.md from the library's XML doc
// comments: every model property whose summary says "as specified by <c>key</c>" becomes a row, grouped by
// its declaring type into a section. The rest of the page (the framing text around the markers) is
// hand-written and left untouched.
//
// Usage: node build-supported-properties.mjs <xmlDocPath> <markdownPath>
import { readFile, writeFile } from "node:fs/promises";

const [, , xmlDocPath, markdownPath] = process.argv;
if (!xmlDocPath || !markdownPath) {
  console.error("Usage: node build-supported-properties.mjs <xmlDocPath> <markdownPath>");
  process.exit(1);
}

const beginMarker = "<!-- supported-properties:begin -->";
const endMarker = "<!-- supported-properties:end -->";

// Order matters: roughly the order a reader meets these while writing a compose file - a service's own
// properties first, then the nested concepts a service can reference, then the top-level entries.
const sections = [
  { type: "ServiceDefinition", title: "Services", path: "`services.<name>`" },
  { type: "HealthcheckDefinition", title: "Healthcheck", path: "`services.<name>.healthcheck`" },
  { type: "DependencyDefinition", title: "Service dependency, long syntax", path: "`services.<name>.depends_on.<name>`" },
  { type: "EnvFileEntry", title: "Env file entry, long syntax", path: "`services.<name>.env_file`" },
  { type: "LoggingDefinition", title: "Logging", path: "`services.<name>.logging`" },
  { type: "PortDefinition", title: "Port, long syntax", path: "`services.<name>.ports`" },
  { type: "NetworkAttachment", title: "Service network attachment", path: "`services.<name>.networks.<name>`" },
  { type: "MountDefinition", title: "Mount, long syntax", path: "`services.<name>.volumes`" },
  { type: "BindOptions", title: "Bind mount options", path: "`services.<name>.volumes[].bind`" },
  { type: "VolumeOptions", title: "Named volume mount options", path: "`services.<name>.volumes[].volume`" },
  { type: "TmpfsOptions", title: "Tmpfs mount options", path: "`services.<name>.volumes[].tmpfs`" },
  { type: "ImageOptions", title: "Image mount options", path: "`services.<name>.volumes[].image`" },
  { type: "NetworkDefinition", title: "Networks, top level", path: "`networks.<name>`" },
  { type: "IpamDefinition", title: "IPAM", path: "`networks.<name>.ipam`" },
  { type: "IpamConfigDefinition", title: "IPAM address pool", path: "`networks.<name>.ipam.config`" },
  { type: "VolumeDefinition", title: "Volumes, top level", path: "`volumes.<name>`" },
];

function collapseWhitespace(text) {
  return text.replace(/\s+/g, " ").trim();
}

// Converts the small set of inline XML doc tags this codebase actually uses in a summary (just <c>) to
// their Markdown equivalent. Anything else passes through unchanged.
function inlineMarkdown(text) {
  return text.replace(/<c>(.*?)<\/c>/g, "`$1`");
}

function descriptionFor(summary) {
  const [before] = summary.split(/,?\s*as specified by/);
  const text = collapseWhitespace(inlineMarkdown(before));
  return text.endsWith(".") ? text : `${text}.`;
}

function parseMembers(xml) {
  const memberPattern = /<member name="P:DockerComposeFluent\.Models\.(\w+)\.(\w+)">([\s\S]*?)<\/member>/g;
  const keyPattern = /as specified by\s+<c>([A-Za-z0-9_]+)<\/c>/;
  const hrefPattern = /<see href="([^"]+)"/;
  const versionPattern = /Requires Compose ([\d.]+) or later/;

  const byType = new Map();
  for (const [, type, property, body] of xml.matchAll(memberPattern)) {
    const keyMatch = keyPattern.exec(body);
    if (!keyMatch) {
      continue;
    }

    const summaryMatch = /<summary>([\s\S]*?)<\/summary>/.exec(body);
    const hrefMatch = hrefPattern.exec(body);
    const versionMatch = versionPattern.exec(body);

    if (!byType.has(type)) {
      byType.set(type, []);
    }

    byType.get(type).push({
      property,
      key: keyMatch[1],
      description: summaryMatch ? descriptionFor(summaryMatch[1]) : "",
      href: hrefMatch ? hrefMatch[1] : null,
      sinceCompose: versionMatch ? `${versionMatch[1]}+` : "Core spec",
    });
  }

  return byType;
}

function renderTable(rows) {
  const lines = ["| Property | Description | Since Compose |", "| :--- | :--- | :--- |"];
  for (const row of rows) {
    const property = row.href ? `[\`${row.key}\`](${row.href})` : `\`${row.key}\``;
    lines.push(`| ${property} | ${row.description} | ${row.sinceCompose} |`);
  }

  return lines.join("\n");
}

async function main() {
  const xml = await readFile(xmlDocPath, "utf8");
  const byType = parseMembers(xml);

  const parts = [];
  for (const section of sections) {
    const rows = byType.get(section.type);
    if (!rows || rows.length === 0) {
      continue;
    }

    parts.push(`### ${section.title} (${section.path})`, "", renderTable(rows), "");
  }

  const generated = parts.join("\n").trimEnd();

  const markdown = await readFile(markdownPath, "utf8");
  const beginIndex = markdown.indexOf(beginMarker);
  const endIndex = markdown.indexOf(endMarker);
  if (beginIndex === -1 || endIndex === -1 || endIndex < beginIndex) {
    throw new Error(`${markdownPath} is missing matching ${beginMarker} / ${endMarker} markers.`);
  }

  const updated = `${markdown.slice(0, beginIndex + beginMarker.length)}\n\n${generated}\n\n${markdown.slice(endIndex)}`;
  await writeFile(markdownPath, updated);

  const sectionCount = sections.filter((s) => byType.has(s.type)).length;
  const rowCount = [...byType.values()].reduce((sum, rows) => sum + rows.length, 0);
  console.log(`Wrote ${rowCount} properties across ${sectionCount} sections to ${markdownPath}`);
}

await main();
