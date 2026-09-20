using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeographicDdmTests : IDisposable
    {
        private bool disposedValue;

        private const Int32 POSITIONAL_PRECISION = 5;

        public GeographicDdmTests()
        {
        }

        [Fact]
        public void TestDefaultConstructor()
        {
            GeographicDdm ddm = new GeographicDdm();

            Assert.Equal(0, ddm.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, ddm.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', ddm.LatitudeDirection);
            Assert.Equal(0, ddm.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, ddm.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', ddm.LongitudeDirection);
        }

        [Fact]
        public void TestPropertiesSetAndGet()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.2,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.3,
                LongitudeDirection = 'E',
            };

            Assert.Equal(52, ddm.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, ddm.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', ddm.LatitudeDirection);
            Assert.Equal(13, ddm.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, ddm.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', ddm.LongitudeDirection);
        }

        [Fact]
        public void TestLatitudeDegreesClampedTo90()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 120,
            };

            Assert.Equal(90, ddm.LatitudeDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestLatitudeDegreesClampedToNegative()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = -120,
            };

            Assert.Equal(90, ddm.LatitudeDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestLongitudeDegreesClampedTo180()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LongitudeDegrees = 200,
            };

            Assert.Equal(180, ddm.LongitudeDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestLongitudeDegreesClampedToNegative()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LongitudeDegrees = -200,
            };

            Assert.Equal(180, ddm.LongitudeDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_ReturnsCorrectValues()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 52.52,
                LongitudeDecimalDegrees = 13.405,
                AltitudeMetres = 100.5,
            };

            GeographicDdm result = GeographicConverter.ToDdm(source);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_NegativeValues()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = -52.52,
                LongitudeDecimalDegrees = -13.405,
                AltitudeMetres = 100.5,
            };

            GeographicDdm result = GeographicConverter.ToDdm(source);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('S', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('W', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_Equator()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 0,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0,
            };

            GeographicDdm result = GeographicConverter.ToDdm(source);

            Assert.Equal(0, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(0, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_NorthPole()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 90,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0,
            };

            GeographicDdm result = GeographicConverter.ToDdm(source);

            Assert.Equal(90, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_SouthPole()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = -90,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0,
            };

            GeographicDdm result = GeographicConverter.ToDdm(source);

            Assert.Equal(90, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('S', result.LatitudeDirection);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_PrimeMeridian()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 0,
                LongitudeDecimalDegrees = 0,
                AltitudeMetres = 0,
            };

            GeographicDdm result = GeographicConverter.ToDdm(source);

            Assert.Equal(0, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_Antimeridian()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 0,
                LongitudeDecimalDegrees = 180,
                AltitudeMetres = 0,
            };

            GeographicDdm result = GeographicConverter.ToDdm(source);

            Assert.Equal(180, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('W', result.LongitudeDirection);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_NullSource()
        {
            GeographicCoordinateSystem source = null;

            Assert.Throws<ArgumentNullException>(() => GeographicConverter.ToDdm(source));
        }

        [Fact]
        public void TestToString_NegativeValues()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.2,
                LatitudeDirection = 'S',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.3,
                LongitudeDirection = 'W',
            };

            string result = ddm.ToString();
            Assert.Equal("52° 31.200000' S, 13° 24.300000' W", result);
        }

        [Fact]
        public void TestToString_ZeroValues()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 0,
                LatitudeMinutes = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString();
            Assert.Equal("0° 0.000000' N, 0° 0.000000' E", result);
        }

        [Fact]
        public void TestToString_NorthPole()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 90,
                LatitudeMinutes = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString();
            Assert.Equal("90° 0.000000' N, 0° 0.000000' E", result);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_RoundTrip()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 1.309432,
                LongitudeDecimalDegrees = 103.780349,
                AltitudeMetres = 25.0,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);

            double reconstructedLatitude = ddm.LatitudeDegrees + ddm.LatitudeMinutes / 60.0;
            if (ddm.LatitudeDirection == 'S')
            {
                reconstructedLatitude = -reconstructedLatitude;
            }

            double reconstructedLongitude = ddm.LongitudeDegrees + ddm.LongitudeMinutes / 60.0;
            if (ddm.LongitudeDirection == 'W')
            {
                reconstructedLongitude = -reconstructedLongitude;
            }

            Assert.Equal(source.LatitudeDecimalDegrees, reconstructedLatitude, POSITIONAL_PRECISION);
            Assert.Equal(source.LongitudeDecimalDegrees, reconstructedLongitude, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestToString_PrecisionZero()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.234567,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.345678,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString(0);
            Assert.Equal("52° 31' N, 13° 24' E", result);
        }

        [Fact]
        public void TestToString_PrecisionThree()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.234567,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.345678,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString(3);
            Assert.Equal("52° 31.235' N, 13° 24.346' E", result);
        }

        [Fact]
        public void TestToString_PrecisionSix()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.234567,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.345678,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString(6);
            Assert.Equal("52° 31.234567' N, 13° 24.345678' E", result);
        }

        [Fact]
        public void TestToString_PrecisionClampedToSix()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.2345678901,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.3456789012,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString(10);
            Assert.Equal("52° 31.234568' N, 13° 24.345679' E", result);
        }

        [Fact]
        public void TestToString_MinuteRoundingToSixty()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 59.9999995,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString(6);
            Assert.Equal("53° 0.000000' N, 13° 24.000000' E", result);
        }

        [Fact]
        public void TestToString_MinuteOverflowIntoDegrees()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 59.5,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 59.9999995,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString(6);
            Assert.Equal("52° 59.500000' N, 14° 0.000000' E", result);
        }

        [Fact]
        public void TestToString_MaximumPrecision()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.9999995,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.9999995,
                LongitudeDirection = 'E',
            };

            string result = ddm.ToString(6);
            Assert.Equal("52° 32.000000' N, 13° 25.000000' E", result);
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_ReturnsCorrectValues()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.2,
                LatitudeDirection = 'N',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.3,
                LongitudeDirection = 'E',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_NegativeValues()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 52,
                LatitudeMinutes = 31.2,
                LatitudeDirection = 'S',
                LongitudeDegrees = 13,
                LongitudeMinutes = 24.3,
                LongitudeDirection = 'W',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

            Assert.Equal(-52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(-13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_Equator()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 0,
                LatitudeMinutes = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeDirection = 'E',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_NorthPole()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 90,
                LatitudeMinutes = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeDirection = 'E',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

            Assert.Equal(90, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_SouthPole()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 90,
                LatitudeMinutes = 0,
                LatitudeDirection = 'S',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeDirection = 'E',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

            Assert.Equal(-90, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_PrimeMeridian()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 0,
                LatitudeMinutes = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 0,
                LongitudeMinutes = 0,
                LongitudeDirection = 'W',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_Antimeridian()
        {
            GeographicDdm ddm = new GeographicDdm
            {
                LatitudeDegrees = 0,
                LatitudeMinutes = 0,
                LatitudeDirection = 'N',
                LongitudeDegrees = 180,
                LongitudeMinutes = 0,
                LongitudeDirection = 'W',
            };

            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(-180, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_NullSource()
        {
            GeographicDdm ddm = null;

            Assert.Throws<ArgumentNullException>(() => GeographicConverter.FromDdm(ddm));
        }

        [Fact]
        public void TestFromDdmFromGeographicConverter_RoundTrip()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = 1.309432,
                LongitudeDecimalDegrees = 103.780349,
                AltitudeMetres = 25.0,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

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
