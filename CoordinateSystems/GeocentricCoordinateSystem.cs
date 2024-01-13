using System;
using System.Collections.Generic;
using System.Text;

namespace TensionDev.CoordinateSystems
{
    /// <summary>
    /// Geocentric Coordinates
    /// </summary>
    public class GeocentricCoordinateSystem
    {
        private double _x;
        private double _y;
        private double _z;

        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }
        public double Z { get => _z; set => _z = value; }
    }
}
