using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeocentricConverterTests
    {
        private const Double PositionToleranceMetres = 10.0;

        [Fact]
        public void FromThrowsForNullGeographicCoordinateSystem()
        {
            GeographicCoordinateSystem source = null;

            Assert.Throws<ArgumentNullException>(() => GeocentricConverter.From(source));
        }

        [Fact]
        public void FromConvertsEquatorPrimeMeridian()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 0.0,
                LongitudeDecimalDegrees = 0.0,
                AltitudeMetres = 0.0,
            };

            GeocentricCoordinateSystem actual = GeocentricConverter.From(source);

            AssertWithin(6378137.0, actual.X, PositionToleranceMetres);
            AssertWithin(0.0, actual.Y, PositionToleranceMetres);
            AssertWithin(0.0, actual.Z, PositionToleranceMetres);
        }

        [Fact]
        public void FromConvertsNorthPole()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 90.0,
                LongitudeDecimalDegrees = 0.0,
                AltitudeMetres = 0.0,
            };

            GeocentricCoordinateSystem actual = GeocentricConverter.From(source);

            AssertWithin(0.0, actual.X, PositionToleranceMetres);
            AssertWithin(0.0, actual.Y, PositionToleranceMetres);
            AssertWithin(6356752.31425, actual.Z, PositionToleranceMetres);
        }

        private static void AssertWithin(Double expected, Double actual, Double tolerance)
        {
            Assert.InRange(actual, expected - tolerance, expected + tolerance);
        }
    }
}
