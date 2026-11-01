using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeographicConverterTests
    {
        private const Double PositionToleranceMetres = 0.001;
        private const Double PositionToleranceRadians = 1e-8;

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
                LatitudeDecimalRadians = 1.309432 * Math.PI / 180.0,
                LongitudeDecimalRadians = 103.780349 * Math.PI / 180.0,
                AltitudeMetres = 25.0,
            };

            GeocentricCoordinateSystem geocentric = GeocentricConverter.From(source);
            GeographicCoordinateSystem actual = GeographicConverter.From(geocentric);

            AssertWithin(source.LatitudeDecimalRadians, actual.LatitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(source.LongitudeDecimalRadians, actual.LongitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(source.AltitudeMetres, actual.AltitudeMetres, PositionToleranceMetres);
        }

        [Fact]
        public void GeographicCoordinateSystemDegreeAndRadianPropertiesRepresentTheSameAngle()
        {
            GeographicCoordinateSystem coordinate = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = Math.PI / 4.0,
                LongitudeDecimalRadians = -Math.PI / 2.0,
            };

            AssertWithin(45.0, coordinate.LatitudeDecimalDegrees, 1e-12);
            AssertWithin(-90.0, coordinate.LongitudeDecimalDegrees, 1e-12);

            coordinate.LatitudeDecimalDegrees = -30.0;
            coordinate.LongitudeDecimalDegrees = 120.0;

            AssertWithin(-Math.PI / 6.0, coordinate.LatitudeDecimalRadians, 1e-12);
            AssertWithin(2.0 * Math.PI / 3.0, coordinate.LongitudeDecimalRadians, 1e-12);
        }

        [Fact]
        public void FromMatchesIogpWgs84GeocentricGeographicExample()
        {
            // IOGP Report 373-07-02, section 4.1.1 (North Sea WGS 84 example).
            GeocentricCoordinateSystem source = new GeocentricCoordinateSystem
            {
                X = 3771793.968,
                Y = 140253.342,
                Z = 5124304.349,
            };

            GeographicCoordinateSystem actual = GeographicConverter.From(source);

            AssertWithin(0.939151102, actual.LatitudeDecimalRadians, PositionToleranceRadians);
            Double expectedLongitudeRadians = (2.0 + 7.0 / 60.0 + 46.380 / 3600.0) * Math.PI / 180.0;
            AssertWithin(expectedLongitudeRadians, actual.LongitudeDecimalRadians, PositionToleranceRadians);
            AssertWithin(73.0, actual.AltitudeMetres, 0.005);
        }

        [Fact]
        public void DmsAndDdmDirectConversionsPreserveCoordinateAndAltitude()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 12,
                LatitudeDirection = 'S',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18,
                LongitudeDirection = 'W',
                AltitudeMetres = 125.5,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(dms);

            Assert.Equal(52, ddm.LatitudeDegrees);
            Assert.Equal(31.2, ddm.LatitudeMinutes, 12);
            Assert.Equal('S', ddm.LatitudeDirection);
            Assert.Equal(13, ddm.LongitudeDegrees);
            Assert.Equal(24.3, ddm.LongitudeMinutes, 12);
            Assert.Equal('W', ddm.LongitudeDirection);
            Assert.Equal(125.5, ddm.AltitudeMetres);

            GeographicDms roundTripped = GeographicConverter.ToDms(ddm);

            Assert.Equal(52, roundTripped.LatitudeDegrees);
            Assert.Equal(31, roundTripped.LatitudeMinutes);
            Assert.Equal(12, roundTripped.LatitudeSeconds, 10);
            Assert.Equal('S', roundTripped.LatitudeDirection);
            Assert.Equal(13, roundTripped.LongitudeDegrees);
            Assert.Equal(24, roundTripped.LongitudeMinutes);
            Assert.Equal(18, roundTripped.LongitudeSeconds, 10);
            Assert.Equal('W', roundTripped.LongitudeDirection);
            Assert.Equal(125.5, roundTripped.AltitudeMetres);
        }

        [Fact]
        public void GeographicRepresentationsConvertInEveryDirectionAndPreserveAltitude()
        {
            GeographicDd dd = new GeographicDd
            {
                LatitudeDecimalDegrees = -33.86882,
                LongitudeDecimalDegrees = 151.20929,
                AltitudeMetres = 125.5,
            };
            GeographicCoordinateSystem canonical = GeographicConverter.From(dd);
            GeographicDdm ddm = GeographicConverter.ToDdm(dd);
            GeographicDms dms = GeographicConverter.ToDms(dd);

            AssertGeographicDd(dd, GeographicConverter.ToDd(canonical));
            AssertGeographicDd(dd, GeographicConverter.ToDd(ddm));
            AssertGeographicDd(dd, GeographicConverter.ToDd(dms));
            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.From(dd)));

            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.FromDdm(ddm)));
            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.FromDms(dms)));
            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.ToDdm(canonical)));
            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.ToDdm(dms)));
            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.ToDms(canonical)));
            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.ToDms(ddm)));
            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.ToDdm(dd)));
            AssertGeographicDd(dd, GeographicConverter.ToDd(GeographicConverter.ToDms(dd)));
        }

        private static void AssertGeographicDd(GeographicDd expected, GeographicDd actual)
        {
            AssertWithin(expected.LatitudeDecimalDegrees, actual.LatitudeDecimalDegrees, 1e-6);
            AssertWithin(expected.LongitudeDecimalDegrees, actual.LongitudeDecimalDegrees, 1e-6);
            AssertWithin(expected.AltitudeMetres, actual.AltitudeMetres, PositionToleranceMetres);
        }

        private static void AssertWithin(Double expected, Double actual, Double tolerance)
        {
            Assert.InRange(actual, expected - tolerance, expected + tolerance);
        }
    }
}
