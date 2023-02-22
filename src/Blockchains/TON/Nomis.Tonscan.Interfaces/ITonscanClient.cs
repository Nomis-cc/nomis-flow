// ------------------------------------------------------------------------------------------------------
// <copyright file="ITonscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Tonscan.Interfaces.Models;

namespace Nomis.Tonscan.Interfaces
{
    /// <summary>
    /// Tonscan client.
    /// </summary>
    public interface ITonscanClient
    {
        /// <summary>
        /// Get the account balance in Wei.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="TonscanBalance"/>.</returns>
        Task<TonscanBalance> GetBalanceAsync(string address);

        /// <summary>
        /// Get list of transactions of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of <see cref="TonscanTransaction"/> of the given account.</returns>
        Task<IEnumerable<TonscanTransaction>> GetTransactionsAsync(string address);

        /// <summary>
        /// Get list of assets of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="TonscanAccountAssetBalances"/>.</returns>
        Task<TonscanAccountAssetBalances> GetAssetsAsync(string address);
    }
}