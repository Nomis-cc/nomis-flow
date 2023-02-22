// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonriverHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

using Nomis.MoonriverMoonscan.Interfaces.Models;

namespace Nomis.MoonriverMoonscan.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for moonriver.
    /// </summary>
    public static class MoonriverHelpers
    {
        /// <summary>
        /// Convert Wei value to MOVR.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total MOVR.</returns>
        public static decimal ToMovr(this string valueInWei)
        {
            return BigInteger
                .TryParse(valueInWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var value)
                ? value.ToMovr()
                : 0;
        }

        /// <summary>
        /// Convert Wei value to MOVR.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total MOVR.</returns>
        public static decimal ToMovr(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to MOVR.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total MOVR.</returns>
        public static decimal ToMovr(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToMovr();
        }

        /// <summary>
        /// Get token UID based on it ContractAddress and Id.
        /// </summary>
        /// <param name="token">Token info.</param>
        /// <returns>Returns token UID.</returns>
        public static string GetTokenUid(this IMoonriverMoonscanAccountNftTokenEvent token)
        {
            return token.ContractAddress + "_" + token.TokenId;
        }
    }
}