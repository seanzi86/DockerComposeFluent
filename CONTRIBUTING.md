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

## API design conventions

This library follows a few consistent conventions across the whole fluent API — please follow them in new code rather than introducing a one-off shape:

1. **Every nested compose concept gets a `Definition` + `Builder` pair.** e.g. a new `HealthcheckDefinition` record should come with a `HealthcheckBuilder`, and the parent builder exposes both a raw-model overload and an `Action<HealthcheckBuilder>` overload.
2. **Every property with multiple YAML shapes gets a matching overload set** on the same method name: a raw short-form value, a common-params overload for the typical case, and an `Action<XBuilder>` overload for full control. Add a plural `WithXs(IEnumerable<...>)` for bulk-add where it makes sense.
3. **Models are immutable.** Records with init-only properties — fluent calls produce new values, not in-place mutation.
4. **Serialization stays out of the models.** YAML conversion logic belongs in the `Serialization/` layer (`IYamlTypeConverter` implementations), not as attributes or interfaces on the model types.

## Tests

- Add object-graph unit tests for new builder methods (asserting on the resulting model).
- Add or extend a golden-file test (`.yml` fixture comparison) when changing what YAML gets emitted.

## Issues and roadmap

Work is tracked via GitHub Issues and Milestones, grouped by release. Check the [milestones](../../milestones) page for the current roadmap and what's in scope for the next release.
