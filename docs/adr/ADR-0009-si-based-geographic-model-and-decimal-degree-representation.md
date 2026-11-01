# ADR-0009: SI-Based Geographic Model and Decimal-Degree Representation

- **Status:** Accepted
- **Date:** 2026-10-24

## Context

`GeographicCoordinateSystem` currently represents latitude and longitude in decimal degrees, while the library also supports radians, DDM, and DMS formatting and parsing. The model therefore mixes a convenient human-facing representation with the units needed for consistent numerical computation.

Radians and metres are SI units for the angular and linear quantities represented by the geographic model. Decimal degrees, DDM, and DMS remain useful named representations for users and interchange formats. The library needs a clear canonical model and explicit conversions among these representations.

## Decision

1. `GeographicCoordinateSystem` is the canonical geographic point model. Its numeric latitude and longitude values are stored and exposed in radians; altitude is stored and exposed in metres.
2. Add `GeographicDd` as a representation-specific model whose latitude and longitude values are expressed in decimal degrees. Its altitude is expressed in metres.
3. Do not add a separate `GeographicRadians` type. Radians are the unit of the canonical `GeographicCoordinateSystem` model.
4. `GeographicConverter` supports conversion between every distinct pair of the four geographic representations: `GeographicCoordinateSystem`, `GeographicDd`, `GeographicDdm`, and `GeographicDms`. Each pair is supported in both directions. The full conversion set is:
   - `GeographicCoordinateSystem` ↔ `GeographicDd`
   - `GeographicCoordinateSystem` ↔ `GeographicDdm`
   - `GeographicCoordinateSystem` ↔ `GeographicDms`
   - `GeographicDd` ↔ `GeographicDdm`
   - `GeographicDd` ↔ `GeographicDms`
   - `GeographicDdm` ↔ `GeographicDms`

   Converting a representation to the same representation is an identity operation and does not require a separate converter. Implementations may share conversion logic internally, but callers must not need to chain through another representation. Conversion preserves the represented point, subject to normal floating-point precision.
5. The coordinate ranges are:
   - Latitude: -π/2 to π/2 radians in `GeographicCoordinateSystem`; -90 to 90 degrees in `GeographicDd`.
   - Longitude: -π to π radians in `GeographicCoordinateSystem`; -180 to 180 degrees in `GeographicDd`.
6. String parsing and formatting are representation boundaries. A bare numeric latitude/longitude pair is interpreted as decimal degrees for compatibility with common GPS and geographic interchange conventions. Radian-formatted input must be explicitly identified; parsers must not infer units from numeric ranges. Parsed values are converted into the target model's declared units.
7. Formatting may continue to offer DD, DDM, DMS, and radians where already supported, but the requested representation must be explicit when ambiguity is possible.

The models represent the same geographic point, not different datums, reference systems, or levels of accuracy. The initial geodetic conversion assumption remains WGS 84, as established by ADR-0002.

## Rationale

The canonical model should use consistent computational units, while named representation models make commonly used external forms clear. `GeographicDd` follows the established `GeographicDdm` and `GeographicDms` naming pattern and is useful for GPS input, display, and formats such as GeoJSON.

There is no need for a second type to represent radians because `GeographicCoordinateSystem` already owns the canonical point and can use radians directly. Explicit conversions keep unit changes visible and prevent a numeric value from being silently interpreted under a guessed unit.

## Consequences

- `GeographicCoordinateSystem` numeric property semantics change from degrees to radians; this is a behavioral compatibility change that must be called out in release notes and reflected in API documentation.
- Existing callers that read or assign degree values through `GeographicCoordinateSystem` must convert those values or use `GeographicDd`.
- `GeographicConverter` must support all 12 directed conversions between the four distinct geographic representations listed above.
- Parsing common unmarked coordinate pairs continues to accept decimal degrees, then stores the resulting point in radians when the target is `GeographicCoordinateSystem`.
- Tests must cover unit conversions in both directions, boundary values, altitude preservation in metres, parsing/formatting by declared representation, and round trips within defined floating-point tolerances.

## Alternatives Considered

### Keep decimal degrees as the canonical units of `GeographicCoordinateSystem`

Rejected because it leaves the computational model in a non-SI angular unit and keeps the model's unit semantics tied to its common text representation.

### Add `GeographicRadians` alongside `GeographicCoordinateSystem`

Rejected because it would duplicate the canonical model's responsibility. The existing type itself will use radians.

### Infer whether unmarked values are degrees or radians

Rejected because valid values overlap and magnitude-based interpretation can silently change a coordinate's meaning. Unmarked pairs use the documented decimal-degree input convention.

## Relationship to Earlier Decisions

This ADR follows ADR-0001 for destination-oriented converter ownership. ADR-0001, ADR-0006, and ADR-0007 establish that geographic representations are convertible, but do not enumerate complete pairwise coverage; the matrix above makes that coverage explicit. This ADR supersedes the degree-based canonical-unit descriptions in ADR-0007 and the unmarked-radian parsing examples in ADR-0008. ADR-0007 continues to apply to representation-specific formatting. An unmarked numeric pair is decimal-degree input, and a parsed `GeographicCoordinateSystem` stores that point in radians.
