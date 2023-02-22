﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="CronosHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

using Nomis.Cronoscan.Interfaces.Models;

namespace Nomis.Cronoscan.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Cronos.
    /// </summary>
    public static class CronosHelpers
    {
        /// <summary>
        /// Convert Wei value to CRO.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total CRO.</returns>
        public static decimal ToCro(this string valueInWei)
        {
            return BigInteger
                .TryParse(valueInWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var value)
                ? value.ToCro()
                : 0;
        }

        /// <summary>
        /// Convert Wei value to CRO.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total CRO.</returns>
        public static decimal ToCro(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to CRO.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total CRO.</returns>
        public static decimal ToCro(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToCro();
        }

        /// <summary>
        /// Get token UID based on it ContractAddress and Id.
        /// </summary>
        /// <param name="token">Token info.</param>
        /// <returns>Returns token UID.</returns>
        public static string GetTokenUid(this ICronoscanAccountNftTokenEvent token)
        {
            return token.ContractAddress + "_" + token.TokenId;
        }
    }
}