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
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace TensionDev.CoordinateSystems
{
    /// <summary>
    /// Encodes and decodes the Base32 alphabet used by the Geohash standard.
    /// </summary>
    internal static class GeohashBase32
    {
        public const Int32 BitsPerCharacter = 5;
        public const Int32 MaximumHashLength = 12;

        private const String Base32GeohashPattern = "^(?:[0-9b-hj-km-np-z]+)$";
        private const Int32 RegexTimeoutMilliseconds = 100;
        private const String Alphabet = "0123456789bcdefghjkmnpqrstuvwxyz";

        /// <summary>
        /// Validates whether a value can be represented as a supported Geohash Base32 string.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> when the value is valid; otherwise, <c>false</c>.</returns>
        public static Boolean IsValid(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Length > MaximumHashLength)
            {
                return false;
            }

            Match match = Regex.Match(
                value,
                Base32GeohashPattern,
                RegexOptions.None,
                TimeSpan.FromMilliseconds(RegexTimeoutMilliseconds));

            return match.Success;
        }

        /// <summary>
        /// Encodes a bit notation value into a Geohash Base32 string.
        /// </summary>
        /// <param name="bitNotation">The bit notation value to encode.</param>
        /// <param name="hashLength">The output hash length.</param>
        /// <returns>The encoded Geohash Base32 string.</returns>
        public static String Encode(UInt64 bitNotation, Int32 hashLength)
        {
            if (hashLength <= 0 || hashLength > MaximumHashLength)
            {
                throw new ArgumentOutOfRangeException(nameof(hashLength), $"GeoHash of length {hashLength} is not supported!");
            }

            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < hashLength; ++i)
            {
                UInt64 value = bitNotation % 32;
                bitNotation >>= BitsPerCharacter;

                sb.Insert(0, Alphabet[(Int32)value]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Decodes a Geohash Base32 string into its bit notation.
        /// </summary>
        /// <param name="hash">The Geohash Base32 string.</param>
        /// <returns>The decoded bit notation and bit length.</returns>
        public static (BitArray bitNotation, UInt32 length) Decode(String hash)
        {
            if (!IsValid(hash))
            {
                throw new ArgumentException($"{hash} is not based on Geohash Base32!", nameof(hash));
            }

            UInt64 bitStream = 0;
            UInt32 length = 0;

            for (Int32 i = 0; i < hash.Length; ++i)
            {
                Int32 value = Alphabet.IndexOf(hash[i]);

                if (value < 0)
                {
                    throw new InvalidOperationException("GeoHash is not based on Geohash Base32.");
                }

                bitStream <<= BitsPerCharacter;
                bitStream += (UInt64)value;

                length += BitsPerCharacter;
            }

            BitArray bitNotation = new BitArray(BitConverter.GetBytes(bitStream))
            {
                Length = (Int32)length
            };

            return (bitNotation, length);
        }
    }
}
