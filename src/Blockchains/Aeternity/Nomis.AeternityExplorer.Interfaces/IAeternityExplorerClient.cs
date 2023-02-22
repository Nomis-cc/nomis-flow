// ------------------------------------------------------------------------------------------------------
// <copyright file="IAeternityExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.AeternityExplorer.Interfaces.Models;

namespace Nomis.AeternityExplorer.Interfaces
{
    /// <summary>
    /// AeternityExplorer client.
    /// </summary>
    public interface IAeternityExplorerClient
    {
        /// <summary>
        /// Get the account balance in Wei.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="AeternityExplorerAccount"/>.</returns>
        Task<AeternityExplorerAccount> GetBalanceAsync(string address);

        /// <summary>
        /// Get list of specific transactions/transfers of the given account.
        /// </summary>
        /// <typeparam name="TResult">The type of returned response.</typeparam>
        /// <typeparam name="TResultItem">The type of returned response data items.</typeparam>
        /// <param name="address">Account address.</param>
        /// <param name="from">From account.</param>
        /// <returns>Returns list of specific transactions/transfers of the given account.</returns>
        Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address, bool from = true)
            where TResult : IAeternityExplorerTransferList<TResultItem>
            where TResultItem : class;
    }
}