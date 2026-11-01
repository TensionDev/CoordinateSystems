// SPDX-License-Identifier: Apache-2.0
// Copyright 2021 - 2026 TensionDev

using System;
using System.Globalization;

namespace TensionDev.CoordinateSystems
{
    internal static class GeographicAltitudeParser
    {
        private const double FeetToMetres = 0.3048;

        internal static bool TryExtractAltitude(ref string coordinateText, out double altitudeMetres)
        {
            altitudeMetres = 0.0;
            int index = coordinateText.LastIndexOf("Altitude:", StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return true;
            }

            string[] parts = coordinateText.Substring(index + "Altitude:".Length)
                .Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 ||
                !double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double value) ||
                !GeographicCoordinateSystem.IsFinite(value))
            {
                return false;
            }

            if (string.Equals(parts[1], "m", StringComparison.OrdinalIgnoreCase))
            {
                altitudeMetres = value;
            }
            else if (string.Equals(parts[1], "ft", StringComparison.OrdinalIgnoreCase))
            {
                altitudeMetres = value * FeetToMetres;
            }
            else
            {
                return false;
            }

            if (!GeographicCoordinateSystem.IsFinite(altitudeMetres))
            {
                return false;
            }

            coordinateText = coordinateText.Substring(0, index).TrimEnd();
            return true;
        }
    }
}
