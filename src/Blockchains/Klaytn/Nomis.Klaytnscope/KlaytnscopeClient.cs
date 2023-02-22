// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.Blockchain.Abstractions.Converters;
using Nomis.Klaytnscope.Interfaces;
using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Klaytnscope.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.Klaytnscope
{
    /// <inheritdoc cref="IKlaytnscopeClient"/>
    internal sealed class KlaytnscopeClient :
        IKlaytnscopeClient
    {
        private const int ItemsFetchLimit = 1000; // TODO - change value
        private readonly KlaytnscopeSettings _klaytnscopeSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="KlaytnscopeClient"/>.
        /// </summary>
        /// <param name="klaytnscopeSettings"><see cref="KlaytnscopeSettings"/>.</param>
        public KlaytnscopeClient(
            IOptions<KlaytnscopeSettings> klaytnscopeSettings)
        {
            _klaytnscopeSettings = klaytnscopeSettings.Value;
            _client = new()
            {
                BaseAddress = new(klaytnscopeSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(klaytnscopeSettings.Value.ApiBaseUrl))),
            };
            _client.DefaultRequestHeaders.Add("Origin", "https://scope.klaytn.com");
            _client.DefaultRequestHeaders.Add("Referer", "https://scope.klaytn.com/");
        }

        /// <inheritdoc/>
        public async Task<KlaytnscopeAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/v2/accounts/{address}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<KlaytnscopeAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<KlaytnscopeAccountTokenBalances> GetTokenBalancesAsync(string address)
        {
            string request =
                $"/v2/accounts/{address}/ftBalances";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<KlaytnscopeAccountTokenBalances>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IKlaytnscopeTransferList<TResultItem>
            where TResultItem : class
        {
            var result = new List<TResultItem>();
            int page = 1;
            var transactionsData = await GetTransactionList<TResult>(address, page).ConfigureAwait(false);
            result.AddRange(transactionsData.Result ?? new List<TResultItem>());
            while (result.Count < transactionsData?.Total && result.Count < ItemsFetchLimit) // TODO - remove limit
            {
                page++;
                transactionsData = await GetTransactionList<TResult>(address, page).ConfigureAwait(false);
                result.AddRange(transactionsData?.Result ?? new List<TResultItem>());
            }

            return result;
        }

        private async Task<TResult> GetTransactionList<TResult>(
            string address,
            int page = 1)
        {
            string request = $"/v2/accounts/{address}";

            if (typeof(TResult) == typeof(KlaytnscopeAccountNormalTransactions))
            {
                request += "/txs";
            }
            else if (typeof(TResult) == typeof(KlaytnscopeAccountInternalTransactions))
            {
                request += "/itxs";
            }
            else if (typeof(TResult) == typeof(KlaytnscopeAccountKIP37TokenEvents))
            {
                request += "/kip37Balances";
            }
            else if (typeof(TResult) == typeof(KlaytnscopeAccountKIP17TokenEvents))
            {
                request += "/kip17Balances";
            }
            else if (typeof(TResult) == typeof(KlaytnscopeAccountNftTransfers))
            {
                request += "/nftTransfers";
            }

            request += $"?limit=25&page={page}";

            Thread.Sleep(100);
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResult>(new JsonSerializerOptions
            {
                Converters = { new BigIntegerConverter() }
            }).ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}