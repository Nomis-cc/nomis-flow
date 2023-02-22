// ------------------------------------------------------------------------------------------------------
// <copyright file="CantoExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.CantoExplorer.Interfaces;
using Nomis.CantoExplorer.Interfaces.Models;
using Nomis.CantoExplorer.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.CantoExplorer
{
    /// <inheritdoc cref="ICantoExplorerClient"/>
    internal sealed class CantoExplorerClient :
        ICantoExplorerClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly CantoExplorerSettings _cantoExplorerSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="CantoExplorerClient"/>.
        /// </summary>
        /// <param name="cantoExplorerSettings"><see cref="CantoExplorerSettings"/>.</param>
        public CantoExplorerClient(
            IOptions<CantoExplorerSettings> cantoExplorerSettings)
        {
            _cantoExplorerSettings = cantoExplorerSettings.Value;
            _client = new()
            {
                BaseAddress = new(cantoExplorerSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(cantoExplorerSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<CantoExplorerAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CantoExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<CantoExplorerAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CantoExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : ICantoExplorerTransferList<TResultItem>
            where TResultItem : ICantoExplorerTransfer
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

            if (typeof(TResult) == typeof(CantoExplorerAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(CantoExplorerAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(CantoExplorerAccountERC20TokenEvents))
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

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}