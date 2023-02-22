// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.AeternityExplorer.Interfaces.Models;

namespace Nomis.AeternityExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for aeternity.
    /// </summary>
    public static class AeternityHelpers
    {
        /// <summary>
        /// Convert Wei value to Aeternity.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Aeternity.</returns>
        public static decimal ToAeternity(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert Wei value to Aeternity.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Aeternity.</returns>
        public static decimal ToAeternity(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToAeternity();
        }

        /// <summary>
        /// Get token UID based on it ContractAddress and Id.
        /// </summary>
        /// <param name="token">Token info.</param>
        /// <returns>Returns token UID.</returns>
        public static string GetTokenUid(this AeternityExplorerAccountAEX141TokenEvent token)
        {
            return token.ContractId + "_" + token.TokenId;
        }
    }
}