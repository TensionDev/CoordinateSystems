ADR-0006: Alternative Geographic Coordinate Representations

* Status: Proposed
* Date: 2026-09-03

Context

Geographic represents a geographic point using decimal degrees for latitude and longitude. This provides the canonical numerical representation used by the library.

However, decimal degrees are not the only commonly used representation of geographic coordinates. In particular:

* Degrees and decimal minutes (DDM)
* Degrees, minutes, and seconds (DMS)

are commonly used when presenting coordinates to users.

These representations contain numerical components that are useful independently of their formatted string representation. For example, a user interface may need to display the degree, minute, and second components separately rather than consume the result of ToString().

Storing these derived components directly within Geographic would duplicate information already represented by the coordinate’s decimal-degree value and introduce unnecessary state.

Returning tuples or multiple primitive values from conversion methods would provide the numerical data, but would not provide a named representation with clear domain meaning or a natural place for validation and future behaviour.

Decision

Alternative geographic coordinate representations will be represented by dedicated models:

* Geographic — decimal degrees (DD)
* GeographicDdm — degrees and decimal minutes (DDM)
* GeographicDms — degrees, minutes, and seconds (DMS)

Geographic remains the canonical representation of the geographic point.

GeographicDdm and GeographicDms are derived representations and SHALL NOT be stored as additional state within Geographic.

Conversion between representations will be performed by conversion functionality rather than by maintaining multiple representations within the geographic model.

The alternative representation models SHALL expose their numerical components directly so that consumers can use them for presentation, UI, serialization, or other purposes without parsing a formatted string.

Formatted output, including ToString(), remains a presentation concern and SHALL NOT be the sole mechanism for accessing the component values.

Consequences

Positive

* Geographic remains a simple canonical representation of a geographic point.
* Derived coordinate components are not duplicated or subject to synchronization issues.
* DDM and DMS representations have explicit domain types and meaningful names.
* Consumers can access numerical components directly without parsing formatted strings.
* The models provide a natural location for validation and representation-specific behaviour.
* Additional coordinate representations can be introduced later without expanding Geographic with representation-specific properties.

Negative

* Additional types are introduced for DDM and DMS representations.
* Conversion creates an additional object/value rather than exposing every representation directly from Geographic.
* Consumers need to choose the appropriate representation when converting coordinates.

Alternatives Considered

Returning tuples or multiple primitive values

Conversion could return tuples such as:

(double degrees, double minutes, double seconds)

This was rejected because the returned values have domain-specific meaning and are better represented by a named type. A dedicated model also provides a place for validation and future representation-specific behaviour.

Storing all representations in Geographic

Geographic could contain decimal degrees, decimal minutes, and DMS components simultaneously.

This was rejected because DDM and DMS are derived representations of the same underlying coordinate. Storing them would duplicate state without providing additional source-of-truth information.

Using ToString() as the representation

Consumers could obtain DDM/DMS components by parsing the formatted coordinate string.

This was rejected because formatted text is presentation output, not a suitable data interface for programmatic consumers.

Scope

This decision covers the representation of geographic points in:

* Decimal Degrees (DD)
* Degrees and Decimal Minutes (DDM)
* Degrees, Minutes, and Seconds (DMS)

It does not prescribe additional coordinate systems or projections.

Future coordinate representations may be introduced through separate decisions where their requirements differ materially from those described here.