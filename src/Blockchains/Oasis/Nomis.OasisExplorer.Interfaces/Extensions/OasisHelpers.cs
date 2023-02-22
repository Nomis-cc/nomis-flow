// ------------------------------------------------------------------------------------------------------
// <copyright file="OasisHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

namespace Nomis.OasisExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Oasis.
    /// </summary>
    public static class OasisHelpers
    {
        /// <summary>
        /// Convert Wei value to ROSE.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total ROSE.</returns>
        public static decimal ToRose(this string valueInWei)
        {
            return BigInteger
                .TryParse(valueInWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var value)
                ? value.ToRose()
                : 0;
        }

        /// <summary>
        /// Convert Wei value to ROSE.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total ROSE.</returns>
        public static decimal ToRose(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to ROSE.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total ROSE.</returns>
        public static decimal ToRose(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToRose();
        }
    }
}