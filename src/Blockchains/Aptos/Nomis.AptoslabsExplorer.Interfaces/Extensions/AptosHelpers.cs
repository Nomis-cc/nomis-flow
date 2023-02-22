// ------------------------------------------------------------------------------------------------------
// <copyright file="AptosHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

namespace Nomis.AptoslabsExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Aptos.
    /// </summary>
    public static class AptosHelpers
    {
        /// <summary>
        /// Convert Wei value to Aptos.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Aptos.</returns>
        public static decimal ToAptos(this ulong valueInWei)
        {
            return new BigInteger(valueInWei).ToAptos();
        }

        /// <summary>
        /// Convert Wei value to Aptos.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Aptos.</returns>
        public static decimal ToAptos(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(10_000_000));
            }

            return (decimal)valueInWei * 0.000_000_01M;
        }

        /// <summary>
        /// Convert Wei value to Aptos.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Aptos.</returns>
        public static decimal ToAptos(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToAptos();
        }
    }
}