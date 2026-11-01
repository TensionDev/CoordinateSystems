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
                LatitudeDecimalRadians = 52.52 * Math.PI / 180.0,
                LongitudeDecimalRadians = 13.405 * Math.PI / 180.0,
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
                LatitudeDecimalRadians = -52.52 * Math.PI / 180.0,
                LongitudeDecimalRadians = -13.405 * Math.PI / 180.0,
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
                LatitudeDecimalRadians = 0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0 * Math.PI / 180.0,
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
                LatitudeDecimalRadians = 90 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0 * Math.PI / 180.0,
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
                LatitudeDecimalRadians = -90 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0 * Math.PI / 180.0,
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
                LatitudeDecimalRadians = 0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0 * Math.PI / 180.0,
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
                LatitudeDecimalRadians = 0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 180 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            GeographicDdm result = GeographicConverter.ToDdm(source);

            Assert.Equal(180, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
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
            Assert.Equal("52° 31.200000' S, 13° 24.300000' W Altitude: 0.000 m", result);
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
            Assert.Equal("0° 0.000000' N, 0° 0.000000' E Altitude: 0.000 m", result);
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
            Assert.Equal("90° 0.000000' N, 0° 0.000000' E Altitude: 0.000 m", result);
        }

        [Fact]
        public void TestToDdmFromGeographicConverter_RoundTrip()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = 1.309432 * Math.PI / 180.0,
                LongitudeDecimalRadians = 103.780349 * Math.PI / 180.0,
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
            Assert.Equal("52° 31' N, 13° 24' E Altitude: 0.000 m", result);
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
            Assert.Equal("52° 31.235' N, 13° 24.346' E Altitude: 0.000 m", result);
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
            Assert.Equal("52° 31.234567' N, 13° 24.345678' E Altitude: 0.000 m", result);
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
            Assert.Equal("52° 31.234568' N, 13° 24.345679' E Altitude: 0.000 m", result);
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
            Assert.Equal("53° 0.000000' N, 13° 24.000000' E Altitude: 0.000 m", result);
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
            Assert.Equal("52° 59.500000' N, 14° 0.000000' E Altitude: 0.000 m", result);
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
            Assert.Equal("52° 32.000000' N, 13° 25.000000' E Altitude: 0.000 m", result);
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
                LatitudeDecimalRadians = 1.309432 * Math.PI / 180.0,
                LongitudeDecimalRadians = 103.780349 * Math.PI / 180.0,
                AltitudeMetres = 25.0,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            GeographicCoordinateSystem result = GeographicConverter.FromDdm(ddm);

            Assert.Equal(source.LatitudeDecimalDegrees, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(source.LongitudeDecimalDegrees, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        #region Parse / TryParse Tests

        [Fact]
        public void Parse_ValidDdmString_ReturnsCorrectValues()
        {
            string input = "52° 31.20' N, 13° 24.30' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_DdmWithSouthernWesternDirections_ReturnsCorrectValues()
        {
            string input = "33° 51.67' S, 18° 25.27' W";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(33, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(51.67, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('S', result.LatitudeDirection);
            Assert.Equal(18, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(25.27, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('W', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_DdmCaseInsensitiveDirection_ReturnsCorrectValues()
        {
            string input = "52° 31.20' n, 13° 24.30' e";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_DdmMixedCaseDirection_ReturnsCorrectValues()
        {
            string input = "52° 31.20' N, 13° 24.30' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_ZeroValues_ReturnsCorrectValues()
        {
            string input = "0° 0.000000' N, 0° 0.000000' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(0, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(0, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_NorthPole_ReturnsCorrectValues()
        {
            string input = "90° 0.000000' N, 0° 0.000000' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(90, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
        }

        [Fact]
        public void Parse_SouthPole_ReturnsCorrectValues()
        {
            string input = "90° 0.000000' S, 0° 0.000000' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(90, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('S', result.LatitudeDirection);
        }

        [Fact]
        public void Parse_PrimeMeridian_ReturnsCorrectValues()
        {
            string input = "0° 0.000000' N, 0° 0.000000' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(0, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_Antimeridian_ReturnsCorrectValues()
        {
            string input = "0° 0.000000' N, 180° 0.000000' W";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(180, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('W', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_DdmWithAltitudeMetres_ParsesAltitude()
        {
            string input = "52° 31.20' N, 13° 24.30' E Altitude: 100.500 m";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_DdmWithAltitudeFeet_ParsesAltitude()
        {
            string input = "52° 31.20' N, 13° 24.30' E Altitude: 329.724 ft";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_DdmWithoutAltitude_DefaultsToZero()
        {
            string input = "52° 31.20' N, 13° 24.30' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_SpaceDelimitedDdm_ReturnsCorrectValues()
        {
            string input = "52 31.20 N, 13 24.30 E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_SpaceDelimitedDdmWithTab_ReturnsCorrectValues()
        {
            string input = "52\t31.20\tN, 13\t24.30\tE";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(52, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, result.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', result.LatitudeDirection);
            Assert.Equal(13, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, result.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', result.LongitudeDirection);
        }

        [Fact]
        public void Parse_SpaceDelimitedDdmNonIntegerDegrees_ThrowsFormatException()
        {
            string input = "52.5 31.20 N, 13 24.30 E";

            Assert.Throws<FormatException>(() => GeographicDdm.Parse(input));
        }

        [Fact]
        public void Parse_EmptyString_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicDdm.Parse(""));
        }

        [Fact]
        public void Parse_NullString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => GeographicDdm.Parse(null!));
        }

        [Fact]
        public void Parse_WhitespaceString_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicDdm.Parse("   "));
        }

        [Fact]
        public void Parse_MissingComma_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicDdm.Parse("52° 31.20' N 13° 24.30' E"));
        }

        [Fact]
        public void Parse_MissingDirectionIndicator_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicDdm.Parse("52° 31.20, 13° 24.30' E"));
        }

        [Fact]
        public void Parse_InvalidDirection_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicDdm.Parse("52° 31.20' X, 13° 24.30' E"));
        }

        [Fact]
        public void Parse_NonNumericDegrees_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicDdm.Parse("abc° 31.20' N, 13° 24.30' E"));
        }

        [Fact]
        public void Parse_NonNumericMinutes_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicDdm.Parse("52° xyz' N, 13° 24.30' E"));
        }

        [Fact]
        public void Parse_InvalidAltitudeFormat_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicDdm.Parse("52° 31.20' N, 13° 24.30' E Altitude: abc m"));
        }

        [Fact]
        public void TryParse_ValidDdmString_ReturnsTrue()
        {
            string input = "52° 31.20' N, 13° 24.30' E";

            bool result = GeographicDdm.TryParse(input, out GeographicDdm ddm);

            Assert.True(result);
            Assert.Equal(52, ddm.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, ddm.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', ddm.LatitudeDirection);
            Assert.Equal(13, ddm.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, ddm.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', ddm.LongitudeDirection);
        }

        [Fact]
        public void TryParse_InvalidString_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParse("not a valid DDM", out GeographicDdm ddm);

            Assert.False(result);
        }

        [Fact]
        public void TryParse_EmptyString_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParse("", out GeographicDdm ddm);

            Assert.False(result);
        }

        [Fact]
        public void TryParse_NullString_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParse(null!, out GeographicDdm ddm);

            Assert.False(result);
        }

        [Fact]
        public void TryParse_SpaceDelimitedDdm_ReturnsTrue()
        {
            string input = "52 31.20 N, 13 24.30 E";

            bool result = GeographicDdm.TryParse(input, out GeographicDdm ddm);

            Assert.True(result);
            Assert.Equal(52, ddm.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, ddm.LatitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('N', ddm.LatitudeDirection);
            Assert.Equal(13, ddm.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(24.3, ddm.LongitudeMinutes, POSITIONAL_PRECISION);
            Assert.Equal('E', ddm.LongitudeDirection);
        }

        [Fact]
        public void TryParse_SpaceDelimitedNonIntegerDegrees_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParse("52.5 31.20 N, 13 24.30 E", out GeographicDdm ddm);

            Assert.False(result);
        }

        #endregion

        #region Round-Trip Tests

        [Fact]
        public void Parse_ToString_RoundTrip()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = 52.52 * Math.PI / 180.0,
                LongitudeDecimalRadians = 13.405 * Math.PI / 180.0,
                AltitudeMetres = 100.5,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            string formatted = ddm.ToString();
            GeographicDdm parsed = GeographicDdm.Parse(formatted);
            string reparsed = parsed.ToString();

            Assert.Equal(formatted, reparsed);
        }

        [Fact]
        public void Parse_ToString_RoundTrip_NegativeValues()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = -52.52 * Math.PI / 180.0,
                LongitudeDecimalRadians = -13.405 * Math.PI / 180.0,
                AltitudeMetres = 100.5,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            string formatted = ddm.ToString();
            GeographicDdm parsed = GeographicDdm.Parse(formatted);
            string reparsed = parsed.ToString();

            Assert.Equal(formatted, reparsed);
        }

        [Fact]
        public void Parse_ToString_RoundTrip_ZeroValues()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = 0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            string formatted = ddm.ToString();
            GeographicDdm parsed = GeographicDdm.Parse(formatted);
            string reparsed = parsed.ToString();

            Assert.Equal(formatted, reparsed);
        }

        [Fact]
        public void Parse_ToString_RoundTrip_NorthPole()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = 90 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            string formatted = ddm.ToString();
            GeographicDdm parsed = GeographicDdm.Parse(formatted);
            string reparsed = parsed.ToString();

            Assert.Equal(formatted, reparsed);
        }

        [Fact]
        public void Parse_ToString_RoundTrip_SouthPole()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = -90 * Math.PI / 180.0,
                LongitudeDecimalRadians = 0 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            string formatted = ddm.ToString();
            GeographicDdm parsed = GeographicDdm.Parse(formatted);
            string reparsed = parsed.ToString();

            Assert.Equal(formatted, reparsed);
        }

        [Fact]
        public void Parse_ToString_RoundTrip_Antimeridian()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = 0 * Math.PI / 180.0,
                LongitudeDecimalRadians = 180 * Math.PI / 180.0,
                AltitudeMetres = 0,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            string formatted = ddm.ToString();
            GeographicDdm parsed = GeographicDdm.Parse(formatted);
            string reparsed = parsed.ToString();

            Assert.Equal(formatted, reparsed);
        }

        [Fact]
        public void Parse_ToString_RoundTrip_Singapore()
        {
            GeographicCoordinateSystem source = new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = 1.309432 * Math.PI / 180.0,
                LongitudeDecimalRadians = 103.780349 * Math.PI / 180.0,
                AltitudeMetres = 25.0,
            };

            GeographicDdm ddm = GeographicConverter.ToDdm(source);
            string formatted = ddm.ToString();
            GeographicDdm parsed = GeographicDdm.Parse(formatted);
            string reparsed = parsed.ToString();

            Assert.Equal(formatted, reparsed);
        }

        [Fact]
        public void Parse_ToString_PrecisionAwareRoundTrip()
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

            string formatted = ddm.ToString(6);
            GeographicDdm parsed = GeographicDdm.Parse(formatted);
            string reparsed = parsed.ToString(6);

            Assert.Equal(formatted, reparsed);
        }

        #endregion

        #region Clamping Tests

        [Fact]
        public void Parse_LatitudeDegreesExceeds90_RejectsValue()
        {
            string input = "95° 10.00' N, 13° 24.30' E";

            Assert.False(GeographicDdm.TryParse(input, out _));
        }

        [Fact]
        public void Parse_LongitudeDegreesExceeds180_RejectsValue()
        {
            string input = "52° 31.20' N, 185° 10.00' E";

            Assert.False(GeographicDdm.TryParse(input, out _));
        }

        [Fact]
        public void Parse_LatitudeMinutesEquals60_ClawsToNextDegree()
        {
            string input = "52° 60.00' N, 13° 24.30' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(53, result.LatitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LatitudeMinutes, POSITIONAL_PRECISION);
        }

        [Fact]
        public void Parse_LatitudeMinutesExceeds60_RejectsValue()
        {
            string input = "52° 61.50' N, 13° 24.30' E";

            Assert.False(GeographicDdm.TryParse(input, out _));
        }

        [Fact]
        public void Parse_LongitudeMinutesEquals60_ClawsToNextDegree()
        {
            string input = "52° 31.20' N, 13° 60.00' E";

            GeographicDdm result = GeographicDdm.Parse(input);

            Assert.Equal(14, result.LongitudeDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeMinutes, POSITIONAL_PRECISION);
        }

        #endregion

        #region Internal Helper Tests — TryParseDdmCoordinate

        [Fact]
        public void TryParseDdmCoordinate_SymbolDelimited_NorthEast_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52° 31.20' N", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(52, degrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, minutes, POSITIONAL_PRECISION);
            Assert.Equal('N', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_SymbolDelimited_South_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("33° 51.67' S", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(33, degrees, POSITIONAL_PRECISION);
            Assert.Equal(51.67, minutes, POSITIONAL_PRECISION);
            Assert.Equal('S', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_SymbolDelimitedLowercaseDirection_ReturnsTrueWithUppercase()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52° 31.20' n", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(52, degrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, minutes, POSITIONAL_PRECISION);
            Assert.Equal('N', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_SpaceDelimited_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52 31.20 N", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(52, degrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, minutes, POSITIONAL_PRECISION);
            Assert.Equal('N', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_SpaceDelimitedTabDelimited_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52\t31.20\tN", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(52, degrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, minutes, POSITIONAL_PRECISION);
            Assert.Equal('N', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_EmptyPart_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.False(result);
        }

        [Fact]
        public void TryParseDdmCoordinate_WhitespacePart_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("   ", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.False(result);
        }

        [Fact]
        public void TryParseDdmCoordinate_InvalidDirection_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52° 31.20' X", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.False(result);
        }

        [Fact]
        public void TryParseDdmCoordinate_NonNumericDegrees_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("abc° 31.20' N", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.False(result);
        }

        [Fact]
        public void TryParseDdmCoordinate_NonNumericMinutes_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52° xyz' N", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.False(result);
        }

        [Fact]
        public void TryParseDdmCoordinate_SymbolDelimitedSinglePart_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52° 31.20' N", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(52, degrees, POSITIONAL_PRECISION);
            Assert.Equal(31.2, minutes, POSITIONAL_PRECISION);
            Assert.Equal('N', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_SpaceDelimitedNonIntegerDegrees_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52.5 31.20 N", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.False(result);
        }

        [Fact]
        public void TryParseDdmCoordinate_SpaceDelimitedWrongArity_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52 31", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.False(result);
        }

        [Fact]
        public void TryParseDdmCoordinate_SpaceDelimitedMissingDirection_ReturnsFalse()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("52 31.20", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.False(result);
        }

        [Fact]
        public void TryParseDdmCoordinate_ZeroValues_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("0° 0.000000' N", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(0, degrees, POSITIONAL_PRECISION);
            Assert.Equal(0, minutes, POSITIONAL_PRECISION);
            Assert.Equal('N', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_NorthPole_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("90° 0.000000' N", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(90, degrees, POSITIONAL_PRECISION);
            Assert.Equal(0, minutes, POSITIONAL_PRECISION);
            Assert.Equal('N', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_SouthPole_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("90° 0.000000' S", isLatitude: true, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(90, degrees, POSITIONAL_PRECISION);
            Assert.Equal(0, minutes, POSITIONAL_PRECISION);
            Assert.Equal('S', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_Antimeridian_ReturnsTrue()
        {
            bool result = GeographicDdm.TryParseDdmCoordinate("180° 0.000000' W", isLatitude: false, out double degrees, out double minutes, out char direction);

            Assert.True(result);
            Assert.Equal(180, degrees, POSITIONAL_PRECISION);
            Assert.Equal(0, minutes, POSITIONAL_PRECISION);
            Assert.Equal('W', direction);
        }

        [Fact]
        public void TryParseDdmCoordinate_SpaceDelimitedAllDirections_ReturnsTrue()
        {
            Assert.True(GeographicDdm.TryParseDdmCoordinate("52 31.20 N", isLatitude: true, out _, out _, out var d1));
            Assert.Equal('N', d1);

            Assert.True(GeographicDdm.TryParseDdmCoordinate("52 31.20 S", isLatitude: true, out _, out _, out var d2));
            Assert.Equal('S', d2);

            Assert.True(GeographicDdm.TryParseDdmCoordinate("52 31.20 E", isLatitude: false, out _, out _, out var d3));
            Assert.Equal('E', d3);

            Assert.True(GeographicDdm.TryParseDdmCoordinate("52 31.20 W", isLatitude: false, out _, out _, out var d4));
            Assert.Equal('W', d4);
        }

        [Fact]
        public void TryParseDdmCoordinate_SymbolDelimitedAllDirections_ReturnsTrue()
        {
            Assert.True(GeographicDdm.TryParseDdmCoordinate("52° 31.20' N", isLatitude: true, out _, out _, out var d1));
            Assert.Equal('N', d1);

            Assert.True(GeographicDdm.TryParseDdmCoordinate("52° 31.20' S", isLatitude: true, out _, out _, out var d2));
            Assert.Equal('S', d2);

            Assert.True(GeographicDdm.TryParseDdmCoordinate("52° 31.20' E", isLatitude: false, out _, out _, out var d3));
            Assert.Equal('E', d3);

            Assert.True(GeographicDdm.TryParseDdmCoordinate("52° 31.20' W", isLatitude: false, out _, out _, out var d4));
            Assert.Equal('W', d4);
        }

        #endregion

        #region GeographicCoordinateSystem Direction Validation Tests

        [Theory]
        [InlineData('N', true)]
        [InlineData('n', true)]
        [InlineData('S', true)]
        [InlineData('s', true)]
        [InlineData('E', true)]
        [InlineData('e', true)]
        [InlineData('W', true)]
        [InlineData('w', true)]
        public void IsValidDirection_ValidDirection_ReturnsTrue(char c, bool expected)
        {
            bool result = GeographicCoordinateSystem.IsValidDirection(c);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData('X')]
        [InlineData('A')]
        [InlineData('0')]
        [InlineData(' ')]
        [InlineData('\0')]
        public void IsValidDirection_InvalidDirection_ReturnsFalse(char c)
        {
            Assert.False(GeographicCoordinateSystem.IsValidDirection(c));
        }

        #endregion

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
