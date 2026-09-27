# DockerComposeFluent docs site

The documentation site at [seanzi86.github.io/DockerComposeFluent](https://seanzi86.github.io/DockerComposeFluent/),
built with [Docusaurus](https://docusaurus.io/).

## Local development

```bash
npm install
npm start
```

Starts a local dev server and opens a browser tab. Most changes reload live.

## Build

```bash
npm run build
```

Generates the static site into `build/`, and fails if any internal link is broken
(`onBrokenLinks`/`onBrokenMarkdownLinks` in `docusaurus.config.ts`).

## Deployment

Handled by [`.github/workflows/docs.yml`](../.github/workflows/docs.yml): every push to `main` that touches
`website/` builds the site and deploys it to GitHub Pages via the official Pages actions. A pull request only
runs the build step, to catch a broken build or a broken link before merging. There is no manual deploy step.
