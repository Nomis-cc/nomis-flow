// ------------------------------------------------------------------------------------------------------
// <copyright file="ITronscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Tronscan.Interfaces.Models;

namespace Nomis.Tronscan.Interfaces
{
    /// <summary>
    /// Tronscan client.
    /// </summary>
    public interface ITronscanClient
    {
        /// <summary>
        /// Get the account balance.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="TronscanAccount"/>.</returns>
        Task<TronscanAccount> GetBalanceAsync(string address);

        /// <summary>
        /// Get the account tokens.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="TronscanAccountTokens"/>.</returns>
        Task<TronscanAccountTokens> GetTokensAsync(string address);

        /// <summary>
        /// Get list of specific transactions/transfers of the given account.
        /// </summary>
        /// <typeparam name="TResult">The type of returned response.</typeparam>
        /// <typeparam name="TResultItem">The type of returned response data items.</typeparam>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of specific transactions/transfers of the given account.</returns>
        Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : ITronscanTransferList<TResultItem>
            where TResultItem : ITronscanTransfer;

        /// <summary>
        /// Get the account contracts.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="TronscanAccountContracts"/>.</returns>
        Task<TronscanAccountContracts> GetContractsAsync(string address);
    }
}