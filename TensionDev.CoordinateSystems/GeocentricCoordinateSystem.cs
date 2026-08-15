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
    }
}
