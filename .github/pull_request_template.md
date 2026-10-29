## What

<!-- What this PR changes and why. Link the Jira ticket. -->

## Change type

<!-- Pick one. For API alignment work, also name the plan step (e.g. "Step 2.4"). -->

- [ ] **Additive**: new types, properties, enum members or constructor overloads only. Nothing on the wire changes for existing code.
- [ ] **Bridge**: changes what is sent, toward the documented form, while existing code keeps compiling and working.
- [ ] **Patch**: fixes the SDK's own behavior (e.g. a value the caller passed that never reached the wire).
- [ ] **Other** (tooling, tests, docs).

Plan step:

## Public API

<!-- Nothing checks this automatically; reviewers, please verify. -->

- [ ] No public type, property, constructor or method was removed, renamed, or had its type or signature changed.
- [ ] No parameter was added to an existing constructor or method (add an overload instead).
- [ ] Enum members were only appended, never reordered.
- [ ] New properties are nullable (or reference types), so they're omitted unless set.
- [ ] `[Obsolete]` is a warning only (no `error: true`), and new obsolete members are listed in the changelog.

## Golden diff

<!-- Paste the diff of Riskified.SDK.Tests/Golden/Snapshots, or write "None". Regenerate snapshots with
     GOLDEN_UPDATE=1 dotnet test Riskified.SDK.Tests/Riskified.SDK.Tests.csproj
     Every changed line must be explained here and in the changelog. -->

```diff
```

## Schema adjustments

<!-- Changes to how LoadRequestSchema in Golden/GoldenSchemaTests.cs adjusts the API schema (a field
     the server accepts in a form the docs don't describe), with the API team's answer, or "None". -->
