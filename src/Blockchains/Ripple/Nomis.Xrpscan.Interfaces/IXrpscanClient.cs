// ------------------------------------------------------------------------------------------------------
// <copyright file="IXrpscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Xrpscan.Interfaces.Models;

namespace Nomis.Xrpscan.Interfaces
{
    /// <summary>
    /// Xrpscan client.
    /// </summary>
    public interface IXrpscanClient
    {
        /// <summary>
        /// Get the account data.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns account data.</returns>
        Task<XrpscanAccount> GetAccountDataAsync(string address);

        /// <summary>
        /// Get list of transactions of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of transactions of the given account.</returns>
        Task<IEnumerable<XrpscanTransaction>> GetTransactionsDataAsync(string address);

        /// <summary>
        /// Get list of NFTs of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of NFTs of the given account.</returns>
        Task<IEnumerable<XrpscanNft>> GetNftsDataAsync(string address);

        /// <summary>
        /// Get list of assets of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        Task<IEnumerable<XrpscanAsset>> GetAssetsDataAsync(string address);

        /// <summary>
        /// Get list of orders of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        Task<IEnumerable<XrpscanOrder>> GetOrdersDataAsync(string address);

        /// <summary>
        /// Get KYC status of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        Task<XrpscanKyc> GetKycDataAsync(string address);

        /// <summary>
        /// Get list of obligations of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        Task<IEnumerable<XrpscanObligation>> GetObligationsDataAsync(string address);
    }
}