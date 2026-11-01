# ADR-0008: String Parsing for Coordinate Systems

- **Status:** Accepted
- **Date:** 2026-09-23

## Context

The library provides extensive string formatting for coordinate representations:

- `GeographicCoordinateSystem.ToString(AngularFormat, DistanceUnit, int, int)` covers decimal degrees, DDM, DMS, and radians with configurable precision.
- `GeographicDd.ToString()` produces a decimal-degree representation.
- `GeographicDdm.ToString(int)` and `GeographicDms.ToString(int)` produce formatted DDM and DMS strings.
- `GeocentricCoordinateSystem.ToString(DistanceUnit, int)` formats X, Y, Z values.

However, there is no corresponding way to parse strings back into coordinate objects. Consumers who receive coordinate data as text (e.g., from user input, configuration files, APIs, or log entries) have no library-supported path to convert it back into `GeographicCoordinateSystem`, `GeographicDd`, `GeocentricCoordinateSystem`, `GeographicDdm`, or `GeographicDms` instances.

The existing `GeographicConverter` class provides `FromDdm(GeographicDdm)` and `FromDms(GeographicDms)` for converting between *typed* model objects, and `From(GeocentricCoordinateSystem)` for geocentric-to-geographic conversion. These bridge typed representations but do not address the string-to-object gap.

## Decision

`GeographicCoordinateSystem`, `GeographicDd`, and `GeocentricCoordinateSystem` will support string parsing through the standard .NET `Parse` and `TryParse` pattern. `GeographicDdm` and `GeographicDms` will support the same pattern for their own representations.

### GeographicCoordinateSystem

`GeographicCoordinateSystem` will provide:

- `Parse(string)` — parses a coordinate string and returns a `GeographicCoordinateSystem`. Throws on invalid input.
- `TryParse(string, out GeographicCoordinateSystem)` — attempts to parse a coordinate string; returns `true` on success, `false` on failure.

The `Parse` method will accept strings in all formats supported by `ToString`, plus space-delimited variants:

- Decimal degrees: `"52.520000, 13.405000"` or with altitude `"52.520000, 13.405000 Altitude: 100.000 m"`
- DDM: `"52° 31.20' N, 13° 24.30' E"` or with altitude `"52° 31.20' N, 13° 24.30' E Altitude: 100.000 m"` (altitude is optional; if absent, defaults to 0)
- DMS: `"52° 31' 12.00\" N, 13° 24' 18.00\" E"` or with altitude `"52° 31' 12.00\" N, 13° 24' 18.00\" E Altitude: 100.000 m"` (altitude is optional; if absent, defaults to 0)
- Radians: values explicitly marked as radians (for example, `"0.916578 rad, 0.233904 rad"`), optionally with altitude. A bare numeric pair is always interpreted as decimal degrees, not radians.

The `Parse` method also accepts space-delimited variants where the degree symbol, minute/second symbols, and direction characters are omitted. These formats are identified by the number of numeric components:

- **Space-delimited DDM** (two numeric components): `"52 31.20 N, 13 24.30 E"` — latitude and longitude are separated by two numbers followed by a direction indicator. `10 0.000 N` is interpreted as DDM.
- **Space-delimited DMS** (three numeric components): `"52 31 12 N, 13 24 18 E"` — latitude and longitude are separated by three numbers followed by a direction indicator. `10 0 13.123 N` is interpreted as DMS.

The direction indicator (N/S/E/W) is still required to disambiguate the final number. The latitude and longitude components must use the same format (both symbol-delimited or both space-delimited).

For space-delimited formats, the integer constraint is enforced:

- **Space-delimited DDM** (two numeric components): the first number (degrees) must be an integer. A non-integer value such as `"10.2 31.20 N"` will throw `FormatException`.
- **Space-delimited DMS** (three numeric components): the first two numbers (degrees and minutes) must be integers. A non-integer value such as `"10.2 10.3 10.4 N"` will throw `FormatException`.

Altitude is appended with a leading space, the literal `"Altitude: "`, the value formatted with `distancePrecision` decimal places, a space, and the unit symbol (`"m"` or `"ft"`). If absent, altitude defaults to 0.

Direction indicators (N, S, E, W) are required for DDM and DMS formats. Direction characters are case-insensitive (e.g., `"n"` and `"N"` are equivalent). Decimal degrees and radians do not use direction indicators.

Parsing will use `CultureInfo.InvariantCulture` to ensure consistent behaviour regardless of the system's culture settings. Decimal separators are always `.` and thousands separators are never expected.

`GeographicCoordinateSystem.Parse` converts the parsed angular values to the canonical radians used by `GeographicCoordinateSystem`, as defined by ADR-0009. Altitude is stored in metres. An unmarked decimal-degree string remains accepted and is converted to radians when constructing the result.

