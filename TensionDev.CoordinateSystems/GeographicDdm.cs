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
using System.Globalization;

namespace TensionDev.CoordinateSystems
{
    /// <summary>
    /// A geographic coordinate represented in degrees and decimal minutes (DDM).
    /// </summary>
    public class GeographicDdm
    {
        private double _latitudeDegrees;
        private double _longitudeDegrees;

        /// <summary>Altitude in metres.</summary>
        public double AltitudeMetres { get; set; }

        /// <summary>Altitude in feet.</summary>
        public double AltitudeFeet
        {
            get => AltitudeMetres / 0.3048;
            set => AltitudeMetres = value * 0.3048;
        }

        /// <summary>
        /// Latitude degrees (0-90). Always non-negative; direction is indicated by <see cref="LatitudeDirection"/>.
        /// </summary>
        public double LatitudeDegrees
        {
            get => _latitudeDegrees;
            set => _latitudeDegrees = Math.Max(Math.Min(Math.Abs(value), 90), 0);
        }

        /// <summary>
        /// Latitude minutes (0-60). Always non-negative; direction is indicated by <see cref="LatitudeDirection"/>.
        /// </summary>
        public double LatitudeMinutes { get; set; }

        /// <summary>
        /// Latitude direction: 'N' for north, 'S' for south.
        /// </summary>
        public char LatitudeDirection { get; set; } = 'N';

        /// <summary>
        /// Longitude degrees (0-180). Always non-negative; direction is indicated by <see cref="LongitudeDirection"/>.
        /// </summary>
        public double LongitudeDegrees
        {
            get => _longitudeDegrees;
            set => _longitudeDegrees = Math.Max(Math.Min(Math.Abs(value), 180), 0);
        }

        /// <summary>
        /// Longitude minutes (0-60). Always non-negative; direction is indicated by <see cref="LongitudeDirection"/>.
        /// </summary>
        public double LongitudeMinutes { get; set; }

        /// <summary>
        /// Longitude direction: 'E' for east, 'W' for west.
        /// </summary>
        public char LongitudeDirection { get; set; } = 'E';

        /// <summary>
        /// Returns a formatted string representation of this coordinate in degrees and decimal minutes.
        /// </summary>
        /// <returns>A formatted string in the form "lat deg deg min' D, lon deg deg min' D".</returns>
        public override string ToString()
        {
            return ToString(6);
        }

        /// <summary>
        /// Returns a formatted string representation of this coordinate in degrees and decimal minutes.
        /// </summary>
        /// <param name="precision">The number of decimal places for the minutes component (default 6, maximum 6).</param>
        /// <returns>A formatted string in the form "lat deg deg min' D, lon deg deg min' D".</returns>
        public string ToString(int precision)
        {
            int clampedPrecision = Math.Min(precision, 6);
            string precisionFormat = "F" + clampedPrecision;
            double roundingThreshold = 60.0 - Math.Pow(10.0, -clampedPrecision) / 2.0;
            double precisionUnit = Math.Pow(10.0, -clampedPrecision) / 2.0;

            // Format latitude with rounding overflow normalization
            double latDegrees = LatitudeDegrees;
            double latMinutes = LatitudeMinutes;
            char latDirection = LatitudeDirection;

            if (Math.Abs(latMinutes) >= roundingThreshold)
            {
                latDegrees += Math.Sign(latMinutes);
                latMinutes -= 60.0 * Math.Sign(latMinutes);
                if (Math.Abs(latMinutes) < precisionUnit) latMinutes = 0.0; // avoid -0.000000
            }

            // Format longitude with rounding overflow normalization
            double lonDegrees = LongitudeDegrees;
            double lonMinutes = LongitudeMinutes;
            char lonDirection = LongitudeDirection;

            if (Math.Abs(lonMinutes) >= roundingThreshold)
            {
                lonDegrees += Math.Sign(lonMinutes);
                lonMinutes -= 60.0 * Math.Sign(lonMinutes);
                if (Math.Abs(lonMinutes) < precisionUnit) lonMinutes = 0.0; // avoid -0.000000
            }

            string text = FormatDdm(latDegrees, latMinutes, latDirection, precisionFormat) + ", " + FormatDdm(lonDegrees, lonMinutes, lonDirection, precisionFormat);
            return text + GeographicCoordinateSystem.FormatAltitude(AltitudeMetres, DistanceUnit.Metres, 3);
        }

