# Architecture

## Overview

The library separates coordinate representations from the logic used to convert between them.

Coordinate model types represent coordinate values. Conversion is handled by destination-oriented converter classes.

```text
Source Coordinate
       |
       v
Destination Converter
       |
       v
Destination Coordinate
```

For example:

```csharp
GeodeticConverter.From(geocentric);
GeocentricConverter.From(geographic);
```

The exact public names may evolve with the coordinate terminology already established by the library.

## Coordinate Models

Coordinate model classes represent coordinate values and their basic invariants. They should not contain conversion logic for every other coordinate representation.

## Destination-Oriented Converters

Each supported conversion target may have a dedicated converter.

A destination converter answers:

> How do I obtain this coordinate representation from a supported source representation?

Only conversions that are actually supported need to exist.

## Scope

The library currently focuses on coordinate representations and deterministic conversions between them.

Broader GIS functionality, coordinate-reference-system databases, datum transformations, projections, and geodesic calculations are outside the current scope unless explicitly introduced by a later decision.
