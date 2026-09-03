# ADR-0005: Coordinate String Precision

- **Status:** Proposed
- **Date:** 2026-08-29

## Context

Coordinate string representations require a defined precision so that output is consistent and suitable for display, logging, and interoperability.

Angular and distance values have different scales. A common decimal precision cannot be applied meaningfully to both dimensions. For example, six decimal places for decimal degrees provides approximately sub-metre resolution at the Earth's surface, while six decimal places for metres represents micrometre-level distances.

The coordinate implementations also use `double` values internally. Exposing excessive decimal places would provide little practical value and may result in displaying floating-point representation noise.

## Decision

Coordinate string formatting will support independently configurable precision for angular and distance values.

The default precision will be:

- **Angular values:** 6 decimal places
- **Distance values:** 3 decimal places

The precision will be configurable independently through the `ToString` method.

The maximum supported precision for both angular and distance values will initially be **6 decimal places**.

For example:

```csharp
coordinate.ToString(
    AngularFormat.DecimalDegrees,
    DistanceUnit.Metres,
    angularPrecision: 6,
    distancePrecision: 3);
```

The default `ToString()` representation will therefore use:

- Decimal Degrees with 6 decimal places for angular values.
- Metres with 3 decimal places for distance values.

The precision limit is a formatting constraint and does not define the numerical accuracy or precision of the underlying coordinate values.

## Rationale

- **Sub-metre representation:** Six decimal places for decimal degrees provides approximately sub-metre resolution for geographic coordinates.
- **Independent scales:** Angular and distance values require separate precision settings because identical decimal-place counts have very different physical meanings.
- **Consistency:** Explicit precision defaults ensure predictable string output across coordinate systems.
- **Practicality:** A six-decimal maximum avoids exposing unnecessary floating-point precision or representation noise.
- **Extensibility:** The precision limits can be revised in a future ADR if additional use cases require greater precision.

## Consequences

- `ToString` formatting will require separate angular and distance precision values.
- Unit tests will be required to verify default precision, custom precision, and the six-decimal maximum.
- Existing coordinate values will retain their underlying numerical precision; this ADR only governs their string representation.
- Future requirements for precision greater than six decimal places will require revisiting this decision.