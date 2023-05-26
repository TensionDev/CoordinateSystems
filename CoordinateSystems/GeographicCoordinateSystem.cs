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
        private double latitude;
        private double longitude;
        private double altitude;

        /// <summary>
        /// Latitude in Radians
        /// </summary>
        public Double LatitudeDecimalRadians { get => latitude * Math.PI / 180.0; set => LatitudeDecimalDegrees = value * 180.0 / Math.PI; }

        /// <summary>
        /// Longitude in Radians
        /// </summary>
        public Double LongitudeDecimalRadians { get => longitude * Math.PI / 180.0; set => LongitudeDecimalDegrees = value * 180.0 / Math.PI; }

        /// <summary>
        /// Altitude in Feet
        /// </summary>
        public Double AltitudeFeet { get => altitude / 0.3048; set => altitude = value * 0.3048; }

        /// <summary>
        /// Latitude in Degrees
        /// </summary>
        public Double LatitudeDecimalDegrees { get => latitude; set => latitude = Math.Max(Math.Min(value, 90), -90); }

        /// <summary>
        /// Longitude in Degrees
        /// </summary>
        public Double LongitudeDecimalDegrees { get => longitude; set => longitude = Math.Max(Math.Min(value, 180), -180); }

        /// <summary>
        /// Altitude in Metres
        /// </summary>
        public Double AltitudeMetres { get => altitude; set => altitude = value; }
    }
}
