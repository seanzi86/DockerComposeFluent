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

## Local development

```bash
npm install
npm run docs:api
npm start
```

Starts a local dev server and opens a browser tab. Most changes reload live; re-run `docs:api` after editing
a doc comment.

## Build

```bash
npm run docs:api
npm run build
```

Generates the static site into `build/`, and fails if any internal link is broken
(`onBrokenLinks`/`onBrokenMarkdownLinks` in `docusaurus.config.ts`).

## Deployment

Handled by [`.github/workflows/docs.yml`](../.github/workflows/docs.yml): every push to `main` that touches
`website/`, the library's source, or the generator script regenerates the API reference, builds the site, and
deploys it to GitHub Pages via the official Pages actions. A pull request only runs the build step, to catch a
broken build or a broken link before merging. There is no manual deploy step.
