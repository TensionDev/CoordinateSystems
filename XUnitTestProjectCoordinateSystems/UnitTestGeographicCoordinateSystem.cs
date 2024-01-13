using System;
using TensionDev.CoordinateSystems;
using Xunit;

namespace XUnitTestProjectCoordinateSystems
{
    public class UnitTestGeographicCoordinateSystem
    {
        [Fact]
        public void TestDefaultConstructor()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem();

            Assert.Equal(0, geographicCoordinateSystem.LatitudeDecimalDegrees);
            Assert.Equal(0, geographicCoordinateSystem.LongitudeDecimalDegrees);
            Assert.Equal(0, geographicCoordinateSystem.AltitudeMetres);
            Assert.Equal(0, geographicCoordinateSystem.LatitudeDecimalRadians);
            Assert.Equal(0, geographicCoordinateSystem.LongitudeDecimalRadians);
            Assert.Equal(0, geographicCoordinateSystem.AltitudeFeet);
        }

        [Fact]
        public void TestConstructorWithValues1()
        {
            double latitudeDeg = 30;
            double longitudeDeg = 30;
            double altitudeMetre = 1000;
            double latitudeRad = latitudeDeg * Math.PI / 180.0;
            double longitudeRad = longitudeDeg * Math.PI / 180.0;
            double altitudeFeet = altitudeMetre / 0.3048;

            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = latitudeDeg,
                LongitudeDecimalDegrees = longitudeDeg,
                AltitudeMetres = altitudeMetre,
            };

            Assert.Equal(latitudeDeg, geographicCoordinateSystem.LatitudeDecimalDegrees);
            Assert.Equal(longitudeDeg, geographicCoordinateSystem.LongitudeDecimalDegrees);
            Assert.Equal(altitudeMetre, geographicCoordinateSystem.AltitudeMetres);
            Assert.Equal(latitudeRad, geographicCoordinateSystem.LatitudeDecimalRadians);
            Assert.Equal(longitudeRad, geographicCoordinateSystem.LongitudeDecimalRadians);
            Assert.Equal(altitudeFeet, geographicCoordinateSystem.AltitudeFeet);
        }

        [Fact]
        public void TestConstructorWithValues2()
        {
            double latitudeDeg = -60;
            double longitudeDeg = -90;
            double altitudeMetre = -5000;
            double latitudeRad = latitudeDeg * Math.PI / 180.0;
            double longitudeRad = longitudeDeg * Math.PI / 180.0;
            double altitudeFeet = altitudeMetre / 0.3048;

            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = latitudeDeg,
                LongitudeDecimalDegrees = longitudeDeg,
                AltitudeMetres = altitudeMetre,
            };

            Assert.Equal(latitudeDeg, geographicCoordinateSystem.LatitudeDecimalDegrees);
            Assert.Equal(longitudeDeg, geographicCoordinateSystem.LongitudeDecimalDegrees);
            Assert.Equal(altitudeMetre, geographicCoordinateSystem.AltitudeMetres);
            Assert.Equal(latitudeRad, geographicCoordinateSystem.LatitudeDecimalRadians);
            Assert.Equal(longitudeRad, geographicCoordinateSystem.LongitudeDecimalRadians);
            Assert.Equal(altitudeFeet, geographicCoordinateSystem.AltitudeFeet);
        }

        [Fact]
        public void TestConstructorLimits()
        {
            double latitudeDeg = 90;
            double longitudeDeg = -180;
            double altitudeMetre = 40000;
            double latitudeRad = latitudeDeg * Math.PI / 180.0;
            double longitudeRad = longitudeDeg * Math.PI / 180.0;
            double altitudeFeet = altitudeMetre / 0.3048;

            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = double.MaxValue,
                LongitudeDecimalDegrees = double.MinValue,
                AltitudeMetres = altitudeMetre,
            };

            Assert.Equal(latitudeDeg, geographicCoordinateSystem.LatitudeDecimalDegrees);
            Assert.Equal(longitudeDeg, geographicCoordinateSystem.LongitudeDecimalDegrees);
            Assert.Equal(altitudeMetre, geographicCoordinateSystem.AltitudeMetres);
            Assert.Equal(latitudeRad, geographicCoordinateSystem.LatitudeDecimalRadians);
            Assert.Equal(longitudeRad, geographicCoordinateSystem.LongitudeDecimalRadians);
            Assert.Equal(altitudeFeet, geographicCoordinateSystem.AltitudeFeet);
        }
    }
}