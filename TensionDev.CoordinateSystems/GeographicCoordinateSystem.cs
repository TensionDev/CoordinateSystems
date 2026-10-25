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
using System.Text;

namespace TensionDev.CoordinateSystems
{
    /// <summary>
    /// Geodetic Coordinates
    /// </summary>
    public class GeographicCoordinateSystem
    {
        private const double FEET_TO_METRES = 0.3048;

        private double _latitudeRadians;
        private double _longitudeRadians;
        private double _altitude;

        /// <summary>
        /// Latitude in Radians
        /// </summary>
        public Double LatitudeDecimalRadians
        {
            get => _latitudeRadians;
            set => _latitudeRadians = Math.Max(Math.Min(value, Math.PI / 2.0), -Math.PI / 2.0);
        }

        /// <summary>
        /// Longitude in Radians
        /// </summary>
        public Double LongitudeDecimalRadians
        {
            get => _longitudeRadians;
            set => _longitudeRadians = Math.Max(Math.Min(value, Math.PI), -Math.PI);
        }

        /// <summary>
        /// Altitude in Feet
        /// </summary>
        public Double AltitudeFeet { get => _altitude / FEET_TO_METRES; set => _altitude = value * FEET_TO_METRES; }

        /// <summary>
        /// Latitude in Degrees
        /// </summary>
        public Double LatitudeDecimalDegrees
        {
            get => _latitudeRadians * 180.0 / Math.PI;
            set => LatitudeDecimalRadians = value * Math.PI / 180.0;
        }

        /// <summary>
        /// Longitude in Degrees
        /// </summary>
        public Double LongitudeDecimalDegrees
        {
            get => _longitudeRadians * 180.0 / Math.PI;
            set => LongitudeDecimalRadians = value * Math.PI / 180.0;
        }

        /// <summary>
        /// Altitude in Metres
        /// </summary>
        public Double AltitudeMetres { get => _altitude; set => _altitude = value; }

        /// <summary>
        /// Returns a string representation of the geographic coordinate system using the specified formatting options.
        /// </summary>
        /// <param name="angularFormat">The angular format to use for latitude and longitude</param>
        /// <param name="distanceUnit">The distance unit to use for altitude</param>
        /// <param name="angularPrecision">The number of decimal places for angular values (default 6, maximum 6)</param>
        /// <param name="distancePrecision">The number of decimal places for distance values (default 3, maximum 6)</param>
        /// <returns>A formatted string representation of the geographic coordinate system</returns>
        public string ToString(AngularFormat angularFormat = AngularFormat.DecimalDegrees, DistanceUnit distanceUnit = DistanceUnit.Metres, int angularPrecision = 6, int distancePrecision = 3)
        {
            int clampedAngularPrecision = Math.Min(angularPrecision, 6);
            int clampedDistancePrecision = Math.Min(distancePrecision, 6);

            switch (angularFormat)
            {
                case AngularFormat.DecimalDegrees:
                    return ToStringDecimalDegrees(distanceUnit, clampedAngularPrecision, clampedDistancePrecision);
                case AngularFormat.DegreesDecimalMinutes:
                    return ToStringDegreesDecimalMinutes(distanceUnit, clampedAngularPrecision, clampedDistancePrecision);
                case AngularFormat.DegreesMinutesSeconds:
                    return ToStringDegreesMinutesSeconds(distanceUnit, clampedAngularPrecision, clampedDistancePrecision);
                case AngularFormat.Radians:
                    return ToStringRadians(distanceUnit, clampedAngularPrecision, clampedDistancePrecision);
                default:
                    throw new NotImplementedException();
            }
        }

        internal string ToStringDecimalDegrees(DistanceUnit distanceUnit, int angularPrecision, int distancePrecision)
        {
            StringBuilder sb = new StringBuilder();
            string angularFormat = "F" + angularPrecision;
            sb.Append($"{LatitudeDecimalDegrees.ToString(angularFormat, CultureInfo.InvariantCulture)}, {LongitudeDecimalDegrees.ToString(angularFormat, CultureInfo.InvariantCulture)}");
            sb.Append(FormatAltitude(_altitude, distanceUnit, distancePrecision));
            return sb.ToString();
        }

        internal string ToStringDegreesDecimalMinutes(DistanceUnit distanceUnit, int angularPrecision, int distancePrecision)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{FormatDegreesDecimalMinutes(LatitudeDecimalDegrees, true)}, {FormatDegreesDecimalMinutes(LongitudeDecimalDegrees, false)}");
            sb.Append(FormatAltitude(_altitude, distanceUnit, distancePrecision));
            return sb.ToString();
        }

