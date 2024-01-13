using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace TensionDev.CoordinateSystems
{
    /// <summary>
    /// Geohash Geocode System
    /// </summary>
    public class Geohash
    {
        private const String REGEX_32GHS = "^(?:[0-9b-hj-km-np-z]+)$";

        private String _hash;

        /// <summary>
        /// Constructs a Geohash object based on provided value.
        /// </summary>
        /// <param name="hash">Geohash value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Geohash(String hash)
        {
            Hash = hash;
        }

        /// <summary>
        /// Geohash
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public String Hash
        {
            get => _hash;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value));

                if (value.Length > 12)
                    throw new ArgumentException($"GeoHash of length {value.Length} is not supported!", nameof(value));

                Match match = Regex.Match(value, REGEX_32GHS);
                if (!match.Success)
                    throw new ArgumentException($"{value} is not based on 32ghs!", nameof(value));

                _hash = value;
            }
        }

        public GeographicCoordinateSystem ToGeographicCoordinateSystem()
        {
            (BitArray bitNotation, UInt32 length) = ToBitNotation();

            GeographicCoordinateSystem result = BitNotationDivisions(bitNotation, length);

            return result;
        }

        private (BitArray bitNotation, UInt32 length) ToBitNotation()
        {
            if (_hash.Length > 12)
                throw new InvalidOperationException();

            const Int32 baseLength = 5;

            UInt64 bitStream = 0;
            UInt32 length = 0;

            for (Int32 i = 0; i < _hash.Length; ++i)
            {
                Char c = _hash[i];
                UInt64 value;

                if (c >= '0' && c <= '9')
                    value = (UInt64)c - 48;
                else if (c >= 'b' && c <= 'h')
                    value = (UInt64)c - 88;
                else if (c >= 'j' && c <= 'k')
                    value = (UInt64)c - 89;
                else if (c >= 'm' && c <= 'n')
                    value = (UInt64)c - 90;
                else if (c >= 'p' && c <= 'z')
                    value = (UInt64)c - 91;
                else
                    throw new InvalidOperationException("GeoHash is not based on 32ghs.");

                bitStream <<= baseLength;
                bitStream += value;

                length += baseLength;
            }

            BitArray bitNotation = new BitArray(BitConverter.GetBytes(bitStream))
            {
                Length = (int)length
            };

            return (bitNotation, length);
        }

        private static GeographicCoordinateSystem BitNotationDivisions(BitArray bitNotation, UInt32 length)
        {
            if (length % 5 != 0)
                throw new ArgumentException($"Parameter length \"{length}\" is not a multiple of 5!", nameof(length));

            if (bitNotation.Count % 5 != 0)
                throw new ArgumentException($"Parameter bitNotation length is not a multiple of 5!", nameof(bitNotation));

            Double latitudeMin = -90;
            Double latitudeMax = 90;
            Double longitudeMin = -180;
            Double longitudeMax = 180;

            for (Int32 i = 0; i < length; ++i)
            {
                Int32 index = (int)(length - i - 1);
                if (i % 2 == 0)
                {
                    (longitudeMin, longitudeMax) = BitNotationDivision(bitNotation[index], longitudeMin, longitudeMax);
                }
                else
                {
                    (latitudeMin, latitudeMax) = BitNotationDivision(bitNotation[index], latitudeMin, latitudeMax);
                }
            }

            return new GeographicCoordinateSystem()
            {
                LatitudeDecimalDegrees = (latitudeMin + latitudeMax) / 2.0,
                LongitudeDecimalDegrees = (longitudeMin + longitudeMax) / 2.0,
                AltitudeMetres = 0
            };
        }

        private static (Double min, Double max) BitNotationDivision(Boolean bit, Double min, Double max)
        {
            Double mean = (min + max) / 2.0;

            if (bit)
            {
                return (mean, max);
            }
            else
            {
                return (min, mean);
            }
        }
    }
}
