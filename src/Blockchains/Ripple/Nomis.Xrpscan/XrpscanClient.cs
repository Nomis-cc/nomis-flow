// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.Xrpscan.Interfaces;
using Nomis.Xrpscan.Interfaces.Models;
using Nomis.Xrpscan.Settings;

namespace Nomis.Xrpscan
{
    /// <inheritdoc cref="IXrpscanClient"/>
    internal sealed class XrpscanClient :
        IXrpscanClient
    {
        private const int ItemsFetchLimit = 25;

        private const int MaxTransactions = 15000;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="XrpscanClient"/>.
        /// </summary>
        /// <param name="xrpscanSettings"><see cref="XrpscanSettings"/>.</param>
        public XrpscanClient(
            IOptions<XrpscanSettings> xrpscanSettings)
        {
            _client = new()
            {
                BaseAddress = new(xrpscanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(xrpscanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<XrpscanAccount> GetAccountDataAsync(string address)
        {
            var response = await _client.GetAsync($"/api/v1/account/{address}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<XrpscanAccount>().ConfigureAwait(false)) !;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<XrpscanTransaction>> GetTransactionsDataAsync(string address)
        {
            var result = new List<XrpscanTransaction>();
            var transactionsData = await GetTxListAsync(address).ConfigureAwait(false);
            result.AddRange(transactionsData?.Transactions ?? new List<XrpscanTransaction>());
            while (transactionsData?.Transactions?.Count >= ItemsFetchLimit && result.Count <= MaxTransactions)
            {
                transactionsData = await GetTxListAsync(address, transactionsData.Marker).ConfigureAwait(false);
                result.AddRange(transactionsData?.Transactions ?? new List<XrpscanTransaction>());
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<XrpscanNft>> GetNftsDataAsync(string address)
        {
            string request =
                $"/api/v1/account/{address}/nfts";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<XrpscanNft>>().ConfigureAwait(false) ?? new List<XrpscanNft>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<XrpscanAsset>> GetAssetsDataAsync(string address)
        {
            string request =
                $"/api/v1/account/{address}/assets";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<XrpscanAsset>>().ConfigureAwait(false) ?? new List<XrpscanAsset>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<XrpscanOrder>> GetOrdersDataAsync(string address)
        {
            string request =
                $"/api/v1/account/{address}/orders";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<XrpscanOrder>>().ConfigureAwait(false) ?? new List<XrpscanOrder>();
        }

        /// <inheritdoc/>
        public async Task<XrpscanKyc> GetKycDataAsync(string address)
        {
            string request =
                $"/api/v1/account/{address}/xummkyc";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<XrpscanKyc>().ConfigureAwait(false) ?? new XrpscanKyc
            {
                Account = address,
                KycApproved = false
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<XrpscanObligation>> GetObligationsDataAsync(string address)
        {
            string request =
                $"/api/v1/account/{address}/obligations";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<XrpscanObligation>>().ConfigureAwait(false) ?? new List<XrpscanObligation>();
        }

        private async Task<XrpscanTransactions?> GetTxListAsync(string address, string? marker = null)
        {
            string request =
                $"/api/v1/account/{address}/transactions";
            if (!string.IsNullOrWhiteSpace(marker))
            {
                request = $"{request}?marker={marker}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<XrpscanTransactions>().ConfigureAwait(false);
        }
    }
}