using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeocentricConverterTests
    {
        private const Double PositionToleranceMetres = 0.001;

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
                LatitudeDecimalRadians = 0.0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0.0 * Math.PI / 180.0,
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
                LatitudeDecimalRadians = 90.0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0.0 * Math.PI / 180.0,
                AltitudeMetres = 0.0,
            };

            GeocentricCoordinateSystem actual = GeocentricConverter.From(source);

            AssertWithin(0.0, actual.X, PositionToleranceMetres);
            AssertWithin(0.0, actual.Y, PositionToleranceMetres);
            AssertWithin(6356752.31425, actual.Z, PositionToleranceMetres);
        }

        [Fact]
        public void FromMatchesIogpWgs84GeographicGeocentricExample()
        {
            // IOGP Report 373-07-02, section 4.1.1 (North Sea WGS 84 example).
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = (53.0 + 48.0 / 60.0 + 33.820 / 3600.0) * Math.PI / 180.0,
                LongitudeDecimalRadians = (2.0 + 7.0 / 60.0 + 46.380 / 3600.0) * Math.PI / 180.0,
                AltitudeMetres = 73.0,
            };

            GeocentricCoordinateSystem actual = GeocentricConverter.From(source);

            AssertWithin(3771793.968, actual.X, 0.01);
            AssertWithin(140253.342, actual.Y, 0.01);
            AssertWithin(5124304.349, actual.Z, 0.01);
        }

        private static void AssertWithin(Double expected, Double actual, Double tolerance)
        {
            Assert.InRange(actual, expected - tolerance, expected + tolerance);
        }
    }
}
