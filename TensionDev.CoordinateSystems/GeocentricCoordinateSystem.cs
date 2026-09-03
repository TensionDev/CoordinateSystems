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
    }
}
