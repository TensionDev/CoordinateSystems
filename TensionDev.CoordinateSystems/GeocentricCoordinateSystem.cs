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
    /// Geocentric Coordinates
    /// </summary>
    public class GeocentricCoordinateSystem
    {
        /// <summary>
        /// X coordinate in Metres.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate in Metres.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z coordinate in Metres.
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Returns a string representation of the geocentric coordinate system using the specified distance unit.
        /// </summary>
        /// <param name="distanceUnit">The distance unit to use for X, Y and Z coordinates</param>
        /// <param name="distancePrecision">The number of decimal places for distance values (default 3, maximum 6)</param>
        /// <returns>A formatted string representation of the geocentric coordinate system</returns>
        public string ToString(DistanceUnit distanceUnit = DistanceUnit.Metres, int distancePrecision = 3)
        {
            int clampedDistancePrecision = Math.Min(distancePrecision, 6);

            switch (distanceUnit)
            {
                case DistanceUnit.Metres:
                    return ToStringMetres(clampedDistancePrecision);
                case DistanceUnit.Feet:
                    return ToStringFeet(clampedDistancePrecision);
                default:
                    throw new NotImplementedException();
            }
        }

        internal string ToStringMetres(int distancePrecision)
        {
            string format = "F" + distancePrecision;
            return $"X: {X.ToString(format, CultureInfo.InvariantCulture)} m, Y: {Y.ToString(format, CultureInfo.InvariantCulture)} m, Z: {Z.ToString(format, CultureInfo.InvariantCulture)} m";
        }

        internal string ToStringFeet(int distancePrecision)
        {
            string format = "F" + distancePrecision;
            return $"X: {(X / 0.3048).ToString(format, CultureInfo.InvariantCulture)} ft, Y: {(Y / 0.3048).ToString(format, CultureInfo.InvariantCulture)} ft, Z: {(Z / 0.3048).ToString(format, CultureInfo.InvariantCulture)} ft";
        }

        /// <summary>
        /// Parses a geocentric coordinate string into a <see cref="GeocentricCoordinateSystem"/> instance.
        /// </summary>
        /// <param name="s">A string in geocentric format (e.g., "X: 1000.000 m, Y: 2000.000 m, Z: 3000.000 m" or with feet units).</param>
        /// <returns>A <see cref="GeocentricCoordinateSystem"/> parsed from the string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the string is null.</exception>
        /// <exception cref="FormatException">Thrown when the string is not in a valid geocentric format.</exception>
        public static GeocentricCoordinateSystem Parse(string s)
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
                throw new FormatException($"String '{s}' is not in a valid geocentric format.");
            }

            return result;
        }

        /// <summary>
        /// Attempts to parse a geocentric coordinate string into a <see cref="GeocentricCoordinateSystem"/> instance.
        /// </summary>
        /// <param name="s">A string in geocentric format.</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="GeocentricCoordinateSystem"/> or default on failure.</param>
        /// <returns>True if the string was parsed successfully; false otherwise.</returns>
        public static bool TryParse(string s, out GeocentricCoordinateSystem result)
        {
            result = new GeocentricCoordinateSystem();

            if (s == null || string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            // Split by comma first to extract X, Y, Z coordinate segments.
            var segments = s.Split(',');
            if (segments.Length != 3)
            {
                return false;
            }

            double[] values = new double[3];
            string[] expectedAxes = { "X", "Y", "Z" };
            string commonUnit = null;

            for (int i = 0; i < 3; i++)
            {
                string segment = segments[i].Trim();
                int colonIdx = segment.IndexOf(':');
                if (colonIdx < 0 || !string.Equals(segment.Substring(0, colonIdx).Trim(), expectedAxes[i], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                string[] valueAndUnit = segment.Substring(colonIdx + 1).Trim().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (valueAndUnit.Length != 2)
                {
                    return false;
                }

                string unit = valueAndUnit[1];
                if (!string.Equals(unit, "m", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(unit, "ft", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (commonUnit != null && !string.Equals(unit, commonUnit, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                commonUnit = unit;

                if (!double.TryParse(valueAndUnit[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    return false;
                }

                // One international foot is exactly 0.3048 metres.
                values[i] = string.Equals(unit, "ft", StringComparison.OrdinalIgnoreCase) ? value * 0.3048 : value;
            }

            result = new GeocentricCoordinateSystem
            {
                X = values[0],
                Y = values[1],
                Z = values[2]
            };

            return true;
        }
    }
}
