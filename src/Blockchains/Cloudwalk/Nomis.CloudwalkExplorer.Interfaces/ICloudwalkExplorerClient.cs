// ------------------------------------------------------------------------------------------------------
// <copyright file="ICloudwalkExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.CloudwalkExplorer.Interfaces.Models;

namespace Nomis.CloudwalkExplorer.Interfaces
{
    /// <summary>
    /// Cloudwalk Explorer client.
    /// </summary>
    public interface ICloudwalkExplorerClient
    {
        /// <summary>
        /// Get the account balance in Wei.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="CloudwalkExplorerAccount"/>.</returns>
        Task<CloudwalkExplorerAccount> GetBalanceAsync(string address);

        /// <summary>
        /// Get the account token balance in Wei.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <param name="contractAddress">Token contract address.</param>
        /// <returns>Returns <see cref="CloudwalkExplorerAccount"/>.</returns>
        Task<CloudwalkExplorerAccount> GetTokenBalanceAsync(string address, string contractAddress);

        /// <summary>
        /// Get list of specific transactions/transfers of the given account.
        /// </summary>
        /// <typeparam name="TResult">The type of returned response.</typeparam>
        /// <typeparam name="TResultItem">The type of returned response data items.</typeparam>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of specific transactions/transfers of the given account.</returns>
        Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : ICloudwalkExplorerTransferList<TResultItem>
            where TResultItem : ICloudwalkExplorerTransfer;
    }
}