# ADR-0004: String Formatting for Coordinate Systems

- **Status:** Accepted
- **Date:** 2026-08-25

## Context

When displaying or logging coordinate values, users often require different string representations depending on the context (e.g., for UI display, for debugging, or for interoperability with other systems). Currently, there is no standardized way to request specific formats for `GeographicCoordinateSystem` and `GeocentricCoordinateSystem`.

## Decision

Introduce standardized string formatting for `GeographicCoordinateSystem` and `GeocentricCoordinateSystem` using an enumeration-based approach for angular representation and distance units.

### Angular Values

Angular values will support the following representations:

- `DecimalDegrees` (DD): e.g., `52.5200, 13.4050`
- `DegreesDecimalMinutes` (DMM): e.g., `52° 31.2' N, 13° 24.3' E`
- `DegreesMinutesSeconds` (DMS): e.g., `52° 31' 12" N, 13° 24' 18" E`
- `Radians`: e.g., `0.9166, 0.2339` (latitude and longitude in radians)

The `AngularFormat` enum will be used to select the desired angular representation. The default will be `DecimalDegrees`.

### Distance Values

Distance values will use a `DistanceUnit` enum to select the desired unit.

The default will be metres. Other units, such as feet, may be supported where appropriate.

For geographic coordinates, this applies to altitude.

For geocentric coordinates, this applies to the Cartesian X, Y and Z values.

### GeographicCoordinateSystem

The `GeographicCoordinateSystem` will expose a public `ToString` method accepting the applicable angular and distance formatting options.

The default representation will therefore use Decimal Degrees for angular values and metres for distance values.

### GeocentricCoordinateSystem

The `GeocentricCoordinateSystem` will implement a `ToString` method using the applicable distance formatting option.

The Cartesian representation will be used for X, Y and Z values, with metres as the default unit.

## Rationale

- **Consistency:** Using a single method with enumeration-based formatting options prevents method name explosion (e.g., `ToStringDMS`, `ToStringDD`) and provides a clean, discoverable API.
- **Separation of Concerns:** Angular representation and distance units are independent concerns and can be selected independently.
- **User Experience:** Providing common geographic angular representations (DMS, DMM, DD and radians) directly satisfies the primary use cases for geographic coordinate representations.
- **Maintainability:** A single entry point for formatting makes it easier to update formatting rules (such as precision or decimal places) globally across the library.
- **Extensibility:** Formatting options can be reused by additional coordinate systems without requiring a new format enumeration for each coordinate system.

## Consequences

- Requires the introduction of `AngularFormat` and `DistanceUnit` enumerations.
- Implementation will require new unit tests to verify the accuracy of the various string outputs for both coordinate systems and supported distance units.
- The default output for geographic coordinates will use Decimal Degrees and metres.
- The default output for geocentric coordinates will use Cartesian coordinates and metres.
