using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeographicDmsTests : IDisposable
    {
        private bool disposedValue;

        private const Int32 POSITIONAL_PRECISION = 5;

        public GeographicDmsTests()
        {
        }

        [Fact]
        public void TestDefaultConstructor()
        {
            GeographicDms dms = new GeographicDms();

            Assert.Equal(0, dms.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, dms.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(0, dms.LatitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('N', dms.LatitudeDirection);
            Assert.Equal(0, dms.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, dms.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(0, dms.LongitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('E', dms.LongitudeDirection);
        }

        [Fact]
        public void TestPropertiesSetAndGet()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 12,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18,
                LongitudeDirection = 'E',
            };

            Assert.Equal(52, dms.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31, dms.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(12, dms.LatitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('N', dms.LatitudeDirection);
            Assert.Equal(13, dms.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24, dms.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(18, dms.LongitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('E', dms.LongitudeDirection);
        }

        [Fact]
        public void TestLatitudeDegreesClampedTo90()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 120,
            };

            Assert.Equal(90, dms.LatitudeDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestLatitudeDegreesClampedToNegative()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = -120,
            };

            Assert.Equal(90, dms.LatitudeDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestLongitudeDegreesClampedTo180()
        {
            GeographicDms dms = new GeographicDms
            {
                LongitudeDegrees = 200,
            };

            Assert.Equal(180, dms.LongitudeDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestLongitudeDegreesClampedToNegative()
        {
            GeographicDms dms = new GeographicDms
            {
                LongitudeDegrees = -200,
            };

            Assert.Equal(180, dms.LongitudeDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_ReturnsCorrectValues()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 52.52,
                LongitudeDecimalDegrees = 13.405,
                AltitudeMetres = 100.5,
            };

            GeographicDms result = GeographicConverter.ToDms(source);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(12, result.LatitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(18, result.LongitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_NegativeValues()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = -52.52,
                LongitudeDecimalDegrees = -13.405,
                AltitudeMetres = 100.5,
            };

            GeographicDms result = GeographicConverter.ToDms(source);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(12, result.LatitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('S', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(18, result.LongitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('W', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_Equator()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 0,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0,
            };

            GeographicDms result = GeographicConverter.ToDms(source);

            Assert.Equal(0, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(0, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_NorthPole()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 90,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0,
            };

            GeographicDms result = GeographicConverter.ToDms(source);

            Assert.Equal(90, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_SouthPole()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = -90,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0,
            };

            GeographicDms result = GeographicConverter.ToDms(source);

            Assert.Equal(90, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('S', result.LatitudeDirection);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_PrimeMeridian()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 0,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0,
            };

            GeographicDms result = GeographicConverter.ToDms(source);

            Assert.Equal(0, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_Antimeridian()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 0,
                LongitudeDecimalDegrees = 180,
                AltitudeMetres = 0,
            };

            GeographicDms result = GeographicConverter.ToDms(source);

            Assert.Equal(180, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeSeconds, POSITIONAL_PRECISION);
            Assert.Equal('W', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_NullSource()
        {
            GeographicCoordinateSystem source = null;

            Assert.Throws<ArgumentNullException>(() => GeographicConverter.ToDms(source));
        }

        [Fact]
        public void TestToString_NegativeValues()
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
            };

            string result = dms.ToString();
            Assert.Equal("52° 31' 12.000000\" S, 13° 24' 18.000000\" W", result);
        }

        [Fact]
        public void TestToString_ZeroValues()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 0,
                LatitudeMinutes = 0,
                LatitudeSeconds = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeSeconds = 0,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString();
            Assert.Equal("0° 0' 0.000000\" N, 0° 0' 0.000000\" E", result);
        }

        [Fact]
        public void TestToString_NorthPole()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 90,
                LatitudeMinutes = 0,
                LatitudeSeconds = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeSeconds = 0,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString();
            Assert.Equal("90° 0' 0.000000\" N, 0° 0' 0.000000\" E", result);
        }

        [Fact]
        public void TestToDmsFromGeographicConverter_RoundTrip()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 1.309432,
                LongitudeDecimalDegrees = 103.780349,
                AltitudeMetres = 25.0,
            };

            GeographicDms dms = GeographicConverter.ToDms(source);

            double reconstructedLatitude = dms.LatitudeDegrees + dms.LatitudeMinutes / 60.0 + dms.LatitudeSeconds / 3600.0;
            if (dms.LatitudeDirection == 'S')
            {
                reconstructedLatitude = -reconstructedLatitude;
            }

            double reconstructedLongitude = dms.LongitudeDegrees + dms.LongitudeMinutes / 60.0 + dms.LongitudeSeconds / 3600.0;
            if (dms.LongitudeDirection == 'W')
            {
                reconstructedLongitude = -reconstructedLongitude;
            }

            Assert.Equal(source.LatitudeDecimalDegrees, reconstructedLatitude, POSITIONAL_PRECISION);
            Assert.Equal(source.LongitudeDecimalDegrees, reconstructedLongitude, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestToString_PrecisionZero()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 12.678901,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18.789012,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(0);
            Assert.Equal("52° 31' 13\" N, 13° 24' 19\" E", result);
        }

        [Fact]
        public void TestToString_PrecisionThree()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 12.678901,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18.789012,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(3);
            Assert.Equal("52° 31' 12.679\" N, 13° 24' 18.789\" E", result);
        }

        [Fact]
        public void TestToString_PrecisionSix()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 12.678901,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18.789012,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(6);
            Assert.Equal("52° 31' 12.678901\" N, 13° 24' 18.789012\" E", result);
        }

        [Fact]
        public void TestToString_PrecisionClampedToSix()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 12.6789012345,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18.7890123456,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(10);
            Assert.Equal("52° 31' 12.678901\" N, 13° 24' 18.789012\" E", result);
        }

        [Fact]
        public void TestToString_SecondsRoundingToSixty()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 59.9999995,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(6);
            Assert.Equal("52° 32' 0.000000\" N, 13° 24' 18.000000\" E", result);
        }

        [Fact]
        public void TestToString_SecondsOverflowIntoMinutes()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 59.9999995,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 59.9999995,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(6);
            Assert.Equal("52° 32' 0.000000\" N, 13° 25' 0.000000\" E", result);
        }

        [Fact]
        public void TestToString_MinuteRoundingToSixty()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 59.9999995,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(6);
            Assert.Equal("53° 0' 0.000000\" N, 13° 24' 18.000000\" E", result);
        }

        [Fact]
        public void TestToString_CascadingOverflow()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 59,
                LatitudeSeconds = 59.9999995,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 59,
                LongitudeSeconds = 59.9999995,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(6);
            Assert.Equal("53° 0' 0.000000\" N, 14° 0' 0.000000\" E", result);
        }

        [Fact]
        public void TestToString_MaximumPrecision()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 59.9999995,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 59.9999995,
                LongitudeDirection = 'E',
            };

            string result = dms.ToString(6);
            Assert.Equal("52° 32' 0.000000\" N, 13° 25' 0.000000\" E", result);
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_ReturnsCorrectValues()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31,
                LatitudeSeconds = 12,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeSeconds = 18,
                LongitudeDirection = 'E',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDms(dms);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_NegativeValues()
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
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDms(dms);

            Assert.Equal(-52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(-13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_Equator()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 0,
                LatitudeMinutes = 0,
                LatitudeSeconds = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeSeconds = 0,
                LongitudeDirection = 'E',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDms(dms);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_NorthPole()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 90,
                LatitudeMinutes = 0,
                LatitudeSeconds = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeSeconds = 0,
                LongitudeDirection = 'E',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDms(dms);

            Assert.Equal(90, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_SouthPole()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 90,
                LatitudeMinutes = 0,
                LatitudeSeconds = 0,
                LatitudeDirection = 'S',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeSeconds = 0,
                LongitudeDirection = 'E',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDms(dms);

            Assert.Equal(-90, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_PrimeMeridian()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 0,
                LatitudeMinutes = 0,
                LatitudeSeconds = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeSeconds = 0,
                LongitudeDirection = 'W',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDms(dms);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_Antimeridian()
        {
            GeographicDms dms = new GeographicDms
            {
                LatitudeDegrees = 0,
                LatitudeMinutes = 0,
                LatitudeSeconds = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 180,
                LongitudeMinutes = 0,
                LongitudeSeconds = 0,
                LongitudeDirection = 'W',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDms(dms);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(-180, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_NullSource()
        {
            GeographicDms dms = null;

            Assert.Throws<ArgumentNullException>(() => GeographicConverter.FromDms(dms));
        }

        [Fact]
        public void TestFromDmsFromGeographicConverter_RoundTrip()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 1.309432,
                LongitudeDecimalDegrees = 103.780349,
                AltitudeMetres = 25.0,
            };

            GeographicDms dms = GeographicConverter.ToDms(source);
            GeographicCoordinateSystem result = GeographicConverter.FromDms(dms);

            Assert.Equal(source.LatitudeDecimalDegrees, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(source.LongitudeDecimalDegrees, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
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
