using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class CoordinateSystemToStringTests : IDisposable
    {
        private bool disposedValue;

        private const Int32 POSITIONAL_PRECISION = 5;

        public CoordinateSystemToStringTests()
        {
        }

        [Fact]
        public void TestGeographicCoordinateSystem_ToString_Default()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 52.5200,
                LongitudeDecimalDegrees = 13.4050,
                AltitudeMetres = 100.5
            };

            string result = geographicCoordinateSystem.ToString();
            Assert.Equal("52.5200, 13.4050 Altitude: 100.50 m", result);
        }

        [Fact]
        public void TestGeographicCoordinateSystem_ToString_DecimalDegrees()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 52.5200,
                LongitudeDecimalDegrees = 13.4050,
                AltitudeMetres = 100.5
            };

            string result = geographicCoordinateSystem.ToString(AngularFormat.DecimalDegrees, DistanceUnit.Metres);
            Assert.Equal("52.5200, 13.4050 Altitude: 100.50 m", result);
        }

        [Fact]
        public void TestGeographicCoordinateSystem_ToString_DegreesDecimalMinutes()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 52.5200,
                LongitudeDecimalDegrees = 13.4050,
                AltitudeMetres = 100.5
            };

            string result = geographicCoordinateSystem.ToString(AngularFormat.DegreesDecimalMinutes, DistanceUnit.Metres);
            Assert.Equal("52° 31.20' N, 13° 24.30' E Altitude: 100.50 m", result);
        }

        [Fact]
        public void TestGeographicCoordinateSystem_ToString_DegreesMinutesSeconds()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 52.5200,
                LongitudeDecimalDegrees = 13.4050,
                AltitudeMetres = 100.5
            };

            string result = geographicCoordinateSystem.ToString(AngularFormat.DegreesMinutesSeconds, DistanceUnit.Metres);
            Assert.Equal("52° 31' 12.00\" N, 13° 24' 18.00\" E Altitude: 100.50 m", result);
        }

        [Fact]
        public void TestGeographicCoordinateSystem_ToString_Radians()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 52.5200,
                LongitudeDecimalDegrees = 13.4050,
                AltitudeMetres = 100.5
            };

            string result = geographicCoordinateSystem.ToString(AngularFormat.Radians, DistanceUnit.Metres);
            Assert.Equal("0.9166, 0.2340 Altitude: 100.50 m", result);
        }

        [Fact]
        public void TestGeographicCoordinateSystem_ToString_DistanceUnit_Feet()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 52.5200,
                LongitudeDecimalDegrees = 13.4050,
                AltitudeMetres = 100.5
            };

            string result = geographicCoordinateSystem.ToString(AngularFormat.DecimalDegrees, DistanceUnit.Feet);
            Assert.Equal("52.5200, 13.4050 Altitude: 329.72 ft", result);
        }

        [Fact]
        public void TestGeocentricCoordinateSystem_ToString_Default()
        {
            GeocentricCoordinateSystem geocentricCoordinateSystem = new GeocentricCoordinateSystem()
            {
                X = 1000.5,
                Y = 2000.7,
                Z = 3000.9
            };

            string result = geocentricCoordinateSystem.ToString();
            Assert.Equal("X: 1000.50 m, Y: 2000.70 m, Z: 3000.90 m", result);
        }

        [Fact]
        public void TestGeocentricCoordinateSystem_ToString_DistanceUnit_Feet()
        {
            GeocentricCoordinateSystem geocentricCoordinateSystem = new GeocentricCoordinateSystem()
            {
                X = 1000.5,
                Y = 2000.7,
                Z = 3000.9
            };

            string result = geocentricCoordinateSystem.ToString(DistanceUnit.Feet);
            Assert.Equal("X: 3282.48 ft, Y: 6563.98 ft, Z: 9845.47 ft", result);
        }

        [Fact]
        public void TestGeographicCoordinateSystem_ToString_NegativeValues()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = -52.5200,
                LongitudeDecimalDegrees = -13.4050,
                AltitudeMetres = 100.5
            };

            string result = geographicCoordinateSystem.ToString(AngularFormat.DegreesDecimalMinutes, DistanceUnit.Metres);
            Assert.Equal("52° 31.20' S, 13° 24.30' W Altitude: 100.50 m", result);
        }

        [Fact]
        public void TestGeographicCoordinateSystem_ToString_ZeroValues()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 0,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0
            };

            string result = geographicCoordinateSystem.ToString(AngularFormat.DegreesMinutesSeconds, DistanceUnit.Metres);
            Assert.Equal("0° 0' 0.00\" N, 0° 0' 0.00\" E Altitude: 0.00 m", result);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}