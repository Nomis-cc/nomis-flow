// ------------------------------------------------------------------------------------------------------
// <copyright file="BttcscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.Bttcscan.Interfaces;
using Nomis.Bttcscan.Interfaces.Models;
using Nomis.Bttcscan.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.Bttcscan
{
    /// <inheritdoc cref="IBttcscanClient"/>
    internal sealed class BttcscanClient :
        IBttcscanClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly BttcscanSettings _bttcscanSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="BttcscanClient"/>.
        /// </summary>
        /// <param name="bttcscanSettings"><see cref="BttcscanSettings"/>.</param>
        public BttcscanClient(
            IOptions<BttcscanSettings> bttcscanSettings)
        {
            _bttcscanSettings = bttcscanSettings.Value;
            _client = new()
            {
                BaseAddress = new(bttcscanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(bttcscanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<BttcscanAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_bttcscanSettings.ApiKey))
            {
                request += $"&apiKey={_bttcscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BttcscanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<BttcscanAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_bttcscanSettings.ApiKey))
            {
                request += $"&apiKey={_bttcscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BttcscanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IBttcscanTransferList<TResultItem>
            where TResultItem : IBttcscanTransfer
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

            if (typeof(TResult) == typeof(BttcscanAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(BttcscanAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(BttcscanAccountERC20TokenEvents))
            {
                request = $"{request}&action=tokentx";
            }
            else if (typeof(TResult) == typeof(BttcscanAccountERC721TokenEvents))
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

            if (!string.IsNullOrWhiteSpace(_bttcscanSettings.ApiKey))
            {
                request += $"&apiKey={_bttcscanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}