// ------------------------------------------------------------------------------------------------------
// <copyright file="TonHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

namespace Nomis.Tonscan.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for TON.
    /// </summary>
    public static class TonHelpers
    {
        /// <summary>
        /// Convert Wei value to TON.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total TON.</returns>
        public static decimal ToTon(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000));
            }

            return (decimal)valueInWei * 0.000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to TON.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total TON.</returns>
        public static decimal ToTon(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToTon();
        }
    }
}