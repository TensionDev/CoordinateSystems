# ADR-0001: Destination-Oriented Coordinate Converters

- **Status:** Accepted
- **Date:** 2026-08-12

## Context

TensionDev.CoordinateSystems contains multiple coordinate representations.

Embedding conversion methods in each coordinate model creates increasing coupling as additional representations are introduced. A single universal converter class also risks becoming a large public facade containing methods for every possible destination.

## Decision

Conversions will be organised around the destination coordinate representation.

A supported destination may expose a dedicated converter, for example:

```csharp
GeographicConverter.From(source);
GeocentricConverter.From(source);
```

The converter is responsible for selecting or implementing the supported conversion from the supplied source representation.

Coordinate model classes remain focused on representing their own coordinate values.

## Rationale

This approach:

- avoids putting knowledge of other coordinate systems into model classes;
- avoids a single universal converter becoming a large class;
- groups conversions by intended result;
- makes the intended destination explicit at the call site;
- provides a predictable place for future conversion implementations.

A destination converter may use private or internal conversion routines. A separate public converter type is not required for every individual conversion direction.

## Consequences

Adding a new destination coordinate representation may introduce a new converter without requiring changes to existing coordinate models.

The public converter surface must remain deliberate because this project is intended for external consumers.

Unsupported source/destination combinations should fail explicitly.

## Alternatives Considered

### Conversion methods on coordinate models

Rejected because each model would progressively acquire knowledge of other coordinate representations.

### Single `CoordinateConverter` facade

Rejected as the primary public structure because it would grow with every destination representation.

### One converter class per conversion direction

Rejected as unnecessarily granular for the current scale.
