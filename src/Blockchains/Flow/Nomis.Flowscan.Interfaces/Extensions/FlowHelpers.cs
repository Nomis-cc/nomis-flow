// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

using Nomis.Flowscan.Interfaces.Models;

namespace Nomis.Flowscan.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Flow.
    /// </summary>
    public static class FlowHelpers
    {
        /// <summary>
        /// Convert Wei value to Flow.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Flow.</returns>
        public static decimal ToFlow(this string? valueInWei)
        {
            return BigInteger
                .TryParse(valueInWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var value)
                ? value.ToFlow()
                : 0;
        }

        /// <summary>
        /// Convert Wei value to Flow.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Flow.</returns>
        public static decimal ToFlow(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(10_000_000));
            }

            return (decimal)valueInWei * 0.000_000_01M;
        }

        /// <summary>
        /// Convert Wei value to Flow.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Flow.</returns>
        public static decimal ToFlow(this in decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToFlow();
        }

        /// <summary>
        /// Get token UID based on it Contract and NftId.
        /// </summary>
        /// <param name="token">Token info.</param>
        /// <returns>Returns token UID.</returns>
        public static string GetTokenUid(this FlowscanAccountNftTransferEdgeNode token)
        {
            return $"{token.Nft?.Contract}_{token.Nft?.NftId}";
        }
    }
}