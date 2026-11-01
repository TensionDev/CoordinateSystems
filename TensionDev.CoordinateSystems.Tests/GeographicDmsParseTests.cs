using System;
using System.Globalization;
using System.Threading;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeographicDmsParseTests
    {
        [Fact]
        public void Parse_ValidDmsWithMetres_ReturnsCoordinate()
        {
            GeographicDms actual = GeographicDms.Parse("52° 31' 12.125\" S, 13° 24' 18.25\" W Altitude: 100.500 m");

            Assert.Equal(52, actual.LatitudeDegrees);
            Assert.Equal(31, actual.LatitudeMinutes);
            Assert.Equal(12.125, actual.LatitudeSeconds, 6);
            Assert.Equal('S', actual.LatitudeDirection);
            Assert.Equal(13, actual.LongitudeDegrees);
            Assert.Equal(24, actual.LongitudeMinutes);
            Assert.Equal(18.25, actual.LongitudeSeconds, 6);
            Assert.Equal('W', actual.LongitudeDirection);
            Assert.Equal(100.5, actual.AltitudeMetres, 3);
        }

        [Fact]
        public void Parse_FeetAltitude_ConvertsToMetres()
        {
            GeographicDms actual = GeographicDms.Parse("52° 31' 12\" N, 13° 24' 18\" E Altitude: 328.084 ft");

            Assert.Equal(100.0, actual.AltitudeMetres, 3);
            Assert.Equal(328.084, actual.AltitudeFeet, 3);
        }

        [Fact]
        public void AltitudeFeet_SetAndGet_UsesMetresAsBackingValue()
        {
            GeographicDms actual = new GeographicDms { AltitudeFeet = 328.084 };

            Assert.Equal(100.0, actual.AltitudeMetres, 3);
            Assert.Equal(328.084, actual.AltitudeFeet, 3);
        }

        [Fact]
        public void TryParse_AltitudeOmitted_DefaultsToZero()
        {
            Assert.True(GeographicDms.TryParse("52° 31' 12\" N, 13° 24' 18\" E", out GeographicDms actual));

            Assert.Equal(0.0, actual.AltitudeMetres);
        }

        [Fact]
        public void TryParse_LowercaseDirectionsAndFlexibleWhitespace_NormalizesDirections()
        {
            Assert.True(GeographicDms.TryParse("  52°31'12.5\"s , 13°24'18.25\"w  ", out GeographicDms actual));

            Assert.Equal('S', actual.LatitudeDirection);
            Assert.Equal('W', actual.LongitudeDirection);
            Assert.Equal(12.5, actual.LatitudeSeconds, 6);
            Assert.Equal(18.25, actual.LongitudeSeconds, 6);
        }

        [Theory]
        [InlineData("90° 0' 0\" N, 180° 0' 0\" E")]
        [InlineData("90° 0' 0\" S, 180° 0' 0\" W")]
        public void TryParse_InclusiveGeographicBoundaries_ReturnsTrue(string input)
        {
            Assert.True(GeographicDms.TryParse(input, out _));
        }

        [Fact]
        public void Parse_MinutesAndSecondsAtSixty_NormalizesCarry()
        {
            GeographicDms actual = GeographicDms.Parse("52° 59' 60\" N, 13° 60' 0\" E");

            Assert.Equal(53, actual.LatitudeDegrees);
            Assert.Equal(0, actual.LatitudeMinutes);
            Assert.Equal(0, actual.LatitudeSeconds);
            Assert.Equal(14, actual.LongitudeDegrees);
            Assert.Equal(0, actual.LongitudeMinutes);
            Assert.Equal(0, actual.LongitudeSeconds);
        }

        [Fact]
        public void Parse_LongitudeSecondsAtSixty_NormalizesCarry()
        {
            GeographicDms actual = GeographicDms.Parse("52° 30' 0\" N, 13° 59' 60\" E");

            Assert.Equal(14, actual.LongitudeDegrees);
            Assert.Equal(0, actual.LongitudeMinutes);
            Assert.Equal(0, actual.LongitudeSeconds);
        }

        [Fact]
        public void TryParse_SpaceDelimitedDmsAndTabs_ReturnsParsedCoordinates()
        {
            Assert.True(GeographicDms.TryParse("52 31 12.5 s, 13\t24\t18.25\tw", out GeographicDms actual));

            Assert.Equal(52, actual.LatitudeDegrees);
            Assert.Equal(31, actual.LatitudeMinutes);
            Assert.Equal(12.5, actual.LatitudeSeconds, 6);
            Assert.Equal('S', actual.LatitudeDirection);
            Assert.Equal(13, actual.LongitudeDegrees);
            Assert.Equal(24, actual.LongitudeMinutes);
            Assert.Equal(18.25, actual.LongitudeSeconds, 6);
            Assert.Equal('W', actual.LongitudeDirection);
        }

        [Fact]
        public void TryParseDmsCoordinate_EmptyPart_ReturnsFalseAndClearsOutputs()
        {
            bool result = GeographicDms.TryParseDmsCoordinate(
                "", isLatitude: true, out double degrees, out double minutes, out double seconds, out char direction);

            Assert.False(result);
            Assert.Equal(0, degrees);
            Assert.Equal(0, minutes);
            Assert.Equal(0, seconds);
            Assert.Equal('\0', direction);
        }

        [Theory]
        [InlineData("90° 0' 0.001\" N, 0° 0' 0\" E")]
        [InlineData("90° 0' 1\" N, 0° 0' 0\" E")]
        [InlineData("0° 0' 0\" N, 180° 0' 0.001\" E")]
        [InlineData("0° 0' 0\" N, 181° 0' 0\" E")]
        [InlineData("52° 61' 0\" N, 13° 0' 0\" E")]
        [InlineData("52° 0' 61\" N, 13° 0' 0\" E")]
        [InlineData("abc° 31' 12\" N, 13° 0' 0\" E")]
        [InlineData("91° 0' 0\" N, 13° 0' 0\" E")]
        [InlineData("-1° 0' 0\" N, 13° 0' 0\" E")]
        [InlineData("52° NaN' 0\" N, 13° 0' 0\" E")]
        [InlineData("52° 0' NaN\" N, 13° 0' 0\" E")]
        [InlineData("52° 31.2' N, 13° 24.3' E")]
        [InlineData("52.52, 13.405")]
        [InlineData("52° 31' 12\" N, 181° 0' 0\" E")]
        [InlineData("52° 31' 12\" N, 13° nope' 0\" E")]
        [InlineData("52° 31' 12\" N, 13° 24' nope\" E")]
        [InlineData("52° 31' 12\" N, 13° 24.3' E")]
        [InlineData("52° 31' 12\" X, 13° 24' 18\" E")]
        [InlineData("52° 31' 12 E, 13° 24' 18\" E")]
        [InlineData("52° 31' 12\" N, 13° 24' 18\" N")]
        [InlineData("malformed input without coordinate separator")]
        [InlineData("52 31 12.5 N, 13.5 24 18 E")]
        [InlineData("52.5 31 12.5 N, 13 24 18 E")]
        [InlineData("52 31.5 12.5 N, 13 24 18 E")]
        [InlineData("52 31 nope N, 13 24 18 E")]
        [InlineData("52 31 12 X, 13 24 18 E")]
        [InlineData("52 31 12 NN, 13 24 18 E")]
        [InlineData("52 31 12 N, 13 24 18 N")]
        [InlineData("91 0 0 N, 13 24 18 E")]
        [InlineData("52 31 12 N, 181 0 0 E")]
        [InlineData("52° 31' 12\" N, 13° 24' 18\" E Altitude: invalid m")]
        public void TryParse_InvalidOrDifferentRepresentation_ReturnsFalse(string input)
        {
            Assert.False(GeographicDms.TryParse(input, out GeographicDms actual));
            Assert.Null(actual);
        }

        [Fact]
        public void Parse_NullAndInvalidInput_ThrowsExpectedExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => GeographicDms.Parse(null));
            Assert.Throws<FormatException>(() => GeographicDms.Parse("   "));
            Assert.Throws<FormatException>(() => GeographicDms.Parse("52° 31' 12\" N, 13° 24' 18\""));
        }

        [Fact]
        public void TryParse_NullAndWhitespace_ReturnsFalseWithNullResult()
        {
            Assert.False(GeographicDms.TryParse(null, out GeographicDms nullResult));
            Assert.Null(nullResult);
            Assert.False(GeographicDms.TryParse("  ", out GeographicDms whitespaceResult));
            Assert.Null(whitespaceResult);
        }

        [Fact]
        public void TryParse_UsesInvariantCulture()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo originalUiCulture = Thread.CurrentThread.CurrentUICulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");

                Assert.True(GeographicDms.TryParse("52° 31' 12.5\" N, 13° 24' 18.25\" E Altitude: 100.5 m", out GeographicDms actual));
                Assert.Equal(12.5, actual.LatitudeSeconds, 6);
                Assert.Equal(18.25, actual.LongitudeSeconds, 6);
                Assert.Equal(100.5, actual.AltitudeMetres, 3);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
                Thread.CurrentThread.CurrentUICulture = originalUiCulture;
            }
        }

        [Fact]
        public void Parse_ToStringRoundTrip_PreservesSemanticValues()
        {
            GeographicDms source = new GeographicDms
            {
                LatitudeDegrees = 1,
                LatitudeMinutes = 18,
                LatitudeSeconds = 33.123456,
                LatitudeDirection = 'S',
                LongitudeDegrees = 103,
                LongitudeMinutes = 46,
                LongitudeSeconds = 49.654321,
                LongitudeDirection = 'E',
                AltitudeMetres = 25.125,
            };

            GeographicDms actual = GeographicDms.Parse(source.ToString());

            Assert.Equal(source.LatitudeDegrees, actual.LatitudeDegrees);
            Assert.Equal(source.LatitudeMinutes, actual.LatitudeMinutes);
            Assert.Equal(source.LatitudeSeconds, actual.LatitudeSeconds, 6);
            Assert.Equal(source.LatitudeDirection, actual.LatitudeDirection);
            Assert.Equal(source.LongitudeDegrees, actual.LongitudeDegrees);
            Assert.Equal(source.LongitudeMinutes, actual.LongitudeMinutes);
            Assert.Equal(source.LongitudeSeconds, actual.LongitudeSeconds, 6);
            Assert.Equal(source.LongitudeDirection, actual.LongitudeDirection);
            Assert.Equal(source.AltitudeMetres, actual.AltitudeMetres, 3);
        }
    }
}
