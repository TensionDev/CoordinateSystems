using System;
using Xunit;

namespace TensionDev.CoordinateSystems.Tests
{
    public class GeohashBase32Tests
    {
        [Theory]
        [InlineData("0123456789bc")]
        [InlineData("ezs42")]
        public void IsValidAcceptsGeohashBase32(String value)
        {
            Assert.True(GeohashBase32.IsValid(value));
        }

        [Theory]
        [InlineData("")]
        [InlineData("ezs42a")]
        [InlineData("iezs42o")]
        public void IsValidRejectsNonGeohashBase32(String value)
        {
            Assert.False(GeohashBase32.IsValid(value));
        }

        [Fact]
        public void EncodeReturnsGeohashBase32String()
        {
            String actual = GeohashBase32.Encode(0b11010, 1);

            Assert.Equal("u", actual);
        }
    }
}
