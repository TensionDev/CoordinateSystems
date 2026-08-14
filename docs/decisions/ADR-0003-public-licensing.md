# ADR-0003: Public Licensing Direction

- **Status:** Proposed
- **Date:** 2026-08-12

## Context

TensionDev.CoordinateSystems is intended to be useful to external consumers and other open-source projects.

The repository is currently licensed under GPL-3.0. A more permissive license would reduce barriers for consumers using the library in different licensing models.

## Decision

The project intends to migrate to Apache License 2.0.

The migration should be an explicit repository change and should update the repository license, package metadata, documentation, and applicable source headers together.

This ADR records the intended direction; the repository remains GPL-3.0 until the migration is completed.

## Consequences

Apache License 2.0 would permit broader reuse while retaining attribution and explicit patent-related terms.

The licensing state must not be described as Apache-2.0 until the repository has actually been migrated.
