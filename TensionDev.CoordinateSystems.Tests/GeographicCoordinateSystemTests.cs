// SPDX-License-Identifier: Apache-2.0
//
//   Copyright 2021 - 2026 TensionDev <TensionDev@outlook.com>
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeographicCoordinateSystemParseTests : IDisposable
    {
        private bool disposedValue;

        private const Int32 POSITIONAL_PRECISION = 5;

        public GeographicCoordinateSystemParseTests()
        {
        }

        #region Parse - Decimal Degrees Format

        [Fact]
        public void TestParse_DecimalDegrees_Metres()
        {
            string input = "52.520000, 13.405000";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.520000, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405000, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DecimalDegrees_Metres_WithAltitude()
        {
            string input = "52.520000, 13.405000 Altitude: 100.000 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.520000, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405000, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DecimalDegrees_Feet()
        {
            string input = "52.520000, 13.405000 Altitude: 328.084 ft";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.520000, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405000, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DecimalDegrees_Negative()
        {
            string input = "-33.868820, 151.209290";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(-33.868820, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(151.209290, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_BareSmallNumericPair_IsDecimalDegrees()
        {
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse("0.5, 0.25");

            Assert.Equal(0.5, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0.25, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DecimalDegrees_Sydney()
        {
            string input = "-33.868820, 151.209290 Altitude: 500.500 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(-33.868820, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(151.209290, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(500.500, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        #endregion

        #region Parse - DDM Format with Symbols

        [Fact]
        public void TestParse_DDM_SymbolDelimited_Metres()
        {
            string input = "52° 31.20' N, 13° 24.30' E";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            // Expected: 52 + 31.20/60 = 52.52, 13 + 24.30/60 = 13.405
            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DDM_SymbolDelimited_WithAltitude()
        {
            string input = "52° 31.20' N, 13° 24.30' E Altitude: 100.000 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DDM_SymbolDelimited_Feet()
        {
            string input = "52° 31.20' N, 13° 24.30' E Altitude: 328.084 ft";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DDM_SymbolDelimited_Negative()
        {
            string input = "33° 52' 08\" S, 151° 12' 33\" E";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            // Expected: -33.868889, 151.209167 (with direction applied)
            Assert.Equal(-33.868889, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(151.209167, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        #endregion

        #region Parse - DMS Format with Symbols

        [Fact]
        public void TestParse_DMS_SymbolDelimited_Metres()
        {
            string input = "52° 31' 12.00\" N, 13° 24' 18.00\" E";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            // Expected: 52 + 31/60 + 12/3600 = 52.52, 13 + 24/60 + 18/3600 = 13.405
            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DMS_SymbolDelimited_WithAltitude()
        {
            string input = "52° 31' 12.00\" N, 13° 24' 18.00\" E Altitude: 100.000 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DMS_SymbolDelimited_Feet()
        {
            string input = "52° 31' 12.00\" N, 13° 24' 18.00\" E Altitude: 328.084 ft";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        #endregion

        #region Parse - Space-Delimited DDM Format

        [Fact]
        public void TestParse_DDM_SpaceDelimited_Metres()
        {
            string input = "52 31.20 N, 13 24.30 E";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            // Expected: 52 + 31.20/60 = 52.52, 13 + 24.30/60 = 13.405
            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DDM_SpaceDelimited_WithAltitude()
        {
            string input = "52 31.20 N, 13 24.30 E Altitude: 100.000 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DDM_SpaceDelimited_Feet()
        {
            string input = "52 31.20 N, 13 24.30 E Altitude: 328.084 ft";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DDM_SpaceDelimited_Negative()
        {
            string input = "33 52 08 S, 151 12 33 E";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            // Expected: -33.868889, 151.209167 (with direction applied)
            Assert.Equal(-33.868889, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(151.209167, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        #endregion

        #region Parse - Radians Format

        [Fact]
        public void TestParse_Radians_Metres()
        {
            // Berlin: 52.52° N, 13.405° E converted to radians
            double latRad = 52.52 * Math.PI / 180.0;
            double lonRad = 13.405 * Math.PI / 180.0;

            string input = $"{latRad} rad, {lonRad} rad";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(latRad, result.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(lonRad, result.LongitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(0, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Radians_WithAltitude_Metres()
        {
            double latRad = 52.52 * Math.PI / 180.0;
            double lonRad = 13.405 * Math.PI / 180.0;

            string input = $"{latRad} rad, {lonRad} rad Altitude: 100.000 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(latRad, result.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(lonRad, result.LongitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Radians_WithAltitude_Feet()
        {
            double latRad = 52.52 * Math.PI / 180.0;
            double lonRad = 13.405 * Math.PI / 180.0;

            string input = $"{latRad} rad, {lonRad} rad Altitude: 328.084 ft";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(latRad, result.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(lonRad, result.LongitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        #endregion

        #region Parse - Mixed Case Direction Indicators

        [Fact]
        public void TestParse_DDM_LowercaseDirection()
        {
            string input = "52° 31.20' n, 13° 24.30' e";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DDM_MixedCaseDirection()
        {
            string input = "52° 31.20' n, 13° 24.30' e";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        #endregion

        #region Round-Trip Testing

        [Fact]
        public void TestParse_ToString_DecimalDegrees_RoundTrip()
        {
            string input = "52.520000, 13.405000";
            GeographicCoordinateSystem parsed = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.520000, parsed.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405000, parsed.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_ToString_DDM_RoundTrip()
        {
            string originalInput = "52° 31.20' N, 13° 24.30' E";
            GeographicCoordinateSystem parsed = GeographicCoordinateSystem.Parse(originalInput);

            Assert.Equal(52.52, parsed.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, parsed.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_ToString_DMS_RoundTrip()
        {
            string originalInput = "52° 31' 12.00\" N, 13° 24' 18.00\" E";
            GeographicCoordinateSystem parsed = GeographicCoordinateSystem.Parse(originalInput);

            Assert.Equal(52.52, parsed.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, parsed.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_ToString_Radians_RoundTrip()
        {
            double latRad = 52.52 * Math.PI / 180.0;
            double lonRad = 13.405 * Math.PI / 180.0;

            string originalInput = $"{latRad} rad, {lonRad} rad";
            GeographicCoordinateSystem parsed = GeographicCoordinateSystem.Parse(originalInput);
            string output = parsed.ToString(AngularFormat.Radians, DistanceUnit.Metres, 6, 3);
            GeographicCoordinateSystem roundTripped = GeographicCoordinateSystem.Parse(output);

            Assert.Equal(latRad, parsed.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(lonRad, parsed.LongitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(parsed.LatitudeDecimalRadians, roundTripped.LatitudeDecimalRadians, POSITIONAL_PRECISION);
            Assert.Equal(parsed.LongitudeDecimalRadians, roundTripped.LongitudeDecimalRadians, POSITIONAL_PRECISION);
        }

        #endregion

        #region Parse - Boundary Conditions

        [Fact]
        public void TestParse_Latitude_90()
        {
            string input = "90.000000, 0";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(90, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Latitude_Minus90()
        {
            string input = "-90.000000, 0";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(-90, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Longitude_180()
        {
            string input = "0, 180";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(180, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Longitude_Minus180()
        {
            string input = "0, -180";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(-180, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_ZeroLatitudeZeroLongitude()
        {
            string input = "0.000000, 0.000000";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(0, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        #endregion

        #region Parse - Error Cases (TryParse)

        [Fact]
        public void TestTryParse_Null_ReturnsFalse()
        {
            bool result = GeographicCoordinateSystem.TryParse(null, out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_EmptyString_ReturnsFalse()
        {
            bool result = GeographicCoordinateSystem.TryParse("", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_WhitespaceOnly_ReturnsFalse()
        {
            bool result = GeographicCoordinateSystem.TryParse("   \t\n  ", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_MissingComma_ReturnsFalse()
        {
            bool result = GeographicCoordinateSystem.TryParse("52.520000 13.405000", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_InvalidLatitude_ReturnsFalse()
        {
            bool result = GeographicCoordinateSystem.TryParse("100.000000, 13.405000", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_InvalidLongitude_ReturnsFalse()
        {
            bool result = GeographicCoordinateSystem.TryParse("52.520000, 200.000000", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_InvalidLatitudeDDM_ReturnsFalse()
        {
            bool result = GeographicCoordinateSystem.TryParse("100° 30' N, 13° 24' E", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_MissingDirectionInDDM_ReturnsFalse()
        {
            bool result = GeographicCoordinateSystem.TryParse("52° 31.20, 13° 24.30 E", out _);
            Assert.False(result);
        }

        #endregion

        #region Parse - Error Cases (Throws)

        [Fact]
        public void TestParse_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => GeographicCoordinateSystem.Parse(null));
        }

        [Fact]
        public void TestParse_EmptyString_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicCoordinateSystem.Parse(""));
        }

        [Fact]
        public void TestParse_Whitespace_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicCoordinateSystem.Parse("   "));
        }

        [Fact]
        public void TestParse_Malformed_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeographicCoordinateSystem.Parse("52.520000 13.405000"));
        }

        #endregion

        #region Parse - Altitude Edge Cases

        [Fact]
        public void TestParse_AltitudeZero_Metres()
        {
            string input = "52.520000, 13.405000 Altitude: 0.000 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_AltitudeZero_Feet()
        {
            string input = "52.520000, 13.405000 Altitude: 0.000 ft";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Altitude_LargeValue()
        {
            string input = "52.520000, 13.405000 Altitude: 99999.999 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(99999.999, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Altitude_SmallValue()
        {
            string input = "52.520000, 13.405000 Altitude: 0.001 m";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(0.001, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Altitude_IncFeet_WithCapital()
        {
            string input = "52.520000, 13.405000 Altitude: 328.084 FT";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(100.000, result.AltitudeMetres, POSITIONAL_PRECISION);
        }

        #endregion

        #region Parse - Whitespace Tolerance

        [Fact]
        public void TestParse_AdditionalWhitespace()
        {
            string input = " 52.520000 , 13.405000 ";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_DDM_Whitespace_AroundSymbols()
        {
            string input = "52°   31.20'   N , 13°   24.30'   E";
            GeographicCoordinateSystem result = GeographicCoordinateSystem.Parse(input);

            Assert.Equal(52.52, result.LatitudeDecimalDegrees, POSITIONAL_PRECISION);
            Assert.Equal(13.405, result.LongitudeDecimalDegrees, POSITIONAL_PRECISION);
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

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
