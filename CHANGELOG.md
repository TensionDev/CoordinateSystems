# TensionDev.CoordinateSystems

# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Added `GeographicDd` as a decimal-degree coordinate representation.
- Added standard `Parse` and `TryParse` methods for `GeographicCoordinateSystem`, `GeographicDd`, `GeocentricCoordinateSystem`, `GeographicDdm`, and `GeographicDms`, implementing ADR-0008's representation-specific formats and validation.
- Added parsing for optional altitude suffixes, invariant-culture numeric values, and explicitly marked radians; bare geographic numeric pairs are decimal degrees.
- Added conversions in both directions between every distinct pair of `GeographicCoordinateSystem`, `GeographicDd`, `GeographicDdm`, and `GeographicDms`.
- Added metre-based altitude properties and optional altitude parsing to decimal-degree, DDM, and DMS representations.

### Changed
- Made radians the canonical angular unit inside `GeographicCoordinateSystem`; its degree properties remain available as explicit conversions.
- Defined bare numeric latitude/longitude pairs as decimal degrees. Radian strings now require the `rad` marker.
- Geographic parsing now rejects invalid and out-of-range coordinates instead of silently changing their values.


## [v0.4.0] - 2026-09-20
[v0.4.0](https://github.com/TensionDev/CoordinateSystems/releases/tag/v0.4.0)

### Added
- Added DDM and DMS conversion support to `GeographicConverter` (`ToDdm`, `ToDms`, `FromDdm`, `FromDms`).
- Added precision-aware `ToString(int)` to `GeographicDdm` and `GeographicDms` with rounding overflow normalization.
- Added comprehensive tests for `GeographicDdm` and `GeographicDms` model classes and converter methods.
- Added boundary test for DMS minutes rounding to 60.

## [v0.3.0] - 2026-09-03
[v0.3.0](https://github.com/TensionDev/CoordinateSystems/releases/tag/v0.3.0)

### Added
- Added new ToString methods.

## [v0.2.0] - 2026-08-15
[v0.2.0](https://github.com/TensionDev/CoordinateSystems/releases/tag/v0.2.0)

### Added
- Added GeocentricConverter.
- Added GeographicConverter.
- Added GeohashConverter.

### Changed
- Changed license from GNU General Public License Version 3 to Apache License Version 2.0.
- Changed folder structure to match the TensionDev.CoordinateSystems namespace.

## [v0.1.1] - 2024-12-23
[v0.1.1](https://github.com/TensionDev/CoordinateSystems/releases/tag/v0.1.1)

## [v0.1.0] - 2024-01-21
[v0.1.0](https://github.com/TensionDev/CoordinateSystems/releases/tag/v0.1.0)

### Added
- Added FromGeographicCoordinateSystem function to Geohash.

## [v0.1.0-alpha] - 2024-01-13
[v0.1.0-alpha](https://github.com/TensionDev/CoordinateSystems/releases/tag/v0.1.0-alpha)

### Added
- Added Geographic coordinate system.
- Added Geocentric coordinate system.
- Added Geohash Geocode System.
