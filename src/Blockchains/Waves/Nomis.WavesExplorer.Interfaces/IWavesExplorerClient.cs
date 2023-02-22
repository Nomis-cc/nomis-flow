// ------------------------------------------------------------------------------------------------------
// <copyright file="IWavesExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.WavesExplorer.Interfaces.Models;

namespace Nomis.WavesExplorer.Interfaces
{
    /// <summary>
    /// WavesExplorer client.
    /// </summary>
    public interface IWavesExplorerClient
    {
        /// <summary>
        /// Get the account balance in Wei.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="WavesExplorerBalance"/>.</returns>
        Task<WavesExplorerBalance> GetBalanceAsync(string address);

        /// <summary>
        /// Get list of transactions of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of transactions of the given account.</returns>
        Task<IEnumerable<WavesExplorerTransaction>> GetTransactionsAsync(string address);

        /// <summary>
        /// Get list of assets of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of assets of the given account.</returns>
        Task<WavesExplorerAccountAssetBalances> GetAssetsAsync(string address);

        /// <summary>
        /// Get list of NFT assets of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of NFT assets of the given account.</returns>
        Task<IEnumerable<WavesExplorerAccountNftAsset>> GetNftAssetsAsync(string address);
    }
}