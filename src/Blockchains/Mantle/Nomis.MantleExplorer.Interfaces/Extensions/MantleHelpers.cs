// ------------------------------------------------------------------------------------------------------
// <copyright file="MantleHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

namespace Nomis.MantleExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Mantle.
    /// </summary>
    public static class MantleHelpers
    {
        /// <summary>
        /// Convert Wei value to BIT.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total BIT.</returns>
        public static decimal ToBit(this string valueInWei)
        {
            return BigInteger
                .TryParse(valueInWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var value)
                ? value.ToBit()
                : 0;
        }

        /// <summary>
        /// Convert Wei value to BIT.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total BIT.</returns>
        public static decimal ToBit(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to BIT.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total BIT.</returns>
        public static decimal ToBit(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToBit();
        }
    }
}