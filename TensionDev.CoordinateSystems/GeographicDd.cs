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
    /// A geographic coordinate represented in decimal degrees (DD), with altitude in metres.
    /// </summary>
    public class GeographicDd
    {
        private const double FeetToMetres = 0.3048;
        private double _latitudeDecimalDegrees;
        private double _longitudeDecimalDegrees;
        private double _altitudeMetres;

        /// <summary>Latitude in decimal degrees, clamped to [-90, 90].</summary>
        public double LatitudeDecimalDegrees
        {
            get => _latitudeDecimalDegrees;
            set => _latitudeDecimalDegrees = Math.Max(-90.0, Math.Min(90.0, value));
        }

        /// <summary>Longitude in decimal degrees, clamped to [-180, 180].</summary>
        public double LongitudeDecimalDegrees
        {
            get => _longitudeDecimalDegrees;
            set => _longitudeDecimalDegrees = Math.Max(-180.0, Math.Min(180.0, value));
        }

        /// <summary>Altitude in metres.</summary>
        public double AltitudeMetres
        {
            get => _altitudeMetres;
            set => _altitudeMetres = value;
        }

        /// <summary>Altitude in feet.</summary>
        public double AltitudeFeet
        {
            get => _altitudeMetres / FeetToMetres;
            set => _altitudeMetres = value * FeetToMetres;
        }

        /// <summary>Returns this coordinate in decimal degrees with altitude in metres.</summary>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0:F6}, {1:F6}{2}",
                LatitudeDecimalDegrees,
                LongitudeDecimalDegrees,
                GeographicCoordinateSystem.FormatAltitude(AltitudeMetres, DistanceUnit.Metres, 3));
        }

        /// <summary>Parses a decimal-degree coordinate string.</summary>
        /// <exception cref="ArgumentNullException">The input is null.</exception>
        /// <exception cref="FormatException">The input is malformed or outside the geographic range.</exception>
        public static GeographicDd Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (!TryParse(s, out GeographicDd result))
            {
                throw new FormatException($"String '{s}' is not in a valid decimal-degree format.");
            }

            return result;
        }

        /// <summary>Attempts to parse a decimal-degree coordinate string.</summary>
        public static bool TryParse(string s, out GeographicDd result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            string body = s.Trim();
            double altitudeMetres = 0.0;
            int altitudeIndex = body.LastIndexOf("Altitude:", StringComparison.OrdinalIgnoreCase);
            if (altitudeIndex >= 0)
            {
                if (!TryParseAltitude(body.Substring(altitudeIndex + "Altitude:".Length), out altitudeMetres))
                {
                    return false;
                }
                body = body.Substring(0, altitudeIndex).TrimEnd();
            }

            int commaIndex = body.IndexOf(',');
            if (commaIndex < 0 || body.IndexOf(',', commaIndex + 1) >= 0)
            {
                return false;
            }

            string latitudeText = body.Substring(0, commaIndex).Trim();
            string longitudeText = body.Substring(commaIndex + 1).Trim();
            if (!double.TryParse(latitudeText, NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude) ||
                !double.TryParse(longitudeText, NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude) ||
                !GeographicCoordinateSystem.IsFinite(latitude) || !GeographicCoordinateSystem.IsFinite(longitude) ||
                latitude < -90.0 || latitude > 90.0 ||
                longitude < -180.0 || longitude > 180.0)
            {
                return false;
            }

            result = new GeographicDd
            {
                LatitudeDecimalDegrees = latitude,
                LongitudeDecimalDegrees = longitude,
                AltitudeMetres = altitudeMetres,
            };
            return true;
        }

        private static bool TryParseAltitude(string text, out double altitudeMetres)
        {
            altitudeMetres = 0.0;
            string[] parts = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 1 || parts.Length > 2 ||
                !double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double value) ||
                !GeographicCoordinateSystem.IsFinite(value))
            {
                return false;
            }

            if (parts.Length == 1 || string.Equals(parts[1], "m", StringComparison.OrdinalIgnoreCase))
            {
                altitudeMetres = value;
                return true;
            }

            if (string.Equals(parts[1], "ft", StringComparison.OrdinalIgnoreCase))
            {
                altitudeMetres = value * FeetToMetres;
                return GeographicCoordinateSystem.IsFinite(altitudeMetres);
            }

            return false;
        }

    }
}
