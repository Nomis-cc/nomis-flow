// ------------------------------------------------------------------------------------------------------
// <copyright file="IBscscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using EthScanNet.Lib.Models.ApiResponses.Accounts.Models;

namespace Nomis.Bscscan.Interfaces
{
    /// <summary>
    /// Bscscan client.
    /// </summary>
    public interface IBscscanClient
    {
        /// <summary>
        /// Get list of internal transactions of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of internal transactions of the given account.</returns>
        Task<IEnumerable<EScanTransaction>> GetInternalTransactionsAsync(string address);
    }
}