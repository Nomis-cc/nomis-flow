// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

namespace Nomis.Xrpscan.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for XRP.
    /// </summary>
    public static class XrpHelpers
    {
        /// <summary>
        /// Convert Wei value to XRP.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total XRP.</returns>
        public static decimal ToXrp(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000));
            }

            return (decimal)valueInWei * 0.000_001M;
        }

        /// <summary>
        /// Convert Wei value to XRP.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total XRP.</returns>
        public static decimal ToXrp(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToXrp();
        }
    }
}