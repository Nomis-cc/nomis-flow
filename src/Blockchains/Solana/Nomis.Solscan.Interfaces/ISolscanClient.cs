// ------------------------------------------------------------------------------------------------------
// <copyright file="ISolscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Solscan.Interfaces.Models;

namespace Nomis.Solscan.Interfaces
{
    /// <summary>
    /// Solscan client.
    /// </summary>
    public interface ISolscanClient
    {
        /// <summary>
        /// Get the account balance.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns wallet balance.</returns>
        Task<decimal> GetBalanceAsync(string address);

        /// <summary>
        /// Get list of transactions of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of transactions of the given account.</returns>
        Task<IEnumerable<SolscanTransaction>> GetTransactionsDataAsync(string address);

        /// <summary>
        /// Get the account specific transfers.
        /// </summary>
        /// <typeparam name="TResult">The type of response.</typeparam>
        /// <typeparam name="TResultItem">The type of specific transfers in response.</typeparam>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of specific transfers of the given account.</returns>
        Task<TResult> GetTransfersDataAsync<TResult, TResultItem>(string address)
            where TResult : ISolscanTransferList<TResultItem>, new()
            where TResultItem : ISolscanTransfer;

        /// <summary>
        /// Get the account tokens.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of tokens of the given account.</returns>
        Task<IEnumerable<SolscanToken>> GetTokensAsync(string address);

        /// <summary>
        /// Get the account stakes.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of stakes of the given account.</returns>
        Task<IEnumerable<SolscanStakeAccount>> GetStakesAsync(string address);
    }
}