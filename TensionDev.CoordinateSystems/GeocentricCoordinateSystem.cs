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
        /// <returns>A formatted string representation of the geocentric coordinate system</returns>
        public string ToString(DistanceUnit distanceUnit = DistanceUnit.Metres)
        {
            switch (distanceUnit)
            {
                case DistanceUnit.Metres:
                    return ToStringMetres();
                case DistanceUnit.Feet:
                    return ToStringFeet();
                default:
                    throw new NotImplementedException();
            }
        }

        internal string ToStringMetres()
        {
            return $"X: {X:F2} m, Y: {Y:F2} m, Z: {Z:F2} m";
        }

        internal string ToStringFeet()
        {
            return $"X: {X / 0.3048:F2} ft, Y: {Y / 0.3048:F2} ft, Z: {Z / 0.3048:F2} ft";
        }
    }
}
