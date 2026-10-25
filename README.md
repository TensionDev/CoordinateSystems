# TensionDev.CoordinateSystems

[![.NET](https://github.com/TensionDev/CoordinateSystems/actions/workflows/dotnet.yml/badge.svg)](https://github.com/TensionDev/CoordinateSystems/actions/workflows/dotnet.yml)
[![Package Release](https://github.com/TensionDev/CoordinateSystems/actions/workflows/package-release.yml/badge.svg)](https://github.com/TensionDev/CoordinateSystems/actions/workflows/package-release.yml)
[![CodeQL](https://github.com/TensionDev/CoordinateSystems/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/TensionDev/CoordinateSystems/actions/workflows/github-code-scanning/codeql)

TensionDev.CoordinateSystems is a .NET library providing tools for conversions between coordinate systems.

## Implementation References

This project references the following documents for implementation:

- [Geohash - Wikipedia](https://en.wikipedia.org/wiki/Geohash)

Implementation references identify sources used when implementing coordinate-system algorithms. They do not define the architecture of this library.

## Coordinate Systems

The library currently provides coordinate representations including:

- `GeographicCoordinateSystem` — the canonical geographic model. Latitude and longitude are stored in radians; altitude is stored in metres. Degree accessors are available for convenience.
- `GeographicDd` — latitude and longitude in decimal degrees, with altitude in metres.
- `GeographicDdm` and `GeographicDms` — degree-minute and degree-minute-second representations, with altitude in metres.
- `GeocentricCoordinateSystem` — Earth-centred Cartesian coordinates in metres (with feet accessors).
- `Geohash` — a compact textual representation of a geographic location.

`GeographicConverter` converts between every distinct pair of the four geographic representations. Conversions preserve altitude in metres and preserve the represented position within floating-point precision.

### Geographic Units and Text Parsing

`GeographicCoordinateSystem.LatitudeDecimalRadians` and `LongitudeDecimalRadians` are the canonical angular properties. Use `LatitudeDecimalDegrees` and `LongitudeDecimalDegrees` when working with decimal degrees, or use `GeographicDd` to make that representation explicit. All geographic models store altitude in metres; feet properties are convenience conversions.

`GeographicCoordinateSystem.Parse` accepts decimal degrees, DDM, DMS, and radians marked explicitly with `rad`. A bare numeric pair such as `"52.52, 13.405"` always means decimal degrees. Radians must include the unit marker, for example `"0.9163 rad, 0.2338 rad"`. `GeographicDd`, `GeographicDdm`, and `GeographicDms` each parse only their own representation through `Parse` and `TryParse`.

Parsing uses invariant culture and accepts an optional altitude suffix, such as `"Altitude: 100 m"` or `"Altitude: 328.084 ft"`. Parsed feet are converted to metres internally.

See [ADR-0008](docs/adr/ADR-0008-string-parsing-for-coordinate-systems.md) for parsing rules and [ADR-0009](docs/adr/ADR-0009-si-based-geographic-model-and-decimal-degree-representation.md) for model units and conversions.

## Documentation

- [Architecture](docs/Architecture.md)
- [Architecture Decisions](docs/adr/)

## License

This project is currently licensed under Apache-2.0. Any future licensing change will be documented separately.
