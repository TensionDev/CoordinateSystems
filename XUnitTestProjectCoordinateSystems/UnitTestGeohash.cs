using System;
using TensionDev.CoordinateSystems;
using Xunit;

namespace XUnitTestProjectCoordinateSystems
{
    public class UnitTestGeohash : IDisposable
    {
        private bool disposedValue;

        // Geohash Length 5
        const Double LatitudeError5 = 0.022;
        const Double LongitudeError5 = 0.022;

        // Geohash Length 8
        const Double LatitudeError8 = 0.000085;
        const Double LongitudeError8 = 0.00017;

        public UnitTestGeohash()
        {
        }

        [Fact]
        public void TestConstructorNullString()
        {
            String geohashString = null;
            Geohash geohash = null;

            Assert.Throws<ArgumentNullException>(() => geohash = new Geohash(geohashString));
        }

        [Fact]
        public void TestConstructorEmptyString()
        {
            String geohashString = String.Empty;
            Geohash geohash = null;

            Assert.Throws<ArgumentNullException>(() => geohash = new Geohash(geohashString));
        }

        [Fact]
        public void TestConstructorInvalidLengthString()
        {
            String geohashString = "u4pruydqqvjs1";
            Geohash geohash = null;

            Assert.Throws<ArgumentException>(() => geohash = new Geohash(geohashString));
        }

        [Fact]
        public void TestConstructorInvalidGeohashEncoding1()
        {
            String geohashString = "ezs42a";
            Geohash geohash = null;

            Assert.Throws<ArgumentException>(() => geohash = new Geohash(geohashString));
        }

        [Fact]
        public void TestConstructorInvalidGeohashEncoding2()
        {
            String geohashString = "lezs42";
            Geohash geohash = null;

            Assert.Throws<ArgumentException>(() => geohash = new Geohash(geohashString));
        }

        [Fact]
        public void TestConstructorInvalidGeohashEncoding3()
        {
            String geohashString = "iezs42o";
            Geohash geohash = null;

            Assert.Throws<ArgumentException>(() => geohash = new Geohash(geohashString));
        }

        [Fact]
        public void TestConstructorLimits()
        {
            GeographicCoordinateSystem expected = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 1.309432,
                LongitudeDecimalDegrees = 103.780349,
                AltitudeMetres = 0,
            };

            String geohashString = "w21z3w6ecruf";
            Geohash geohash = new Geohash(geohashString);

            GeographicCoordinateSystem actual = geohash.ToGeographicCoordinateSystem();

            Assert.Equal(expected.LatitudeDecimalDegrees, actual.LatitudeDecimalDegrees, LatitudeError8);
            Assert.Equal(expected.LongitudeDecimalDegrees, actual.LongitudeDecimalDegrees, LongitudeError8);
            Assert.Equal(expected.AltitudeMetres, actual.AltitudeMetres);
        }

        [Fact]
        public void TestToGeographicCoordinateSystem()
        {
            GeographicCoordinateSystem expected = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 42.605,
                LongitudeDecimalDegrees = -5.603,
                AltitudeMetres = 0,
            };

            String geohashString = "ezs42";
            Geohash geohash = new Geohash(geohashString);

            GeographicCoordinateSystem actual = geohash.ToGeographicCoordinateSystem();

            Assert.Equal(expected.LatitudeDecimalDegrees, actual.LatitudeDecimalDegrees, LatitudeError5);
            Assert.Equal(expected.LongitudeDecimalDegrees, actual.LongitudeDecimalDegrees, LongitudeError5);
            Assert.Equal(expected.AltitudeMetres, actual.AltitudeMetres);
        }

        [Fact]
        public void TestFromGeographicCoordinateSystem()
        {
            String geohashString = "u4pruydqqvj";
            Geohash expected = new Geohash(geohashString);

            GeographicCoordinateSystem value = new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = 57.64911,
                LongitudeDecimalDegrees = 10.40744,
                AltitudeMetres = 0,
            };

            Geohash actual = Geohash.FromGeographicCoordinateSystem(value, 11);

            Assert.Equal(expected.Hash, actual.Hash);
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
        // ~UnitTestGeohash()
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
