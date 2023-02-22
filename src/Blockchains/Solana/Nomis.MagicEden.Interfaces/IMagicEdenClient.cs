// ------------------------------------------------------------------------------------------------------
// <copyright file="IMagicEdenClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.MagicEden.Interfaces.Models;

namespace Nomis.MagicEden.Interfaces
{
    /// <summary>
    /// MagicEden client.
    /// </summary>
    public interface IMagicEdenClient
    {
        /// <summary>
        /// Get MagicEden activity of the given account.
        /// </summary>
        /// <param name="address">Solana wallet address.</param>
        /// <returns>TODO.</returns>
        Task<IEnumerable<MagicEdenWalletActivity>> GetWalletActivitiesData(string address);
    }
}