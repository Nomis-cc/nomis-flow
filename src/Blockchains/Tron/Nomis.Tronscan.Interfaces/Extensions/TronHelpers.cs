// ------------------------------------------------------------------------------------------------------
// <copyright file="TronHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Tronscan.Interfaces.Models;

namespace Nomis.Tronscan.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for tron.
    /// </summary>
    public static class TronHelpers
    {
        /// <summary>
        /// Convert Wei value to TRX.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total TRX.</returns>
        public static decimal ToTrx(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000));
            }

            return (decimal)valueInWei * 0.000_001M;
        }

        /// <summary>
        /// Convert Wei value to TRX.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total TRX.</returns>
        public static decimal ToTrx(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToTrx();
        }

        /// <summary>
        /// Get token UID based on it Token Name and Id.
        /// </summary>
        /// <param name="token">Token info.</param>
        /// <returns>Returns token UID.</returns>
        public static string GetTokenUid(this TronscanAccountTransfer token)
        {
            return token.TokenInfo + "_" + token.TokenInfo?.TokenId;
        }
    }
}