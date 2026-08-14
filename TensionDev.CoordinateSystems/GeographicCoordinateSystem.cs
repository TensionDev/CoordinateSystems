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
    }
}
