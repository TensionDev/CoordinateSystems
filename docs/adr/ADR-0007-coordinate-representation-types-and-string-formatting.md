# ADR-0007: Coordinate Representation Types and String Formatting

- **Status:** Accepted
- **Date:** 2026-09-04

## Context

The coordinate library provides multiple representations of geographic coordinates:

- GeographicCoordinateSystem — decimal degrees (DD)
- GeographicDdm — degrees and decimal minutes (DDM)
- GeographicDms — degrees, minutes and seconds (DMS)

Version 0.3.0 introduced basic ToString() implementations for these representations.

As the coordinate types evolve, there is a distinction between:

1. A type representing a specific coordinate representation.
2. A general coordinate type capable of converting between representations.

There is also a precision-related issue when formatting DDM and DMS values. Naïve rounding can produce invalid coordinate components, such as:

```
1° 60.0000′ N
```

or:

```
1° 59′ 60.0000″ N
```

These values are mathematically understandable but are not valid canonical DDM/DMS representations.

The library therefore needs clear ownership of formatting responsibilities and defined behaviour when rounding causes a component to reach its upper boundary.

## Decision

1. Representation-specific types own their string representation.

GeographicDdm and GeographicDms provide their own ToString() implementations. GeographicDdm.ToString() produces a DDM representation; GeographicDms.ToString() produces a DMS representation. These types do not expose formatting options that convert their output into another coordinate representation.

GeographicCoordinateSystem remains responsible for conversion between coordinate representations and provides the flexibility to obtain DD, DDM or DMS representations where required.

This establishes the following responsibility:

> A representation-specific type knows how to represent itself; the general coordinate type knows how to transform between representations.

2. Precision is supported by representation-specific formatting.

GeographicDdm and GeographicDms support precision when producing their textual representations. Precision applies to the fractional component of the representation (decimal places of minutes for DDM, decimal places of seconds for DMS). The existing precision limit remains capped at six decimal places.

3. Rounding must be followed by normalization.

Formatting accounts for the possibility that rounding causes a component to reach its upper boundary (minutes < 60, seconds < 60). The formatting process is:

```
Coordinate value
    ↓
Convert to target representation
    ↓
Round to requested precision
    ↓
Normalize component overflow
    ↓
Format string
```

For example:

```
1° 59.999999′ N
```

rounded to four decimal places must become:

```
2° 00.0000′ N
```

rather than:

```
1° 60.0000′ N
```

Cascading overflow is handled for DMS: seconds overflow into minutes, minutes overflow into degrees.

4. Canonical representations are preferred.

The string representation always produces a valid, canonical representation rather than exposing intermediate values created by rounding. Minutes or seconds equal to 60 must never be emitted as part of a DDM or DMS representation.

## Consequences

- Each coordinate representation has a predictable ToString() behaviour.
- Conversion responsibilities remain centralized in GeographicCoordinateSystem.
- DDM and DMS strings remain valid and canonical at all supported precision levels.
- Rounding behaviour is deterministic and testable.
- The existing 0.3.0 string representation feature can be refined without introducing a new abstraction.
- Formatting requires additional normalization logic.
- Boundary cases require explicit testing.
- Rounding can alter the degree component, meaning the formatted result cannot always be produced by independently formatting each component.

## Alternatives Considered

### Allow ToString() to select any coordinate representation

A formatting parameter could allow a GeographicDdm instance to produce DMS or DD output.

Rejected because it gives representation-specific types responsibilities belonging to the general coordinate system.

### Leave rounding overflow unnormalized

This would simplify the implementation but could produce representations such as:

```
1° 60.0000′ N
```

Although numerically equivalent, this is not a valid canonical DDM representation and is undesirable for a public string representation.

### Normalize before rounding

Normalizing the unrounded components does not solve the problem because rounding itself can create the overflow. Normalization must therefore occur after precision rounding.
