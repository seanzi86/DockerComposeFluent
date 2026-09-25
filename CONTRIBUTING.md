# Contributing

Thanks for considering a contribution to DockerComposeFluent.

## Getting started

Requires the .NET SDK (`netstandard2.0;net10.0` are the library's target frameworks; the SDK itself just needs to support `net10.0`).

```bash
dotnet restore
dotnet build
dotnet test
```

## Workflow

- Branch off `main` using `Features/<ShortDescription>` naming (e.g. `Features/CoreModels`), open a pull request against `main`.
- `main` is protected: PRs require the CI check to pass before merging, and merges are squash-only.
- Keep PRs scoped to a single issue/concern where possible.
- Reference the issue you're addressing in the PR description (e.g. `Closes #12`).

## Coding standards

Enforced via `.editorconfig` and `EnforceCodeStyleInBuild` (violations fail the build):

- **Block-scoped namespaces** (`namespace X { ... }`), not file-scoped (`namespace X;`).
- **No `var`** — always use the explicit type.
- **Private fields use an underscore prefix** (`_camelCase`, e.g. `_services`).
- **UK English** in identifiers, comments and docs (e.g. `Serialise`, `Normalise`), except where a third-party or compose-spec name dictates otherwise.
- **XML `<summary>` doc comments on every public type and member** — this is a public package, so these become the IntelliSense docs consumers see. Missing docs on public members fail the build (`CS1591`); internal types and members should be documented too, but that is checked in review.
- **Note the minimum Compose version** when the spec marks a property with a version badge: add `<remarks>Requires Compose 2.35.0 or later.</remarks>` to the model property, its builder methods, and any type that only exists for that property. The target is the current Compose release, so only properties that need something newer than older releases carry a note.
- **Link each summary back to the relevant [compose-spec](https://github.com/compose-spec/compose-spec) section** via `<see href="..."/>`, pointing at `main` (the spec has no tagged releases). An established property's spec text is unlikely to change shape; if the spec does add something new, that's new scope for its own release anyway.

## API design conventions

This library follows a few consistent conventions across the whole fluent API — please follow them in new code rather than introducing a one-off shape:

1. **Every nested compose concept gets a `Definition` + `Builder` pair.** e.g. a new `HealthcheckDefinition` record should come with a `HealthcheckBuilder`, and the parent builder exposes both a raw-model overload and an `Action<HealthcheckBuilder>` overload.
2. **Every property with multiple YAML shapes gets a matching overload set** on the same method name: a raw short-form value, a common-params overload for the typical case, and an `Action<XBuilder>` overload for full control. Add a plural `WithXs(IEnumerable<...>)` for bulk-add where it makes sense.
3. **Models are immutable.** Records with init-only properties — fluent calls produce new values, not in-place mutation.
4. **Serialisation stays out of the models.** YAML conversion logic belongs in the `Serialisation/` layer (`IYamlTypeConverter` implementations), not as attributes or interfaces on the model types.

## Tests

- Add object-graph unit tests for new builder methods (asserting on the resulting model).
- Add or extend a golden-file scenario when changing what YAML gets emitted: add an entry to `GoldenScenarios` in `test/DockerComposeFluent.Tests.Unit/Golden`, then run `dotnet test`. Golden files are managed by [Verify](https://github.com/VerifyTests/Verify): a new or changed scenario fails and writes a `<name>.received.yml` next to its `<name>.verified.yml` fixture in `Golden/Fixtures`. Review the difference, then accept it by replacing the `.verified.yml` file with the `.received.yml` one (or run `dotnet tool install -g verify.tool` once, then `dotnet verify accept -y`), and review the diff before committing. `.received.yml` files are git-ignored.
- The README quick start is the `readme` golden scenario: its code sits between the `README:begin` and `README:end` markers in `GoldenScenarios.cs`, and its YAML is `Golden/Fixtures/readme.verified.yml`. A test fails if `README.md` stops matching either, so change all three together. Update the short "What is supported" list in the README when a property is added.
- CI runs `docker compose config` over every fixture with a pinned Docker Compose version (see `.github/workflows/ci.yml`), so the YAML must be accepted by the real tool, not just match a file. A fixture that uses a property from a newer Compose than the pinned one will fail there.

## Dependencies

- Dependabot opens a weekly pull request for NuGet packages and one for GitHub Actions (minor and patch updates grouped, major updates separate). They must pass CI like any other change.
- The build fails on any known vulnerability in a NuGet package, direct or transitive (`NuGetAudit` in `Directory.Build.props`, warnings NU1901 to NU1904 as errors). CI also runs weekly, so a newly published advisory fails the build even when no PR is open. Fix it by updating the package; if no fixed version exists, suppress that one advisory with `NuGetAuditSuppress` and a comment explaining why and when to revisit it.
- The Docker Compose version used to validate fixtures in `.github/workflows/ci.yml` is pinned and is not updated by Dependabot, so bump it by hand.

## Releasing

Releases are cut by pushing a tag, for example `git tag v0.1.0 && git push origin v0.1.0`. The release workflow then runs the tests, packs the library with the version taken from the tag (the csproj `Version` is only a local `0.0.0-dev` default), publishes the package and its symbols to nuget.org using Trusted Publishing, and creates a GitHub Release with generated notes and the packages attached. A tag containing a hyphen (`v0.2.0-beta.1`) is published as a prerelease. A release can also be published from the GitHub UI (Releases, then Draft a new release, with a new tag): that creates the tag and starts the same workflow, which then attaches the packages to the release you made instead of creating another. Pushing a tag needs no secrets, but the `release` environment and the nuget.org Trusted Publishing policy must exist.

## Issues and roadmap

Work is tracked via GitHub Issues and Milestones, grouped by release. Check the [milestones](../../milestones) page for the current roadmap and what's in scope for the next release.
