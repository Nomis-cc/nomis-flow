// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.HederaMirrorNode.Interfaces.Models;

namespace Nomis.HederaMirrorNode.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for hedera.
    /// </summary>
    public static class HederaHelpers
    {
        /// <summary>
        /// Convert tinybars value to hbars.
        /// </summary>
        /// <param name="valueInTinybars">Tinybars.</param>
        /// <returns>Returns total hbars.</returns>
        public static decimal ToHbars(this string valueInTinybars)
        {
            if (!decimal.TryParse(valueInTinybars, out decimal tinybars))
            {
                return 0;
            }

            return tinybars.ToHbars();
        }

        /// <summary>
        /// Convert tinybars value to hbars.
        /// </summary>
        /// <param name="valueInTinybars">Tinybars.</param>
        /// <returns>Returns total hbars.</returns>
        public static decimal ToHbars(this ulong valueInTinybars)
        {
            return valueInTinybars * 0.00_000_001M;
        }

        /// <summary>
        /// Convert tinybars value to hbars.
        /// </summary>
        /// <param name="valueInTinybars">Tinybars.</param>
        /// <returns>Returns total hbars.</returns>
        public static decimal ToHbars(this in BigInteger valueInTinybars)
        {
            return (decimal)valueInTinybars * 0.00_000_001M;
        }

        /// <summary>
        /// Convert tinybars value to hbars.
        /// </summary>
        /// <param name="valueInTinybars">Tinybars.</param>
        /// <returns>Returns total hbars.</returns>
        public static decimal ToHbars(this decimal valueInTinybars)
        {
            return new BigInteger(valueInTinybars).ToHbars();
        }

        /// <summary>
        /// Get token UID based on it Account Id and Token Id.
        /// </summary>
        /// <param name="token">Token info.</param>
        /// <returns>Returns token UID.</returns>
        public static string GetTokenUid(this HederaMirrorNodeNftData token)
        {
            return token.AccountId + "_" + token.TokenId;
        }
    }
}