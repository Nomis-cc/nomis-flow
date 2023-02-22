// ------------------------------------------------------------------------------------------------------
// <copyright file="KaruraHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

namespace Nomis.KaruraExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Karura.
    /// </summary>
    public static class KaruraHelpers
    {
        /// <summary>
        /// Convert Wei value to KAR.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total KAR.</returns>
        public static decimal ToKar(this string valueInWei)
        {
            return BigInteger
                .TryParse(valueInWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var value)
                ? value.ToKar()
                : 0;
        }

        /// <summary>
        /// Convert Wei value to KAR.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total KAR.</returns>
        public static decimal ToKar(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to KAR.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total KAR.</returns>
        public static decimal ToKar(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToKar();
        }
    }
}