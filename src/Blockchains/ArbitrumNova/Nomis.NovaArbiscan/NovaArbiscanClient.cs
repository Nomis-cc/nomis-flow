// ------------------------------------------------------------------------------------------------------
// <copyright file="NovaArbiscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.NovaArbiscan.Interfaces;
using Nomis.NovaArbiscan.Interfaces.Models;
using Nomis.NovaArbiscan.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.NovaArbiscan
{
    /// <inheritdoc cref="INovaArbiscanClient"/>
    internal sealed class NovaArbiscanClient :
        INovaArbiscanClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly NovaArbiscanSettings _novaArbiscanSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="NovaArbiscanClient"/>.
        /// </summary>
        /// <param name="novaArbiscanSettings"><see cref="NovaArbiscanSettings"/>.</param>
        public NovaArbiscanClient(
            IOptions<NovaArbiscanSettings> novaArbiscanSettings)
        {
            _novaArbiscanSettings = novaArbiscanSettings.Value;
            _client = new()
            {
                BaseAddress = new(novaArbiscanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(novaArbiscanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<NovaArbiscanAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_novaArbiscanSettings.ApiKey))
            {
                request += $"&apiKey={_novaArbiscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NovaArbiscanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<NovaArbiscanAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_novaArbiscanSettings.ApiKey))
            {
                request += $"&apiKey={_novaArbiscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NovaArbiscanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : INovaArbiscanTransferList<TResultItem>
            where TResultItem : INovaArbiscanTransfer
        {
            var result = new List<TResultItem>();
            var transactionsData = await GetTransactionListAsync<TResult>(address).ConfigureAwait(false);
            result.AddRange(transactionsData.Data ?? new List<TResultItem>());
            while (transactionsData?.Data?.Count >= ItemsFetchLimit)
            {
                transactionsData = await GetTransactionListAsync<TResult>(address, transactionsData.Data.LastOrDefault()?.BlockNumber).ConfigureAwait(false);
                result.AddRange(transactionsData?.Data ?? new List<TResultItem>());
            }

            return result;
        }

        private async Task<TResult> GetTransactionListAsync<TResult>(
            string address,
            string? startBlock = null)
        {
            string request =
                $"/api?module=account&address={address}&sort=asc";

            if (typeof(TResult) == typeof(NovaArbiscanAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(NovaArbiscanAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(NovaArbiscanAccountERC20TokenEvents))
            {
                request = $"{request}&action=tokentx";
            }
            else if (typeof(TResult) == typeof(NovaArbiscanAccountERC721TokenEvents))
            {
                request = $"{request}&action=tokennfttx";
            }
            else
            {
                return default!;
            }

            if (!string.IsNullOrWhiteSpace(startBlock))
            {
                request = $"{request}&startblock={startBlock}";
            }
            else
            {
                request = $"{request}&startblock=0";
            }

            request = $"{request}&endblock=999999999";

            if (!string.IsNullOrWhiteSpace(_novaArbiscanSettings.ApiKey))
            {
                request += $"&apiKey={_novaArbiscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}