        private static string FormatDdm(double degrees, double minutes, char direction, string precisionFormat)
        {
            return degrees + "° " + minutes.ToString(precisionFormat, CultureInfo.InvariantCulture) + "' " + direction;
        }

        /// <summary>
        /// Parses a DDM-formatted string into a <see cref="GeographicDdm"/> instance.
        /// </summary>
        /// <param name="s">A string in DDM format (e.g., "52° 31.20' N, 13° 24.30' E" or with optional altitude suffix).</param>
        /// <returns>A <see cref="GeographicDdm"/> parsed from the string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the string is null.</exception>
        /// <exception cref="FormatException">Thrown when the string is not in a valid DDM format or values are out of range.</exception>
        public static GeographicDdm Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (string.IsNullOrWhiteSpace(s))
            {
                throw new FormatException("Input string was null or whitespace.");
            }

            if (!TryParse(s, out var result))
            {
                throw new FormatException($"String '{s}' is not in a valid DDM format.");
            }

            return result;
        }

        /// <summary>
        /// Attempts to parse a DDM-formatted string into a <see cref="GeographicDdm"/> instance.
        /// </summary>
        /// <param name="s">A string in DDM format.</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="GeographicDdm"/> or default on failure.</param>
        /// <returns>True if the string was parsed successfully; false otherwise.</returns>
        public static bool TryParse(string s, out GeographicDdm result)
        {
            result = default;

            if (s == null || string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            string body = s.Trim();
            if (!GeographicAltitudeParser.TryExtractAltitude(ref body, out double altitudeMetres))
            {
                return false;
            }
            var parsed = new GeographicDdm();

            // Parse the DDM body: "lat deg min' dir, lon deg min' dir"
            // Split by comma to separate latitude and longitude
            int commaIdx = body.IndexOf(',');
            if (commaIdx < 0)
            {
                return false;
            }

            string latPart = body.Substring(0, commaIdx).Trim();
            string lonPart = body.Substring(commaIdx + 1).Trim();

            if (!TryParseDdmCoordinate(latPart, isLatitude: true, out double latDegrees, out double latMinutes, out char latDir))
            {
                return false;
            }

            if (!TryParseDdmCoordinate(lonPart, isLatitude: false, out double lonDegrees, out double lonMinutes, out char lonDir))
            {
                return false;
            }

            // Normalize minutes >= 60 to carry over to degrees
            if (latMinutes >= 60.0)
            {
                latDegrees += Math.Floor(latMinutes / 60.0);
                latMinutes = latMinutes % 60.0;
            }

            if (lonMinutes >= 60.0)
            {
                lonDegrees += Math.Floor(lonMinutes / 60.0);
                lonMinutes = lonMinutes % 60.0;
            }

            if (latDegrees < 0.0 || latDegrees > 90.0 || lonDegrees < 0.0 || lonDegrees > 180.0 ||
                (latDegrees == 90.0 && latMinutes != 0.0) ||
                (lonDegrees == 180.0 && lonMinutes != 0.0) ||
                !GeographicCoordinateSystem.IsFinite(latDegrees) || !GeographicCoordinateSystem.IsFinite(lonDegrees) ||
                !GeographicCoordinateSystem.IsFinite(latMinutes) || !GeographicCoordinateSystem.IsFinite(lonMinutes))
            {
                return false;
            }

            parsed.LatitudeDegrees = latDegrees;
            parsed.LatitudeMinutes = latMinutes;
            parsed.LatitudeDirection = latDir;
            parsed.LongitudeDegrees = lonDegrees;
            parsed.LongitudeMinutes = lonMinutes;
            parsed.LongitudeDirection = lonDir;
            parsed.AltitudeMetres = altitudeMetres;
            result = parsed;

            return true;
        }

        internal static bool TryParseDdmCoordinate(string part, bool isLatitude, out double degrees, out double minutes, out char direction)
        {
            degrees = 0;
            minutes = 0;
            direction = '\0';

            if (string.IsNullOrWhiteSpace(part))
            {
                return false;
            }

            // Try symbol-delimited format first: "52° 31.20' N"
            // Pattern: <degrees>° <minutes>' <direction>
            int degSymbolIdx = part.IndexOf('°');
            int minSymbolIdx = part.IndexOf('\'');
            int dirIdx = -1;

            if (degSymbolIdx >= 0 && minSymbolIdx > degSymbolIdx)
            {
                // Check for direction character after the minute symbol
                var remainingAfterMinutes = part.Substring(minSymbolIdx + 1).Trim();
                dirIdx = remainingAfterMinutes.IndexOfAny(new[] { 'N', 'n', 'S', 's', 'E', 'e', 'W', 'w' });
            }

            if (dirIdx >= 0 && degSymbolIdx >= 0 && minSymbolIdx > degSymbolIdx)
            {
                string degStr = part.Substring(0, degSymbolIdx).Trim();
                string minStr = part.Substring(degSymbolIdx + 1, minSymbolIdx - degSymbolIdx - 1).Trim();
                char potentialDir;

                // Check which direction character is at the end
                int lastNOrS = part.LastIndexOfAny(new[] { 'N', 'n', 'S', 's' });
                int lastEOrW = part.LastIndexOfAny(new[] { 'E', 'e', 'W', 'w' });

                if (isLatitude)
                {
                    // Latitude must end with N or S
                    if (lastNOrS <= minSymbolIdx) return false;  // Missing direction
                    char foundDir = part[lastNOrS];
                    // Validate the found direction is actually N or S (not E or W mistakenly picked up)
                    if (!GeographicCoordinateSystem.IsValidLatitudeDirection(foundDir)) return false;
                    potentialDir = foundDir;
                    direction = char.ToUpperInvariant(potentialDir);
                }
                else
                {
                    // Longitude must end with E or W
                    if (lastEOrW <= minSymbolIdx) return false;  // Missing direction
                    char foundDir = part[lastEOrW];
                    // Validate the found direction is actually E or W (not N or S mistakenly picked up)
                    if (!GeographicCoordinateSystem.IsValidLongitudeDirection(foundDir)) return false;
                    potentialDir = foundDir;
                    direction = char.ToUpperInvariant(potentialDir);
                }

                if (!double.TryParse(degStr, NumberStyles.Float, CultureInfo.InvariantCulture, out degrees))
                {
                    return false;
                }

                if (!double.TryParse(minStr, NumberStyles.Float, CultureInfo.InvariantCulture, out minutes))
                {
                    return false;
                }

                if (!GeographicCoordinateSystem.IsFinite(degrees) || !GeographicCoordinateSystem.IsFinite(minutes) ||
                    degrees < 0.0 || minutes < 0.0 || minutes > 60.0 ||
                    degrees > (isLatitude ? 90.0 : 180.0))
                    return false;

                return true;
            }

            // Try space-delimited format: "52 31.20 N"
            // Pattern: <integer degrees> <decimal minutes> <direction>
            var parts = part.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3)
            {
                string degStr = parts[0];
                string minStr = parts[1];
                string dirStr = parts[2];

                char potentialDir;

                // Validate direction character based on coordinate type
                if (isLatitude)
                {
                    // Latitude must be N or S
                    if (!GeographicCoordinateSystem.IsValidLatitudeDirection(dirStr[0])) return false;
                    potentialDir = dirStr[0];
                    direction = char.ToUpperInvariant(potentialDir);
                }
                else
                {
                    // Longitude must be E or W
                    if (!GeographicCoordinateSystem.IsValidLongitudeDirection(dirStr[0])) return false;
                    potentialDir = dirStr[0];
                    direction = char.ToUpperInvariant(potentialDir);
                }

                // Degrees must be an integer in space-delimited format
                if (!double.TryParse(degStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out double parsedDeg))
                {
                    return false;
                }
                degrees = parsedDeg;

                if (!double.TryParse(minStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedMin))
                {
                    return false;
                }
                minutes = parsedMin;

                // Validate minutes range >= 0 per ADR-0008 Section 93-94 (upper bound checked during normalization)
                if (dirStr.Length != 1 || !GeographicCoordinateSystem.IsFinite(degrees) ||
                    !GeographicCoordinateSystem.IsFinite(minutes) || degrees < 0.0 || minutes < 0.0 || minutes > 60.0 ||
                    degrees > (isLatitude ? 90.0 : 180.0))
                    return false;

                return true;
            }

            return false;
        }

    }
}
