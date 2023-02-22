// ------------------------------------------------------------------------------------------------------
// <copyright file="IEvmosAPIClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.EvmosAPI.Interfaces.Models;

namespace Nomis.EvmosAPI.Interfaces
{
    /// <summary>
    /// EvmosAPI client.
    /// </summary>
    public interface IEvmosAPIClient
    {
        /// <summary>
        /// Get the account balance.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="EvmosAPIAccount"/>.</returns>
        Task<EvmosAPIAccount> GetBalanceAsync(string address);

        /// <summary>
        /// Get the account owned token list.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns the list of <see cref="EvmosAPIAccountCommonTokenEvent"/>.</returns>
        Task<IEnumerable<EvmosAPIAccountCommonTokenEvent>> GetOwnedTokensAsync(string address);

        /// <summary>
        /// Get list of specific transactions/transfers of the given account.
        /// </summary>
        /// <typeparam name="TResult">The type of returned response.</typeparam>
        /// <typeparam name="TResultItem">The type of returned response data items.</typeparam>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of specific transactions/transfers of the given account.</returns>
        Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IEvmosAPITransferList<TResultItem>
            where TResultItem : IEvmosAPITransfer;
    }
}