### GeographicDd

`GeographicDd` will provide the standard .NET parsing methods:

- `Parse(string)` — parses a decimal-degree coordinate string and returns a `GeographicDd`. Throws on invalid input.
- `TryParse(string, out GeographicDd)` — attempts to parse the same format; returns `true` on success and `false` on failure.

`GeographicDd.Parse` accepts only its decimal-degree format, such as `"52.520000, 13.405000"`, with the optional altitude suffix used by `GeographicCoordinateSystem`. Latitude and longitude values are in degrees; altitude is in metres. It does not parse DDM, DMS, radian, GeoJSON, ISO 6709, or geocentric input. Conversion to those representations is performed by the appropriate converter or interchange API.

### GeocentricCoordinateSystem

`GeocentricCoordinateSystem` will provide:

- `Parse(string)` — parses a Cartesian coordinate string and returns a `GeocentricCoordinateSystem`. Throws on invalid input.
- `TryParse(string, out GeocentricCoordinateSystem)` — attempts to parse a coordinate string; returns `true` on success and `false` on failure.

The `Parse` method will accept strings in the formats produced by `ToString`:

- Metres: `"X: 1000.000 m, Y: 2000.000 m, Z: 3000.000 m"`
- Feet: `"X: 3280.840 ft, Y: 6561.680 ft, Z: 9842.520 ft"`

The unit suffix (`m` or `ft`) is parsed to determine the output unit. If absent, metres is assumed.

### GeographicDdm and GeographicDms

`GeographicDdm` and `GeographicDms` will also provide `Parse` and `TryParse` methods:

- `GeographicDdm.Parse(string)` — parses a DDM string into a `GeographicDdm` instance.
- `GeographicDdm.TryParse(string, out GeographicDdm)` — attempts to parse a DDM string.
- `GeographicDms.Parse(string)` — parses a DMS string into a `GeographicDms` instance.
- `GeographicDms.TryParse(string, out GeographicDms)` — attempts to parse a DMS string.

Each type parses **only its own format** — the format produced by its own `ToString` method. `GeographicDdm.Parse` accepts only DDM strings (e.g., `"52° 31.20' N, 13° 24.30' E"`). `GeographicDms.Parse` accepts only DMS strings (e.g., `"52° 31' 12.00\" N, 13° 24' 18.00\" E"`). Neither type accepts decimal degree, radian, or geocentric input.

Each type also parses the optional altitude suffix in the same format as `GeographicCoordinateSystem` (e.g., `"52° 31.20' N, 13° 24.30' E Altitude: 100.000 m"`). If absent, altitude defaults to 0.

### Parsing and Format Boundaries

Any string that does not conform to the expected format for the target type will throw `FormatException`. Parsing never performs format detection or cross-format conversion. For example, a geocentric coordinate string (`"X: 1000.000 m, Y: 2000.000 m, Z: 3000.000 m"`) passed to `GeographicCoordinateSystem.Parse` will throw `FormatException`. A DDM string passed to `GeographicDms.Parse` will throw `FormatException`.

`GeographicCoordinateSystem` is the general parser: it can parse DD, DDM, DMS, explicit radians, and the supported space-delimited variants, then convert the result into its canonical radians. Bare numeric pairs mean decimal degrees. `GeographicDd`, `GeographicDdm`, and `GeographicDms` each parse only their own representation; conversion to a different representation is a converter responsibility.

### Validation

Parsing validates the input against the valid geographic range and converts angular units for the target model:

- Latitude: -90 to 90 degrees (inclusive).
- Longitude: -180 to 180 degrees (inclusive).
- Minutes: 0 to 60.
- Seconds: 0 to 60.

Out-of-range values will cause `Parse` to throw and `TryParse` to return `false`.

`GeographicDd`, `GeographicDdm`, and `GeographicDms` values are validated in degrees. `GeographicCoordinateSystem` values are validated in radians. Parsing rejects out-of-range values rather than silently changing the parsed location.

### Precision and Rounding

Parsing preserves the full precision of the input `double` values. No precision clamping is applied during parsing — the six-decimal-place precision limit documented in ADR-0005 applies only to string formatting, not to the underlying numerical values.

## Rationale

