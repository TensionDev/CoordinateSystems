using System;
using TensionDev.CoordinateSystems;
using Xunit;

namespace XUnitTestProjectCoordinateSystems
{
    public class UnitTestGeocentricCoordinateSystem
    {
        [Fact]
        public void Test1()
        {
            GeocentricCoordinateSystem geocentricCoordinateSystem = new GeocentricCoordinateSystem();

            Assert.Equal(0, geocentricCoordinateSystem.X);
            Assert.Equal(0, geocentricCoordinateSystem.Y);
            Assert.Equal(0, geocentricCoordinateSystem.Z);
        }
    }
}
