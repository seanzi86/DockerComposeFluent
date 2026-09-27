// Restructures the flat output of `defaultdocumentation` (one .md file per namespace/type, all in one
// folder, named by full type name) into a folder per namespace with one flat file per type inside it, and
// adds Docusaurus front matter (a title, taken from the page's own "## X Class/Enum/... " heading).
//
// Usage: node build-api-reference.mjs <rawDir> <outDir>
import { readdir, readFile, writeFile, mkdir, rm } from "node:fs/promises";
import path from "node:path";

const [, , rawDir, outDir] = process.argv;
if (!rawDir || !outDir) {
  console.error("Usage: node build-api-reference.mjs <rawDir> <outDir>");
  process.exit(1);
}

const assemblyPrefix = "DockerComposeFluent.";
const headingSuffixes = [" Assembly", " Namespace", " Class", " Struct", " Enum", " Interface", " Delegate"];

// Maps every raw file name (as `defaultdocumentation` wrote it) to its new path, relative to outDir.
function newPathFor(rawName) {
  if (rawName === "index.md") {
    return "index.md";
  }

  // "DockerComposeFluent.Builders.md" (a namespace page) -> "Builders/index.md"
  // "DockerComposeFluent.Builders.ServiceBuilder.md" (a type page) -> "Builders/ServiceBuilder.md"
  const withoutPrefix = rawName.slice(assemblyPrefix.length, -".md".length);
  const segments = withoutPrefix.split(".");
  if (segments.length === 1) {
    return `${segments[0]}/index.md`;
  }

  const [namespaceName, ...typeSegments] = segments;
  return `${namespaceName}/${typeSegments.join(".")}.md`;
}

function unescapeMarkdown(text) {
  return text.replace(/\\([\\`*_{}[\]()#+\-.!])/g, "$1");
}

function titleFor(content, fallback) {
  const heading = content.split("\n").find((line) => line.startsWith("## "));
  if (!heading) {
    return fallback;
  }

  let title = heading.slice("## ".length);
  for (const suffix of headingSuffixes) {
    if (title.endsWith(suffix)) {
      title = title.slice(0, -suffix.length);
      break;
    }
  }

  title = unescapeMarkdown(title);

  // The assembly's own root page ("DockerComposeFluent Assembly") and each namespace page
  // ("DockerComposeFluent.Builders Namespace") carry the assembly name; drop it for a cleaner sidebar label.
  if (title === "DockerComposeFluent") {
    return "API reference";
  }

  return title.startsWith(assemblyPrefix) ? title.slice(assemblyPrefix.length) : title;
}

// `defaultdocumentation` marks each member with a raw `<a name='...'></a>` HTML anchor, which Docusaurus
// does not recognise as a link target (it slugs its own heading text instead). Rather than depend on
// Docusaurus's slug algorithm, every member heading gets an explicit `{#id}` derived from that same raw
// name, and every link to it is rewritten to match - deterministically, with no cross-file map needed.
function sanitizeAnchor(rawName) {
  return rawName
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/^-+|-+$/g, "");
}

// `defaultdocumentation` marks a method, property or enum field with `<a name='...'></a>` right before it,
// whether or not that's followed by a heading. A bare `name` attribute isn't a link target Docusaurus
// recognises, so every one becomes a real `id` instead, using the same sanitized form the link rewriter
// below produces from the same raw name - no cross-file map needed, since it's a pure function of the name.
function addExplicitAnchorIds(content) {
  return content.replace(/<a name='([^']+)'><\/a>/g, (_match, rawName) => `<a id="${sanitizeAnchor(rawName)}"></a>`);
}

function escapeRegExp(text) {
  return text.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
}

// Every member heading on a type's own page repeats the type name ("## ServiceBuilder\.WithCommand\(string\)
// Method"), which is redundant with the page's title and clutters the auto-generated table of contents.
// Strips it, leaving "## WithCommand\(string\) Method". A no-op on namespace/root pages, whose headings
// never start with their own (differently-shaped) title followed by an escaped dot.
function stripSelfReferentialHeadingPrefix(content, title) {
  const prefix = new RegExp(`^## ${escapeRegExp(title)}\\\\\\.`, "gm");
  return content.replace(prefix, "## ");
}

function rewriteLinks(content, currentNewPath, pathByRawName) {
  const currentDir = path.dirname(currentNewPath);

  // A link may carry a "#member-anchor" fragment after the ".md" (a link to one member of a type, from
  // another page's listing), which must be kept (rewritten to the matching explicit heading id) even when
  // the file itself hasn't moved.
  return content.replace(/\]\(([^) '"#]+\.md)?(#[^) '"]*)?(?=[ )])/g, (match, rawName, anchor) => {
    if (!rawName && !anchor) {
      return match;
    }

    let relative = "";
    if (rawName) {
      const targetNewPath = pathByRawName.get(rawName);
      if (!targetNewPath) {
        // A link to something outside the generated set (shouldn't happen) - leave it as-is.
        return match;
      }

      relative = path.relative(currentDir, targetNewPath).split(path.sep).join("/");
      if (!relative.startsWith(".")) {
        relative = `./${relative}`;
      }
    }

    const newAnchor = anchor ? `#${sanitizeAnchor(anchor.slice(1))}` : "";
    return `](${relative}${newAnchor}`;
  });
}

async function main() {
  const rawNames = (await readdir(rawDir)).filter((name) => name.endsWith(".md"));
  const pathByRawName = new Map(rawNames.map((name) => [name, newPathFor(name)]));

  await rm(outDir, { recursive: true, force: true });

  for (const rawName of rawNames) {
    const rawContent = await readFile(path.join(rawDir, rawName), "utf8");
    const newPath = pathByRawName.get(rawName);
    const title = titleFor(rawContent, path.basename(newPath, ".md"));
    const withShortHeadings = stripSelfReferentialHeadingPrefix(rawContent, title);
    const withAnchorIds = addExplicitAnchorIds(withShortHeadings);
    const rewritten = rewriteLinks(withAnchorIds, newPath, pathByRawName);

    const frontMatter = `---\ntitle: "${title.replace(/"/g, '\\"')}"\n---\n\n`;
    const fullPath = path.join(outDir, newPath);
    await mkdir(path.dirname(fullPath), { recursive: true });
    await writeFile(fullPath, frontMatter + rewritten);
  }

  // Docusaurus's own sidebar-category config for this folder. Written here, not hand-maintained, so a full
  // regeneration (which wipes outDir first) can never lose it.
  await writeFile(
    path.join(outDir, "_category_.json"),
    `${JSON.stringify(
      {
        label: "API reference",
        position: 4,
        link: { type: "doc", id: "api-reference/index" },
      },
      null,
      2,
    )}\n`,
  );

  console.log(`Wrote ${rawNames.length} API reference pages to ${outDir}`);
}

await main();
