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
    /// Angular representation format for coordinate string output.
    /// </summary>
    public enum AngularFormat
    {
        /// <summary>
        /// Decimal degrees (e.g., 52.5200).
        /// </summary>
        DecimalDegrees,

        /// <summary>
        /// Degrees and decimal minutes (e.g., 52° 31.2').
        /// </summary>
        DegreesDecimalMinutes,

        /// <summary>
        /// Degrees, minutes, and seconds (e.g., 52° 31' 12").
        /// </summary>
        DegreesMinutesSeconds,

        /// <summary>
        /// Radians (e.g., 0.9166).
        /// </summary>
        Radians
    }
}
