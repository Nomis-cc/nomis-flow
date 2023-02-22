// ------------------------------------------------------------------------------------------------------
// <copyright file="BscscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using EthScanNet.Lib.Models.ApiResponses.Accounts;
using EthScanNet.Lib.Models.ApiResponses.Accounts.Models;
using Microsoft.Extensions.Options;
using Nomis.Bscscan.Interfaces;
using Nomis.Bscscan.Settings;

namespace Nomis.Bscscan
{
    /// <inheritdoc cref="IBscscanClient"/>
    internal sealed class BscscanClient :
        IBscscanClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly BscscanSettings _bscscanSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="BscscanClient"/>.
        /// </summary>
        /// <param name="bscscanSettings"><see cref="BscscanSettings"/>.</param>
        public BscscanClient(
            IOptions<BscscanSettings> bscscanSettings)
        {
            _bscscanSettings = bscscanSettings.Value;
            _client = new()
            {
                BaseAddress = new(bscscanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(bscscanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EScanTransaction>> GetInternalTransactionsAsync(string address)
        {
            var result = new List<EScanTransaction>();
            var transactionsData = await GetTxList(address, 0).ConfigureAwait(false);
            result.AddRange(transactionsData);
            ulong offset = 0;
            while (transactionsData.Length >= ItemsFetchLimit)
            {
                transactionsData = await GetTxList(address, offset).ConfigureAwait(false);
                offset += ItemsFetchLimit;
                result.AddRange(transactionsData);
            }

            return result;
        }

        private async Task<EScanTransaction[]> GetTxList(string address, ulong offset)
        {
            string request =
                $"/api?module=account&action=txlistinternal&address={address}&startblock=0&endblock=99999999&apikey={_bscscanSettings.ApiKey}";

            request = $"{request}&offset={offset}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var transactionsData = await response.Content.ReadFromJsonAsync<EScanTransactions>().ConfigureAwait(false);
            return transactionsData?.Transactions?.ToArray() ?? Array.Empty<EScanTransaction>();
        }
    }
}