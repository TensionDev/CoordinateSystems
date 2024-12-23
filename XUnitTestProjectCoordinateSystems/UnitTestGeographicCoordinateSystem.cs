using System;
using TensionDev.CoordinateSystems;
using Xunit;

namespace XUnitTestProjectCoordinateSystems
{
    public class UnitTestGeographicCoordinateSystem : IDisposable
    {
        private bool disposedValue;

        private const Int32 POSITIONAL_PRECISION = 5;

        public UnitTestGeographicCoordinateSystem()
        {
        }

        [Fact]
        public void TestDefaultConstructor()
        {
            GeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem();

            Assert.Equal(0, geographicCoordinateSystem.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, geographicCoordinateSystem.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, geographicCoordinateSystem.AltitudeMetres, POSITIONAL_PRECISION);
            Assert.Equal(0, geographicCoordinateSystem.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(0, geographicCoordinateSystem.LongitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(0, geographicCoordinateSystem.AltitudeFeet, POSITIONAL_PRECISION);
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

            Assert.Equal(latitudeDeg, geographicCoordinateSystem.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(longitudeDeg, geographicCoordinateSystem.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(altitudeMetre, geographicCoordinateSystem.AltitudeMetres, POSITIONAL_PRECISION);
            Assert.Equal(latitudeRad, geographicCoordinateSystem.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(longitudeRad, geographicCoordinateSystem.LongitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(altitudeFeet, geographicCoordinateSystem.AltitudeFeet, POSITIONAL_PRECISION);
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

            Assert.Equal(latitudeDeg, geographicCoordinateSystem.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(longitudeDeg, geographicCoordinateSystem.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(altitudeMetre, geographicCoordinateSystem.AltitudeMetres, POSITIONAL_PRECISION);
            Assert.Equal(latitudeRad, geographicCoordinateSystem.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(longitudeRad, geographicCoordinateSystem.LongitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(altitudeFeet, geographicCoordinateSystem.AltitudeFeet, POSITIONAL_PRECISION);
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

            Assert.Equal(latitudeDeg, geographicCoordinateSystem.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(longitudeDeg, geographicCoordinateSystem.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(altitudeMetre, geographicCoordinateSystem.AltitudeMetres, POSITIONAL_PRECISION);
            Assert.Equal(latitudeRad, geographicCoordinateSystem.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(longitudeRad, geographicCoordinateSystem.LongitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(altitudeFeet, geographicCoordinateSystem.AltitudeFeet, POSITIONAL_PRECISION);
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

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitTestGeographicCoordinateSystem()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}