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

namespace TensionDev.CoordinateSystems
{
    /// <summary>
    /// Converts supported coordinate representations to Geohash coordinates.
    /// </summary>
    public static class GeohashConverter
    {
        /// <summary>
        /// Converts geographic coordinates to a Geohash.
        /// </summary>
        /// <param name="source">The geographic coordinates to convert.</param>
        /// <param name="length">The output Geohash length.</param>
        /// <returns>The converted Geohash.</returns>
        public static Geohash From(GeographicCoordinateSystem source, Int32 length)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (length <= 0 || length > GeohashBase32.MaximumHashLength)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"GeoHash of length {length} is not supported!");
            }

            Int32 bitLength = length * GeohashBase32.BitsPerCharacter;
            UInt64 bitNotation = 0;
            Boolean bit;

            Double latitudeMin = -90;
            Double latitudeMax = 90;
            Double longitudeMin = -180;
            Double longitudeMax = 180;

            for (Int32 i = 0; i < bitLength; ++i)
            {
                if (i % 2 == 0)
                {
                    (longitudeMin, longitudeMax, bit) = BitNotationMultiplication(
                        longitudeMin,
                        longitudeMax,
                        source.LongitudeDecimalDegrees);
                }
                else
                {
                    (latitudeMin, latitudeMax, bit) = BitNotationMultiplication(
                        latitudeMin,
                        latitudeMax,
                        source.LatitudeDecimalDegrees);
                }

                bitNotation <<= 1;
                bitNotation += bit ? 1u : 0u;
            }

            String hash = GeohashBase32.Encode(bitNotation, length);

            return new Geohash(hash);
        }

        private static (Double min, Double max, Boolean bit) BitNotationMultiplication(Double min, Double max, Double value)
        {
            Double mean = (min + max) / 2.0;

            if (value >= mean)
            {
                return (mean, max, true);
            }
            else
            {
                return (min, mean, false);
            }
        }
    }
}
