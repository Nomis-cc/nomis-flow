// ------------------------------------------------------------------------------------------------------
// <copyright file="FuseHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

namespace Nomis.FuseExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Fuse.
    /// </summary>
    public static class FuseHelpers
    {
        /// <summary>
        /// Convert Wei value to FUSE.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total FUSE.</returns>
        public static decimal ToFuse(this string valueInWei)
        {
            return BigInteger
                .TryParse(valueInWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var value)
                ? value.ToFuse()
                : 0;
        }

        /// <summary>
        /// Convert Wei value to FUSE.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total FUSE.</returns>
        public static decimal ToFuse(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to FUSE.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total FUSE.</returns>
        public static decimal ToFuse(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToFuse();
        }
    }
}