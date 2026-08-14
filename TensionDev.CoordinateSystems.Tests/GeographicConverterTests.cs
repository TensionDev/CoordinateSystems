using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeographicConverterTests
    {
        private const Double PositionToleranceMetres = 10.0;
        private const Double Wgs84EquatorialRadiusMetres = 6378137.0;
        private const Double PositionToleranceRadians = PositionToleranceMetres / Wgs84EquatorialRadiusMetres;

        [Fact]
        public void FromThrowsForNullGeocentricCoordinateSystem()
        {
            GeocentricCoordinateSystem source = null;

            Assert.Throws<ArgumentNullException>(() => GeographicConverter.From(source));
        }

        [Fact]
        public void FromConvertsEquatorPrimeMeridian()
        {
            GeocentricCoordinateSystem source = new GeocentricCoordinateSystem
            {
                X = 6378137.0,
                Y = 0.0,
                Z = 0.0,
            };

            GeographicCoordinateSystem actual = GeographicConverter.From(source);

            AssertWithin(0.0, actual.LatitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(0.0, actual.LongitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(0.0, actual.AltitudeMetres, PositionToleranceMetres);
        }

        [Fact]
        public void FromConvertsNorthPole()
        {
            GeocentricCoordinateSystem source = new GeocentricCoordinateSystem
            {
                X = 0.0,
                Y = 0.0,
                Z = 6356752.31425,
            };

            GeographicCoordinateSystem actual = GeographicConverter.From(source);

            AssertWithin(Math.PI / 2.0, actual.LatitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(0.0, actual.LongitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(0.0, actual.AltitudeMetres, PositionToleranceMetres);
        }

        [Fact]
        public void FromRoundTripsRepresentativeCoordinate()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 1.309432,
                LongitudeDecimalDegrees = 103.780349,
                AltitudeMetres = 25.0,
            };

            GeocentricCoordinateSystem geocentric = GeocentricConverter.From(source);
            GeographicCoordinateSystem actual = GeographicConverter.From(geocentric);

            AssertWithin(source.LatitudeDecimalRadians, actual.LatitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(source.LongitudeDecimalRadians, actual.LongitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(source.AltitudeMetres, actual.AltitudeMetres, PositionToleranceMetres);
        }

        private static void AssertWithin(Double expected, Double actual, Double tolerance)
        {
            Assert.InRange(actual, expected - tolerance, expected + tolerance);
        }
    }
}