        internal string ToStringDegreesMinutesSeconds(DistanceUnit distanceUnit, int angularPrecision, int distancePrecision)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{FormatDegreesMinutesSeconds(LatitudeDecimalDegrees, true)}, {FormatDegreesMinutesSeconds(LongitudeDecimalDegrees, false)}");
            sb.Append(FormatAltitude(_altitude, distanceUnit, distancePrecision));
            return sb.ToString();
        }

        internal string ToStringRadians(DistanceUnit distanceUnit, int angularPrecision, int distancePrecision)
        {
            StringBuilder sb = new StringBuilder();
            string angularFormat = "F" + angularPrecision;
            sb.Append($"{LatitudeDecimalRadians.ToString(angularFormat, CultureInfo.InvariantCulture)} rad, {LongitudeDecimalRadians.ToString(angularFormat, CultureInfo.InvariantCulture)} rad");
            sb.Append(FormatAltitude(_altitude, distanceUnit, distancePrecision));
            return sb.ToString();
        }

        internal static string FormatAltitude(double altitudeMetres, DistanceUnit distanceUnit, int distancePrecision)
        {
            string unitSymbol;
            double altitudeValue;

            switch (distanceUnit)
            {
                case DistanceUnit.Metres:
                    unitSymbol = "m";
                    altitudeValue = altitudeMetres;
                    break;
                case DistanceUnit.Feet:
                    unitSymbol = "ft";
                    altitudeValue = altitudeMetres / FEET_TO_METRES;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return $" Altitude: {altitudeValue.ToString("F" + distancePrecision, CultureInfo.InvariantCulture)} {unitSymbol}";
        }

        private static string FormatDegreesDecimalMinutes(double degrees, bool isLatitude)
        {
            // Handle negative values
            bool isNegative = degrees < 0;
            double absDegrees = Math.Abs(degrees);

            int degreesInt = (int)absDegrees;
            double minutes = (absDegrees - degreesInt) * 60.0;

            string direction = isLatitude ?
                (isNegative ? "S" : "N") :
                (isNegative ? "W" : "E");

            return $"{degreesInt}° {minutes:F2}' {direction}";
        }

        private static string FormatDegreesMinutesSeconds(double degrees, bool isLatitude)
        {
            // Handle negative values
            bool isNegative = degrees < 0;
            double absDegrees = Math.Abs(degrees);

            int degreesInt = (int)absDegrees;
            double minutesDouble = (absDegrees - degreesInt) * 60.0;
            int minutes = (int)minutesDouble;
            double seconds = (minutesDouble - minutes) * 60.0;

            string direction = isLatitude ?
                (isNegative ? "S" : "N") :
                (isNegative ? "W" : "E");

            return $"{degreesInt}° {minutes}' {seconds:F2}\" {direction}";
        }

        /// <summary>
        /// Parses a geographic coordinate string into a <see cref="GeographicCoordinateSystem"/> instance.
        /// Supports multiple angular formats: Decimal Degrees, DDM (Degrees-Decimal Minutes), DMS (Degrees-Minutes-Seconds), and Radians.
        /// Altitude is optional with suffix "Altitude: X.XXX m" or "Altitude: X.XXX ft".
        /// </summary>
        /// <param name="s">A string in geographic format.</param>
        /// <returns>A <see cref="GeographicCoordinateSystem"/> parsed from the string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the string is null.</exception>
        /// <exception cref="FormatException">Thrown when the string is not in a valid geographic format.</exception>
        public static GeographicCoordinateSystem Parse(string s)
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
                throw new FormatException($"String '{s}' is not in a valid geographic format.");
            }

            return result;
        }

        /// <summary>
        /// Attempts to parse a geographic coordinate string into a <see cref="GeographicCoordinateSystem"/> instance.
        /// </summary>
        /// <param name="s">A string in geographic format.</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="GeographicCoordinateSystem"/> or default on failure.</param>
        /// <returns>True if the string was parsed successfully; false otherwise.</returns>
        public static bool TryParse(string s, out GeographicCoordinateSystem result)
        {
            result = null;

            if (s == null || string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            string body = s.Trim();
            if (!GeographicAltitudeParser.TryExtractAltitude(ref body, out double altitudeMetres))
            {
                return false;
            }
            var parsed = new GeographicCoordinateSystem { AltitudeMetres = altitudeMetres };

            // Detect angular format by examining the string structure
            if (body.IndexOf(',') < 0)
            {
                return false;
            }

            int commaIdx = body.IndexOf(',');
            string leftPart = body.Substring(0, commaIdx).Trim();
            string rightPart = body.Substring(commaIdx + 1).Trim();

            // Check for degree symbol to determine DDM vs DMS format
            bool hasDegreeSymbolLeft = HasSpecialChar(leftPart, '°');
            bool hasMinuteSymbolLeft = HasSpecialChar(leftPart, '\'');
            bool hasSecondQuoteLeft = HasSpecialChar(leftPart, '"');

            bool hasDegreeSymbolRight = HasSpecialChar(rightPart, '°');
            bool hasMinuteSymbolRight = HasSpecialChar(rightPart, '\'');
            bool hasSecondQuoteRight = HasSpecialChar(rightPart, '"');

            // Detect direction character index
            int leftDirIdx = GetDirectionIndex(leftPart);
            int rightDirIdx = GetDirectionIndex(rightPart);

            if (hasDegreeSymbolLeft || hasDegreeSymbolRight || hasMinuteSymbolLeft || hasMinuteSymbolRight || leftDirIdx >= 0 || rightDirIdx >= 0)
            {
                // Direction-delimited or symbol-delimited DDM/DMS formats.
                double latResult = ParseCoordinatePart(leftPart, true, leftDirIdx);
                double lonResult = ParseCoordinatePart(rightPart, false, rightDirIdx);

                // Check for sentinel values indicating parse failure (missing direction, invalid format)
                if (!IsFinite(latResult) || !IsFinite(lonResult) ||
                    latResult < -90.0 || latResult > 90.0 || lonResult < -180.0 || lonResult > 180.0)
                    return false;

                parsed.LatitudeDecimalDegrees = latResult;
                parsed.LongitudeDecimalDegrees = lonResult;
                result = parsed;
                return true;
            }

            // Radians must be explicit. Bare numeric pairs are decimal degrees.
            bool leftHasRadians = leftPart.TrimEnd().EndsWith("rad", StringComparison.OrdinalIgnoreCase);
            bool rightHasRadians = rightPart.TrimEnd().EndsWith("rad", StringComparison.OrdinalIgnoreCase);
            if (leftHasRadians != rightHasRadians)
            {
                return false;
            }

            string leftNumeric = leftHasRadians ? leftPart.Trim().Substring(0, leftPart.Trim().Length - 3).Trim() : leftPart;
            string rightNumeric = rightHasRadians ? rightPart.Trim().Substring(0, rightPart.Trim().Length - 3).Trim() : rightPart;
            if (double.TryParse(leftNumeric, NumberStyles.Float, CultureInfo.InvariantCulture, out double latNumeric) &&
                double.TryParse(rightNumeric, NumberStyles.Float, CultureInfo.InvariantCulture, out double lonNumeric) &&
                !double.IsNaN(latNumeric) && !double.IsInfinity(latNumeric) &&
                !double.IsNaN(lonNumeric) && !double.IsInfinity(lonNumeric))
            {
                if (leftHasRadians)
                {
                    if (latNumeric < -Math.PI / 2.0 || latNumeric > Math.PI / 2.0 ||
                        lonNumeric < -Math.PI || lonNumeric > Math.PI)
                    {
                        return false;
                    }

                    parsed.LatitudeDecimalRadians = latNumeric;
                    parsed.LongitudeDecimalRadians = lonNumeric;
                    result = parsed;
                    return true;
                }

                if (latNumeric < -90.0 || latNumeric > 90.0 ||
                    lonNumeric < -180.0 || lonNumeric > 180.0)
                {
                    return false;
                }

                parsed.LatitudeDecimalDegrees = latNumeric;
                parsed.LongitudeDecimalDegrees = lonNumeric;
                result = parsed;
                return true;
            }

            return false;
        }

        private static bool HasSpecialChar(string part, char c)
        {
            if (part == null || string.IsNullOrEmpty(part))
                return false;

            int idx = part.IndexOf(c);
            return idx >= 0;
        }

        private static int GetDirectionIndex(string part)
        {
            if (part == null || string.IsNullOrEmpty(part))
                return -1;

            int index = part.TrimEnd().Length - 1;
            return index >= 0 && IsValidDirection(part[index]) ? index : -1;
        }

        private static double ParseCoordinatePart(string part, bool isLatitude, int dirIdx)
        {
            if (string.IsNullOrWhiteSpace(part))
            {
                return double.MaxValue;
            }

            string trimmed = part.Trim();
            string[] tokens = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 2)
            {
                return double.MaxValue;
            }

            char direction = IsValidDirectionChar(tokens[tokens.Length - 1][0]);
            if (tokens[tokens.Length - 1].Length != 1 || !IsDirectionValidForAxis(direction, isLatitude))
            {
                return double.MaxValue;
            }

            double degrees;
            double minutes = 0.0;
            double seconds = 0.0;
            bool isDdm = false;
            bool isDms = false;

            if (trimmed.Contains("°"))
            {
                int degreeIndex = trimmed.IndexOf('°');
                int minuteIndex = trimmed.IndexOf('\'');
                if (degreeIndex < 0 || minuteIndex <= degreeIndex)
                {
                    return double.MaxValue;
                }

                string degreeText = trimmed.Substring(0, degreeIndex).Trim();
                string minuteText = trimmed.Substring(degreeIndex + 1, minuteIndex - degreeIndex - 1).Trim();
                int secondIndex = trimmed.LastIndexOf('"');

                if (!double.TryParse(degreeText, NumberStyles.Float, CultureInfo.InvariantCulture, out degrees))
                {
                    return double.MaxValue;
                }

                if (secondIndex > minuteIndex)
                {
                    string secondsText = trimmed.Substring(minuteIndex + 1, secondIndex - minuteIndex - 1).Trim();
                    minuteText = minuteText.TrimEnd();
                    if (!double.TryParse(minuteText, NumberStyles.Float, CultureInfo.InvariantCulture, out minutes) ||
                        !double.TryParse(secondsText, NumberStyles.Float, CultureInfo.InvariantCulture, out seconds))
                    {
                        return double.MaxValue;
                    }
                    isDms = true;
                }
                else
                {
                    if (!double.TryParse(minuteText, NumberStyles.Float, CultureInfo.InvariantCulture, out minutes))
                    {
                        return double.MaxValue;
                    }
                    isDdm = true;
                }
            }
            else
            {
                // Drop the final direction token; the remaining tokens determine DD, DDM, or DMS.
                int componentCount = tokens.Length - 1;
                if (componentCount == 1)
                {
                    if (!double.TryParse(tokens[0], NumberStyles.Float, CultureInfo.InvariantCulture, out degrees))
                    {
                        return double.MaxValue;
                    }
                }
                else if (componentCount == 2)
                {
                    if (!double.TryParse(tokens[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out degrees) ||
                        !double.TryParse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture, out minutes))
                    {
                        return double.MaxValue;
                    }
                    isDdm = true;
                }
                else if (componentCount == 3)
                {
                    if (!double.TryParse(tokens[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out degrees) ||
                        !double.TryParse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture, out minutes) ||
                        !double.TryParse(tokens[2], NumberStyles.Float, CultureInfo.InvariantCulture, out seconds))
                    {
                        return double.MaxValue;
                    }
                    isDms = true;
                }
                else
                {
                    return double.MaxValue;
                }
            }

            double maximumDegrees = isLatitude ? 90.0 : 180.0;
            if (degrees < 0.0 || degrees > maximumDegrees ||
                (isDdm && (minutes < 0.0 || minutes > 60.0)) ||
                (isDms && (minutes < 0.0 || minutes > 60.0 || seconds < 0.0 || seconds > 60.0)) ||
                (degrees == maximumDegrees && (minutes != 0.0 || seconds != 0.0)))
            {
                return double.MaxValue;
            }

            double value = degrees;
            if (isDdm)
            {
                value += minutes / 60.0;
            }
            else if (isDms)
            {
                value += minutes / 60.0 + seconds / 3600.0;
            }

            return direction == 'S' || direction == 'W' ? -value : value;
        }

        private static bool IsDirectionValidForAxis(char direction, bool isLatitude)
        {
            return isLatitude
                ? IsValidLatitudeDirection(direction)
                : IsValidLongitudeDirection(direction);
        }
        private static double ConvertDmsToDecimalDegrees(double degrees, double minutes, double seconds)
        {
            return degrees + minutes / 60.0 + seconds / 3600.0;
        }

        private static double ConvertDdmToDecimalDegrees(double degrees, double minutes)
        {
            return degrees + minutes / 60.0;
        }

        private static double ApplyDirection(double value, char direction)
        {
            // For latitude: negative values indicate south (S)
            // For longitude: negative values indicate west (W)
            if (direction == 's' || direction == 'S')
                return -Math.Abs(value);
            if (direction == 'w' || direction == 'W')
                return -Math.Abs(value);
            return value;
        }

        internal static bool IsValidDirection(char c)
        {
            return IsValidLatitudeDirection(c) || IsValidLongitudeDirection(c);
        }

        internal static bool IsValidLatitudeDirection(char c)
        {
            char direction = char.ToUpperInvariant(c);
            return direction == 'N' || direction == 'S';
        }

        internal static bool IsValidLongitudeDirection(char c)
        {
            char direction = char.ToUpperInvariant(c);
            return direction == 'E' || direction == 'W';
        }

        internal static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);

        internal static char IsValidDirectionChar(char c)
        {
            if (IsValidDirection(c))
                return char.ToUpperInvariant(c);
            return '\0';
        }
    }
}
