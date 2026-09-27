# DockerComposeFluent docs site

The documentation site at [seanzi86.github.io/DockerComposeFluent](https://seanzi86.github.io/DockerComposeFluent/),
built with [Docusaurus](https://docusaurus.io/).

## API reference

`docs/api-reference/` is entirely generated from the library's XML doc comments (via
[DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation)) and is git-ignored — never edit it by
hand. Regenerate it after changing any doc comment in `src/DockerComposeFluent`:

```bash
npm run docs:api
```

This needs the .NET SDK (it builds the library first) and restores a local dotnet tool
(`../.config/dotnet-tools.json`) the first time it runs.

## Supported properties

Unlike the API reference, `docs/supported-properties.md` **is** committed — only the tables between its
`<!-- supported-properties:begin/end -->` markers are generated; the surrounding text is hand-written.
Regenerate it the same way after changing a property's doc comment, and commit the diff:

```bash
npm run docs:supported
```

CI regenerates it too and fails the build if the result differs from what's committed, so a stale table can't
merge silently.

## Local development

```bash
npm install
npm run docs:api
npm run docs:supported
npm start
```

Starts a local dev server and opens a browser tab. Most changes reload live; re-run the generator that
applies after editing a doc comment.

## Build

```bash
npm run docs:api
npm run docs:supported
npm run build
```

Generates the static site into `build/`, and fails if any internal link is broken
(`onBrokenLinks`/`onBrokenMarkdownLinks` in `docusaurus.config.ts`).

## Deployment

Handled by [`.github/workflows/docs.yml`](../.github/workflows/docs.yml): every push to `main` that touches
`website/`, the library's source, or a generator script regenerates the API reference and the supported
properties tables, builds the site, and deploys it to GitHub Pages via the official Pages actions. A pull
request only runs the build step, to catch a broken build, a broken link, or a stale generated table before
merging. There is no manual deploy step.
