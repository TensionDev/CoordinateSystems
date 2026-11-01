using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeocentricCoordinateSystemTests : IDisposable
    {
        private bool disposedValue;

        private const Int32 POSITIONAL_PRECISION = 5;

        public GeocentricCoordinateSystemTests()
        {
        }

        [Fact]
        public void TestDefaultConstructor()
        {
            GeocentricCoordinateSystem geocentricCoordinateSystem = new GeocentricCoordinateSystem();

            Assert.Equal(0, geocentricCoordinateSystem.X, POSITIONAL_PRECISION);
            Assert.Equal(0, geocentricCoordinateSystem.Y, POSITIONAL_PRECISION);
            Assert.Equal(0, geocentricCoordinateSystem.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Metres_ReturnsCorrectValues()
        {
            string input = "X: 1234.567 m, Y: 5678.901 m, Z: 9012.345 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(1234.567, result.X, POSITIONAL_PRECISION);
            Assert.Equal(5678.901, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(9012.345, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Feet_ReturnsCorrectValues()
        {
            string input = "X: 4084.334 ft, Y: 18637.209 ft, Z: 29567.989 ft";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            // Per ADR-0008 Section 66: all geocentric coordinates must be in metres after parsing
            // Feet values are converted to metres (multiply by 0.3048).
            double xMetres = 4084.334 * 0.3048;
            double yMetres = 18637.209 * 0.3048;
            double zMetres = 29567.989 * 0.3048;

            Assert.Equal(xMetres, result.X, POSITIONAL_PRECISION);
            Assert.Equal(yMetres, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(zMetres, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_SixDecimalPrecision()
        {
            string input = "X: 1000.123456 m, Y: 2000.789012 m, Z: 3000.345678 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(1000.123456, result.X, POSITIONAL_PRECISION);
            Assert.Equal(2000.789012, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(3000.345678, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_NegativeValues()
        {
            string input = "X: -1000.500 m, Y: -2000.750 m, Z: -3000.250 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(-1000.500, result.X, POSITIONAL_PRECISION);
            Assert.Equal(-2000.750, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(-3000.250, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_ZeroValues()
        {
            string input = "X: 0.000 m, Y: 0.000 m, Z: 0.000 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(0, result.X, POSITIONAL_PRECISION);
            Assert.Equal(0, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(0, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_LargeValues()
        {
            string input = "X: 99999.999 m, Y: 88888.888 m, Z: 77777.777 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(99999.999, result.X, POSITIONAL_PRECISION);
            Assert.Equal(88888.888, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(77777.777, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_FeetUnits_MixedCase()
        {
            string input = "X: 4000.000 FT, Y: 5000.000 Ft, Z: 6000.000 ft";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            // Per ADR-0008 Section 66: all geocentric coordinates must be in metres after parsing
            double xMetres = 4000.000 * 0.3048;
            double yMetres = 5000.000 * 0.3048;
            double zMetres = 6000.000 * 0.3048;

            Assert.Equal(xMetres, result.X, POSITIONAL_PRECISION);
            Assert.Equal(yMetres, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(zMetres, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestTryParse_Metres_ReturnsTrue()
        {
            string input = "X: 1000.000 m, Y: 2000.000 m, Z: 3000.000 m";
            bool result = GeocentricCoordinateSystem.TryParse(input, out var system);

            Assert.True(result);
            Assert.Equal(1000.000, system.X, POSITIONAL_PRECISION);
            Assert.Equal(2000.000, system.Y, POSITIONAL_PRECISION);
            Assert.Equal(3000.000, system.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestTryParse_Feet_ReturnsTrue()
        {
            string input = "X: 3280.840 ft, Y: 6561.680 ft, Z: 9842.520 ft";
            bool result = GeocentricCoordinateSystem.TryParse(input, out var system);

            Assert.True(result);
            // Per ADR-0008 Section 66: all geocentric coordinates must be in metres after parsing
            double xMetres = 3280.840 * 0.3048;
            double yMetres = 6561.680 * 0.3048;
            double zMetres = 9842.520 * 0.3048;

            Assert.Equal(xMetres, system.X, POSITIONAL_PRECISION);
            Assert.Equal(yMetres, system.Y, POSITIONAL_PRECISION);
            Assert.Equal(zMetres, system.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestTryParse_Null_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse(null, out var system);
            Assert.False(result);
            Assert.Equal(default(double), system.X);
        }

        [Fact]
        public void TestTryParse_EmptyString_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_WhitespaceOnly_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("   \t\n  ", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_MissingX_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("Y: 1000.000 m, Z: 2000.000 m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_MissingY_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("X: 1000.000 m, Z: 2000.000 m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_MissingZ_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("X: 1000.000 m, Y: 2000.000 m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_MissingCommas_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("X: 1000.000 m Y: 2000.000 m Z: 3000.000 m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_InvalidX_Value_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("X: abc m, Y: 2000.000 m, Z: 3000.000 m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_InvalidY_Value_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("X: 1000.000 m, Y: xyz m, Z: 3000.000 m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_InvalidZ_Value_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("X: 1000.000 m, Y: 2000.000 m, Z: qwe m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_MixedUnits_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("X: 1000.000 m, Y: 2000.000 ft, Z: 3000.000 m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestTryParse_MissingUnit_ReturnsFalse()
        {
            bool result = GeocentricCoordinateSystem.TryParse("X: 1000.000, Y: 2000.000 m, Z: 3000.000 m", out _);
            Assert.False(result);
        }

        [Fact]
        public void TestParse_InvalidInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => GeocentricCoordinateSystem.Parse(null));
        }

        [Fact]
        public void TestParse_EmptyString_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeocentricCoordinateSystem.Parse(""));
        }

        [Fact]
        public void TestParse_WhitespaceString_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeocentricCoordinateSystem.Parse("   "));
        }

        [Fact]
        public void TestParse_MalformedInput_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeocentricCoordinateSystem.Parse("X: 1000.000 m, Y: 2000.000 m"));
        }

        [Fact]
        public void TestParse_MalformedValues_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => GeocentricCoordinateSystem.Parse("X: abc m, Y: 2000.000 m, Z: 3000.000 m"));
        }

        [Fact]
        public void TestParse_InvalidUnit_ReturnsDefault()
        {
            Assert.False(GeocentricCoordinateSystem.TryParse("X: 1000.000 xyz, Y: 2000.000 m, Z: 3000.000 m", out _));
        }

        [Fact]
        public void TestParse_MissingComma_ReturnsDefault()
        {
            Assert.False(GeocentricCoordinateSystem.TryParse("X: 1000.000 m Y: 2000.000 m Z: 3000.000 m", out _));
        }

        [Fact]
        public void TestParse_TinyValues()
        {
            string input = "X: 0.001 m, Y: 0.002 m, Z: 0.003 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(0.001, result.X, POSITIONAL_PRECISION);
            Assert.Equal(0.002, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(0.003, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_HighPrecisionValues()
        {
            string input = "X: 1.5e+10 m, Y: 2.7e+10 m, Z: 3.9e+10 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(1.5e+10, result.X, POSITIONAL_PRECISION);
            Assert.Equal(2.7e+10, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(3.9e+10, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Feet_WithScientificNotation()
        {
            string input = "X: 4.1e+3 ft, Y: 6.2e+3 ft, Z: 8.3e+3 ft";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            // Per ADR-0008 Section 66: all geocentric coordinates must be in metres after parsing
            double xMetres = 4100.0 * 0.3048;
            double yMetres = 6200.0 * 0.3048;
            double zMetres = 8300.0 * 0.3048;

            Assert.Equal(xMetres, result.X, POSITIONAL_PRECISION);
            Assert.Equal(yMetres, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(zMetres, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Metres_WithNegativeZero()
        {
            string input = "X: -0.000 m, Y: -0.000 m, Z: -0.000 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(0, result.X, POSITIONAL_PRECISION);
            Assert.Equal(0, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(0, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_MixedPositiveNegative()
        {
            string input = "X: 1000.500 m, Y: -2000.250 m, Z: 3000.750 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(1000.500, result.X, POSITIONAL_PRECISION);
            Assert.Equal(-2000.250, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(3000.750, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_UnitAtEnd()
        {
            string input = "X: 1234.567890 m, Y: 5678.901234 m, Z: 9012.345678 m";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            Assert.Equal(1234.567890, result.X, POSITIONAL_PRECISION);
            Assert.Equal(5678.901234, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(9012.345678, result.Z, POSITIONAL_PRECISION);
        }

        [Fact]
        public void TestParse_Feet_ConversionFromMetres()
        {
            // Parse feet value that is exactly 1000 metres converted (3280.84 ft ≈ 1000 m)
            string input = "X: 3280.840 ft, Y: 6561.680 ft, Z: 9842.520 ft";
            GeocentricCoordinateSystem result = GeocentricCoordinateSystem.Parse(input);

            // Verify the values are approximately correct (feet -> metres conversion per ADR-0008 Section 66)
            double xMetres = 3280.840 * 0.3048;
            double yMetres = 6561.680 * 0.3048;
            double zMetres = 9842.520 * 0.3048;

            Assert.Equal(xMetres, result.X, POSITIONAL_PRECISION);
            Assert.Equal(yMetres, result.Y, POSITIONAL_PRECISION);
            Assert.Equal(zMetres, result.Z, POSITIONAL_PRECISION);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitTestGeocentricCoordinateSystem()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
