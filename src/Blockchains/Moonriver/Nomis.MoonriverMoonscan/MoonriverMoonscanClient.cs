// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonriverMoonscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.MoonriverMoonscan.Interfaces;
using Nomis.MoonriverMoonscan.Interfaces.Models;
using Nomis.MoonriverMoonscan.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.MoonriverMoonscan
{
    /// <inheritdoc cref="IMoonriverMoonscanClient"/>
    internal sealed class MoonriverMoonscanClient :
        IMoonriverMoonscanClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly MoonriverMoonscanSettings _moonriverMoonscanSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="MoonriverMoonscanClient"/>.
        /// </summary>
        /// <param name="moonriverMoonscanSettings"><see cref="MoonriverMoonscanSettings"/>.</param>
        public MoonriverMoonscanClient(
            IOptions<MoonriverMoonscanSettings> moonriverMoonscanSettings)
        {
            _moonriverMoonscanSettings = moonriverMoonscanSettings.Value;
            _client = new()
            {
                BaseAddress = new(moonriverMoonscanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(moonriverMoonscanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<MoonriverMoonscanAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_moonriverMoonscanSettings.ApiKey))
            {
                request += $"&apiKey={_moonriverMoonscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MoonriverMoonscanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<MoonriverMoonscanAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_moonriverMoonscanSettings.ApiKey))
            {
                request += $"&apiKey={_moonriverMoonscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MoonriverMoonscanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IMoonriverMoonscanTransferList<TResultItem>
            where TResultItem : IMoonriverMoonscanTransfer
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
                $"/api?module=account&address={address}&sort=asc&apiKey={_moonriverMoonscanSettings.ApiKey}";

            if (typeof(TResult) == typeof(MoonriverMoonscanAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(MoonriverMoonscanAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(MoonriverMoonscanAccountERC20TokenEvents))
            {
                request = $"{request}&action=tokentx";
            }
            else if (typeof(TResult) == typeof(MoonriverMoonscanAccountERC721TokenEvents))
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

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}