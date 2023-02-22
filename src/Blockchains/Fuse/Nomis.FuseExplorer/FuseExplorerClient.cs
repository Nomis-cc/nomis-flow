// ------------------------------------------------------------------------------------------------------
// <copyright file="FuseExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.FuseExplorer.Interfaces;
using Nomis.FuseExplorer.Interfaces.Models;
using Nomis.FuseExplorer.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.FuseExplorer
{
    /// <inheritdoc cref="IFuseExplorerClient"/>
    internal sealed class FuseExplorerClient :
        IFuseExplorerClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly FuseExplorerSettings _fuseExplorerSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="FuseExplorerClient"/>.
        /// </summary>
        /// <param name="fuseExplorerSettings"><see cref="FuseExplorerSettings"/>.</param>
        public FuseExplorerClient(
            IOptions<FuseExplorerSettings> fuseExplorerSettings)
        {
            _fuseExplorerSettings = fuseExplorerSettings.Value;
            _client = new()
            {
                BaseAddress = new(fuseExplorerSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(fuseExplorerSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<FuseExplorerAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_fuseExplorerSettings.ApiKey))
            {
                request += $"&apiKey={_fuseExplorerSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FuseExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<FuseExplorerAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_fuseExplorerSettings.ApiKey))
            {
                request += $"&apiKey={_fuseExplorerSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FuseExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IFuseExplorerTransferList<TResultItem>
            where TResultItem : IFuseExplorerTransfer
        {
            var result = new List<TResultItem>();
            var transactionsData = await GetTransactionListAsync<TResult>(address).ConfigureAwait(false);
            result.AddRange(transactionsData.Data ?? new List<TResultItem>());
            while (transactionsData?.Data?.Count >= ItemsFetchLimit)
            {
                await Task.Delay(100).ConfigureAwait(false);
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

            if (typeof(TResult) == typeof(FuseExplorerAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(FuseExplorerAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(FuseExplorerAccountERC20TokenEvents))
            {
                request = $"{request}&action=tokentx";
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

            if (!string.IsNullOrWhiteSpace(_fuseExplorerSettings.ApiKey))
            {
                request += $"&apiKey={_fuseExplorerSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}