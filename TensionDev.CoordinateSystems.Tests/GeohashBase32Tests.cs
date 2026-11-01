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
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData("ezs42a")]
        [InlineData("iezs42o")]
        [InlineData("0123456789bcd")]
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

        [Fact]
        public void DecodeReturnsExpectedBits()
        {
            (System.Collections.BitArray bits, UInt32 length) = GeohashBase32.Decode("u");

            Assert.Equal((UInt32)5, length);
            Assert.Equal(5, bits.Count);
            Assert.Equal(new[] { false, true, false, true, true }, ToArray(bits));
        }

        [Fact]
        public void DecodeAcceptsMaximumHashLength()
        {
            (System.Collections.BitArray bits, UInt32 length) = GeohashBase32.Decode("0123456789bc");

            Assert.Equal((UInt32)(GeohashBase32.MaximumHashLength * GeohashBase32.BitsPerCharacter), length);
            Assert.Equal(GeohashBase32.MaximumHashLength * GeohashBase32.BitsPerCharacter, bits.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("bad!")]
        [InlineData("0123456789bcd")]
        public void DecodeRejectsInvalidGeohashBase32(String value)
        {
            Assert.Throws<ArgumentException>(() => GeohashBase32.Decode(value));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void EncodeRejectsUnsupportedLength(Int32 length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => GeohashBase32.Encode(0, length));
        }

        private static Boolean[] ToArray(System.Collections.BitArray bits)
        {
            Boolean[] values = new Boolean[bits.Count];
            bits.CopyTo(values, 0);
            return values;
        }
    }
}
