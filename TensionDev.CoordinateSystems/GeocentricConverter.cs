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

namespace TensionDev.CoordinateSystems
{
    /// <summary>
    /// Converts supported coordinate representations to geocentric coordinates.
    /// </summary>
    public static class GeocentricConverter
    {
        /// <summary>
        /// Converts geographic coordinates to geocentric coordinates using WGS 84 ellipsoid assumptions.
        /// </summary>
        /// <param name="source">The geographic coordinates to convert.</param>
        /// <returns>The converted geocentric coordinates.</returns>
        public static GeocentricCoordinateSystem From(GeographicCoordinateSystem source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double latitude = source.LatitudeDecimalRadians;
            double longitude = source.LongitudeDecimalRadians;
            double altitude = source.AltitudeMetres;

            double sinLatitude = Math.Sin(latitude);
            double cosLatitude = Math.Cos(latitude);
            double primeVerticalRadius = Wgs84.SemiMajorAxisMetres
                / Math.Sqrt(1.0 - Wgs84.FirstEccentricitySquared * sinLatitude * sinLatitude);

            return new GeocentricCoordinateSystem
            {
                X = (primeVerticalRadius + altitude) * cosLatitude * Math.Cos(longitude),
                Y = (primeVerticalRadius + altitude) * cosLatitude * Math.Sin(longitude),
                Z = (primeVerticalRadius * (1.0 - Wgs84.FirstEccentricitySquared) + altitude) * sinLatitude,
            };
        }
    }
}
