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
    public class GeographicDdTests
    {
        [Fact]
        public void ParseDecimalDegreesAndAltitude()
        {
            GeographicDd actual = GeographicDd.Parse("-33.868820, 151.209290 Altitude: 125.500 m");

            Assert.Equal(-33.868820, actual.LatitudeDecimalDegrees, 6);
            Assert.Equal(151.209290, actual.LongitudeDecimalDegrees, 6);
            Assert.Equal(125.500, actual.AltitudeMetres, 3);
        }

        [Fact]
        public void ParseFeetAltitudeIntoMetres()
        {
            GeographicDd actual = GeographicDd.Parse("52.520000, 13.405000 Altitude: 328.084 ft");

            Assert.Equal(100.0, actual.AltitudeMetres, 3);
            Assert.Equal(328.084, actual.AltitudeFeet, 3);
        }

        [Fact]
        public void ParseDefaultsAltitudeToZero()
        {
            GeographicDd actual = GeographicDd.Parse("52.520000, 13.405000");

            Assert.Equal(0.0, actual.AltitudeMetres);
        }

        [Theory]
        [InlineData("90, 180")]
        [InlineData("-90, -180")]
        public void ParseAcceptsInclusiveGeographicBoundaries(string input)
        {
            Assert.True(GeographicDd.TryParse(input, out _));
        }

        [Theory]
        [InlineData("90.000001, 0")]
        [InlineData("0, -180.000001")]
        [InlineData("NaN, 0")]
        [InlineData("52° 31.2' N, 13° 24.3' E")]
        [InlineData("0.9163 rad, 0.2338 rad")]
        public void TryParseRejectsInvalidOrDifferentRepresentations(string input)
        {
            Assert.False(GeographicDd.TryParse(input, out GeographicDd result));
            Assert.Null(result);
        }

        [Fact]
        public void ParseThrowsExpectedExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => GeographicDd.Parse(null));
            Assert.Throws<FormatException>(() => GeographicDd.Parse("91, 0"));
        }

        [Fact]
        public void ToStringCanBeParsedBack()
        {
            GeographicDd source = new GeographicDd
            {
                LatitudeDecimalDegrees = 1.309432,
                LongitudeDecimalDegrees = 103.780349,
                AltitudeMetres = 25.125,
            };

            GeographicDd actual = GeographicDd.Parse(source.ToString());

            Assert.Equal(source.LatitudeDecimalDegrees, actual.LatitudeDecimalDegrees, 6);
            Assert.Equal(source.LongitudeDecimalDegrees, actual.LongitudeDecimalDegrees, 6);
            Assert.Equal(source.AltitudeMetres, actual.AltitudeMetres, 3);
        }
    }
}
