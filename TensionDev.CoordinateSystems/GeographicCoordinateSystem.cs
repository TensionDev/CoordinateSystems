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
using System.Collections.Generic;
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

        private double _latitude;
        private double _longitude;
        private double _altitude;

        /// <summary>
        /// Latitude in Radians
        /// </summary>
        public Double LatitudeDecimalRadians { get => _latitude * Math.PI / 180.0; set => LatitudeDecimalDegrees = value * 180.0 / Math.PI; }

        /// <summary>
        /// Longitude in Radians
        /// </summary>
        public Double LongitudeDecimalRadians { get => _longitude * Math.PI / 180.0; set => LongitudeDecimalDegrees = value * 180.0 / Math.PI; }

        /// <summary>
        /// Altitude in Feet
        /// </summary>
        public Double AltitudeFeet { get => _altitude / FEET_TO_METRES; set => _altitude = value * FEET_TO_METRES; }

        /// <summary>
        /// Latitude in Degrees
        /// </summary>
        public Double LatitudeDecimalDegrees { get => _latitude; set => _latitude = Math.Max(Math.Min(value, 90), -90); }

        /// <summary>
        /// Longitude in Degrees
        /// </summary>
        public Double LongitudeDecimalDegrees { get => _longitude; set => _longitude = Math.Max(Math.Min(value, 180), -180); }

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
            sb.Append(ToStringAltitude(distanceUnit, distancePrecision));
            return sb.ToString();
        }

        internal string ToStringDegreesDecimalMinutes(DistanceUnit distanceUnit, int angularPrecision, int distancePrecision)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{FormatDegreesDecimalMinutes(LatitudeDecimalDegrees, true)}, {FormatDegreesDecimalMinutes(LongitudeDecimalDegrees, false)}");
            sb.Append(ToStringAltitude(distanceUnit, distancePrecision));
            return sb.ToString();
        }

        internal string ToStringDegreesMinutesSeconds(DistanceUnit distanceUnit, int angularPrecision, int distancePrecision)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{FormatDegreesMinutesSeconds(LatitudeDecimalDegrees, true)}, {FormatDegreesMinutesSeconds(LongitudeDecimalDegrees, false)}");
            sb.Append(ToStringAltitude(distanceUnit, distancePrecision));
            return sb.ToString();
        }

        internal string ToStringRadians(DistanceUnit distanceUnit, int angularPrecision, int distancePrecision)
        {
            StringBuilder sb = new StringBuilder();
            string angularFormat = "F" + angularPrecision;
            sb.Append($"{LatitudeDecimalRadians.ToString(angularFormat, CultureInfo.InvariantCulture)}, {LongitudeDecimalRadians.ToString(angularFormat, CultureInfo.InvariantCulture)}");
            sb.Append(ToStringAltitude(distanceUnit, distancePrecision));
            return sb.ToString();
        }

        internal string ToStringAltitude(DistanceUnit distanceUnit, int distancePrecision)
        {
            string unitSymbol;
            double altitudeValue;

            switch (distanceUnit)
            {
                case DistanceUnit.Metres:
                    unitSymbol = "m";
                    altitudeValue = _altitude;
                    break;
                case DistanceUnit.Feet:
                    unitSymbol = "ft";
                    altitudeValue = _altitude / FEET_TO_METRES;
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
    }
}
