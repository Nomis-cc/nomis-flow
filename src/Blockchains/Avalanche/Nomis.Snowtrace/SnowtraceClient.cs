// ------------------------------------------------------------------------------------------------------
// <copyright file="SnowtraceClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.Snowtrace.Interfaces;
using Nomis.Snowtrace.Interfaces.Models;
using Nomis.Snowtrace.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.Snowtrace
{
    /// <inheritdoc cref="ISnowtraceClient"/>
    internal sealed class SnowtraceClient :
        ISnowtraceClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly SnowtraceSettings _snowtraceSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="SnowtraceClient"/>.
        /// </summary>
        /// <param name="snowtraceSettings"><see cref="SnowtraceSettings"/>.</param>
        public SnowtraceClient(
            IOptions<SnowtraceSettings> snowtraceSettings)
        {
            _snowtraceSettings = snowtraceSettings.Value;
            _client = new()
            {
                BaseAddress = new(snowtraceSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(snowtraceSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<SnowtraceAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_snowtraceSettings.ApiKey))
            {
                request += $"&apiKey={_snowtraceSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SnowtraceAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<SnowtraceAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_snowtraceSettings.ApiKey))
            {
                request += $"&apiKey={_snowtraceSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SnowtraceAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : ISnowtraceTransferList<TResultItem>
            where TResultItem : ISnowtraceTransfer
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

            if (typeof(TResult) == typeof(SnowtraceAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(SnowtraceAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(SnowtraceAccountERC20TokenEvents))
            {
                request = $"{request}&action=tokentx";
            }
            else if (typeof(TResult) == typeof(SnowtraceAccountERC721TokenEvents))
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

            if (!string.IsNullOrWhiteSpace(_snowtraceSettings.ApiKey))
            {
                request += $"&apiKey={_snowtraceSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}