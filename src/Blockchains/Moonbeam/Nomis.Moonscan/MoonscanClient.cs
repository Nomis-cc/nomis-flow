// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.Moonscan.Interfaces;
using Nomis.Moonscan.Interfaces.Models;
using Nomis.Moonscan.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.Moonscan
{
    /// <inheritdoc cref="IMoonscanClient"/>
    internal sealed class MoonscanClient :
        IMoonscanClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly MoonscanSettings _moonscanSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="MoonscanClient"/>.
        /// </summary>
        /// <param name="moonscanSettings"><see cref="MoonscanSettings"/>.</param>
        public MoonscanClient(
            IOptions<MoonscanSettings> moonscanSettings)
        {
            _moonscanSettings = moonscanSettings.Value;
            _client = new()
            {
                BaseAddress = new(moonscanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(moonscanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<MoonscanAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_moonscanSettings.ApiKey))
            {
                request += $"&apiKey={_moonscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MoonscanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<MoonscanAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_moonscanSettings.ApiKey))
            {
                request += $"&apiKey={_moonscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MoonscanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IMoonscanTransferList<TResultItem>
            where TResultItem : IMoonscanTransfer
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
                $"/api?module=account&address={address}&sort=asc&apiKey={_moonscanSettings.ApiKey}";

            if (typeof(TResult) == typeof(MoonscanAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(MoonscanAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(MoonscanAccountERC20TokenEvents))
            {
                request = $"{request}&action=tokentx";
            }
            else if (typeof(TResult) == typeof(MoonscanAccountERC721TokenEvents))
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