using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeohashTests : IDisposable
    {
        private bool disposedValue;

        // Geohash Length 1
        const Double LatitudeError1 = 23;
        const Double LongitudeError1 = 23;

        // Geohash Length 5
        const Double LatitudeError5 = 0.022;
        const Double LongitudeError5 = 0.022;

        // Geohash Length 8
        const Double LatitudeError8 = 0.000085;
        const Double LongitudeError8 = 0.00017;

        public GeohashTests()
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

        [Theory]
        [InlineData("u4pruydqqvjs1")]
        [InlineData("ezs42a")]
        [InlineData("lezs42")]
        [InlineData("iezs42o")]
        public void TestConstructorInvalidStrings(String geohashString)
        {
            Geohash geohash = null;

            Assert.Throws<ArgumentException>(() => geohash = new Geohash(geohashString));
        }

        [Fact]
        public void TestConstructorLimits()
        {
            GeographicCoordinateSystem expected = new GeographicCoordinateSystem()
            {
                LatitudeDecimalRadians = 1.309432 * Math.PI / 180.0,
                LongitudeDecimalRadians = 103.780349 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            String geohashString = "w21z3w6ecruf";
            Geohash geohash = new Geohash(geohashString);

            GeographicCoordinateSystem actual = GeographicConverter.From(geohash);

            Assert.Equal(expected.LatitudeDecimalDegrees, actual.LatitudeDecimalDegrees, LatitudeError8);
            Assert.Equal(expected.LongitudeDecimalDegrees, actual.LongitudeDecimalDegrees, LongitudeError8);
            Assert.Equal(expected.AltitudeMetres, actual.AltitudeMetres);
        }

        [Fact]
        public void TestGeographicConverterFromGeohash()
        {
            GeographicCoordinateSystem expected = new GeographicCoordinateSystem()
            {
                LatitudeDecimalRadians = 42.605 * Math.PI / 180.0,
                LongitudeDecimalRadians = -5.603 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            String geohashString = "ezs42";
            Geohash geohash = new Geohash(geohashString);

            GeographicCoordinateSystem actual = GeographicConverter.From(geohash);

            Assert.Equal(expected.LatitudeDecimalDegrees, actual.LatitudeDecimalDegrees, LatitudeError5);
            Assert.Equal(expected.LongitudeDecimalDegrees, actual.LongitudeDecimalDegrees, LongitudeError5);
            Assert.Equal(expected.AltitudeMetres, actual.AltitudeMetres);
        }

        [Fact]
        public void TestGeohashConverterFromGeographicCoordinateSystemUnderLimit()
        {
            GeographicCoordinateSystem value = new GeographicCoordinateSystem()
            {
                LatitudeDecimalRadians = 57.64911 * Math.PI / 180.0,
                LongitudeDecimalRadians = 10.40744 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => GeohashConverter.From(value, 0));
        }

        [Fact]
        public void TestGeohashConverterFromGeographicCoordinateSystemLengthOne()
        {
            String geohashString = "m";
            Geohash expected = new Geohash(geohashString);

            GeographicCoordinateSystem value = new GeographicCoordinateSystem()
            {
                LatitudeDecimalRadians = -22 * Math.PI / 180.0,
                LongitudeDecimalRadians = 68 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            Geohash actual = GeohashConverter.From(value, 1);

            Assert.Equal(expected.Hash, actual.Hash);
        }

        [Fact]
        public void TestGeohashConverterFromGeographicCoordinateSystem()
        {
            String geohashString = "u4pruydqqvj";
            Geohash expected = new Geohash(geohashString);

            GeographicCoordinateSystem value = new GeographicCoordinateSystem()
            {
                LatitudeDecimalRadians = 57.64911 * Math.PI / 180.0,
                LongitudeDecimalRadians = 10.40744 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            Geohash actual = GeohashConverter.From(value, 11);

            Assert.Equal(expected.Hash, actual.Hash);
        }

        [Theory]
        [InlineData(0.0, 0.0, 9, "s00000000")]
        [InlineData(48.856667, 2.352222, 9, "u09tvw0fd")]
        [InlineData(-89.99, -179.99, 4, "0000")]
        public void FromMatchesCta5009ATestVectors(Double latitude, Double longitude, Int32 length, String expectedHash)
        {
            // CTA-5009-A, Annex A (encoding test vectors).
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = latitude * Math.PI / 180.0,
                LongitudeDecimalRadians = longitude * Math.PI / 180.0,
            };

            Geohash actual = GeohashConverter.From(source, length);

            Assert.Equal(expectedHash, actual.Hash);
        }

        [Fact]
        public void FromRejectsNorthPoleEndpoint()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = 90.0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0.0 * Math.PI / 180.0,
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => GeohashConverter.From(source, 1));
        }

        [Fact]
        public void FromRejectsAntimeridianPositiveEndpoint()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = 0.0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 180.0 * Math.PI / 180.0,
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => GeohashConverter.From(source, 1));
        }

        [Fact]
        public void TestGeohashConverterFromGeographicCoordinateSystemLengthMax()
        {
            String geohashString = "09bhjkmnpz2c";
            Geohash expected = new Geohash(geohashString);

            GeographicCoordinateSystem value = new GeographicCoordinateSystem()
            {
                LatitudeDecimalRadians = -79.428710 * Math.PI / 180.0,
                LongitudeDecimalRadians = -157.262087 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            Geohash actual = GeohashConverter.From(value, 12);

            Assert.Equal(expected.Hash, actual.Hash);
        }

        [Fact]
        public void TestGeohashConverterFromGeographicCoordinateSystemOverLimit()
        {
            GeographicCoordinateSystem value = new GeographicCoordinateSystem()
            {
                LatitudeDecimalRadians = 57.64911 * Math.PI / 180.0,
                LongitudeDecimalRadians = 10.40744 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => GeohashConverter.From(value, 13));
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
