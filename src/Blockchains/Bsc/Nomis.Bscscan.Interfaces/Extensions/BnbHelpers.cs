// ------------------------------------------------------------------------------------------------------
// <copyright file="BnbHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using EthScanNet.Lib.Models.ApiResponses.Accounts.Models;

namespace Nomis.Bscscan.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for BSC.
    /// </summary>
    public static class BscHelpers
    {
        /// <summary>
        /// Convert Wei value to BNB.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total BNB.</returns>
        public static decimal ToBnb(this BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to BNB.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total BNB.</returns>
        public static decimal ToBnb(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToBnb();
        }

        /// <summary>
        /// Get token UID based on it ContractAddress and Id.
        /// </summary>
        /// <param name="token">Token info.</param>
        /// <returns>Returns token UID.</returns>
        public static string GetTokenUid(this EScanTokenTransferEvent token)
        {
            return token.ContractAddress + "_" + token.TokenId;
        }
    }
}