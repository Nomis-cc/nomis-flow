// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.Blockchain.Abstractions.Converters;
using Nomis.HederaMirrorNode.Interfaces;
using Nomis.HederaMirrorNode.Interfaces.Models;
using Nomis.HederaMirrorNode.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.HederaMirrorNode
{
    /// <inheritdoc cref="IHederaMirrorNodeClient"/>
    internal sealed class HederaMirrorNodeClient :
        IHederaMirrorNodeClient
    {
        private const int ItemsFetchLimit = 100;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="HederaMirrorNodeClient"/>.
        /// </summary>
        /// <param name="hederaMirrorNodeSettings"><see cref="HederaMirrorNodeSettings"/>.</param>
        public HederaMirrorNodeClient(
            IOptions<HederaMirrorNodeSettings> hederaMirrorNodeSettings)
        {
            _client = new()
            {
                BaseAddress = new(hederaMirrorNodeSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(hederaMirrorNodeSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<HederaMirrorNodeAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api/v1/accounts/{address}?limit={ItemsFetchLimit}&order=asc";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<HederaMirrorNodeAccount>(new JsonSerializerOptions
            {
                Converters = { new BigIntegerConverter() }
            }).ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");

            var tempResult = result;
            while (!string.IsNullOrWhiteSpace(tempResult.Links?.Next))
            {
                response = await _client.GetAsync(tempResult.Links.Next).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                tempResult = await response.Content.ReadFromJsonAsync<HederaMirrorNodeAccount>(new JsonSerializerOptions
                {
                    Converters = { new BigIntegerConverter() }
                }).ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");

                foreach (var transaction in tempResult.Transactions)
                {
                    result.Transactions.Add(transaction);
                }

                result.Links = tempResult.Links;
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<HederaMirrorNodeNfts> GetNftsAsync(string address)
        {
            string request =
                $"/api/v1/accounts/{address}/nfts?limit={ItemsFetchLimit}&order=asc";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<HederaMirrorNodeNfts>(new JsonSerializerOptions
            {
                Converters = { new BigIntegerConverter() }
            }).ConfigureAwait(false) ?? throw new CustomException("Can't get account NFTs.");

            var tempResult = result;
            while (!string.IsNullOrWhiteSpace(tempResult.Links?.Next))
            {
                response = await _client.GetAsync(tempResult.Links.Next).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                tempResult = await response.Content.ReadFromJsonAsync<HederaMirrorNodeNfts>(new JsonSerializerOptions
                {
                    Converters = { new BigIntegerConverter() }
                }).ConfigureAwait(false) ?? throw new CustomException("Can't get account NFTs.");

                foreach (var nft in tempResult.Nfts)
                {
                    result.Nfts.Add(nft);
                }

                result.Links = tempResult.Links;
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<HederaMirrorNodeNftTransactions> GetNftTransactionsAsync(string tokenId, long serialNumber)
        {
            string request =
                $"/api/v1/tokens/{tokenId}/nfts/{serialNumber}/transactions?limit={ItemsFetchLimit}&order=asc";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<HederaMirrorNodeNftTransactions>(new JsonSerializerOptions
            {
                Converters = { new BigIntegerConverter() }
            }).ConfigureAwait(false) ?? throw new CustomException("Can't get NFT transactions.");

            var tempResult = result;
            while (!string.IsNullOrWhiteSpace(tempResult.Links?.Next))
            {
                response = await _client.GetAsync(tempResult.Links.Next).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                tempResult = await response.Content.ReadFromJsonAsync<HederaMirrorNodeNftTransactions>(new JsonSerializerOptions
                {
                    Converters = { new BigIntegerConverter() }
                }).ConfigureAwait(false) ?? throw new CustomException("Can't get NFT transactions.");

                foreach (var transaction in tempResult.Transactions)
                {
                    result.Transactions.Add(transaction);
                }

                result.Links = tempResult.Links;
            }

            return result;
        }
    }
}