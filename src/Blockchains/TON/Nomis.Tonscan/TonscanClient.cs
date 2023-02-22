// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.Blockchain.Abstractions.Converters;
using Nomis.Tonscan.Interfaces;
using Nomis.Tonscan.Interfaces.Models;
using Nomis.Tonscan.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.Tonscan
{
    /// <inheritdoc cref="ITonscanClient"/>
    internal sealed class TonscanClient :
        ITonscanClient
    {
        private const int ItemsFetchLimit = 1000;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="TonscanClient"/>.
        /// </summary>
        /// <param name="tonscanSettings"><see cref="TonscanSettings"/>.</param>
        public TonscanClient(
            IOptions<TonscanSettings> tonscanSettings)
        {
            _client = new()
            {
                BaseAddress = new(tonscanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(tonscanSettings.Value.ApiBaseUrl)))
            };
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tonscanSettings.Value.ApiKey}");
        }

        /// <inheritdoc/>
        public async Task<TonscanBalance> GetBalanceAsync(string address)
        {
            string request =
                $"/v1/blockchain/getAccount?account={address}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TonscanBalance>(new JsonSerializerOptions
                   {
                       Converters = { new BigIntegerConverter() }
                   })
.ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TonscanTransaction>> GetTransactionsAsync(string address)
        {
            var result = new List<TonscanTransaction>();
            var transactionsData = await GetTransactionList(address).ConfigureAwait(false);
            result.AddRange(transactionsData.Transactions);
            while (transactionsData.Transactions.Count >= ItemsFetchLimit)
            {
                transactionsData = await GetTransactionList(address, transactionsData.Transactions.LastOrDefault()?.Utime).ConfigureAwait(false);
                result.AddRange(transactionsData.Transactions);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<TonscanAccountAssetBalances> GetAssetsAsync(string address)
        {
            string request =
                $"/v1/jetton/getBalances?account={address}";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TonscanAccountAssetBalances>(new JsonSerializerOptions
                   {
                       Converters = { new BigIntegerConverter() }
                   })
.ConfigureAwait(false) ?? throw new CustomException("Can't get account assets.");
        }

        private async Task<TonscanTransactions> GetTransactionList(
            string address,
            BigInteger? minLt = null)
        {
            string request =
                $"/v1/blockchain/getTransactions?account={address}&limit={ItemsFetchLimit}";

            if (minLt != null)
            {
                request = $"{request}&minLt={minLt.Value}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TonscanTransactions>(new JsonSerializerOptions
                   {
                       Converters = { new BigIntegerConverter() }
                   })
.ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}