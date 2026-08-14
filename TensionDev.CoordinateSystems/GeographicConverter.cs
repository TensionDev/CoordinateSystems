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
using System.Collections;

namespace TensionDev.CoordinateSystems
{
    /// <summary>
    /// Converts supported coordinate representations to geographic coordinates.
    /// </summary>
    public static class GeographicConverter
    {
        /// <summary>
        /// Converts geocentric coordinates to geographic coordinates using WGS 84 ellipsoid assumptions.
        /// </summary>
        /// <param name="source">The geocentric coordinates to convert.</param>
        /// <returns>The converted geographic coordinates.</returns>
        public static GeographicCoordinateSystem From(GeocentricCoordinateSystem source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double longitude = Math.Atan2(source.Y, source.X);
            double distanceFromPolarAxis = Math.Sqrt(source.X * source.X + source.Y * source.Y);

            if (distanceFromPolarAxis == 0.0)
            {
                return FromPolarAxis(source.Z, longitude);
            }

            double theta = Math.Atan2(
                source.Z * Wgs84.SemiMajorAxisMetres,
                distanceFromPolarAxis * Wgs84.SemiMinorAxisMetres);

            double sinTheta = Math.Sin(theta);
            double cosTheta = Math.Cos(theta);

            double latitude = Math.Atan2(
                source.Z + Wgs84.SecondEccentricitySquared * Wgs84.SemiMinorAxisMetres * sinTheta * sinTheta * sinTheta,
                distanceFromPolarAxis - Wgs84.FirstEccentricitySquared * Wgs84.SemiMajorAxisMetres * cosTheta * cosTheta * cosTheta);

            double sinLatitude = Math.Sin(latitude);
            double primeVerticalRadius = Wgs84.SemiMajorAxisMetres
                / Math.Sqrt(1.0 - Wgs84.FirstEccentricitySquared * sinLatitude * sinLatitude);

            double altitude = distanceFromPolarAxis / Math.Cos(latitude) - primeVerticalRadius;

            return new GeographicCoordinateSystem
            {
                LatitudeDecimalRadians = latitude,
                LongitudeDecimalRadians = longitude,
                AltitudeMetres = altitude,
            };
        }

        /// <summary>
        /// Converts a Geohash to geographic coordinates.
        /// </summary>
        /// <param name="source">The Geohash to convert.</param>
        /// <returns>The converted geographic coordinates.</returns>
        public static GeographicCoordinateSystem From(Geohash source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (BitArray bitNotation, UInt32 length) = GeohashBase32.Decode(source.Hash);

            return BitNotationDivisions(bitNotation, length);
        }

        private static GeographicCoordinateSystem FromPolarAxis(double z, double longitude)
        {
            if (z == 0.0)
            {
                return new GeographicCoordinateSystem
                {
                    LatitudeDecimalDegrees = 0.0,
                    LongitudeDecimalRadians = longitude,
                    AltitudeMetres = -Wgs84.SemiMajorAxisMetres,
                };
            }

            return new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = z > 0.0 ? 90.0 : -90.0,
                LongitudeDecimalRadians = longitude,
                AltitudeMetres = Math.Abs(z) - Wgs84.SemiMinorAxisMetres,
            };
        }

        private static GeographicCoordinateSystem BitNotationDivisions(BitArray bitNotation, UInt32 length)
        {
            if (length % GeohashBase32.BitsPerCharacter != 0)
            {
                throw new ArgumentException($"Parameter length \"{length}\" is not a multiple of 5!", nameof(length));
            }

            if (bitNotation.Count % GeohashBase32.BitsPerCharacter != 0)
            {
                throw new ArgumentException($"Parameter bitNotation length is not a multiple of 5!", nameof(bitNotation));
            }

            Double latitudeMin = -90;
            Double latitudeMax = 90;
            Double longitudeMin = -180;
            Double longitudeMax = 180;

            for (Int32 i = 0; i < length; ++i)
            {
                Int32 index = (Int32)(length - i - 1);
                if (i % 2 == 0)
                {
                    (longitudeMin, longitudeMax) = BitNotationDivision(bitNotation[index], longitudeMin, longitudeMax);
                }
                else
                {
                    (latitudeMin, latitudeMax) = BitNotationDivision(bitNotation[index], latitudeMin, latitudeMax);
                }
            }

            return new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = (latitudeMin + latitudeMax) / 2.0,
                LongitudeDecimalDegrees = (longitudeMin + longitudeMax) / 2.0,
                AltitudeMetres = 0
            };
        }

        private static (Double min, Double max) BitNotationDivision(Boolean bit, Double min, Double max)
        {
            Double mean = (min + max) / 2.0;

            if (bit)
            {
                return (mean, max);
            }
            else
            {
                return (min, mean);
            }
        }
    }
}
