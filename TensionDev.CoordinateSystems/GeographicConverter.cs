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

        /// <summary>
        /// Converts geographic coordinates to a degrees and decimal minutes (DDM) representation.
        /// </summary>
        /// <param name="source">The geographic coordinates to convert.</param>
        /// <returns>A <see cref="GeographicDdm"/> representing the same position in DDM.</returns>
        public static GeographicDdm ToDdm(GeographicCoordinateSystem source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (double latDegrees, double latMinutes, char latDirection) = Decompose(source.LatitudeDecimalDegrees, true);
            (double lonDegrees, double lonMinutes, char lonDirection) = Decompose(source.LongitudeDecimalDegrees, false);

            return new GeographicDdm
            {
                LatitudeDegrees = latDegrees,
                LatitudeMinutes = latMinutes,
                LatitudeDirection = latDirection,
                LongitudeDegrees = lonDegrees,
                LongitudeMinutes = lonMinutes,
                LongitudeDirection = lonDirection,
            };
        }

        /// <summary>
        /// Converts geographic coordinates to a degrees, minutes, and seconds (DMS) representation.
        /// </summary>
        /// <param name="source">The geographic coordinates to convert.</param>
        /// <returns>A <see cref="GeographicDms"/> representing the same position in DMS.</returns>
        public static GeographicDms ToDms(GeographicCoordinateSystem source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (double latDegrees, double latMinutes, double latSeconds, char latDirection) = DecomposeWithSeconds(source.LatitudeDecimalDegrees, true);
            (double lonDegrees, double lonMinutes, double lonSeconds, char lonDirection) = DecomposeWithSeconds(source.LongitudeDecimalDegrees, false);

            return new GeographicDms
            {
                LatitudeDegrees = latDegrees,
                LatitudeMinutes = latMinutes,
                LatitudeSeconds = latSeconds,
                LatitudeDirection = latDirection,
                LongitudeDegrees = lonDegrees,
                LongitudeMinutes = lonMinutes,
                LongitudeSeconds = lonSeconds,
                LongitudeDirection = lonDirection,
            };
        }

        /// <summary>
        /// Converts a degrees and decimal minutes (DDM) representation to geographic coordinates.
        /// </summary>
        /// <param name="source">The DDM representation to convert.</param>
        /// <returns>The converted geographic coordinates.</returns>
        public static GeographicCoordinateSystem FromDdm(GeographicDdm source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double latitudeDecimalDegrees = Recompose(source.LatitudeDegrees, source.LatitudeMinutes, source.LatitudeDirection, true);
            double longitudeDecimalDegrees = Recompose(source.LongitudeDegrees, source.LongitudeMinutes, source.LongitudeDirection, false);

            return new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = latitudeDecimalDegrees,
                LongitudeDecimalDegrees = longitudeDecimalDegrees,
            };
        }

        /// <summary>
        /// Converts a degrees, minutes, and seconds (DMS) representation to geographic coordinates.
        /// </summary>
        /// <param name="source">The DMS representation to convert.</param>
        /// <returns>The converted geographic coordinates.</returns>
        public static GeographicCoordinateSystem FromDms(GeographicDms source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double latitudeDecimalDegrees = RecomposeWithSeconds(source.LatitudeDegrees, source.LatitudeMinutes, source.LatitudeSeconds, source.LatitudeDirection, true);
            double longitudeDecimalDegrees = RecomposeWithSeconds(source.LongitudeDegrees, source.LongitudeMinutes, source.LongitudeSeconds, source.LongitudeDirection, false);

            return new GeographicCoordinateSystem
            {
                LatitudeDecimalDegrees = latitudeDecimalDegrees,
                LongitudeDecimalDegrees = longitudeDecimalDegrees,
            };
        }

        private static double Recompose(double degrees, double minutes, char direction, bool isLatitude)
        {
            double absoluteDegrees = degrees + minutes / 60.0;

            if (isLatitude && direction == 'S')
            {
                return -absoluteDegrees;
            }

            if (!isLatitude && direction == 'W')
            {
                return -absoluteDegrees;
            }

            return absoluteDegrees;
        }

        private static double RecomposeWithSeconds(double degrees, double minutes, double seconds, char direction, bool isLatitude)
        {
            double absoluteDegrees = degrees + minutes / 60.0 + seconds / 3600.0;

            if (isLatitude && direction == 'S')
            {
                return -absoluteDegrees;
            }

            if (!isLatitude && direction == 'W')
            {
                return -absoluteDegrees;
            }

            return absoluteDegrees;
        }

        private static (double degrees, double minutes, char direction) Decompose(double degrees, bool isLatitude)
        {
            bool isNegative = degrees < 0;
            double absDegrees = Math.Abs(degrees);

            int degreesInt = (int)absDegrees;
            double minutes = (absDegrees - degreesInt) * 60.0;

            char direction = isLatitude ?
                (isNegative ? 'S' : 'N') :
                (degreesInt == 180 ? 'W' : (isNegative ? 'W' : 'E'));

            return ((double)degreesInt, minutes, direction);
        }

        private static (double degrees, double minutes, double seconds, char direction) DecomposeWithSeconds(double degrees, bool isLatitude)
        {
            bool isNegative = degrees < 0;
            double absDegrees = Math.Abs(degrees);

            int degreesInt = (int)absDegrees;
            double minutesDouble = (absDegrees - degreesInt) * 60.0;
            int minutesInt = (int)minutesDouble;
            double seconds = (minutesDouble - minutesInt) * 60.0;

            char direction = isLatitude ?
                (isNegative ? 'S' : 'N') :
                (degreesInt == 180 ? 'W' : (isNegative ? 'W' : 'E'));

            return ((double)degreesInt, (double)minutesInt, seconds, direction);
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
