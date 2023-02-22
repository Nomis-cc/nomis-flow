// ------------------------------------------------------------------------------------------------------
// <copyright file="IKlaytnscopeClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Klaytnscope.Interfaces.Models;

namespace Nomis.Klaytnscope.Interfaces
{
    /// <summary>
    /// Klaytnscope client.
    /// </summary>
    public interface IKlaytnscopeClient
    {
        /// <summary>
        /// Get the account balance in Wei.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="KlaytnscopeAccount"/>.</returns>
        Task<KlaytnscopeAccount> GetBalanceAsync(string address);

        /// <summary>
        /// Get the account token balances.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="KlaytnscopeAccountTokenBalances"/>.</returns>
        Task<KlaytnscopeAccountTokenBalances> GetTokenBalancesAsync(string address);

        /// <summary>
        /// Get list of specific transactions/transfers of the given account.
        /// </summary>
        /// <typeparam name="TResult">The type of returned response.</typeparam>
        /// <typeparam name="TResultItem">The type of returned response data items.</typeparam>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of specific transactions/transfers of the given account.</returns>
        Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IKlaytnscopeTransferList<TResultItem>
            where TResultItem : class;
    }
}