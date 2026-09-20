# ADR-0002: Geographic and Geocentric Conversion

- **Status:** Accepted
- **Date:** 2026-08-12

## Context

The library already contains `GeographicCoordinateSystem` and `GeocentricCoordinateSystem` representations.

The next required capability is conversion between these two representations.

The existing geographic model documents itself as representing geodetic coordinates, while its public type name is `GeographicCoordinateSystem`. The existing public name should therefore not be changed solely to implement this conversion.

## Decision

Introduce conversion support between the existing geographic and geocentric coordinate representations through destination-oriented converters.

The initial implementation will:

- support both conversion directions;
- keep conversion algorithms outside the coordinate model classes;
- use WGS 84 assumptions initially;
- avoid introducing a configurable ellipsoid/reference-frame abstraction until there is a concrete requirement for it.

## Consequences

The library gains a useful first coordinate transformation without introducing a broader coordinate-reference-system framework.

WGS 84 is an implementation assumption for the initial conversion and should be clearly documented. Supporting additional ellipsoids or reference frames can be introduced later through a separate architectural decision.

The conversion implementation should include representative-coordinate tests, boundary cases, and round-trip behaviour.
