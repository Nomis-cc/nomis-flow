// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.Blockchain.Abstractions.Converters;
using Nomis.Utils.Exceptions;
using Nomis.WavesExplorer.Interfaces;
using Nomis.WavesExplorer.Interfaces.Models;
using Nomis.WavesExplorer.Settings;

namespace Nomis.WavesExplorer
{
    /// <inheritdoc cref="IWavesExplorerClient"/>
    internal sealed class WavesExplorerClient :
        IWavesExplorerClient
    {
        private const int ItemsFetchLimit = 1000;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="WavesExplorerClient"/>.
        /// </summary>
        /// <param name="wavesExplorerSettings"><see cref="WavesExplorerSettings"/>.</param>
        public WavesExplorerClient(
            IOptions<WavesExplorerSettings> wavesExplorerSettings)
        {
            _client = new()
            {
                BaseAddress = new(wavesExplorerSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(wavesExplorerSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<WavesExplorerBalance> GetBalanceAsync(string address)
        {
            string request =
                $"/addresses/balance/details/{address}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WavesExplorerBalance>(new JsonSerializerOptions()
                   {
                       Converters = { new BigIntegerConverter() }
                   })
.ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<WavesExplorerTransaction>> GetTransactionsAsync(string address)
        {
            var result = new List<WavesExplorerTransaction>();
            var transactionsData = await GetTransactionList(address).ConfigureAwait(false);
            result.AddRange(transactionsData.FirstOrDefault() ?? new List<WavesExplorerTransaction>());
            while (transactionsData.FirstOrDefault()?.Count >= ItemsFetchLimit)
            {
                transactionsData = await GetTransactionList(address, transactionsData.FirstOrDefault()?.LastOrDefault()?.Id).ConfigureAwait(false);
                result.AddRange(transactionsData.FirstOrDefault() ?? new List<WavesExplorerTransaction>());
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<WavesExplorerAccountAssetBalances> GetAssetsAsync(string address)
        {
            string request =
                $"/assets/balance/{address}";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WavesExplorerAccountAssetBalances>()
.ConfigureAwait(false) ?? throw new CustomException("Can't get account assets.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<WavesExplorerAccountNftAsset>> GetNftAssetsAsync(string address)
        {
            var result = new List<WavesExplorerAccountNftAsset>();
            var nftAssets = await GetNftAssetList(address).ConfigureAwait(false);
            result.AddRange(nftAssets);
            while (nftAssets.Count >= ItemsFetchLimit)
            {
                nftAssets = await GetNftAssetList(address, nftAssets.LastOrDefault()?.OriginTransactionId).ConfigureAwait(false);
                result.AddRange(nftAssets);
            }

            return result;
        }

        private async Task<List<List<WavesExplorerTransaction>>> GetTransactionList(
            string address,
            string? after = null)
        {
            string request =
                $"/transactions/address/{address}/limit/{ItemsFetchLimit}";

            if (!string.IsNullOrWhiteSpace(after))
            {
                request = $"{request}?after={after}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<List<WavesExplorerTransaction>>>()
.ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }

        private async Task<List<WavesExplorerAccountNftAsset>> GetNftAssetList(
            string address,
            string? after = null)
        {
            string request =
                $"/assets/nft/{address}/limit/{ItemsFetchLimit}";

            if (!string.IsNullOrWhiteSpace(after))
            {
                request = $"{request}?after={after}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<WavesExplorerAccountNftAsset>>()
.ConfigureAwait(false) ?? throw new CustomException("Can't get account NFT assets.");
        }
    }
}