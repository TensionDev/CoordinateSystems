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
    /// Geohash Geocode System
    /// </summary>
    public class Geohash
    {
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
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Length > GeohashBase32.MaximumHashLength)
                {
                    throw new ArgumentException($"GeoHash of length {value.Length} is not supported!", nameof(value));
                }

                if (!GeohashBase32.IsValid(value))
                {
                    throw new ArgumentException($"{value} is not based on Geohash Base32!", nameof(value));
                }

                _hash = value;
            }
        }
    }
}
