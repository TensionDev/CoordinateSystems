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

- `GeographicCoordinateSystem`
- `GeocentricCoordinateSystem`
- `Geohash`

Conversion support is added independently from the coordinate model types.

## Documentation

- [Architecture](docs/Architecture.md)
- [Architecture Decisions](docs/decisions/)

## License

This project is currently licensed under Apache-2.0. Any future licensing change will be documented separately.
