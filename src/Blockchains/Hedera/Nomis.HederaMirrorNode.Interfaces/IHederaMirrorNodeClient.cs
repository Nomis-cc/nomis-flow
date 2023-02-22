// ------------------------------------------------------------------------------------------------------
// <copyright file="IHederaMirrorNodeClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.HederaMirrorNode.Interfaces.Models;

namespace Nomis.HederaMirrorNode.Interfaces
{
    /// <summary>
    /// Hedera Mirror Node client.
    /// </summary>
    public interface IHederaMirrorNodeClient
    {
        /// <summary>
        /// Get the account balance in Tinybars.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="HederaMirrorNodeAccount"/>.</returns>
        Task<HederaMirrorNodeAccount> GetBalanceAsync(string address);

        /// <summary>
        /// Get the account NFTs.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="HederaMirrorNodeNfts"/>.</returns>
        Task<HederaMirrorNodeNfts> GetNftsAsync(string address);

        /// <summary>
        /// Get the NFT transactions.
        /// </summary>
        /// <param name="tokenId">NFT token id.</param>
        /// <param name="serialNumber">NFT serial number.</param>
        /// <returns>Returns the NFT transactions.</returns>
        Task<HederaMirrorNodeNftTransactions> GetNftTransactionsAsync(string tokenId, long serialNumber);
    }
}