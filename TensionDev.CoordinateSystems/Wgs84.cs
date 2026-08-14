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
    internal static class Wgs84
    {
        internal const double SemiMajorAxisMetres = 6378137.0;
        internal const double InverseFlattening = 298.257223563;
        internal const double Flattening = 1.0 / InverseFlattening;
        internal const double SemiMinorAxisMetres = SemiMajorAxisMetres * (1.0 - Flattening);
        internal const double FirstEccentricitySquared = Flattening * (2.0 - Flattening);
        internal const double SecondEccentricitySquared =
            FirstEccentricitySquared / (1.0 - FirstEccentricitySquared);
    }
}