- **Parse/TryParse convention:** The `Parse`/`TryParse` pattern is the standard .NET idiom for string-to-value conversion (used by `int`, `DateTime`, `Guid`, etc.). It gives consumers both a concise API for trusted input (`Parse`) and a safe path for untrusted input (`TryParse`) without requiring them to catch exceptions for validation.
- **Symmetry with formatting:** ADR-0004 and ADR-0005 established that the library formats coordinates in multiple angular representations and distance units. Parsing in the same set of formats provides round-trip fidelity — a parsed coordinate can be formatted and the result is semantically equivalent.
- **Invariant culture:** Coordinates are often exchanged between systems with different locale settings. Using `InvariantCulture` avoids culture-dependent parsing failures (e.g., `13,4050` vs `13.4050`).
- **GeographicDdm/GeographicDms own their parse:** ADR-0007 established that representation-specific types own their string representation. Parsing is the inverse of formatting, so it follows the same ownership principle. `GeographicDdm.Parse` produces a `GeographicDdm`; `GeographicDms.Parse` produces a `GeographicDms`.
- **Cross-format conversion is a converter responsibility:** `GeographicDdm` and `GeographicDms` do not accept input in other formats. Parsing a DD string into a `GeographicDdm` (or a DMS string into a `GeographicDdm`) is a format conversion, which is the role of `GeographicConverter`. This keeps the parsing surface narrow and predictable — each type parses exactly one format, the one it produces.
- **GeographicCoordinateSystem as the general parser:** `GeographicCoordinateSystem.Parse` accepts all supported angular formats and normalises the result into its canonical radians, as defined by ADR-0009. Bare pairs remain decimal-degree input for compatibility with common GPS and interchange data.
- **GeographicDd owns decimal-degree parsing:** `GeographicDd.Parse` and `TryParse` accept only decimal-degree strings, following the same representation-specific parsing rule as `GeographicDdm` and `GeographicDms`.

## Consequences

- Consumers can round-trip coordinates between string and object without external libraries.
- The `Parse`/`TryParse` pattern is familiar to .NET developers and integrates with existing infrastructure (e.g., `Convert.ChangeType`, source generators).
- Parsing is culture-independent by default, which is predictable but may differ from the default behaviour of `double.Parse` in localised applications.
- DDM and DMS parsing must handle direction characters (N/S/E/W), degree symbols, and optional altitude suffixes. The parser must be tolerant of reasonable whitespace variation while remaining strict about required components.
- `FormatException` is the error type for non-conforming input, consistent with .NET conventions (e.g., `int.Parse`, `DateTime.Parse`).
- Unit tests will be required for:
  - All supported input formats (DD, DDM, DMS, explicitly marked radians; metres and feet for geocentric).
  - Direction indicator variations (upper/lower case).
  - Whitespace tolerance (e.g., `"52°31.2'N,13°24.3'E"` vs `"52° 31.2' N, 13° 24.3' E"`).
  - Missing altitude (default to 0).
  - Invalid input (non-numeric values, out-of-range coordinates, missing direction, malformed symbols).
- Mixed-format rejection (e.g., symbol-delimited latitude with space-delimited longitude, or DDM latitude with DMS longitude).
- Round-trip fidelity: `TryParse(str) → obj → ToString() → str'` where `str'` is semantically equivalent to `str`.
- `GeographicDd`, `GeographicDdm`, and `GeographicDms` parsing needs boundary tests at 90° latitude and 180° longitude, including rejection of values outside those ranges.
- `GeographicDd.Parse` and `GeographicDd.TryParse` will need tests for decimal-degree parsing, optional altitude, ranges, and rejection of other representations.

## Alternatives Considered

### Overriding `object.Parse` as a static method only

A static `Parse` method on a converter (e.g., `GeographicConverter.Parse(string)`) was considered. This would centralise parsing but would require the converter to detect the input format internally, introducing ambiguity about which type the result should be. The `Parse`/`TryParse` pattern on the target type itself is more explicit and idiomatic in .NET.

### Providing only `GeographicCoordinateSystem.Parse`

Limiting parsing to `GeographicCoordinateSystem` would avoid duplicating parse logic across types. However, ADR-0007 established that representation-specific types own their own string representation, and consumers of `GeographicDdm` or `GeographicDms` should not need to parse into `GeographicCoordinateSystem` and then convert. Each type should be able to parse its own format directly.

### Using `string.Split`-based parsing without symbol tolerance

A simpler parser that only accepts a single fixed format would reduce implementation complexity but would be fragile with real-world input. Tolerating whitespace and case variations in direction characters is low-cost and significantly improves usability.

### Using a separate parser class

Introducing a `GeographicCoordinateParser` or similar would isolate parsing logic but would add a type that does not follow the `Parse`/`TryParse` convention, making it less discoverable and inconsistent with .NET idioms.

## Relationship to ADR-0009

ADR-0009 adds `GeographicDd` and defines `GeographicCoordinateSystem` as the canonical radians-based model. This ADR therefore requires `GeographicDd.Parse` and `GeographicDd.TryParse` to parse decimal degrees, while `GeographicCoordinateSystem.Parse` converts the accepted representation into radians. Bare numeric pairs mean decimal degrees. Radian strings must identify radians explicitly; unmarked-radian examples in this ADR and ADR-0004 are superseded.
