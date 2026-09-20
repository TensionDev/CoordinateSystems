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
    /// A geographic coordinate represented in degrees, minutes, and seconds (DMS).
    /// </summary>
    public class GeographicDms
    {
        private double _latitudeDegrees;
        private double _longitudeDegrees;

        /// <summary>
        /// Latitude degrees (0-90). Always non-negative; direction is indicated by <see cref="LatitudeDirection"/>.
        /// </summary>
        public double LatitudeDegrees
        {
            get => _latitudeDegrees;
            set => _latitudeDegrees = Math.Max(Math.Min(Math.Abs(value), 90), 0);
        }

        /// <summary>
        /// Latitude minutes (0-60). Always non-negative.
        /// </summary>
        public double LatitudeMinutes { get; set; }

        /// <summary>
        /// Latitude seconds (0-60). Always non-negative.
        /// </summary>
        public double LatitudeSeconds { get; set; }

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
        /// Longitude minutes (0-60). Always non-negative.
        /// </summary>
        public double LongitudeMinutes { get; set; }

        /// <summary>
        /// Longitude seconds (0-60). Always non-negative.
        /// </summary>
        public double LongitudeSeconds { get; set; }

        /// <summary>
        /// Longitude direction: 'E' for east, 'W' for west.
        /// </summary>
        public char LongitudeDirection { get; set; } = 'E';

        /// <summary>
        /// Returns a formatted string representation of this coordinate in degrees, minutes, and seconds.
        /// </summary>
        /// <returns>A formatted string in the form "lat deg deg min sec" D, lon deg deg min sec" D".</returns>
        public override string ToString()
        {
            return ToString(6);
        }

        /// <summary>
        /// Returns a formatted string representation of this coordinate in degrees, minutes, and seconds.
        /// </summary>
        /// <param name="precision">The number of decimal places for the seconds component (default 6, maximum 6).</param>
        /// <returns>A formatted string in the form "lat deg deg min sec" D, lon deg deg min sec" D".</returns>
        public string ToString(int precision)
        {
            int clampedPrecision = Math.Min(precision, 6);
            string precisionFormat = "F" + clampedPrecision;
            double secondsThreshold = 60.0 - Math.Pow(10.0, -clampedPrecision) / 2.0;
            double minutesThreshold = 60.0 - Math.Pow(10.0, -clampedPrecision) / 2.0;
            double precisionUnit = Math.Pow(10.0, -clampedPrecision) / 2.0;

            // Format latitude with rounding overflow normalization (cascading)
            double latDegrees = LatitudeDegrees;
            double latMinutes = LatitudeMinutes;
            double latSeconds = LatitudeSeconds;
            char latDirection = LatitudeDirection;

            if (Math.Abs(latSeconds) >= secondsThreshold)
            {
                latMinutes += Math.Sign(latSeconds);
                latSeconds -= 60.0 * Math.Sign(latSeconds);
                if (Math.Abs(latSeconds) < precisionUnit) latSeconds = 0.0; // avoid -0.000000
            }

            if (Math.Abs(latMinutes) >= minutesThreshold)
            {
                latDegrees += Math.Sign(latMinutes);
                latMinutes -= 60.0 * Math.Sign(latMinutes);
                if (Math.Abs(latMinutes) < precisionUnit) latMinutes = 0.0; // avoid -0.000000
            }

            // Format longitude with rounding overflow normalization (cascading)
            double lonDegrees = LongitudeDegrees;
            double lonMinutes = LongitudeMinutes;
            double lonSeconds = LongitudeSeconds;
            char lonDirection = LongitudeDirection;

            if (Math.Abs(lonSeconds) >= secondsThreshold)
            {
                lonMinutes += Math.Sign(lonSeconds);
                lonSeconds -= 60.0 * Math.Sign(lonSeconds);
                if (Math.Abs(lonSeconds) < precisionUnit) lonSeconds = 0.0; // avoid -0.000000
            }

            if (Math.Abs(lonMinutes) >= minutesThreshold)
            {
                lonDegrees += Math.Sign(lonMinutes);
                lonMinutes -= 60.0 * Math.Sign(lonMinutes);
                if (Math.Abs(lonMinutes) < precisionUnit) lonMinutes = 0.0; // avoid -0.000000
            }

            return FormatDms(latDegrees, latMinutes, latSeconds, latDirection, precisionFormat) + ", " + FormatDms(lonDegrees, lonMinutes, lonSeconds, lonDirection, precisionFormat);
        }

        private static string FormatDms(double degrees, double minutes, double seconds, char direction, string precisionFormat)
        {
            return degrees + "° " + minutes.ToString("F0", CultureInfo.InvariantCulture) + "' " + seconds.ToString(precisionFormat, CultureInfo.InvariantCulture) + "\" " + direction;
        }
    }
}
