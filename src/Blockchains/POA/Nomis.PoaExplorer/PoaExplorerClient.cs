// ------------------------------------------------------------------------------------------------------
// <copyright file="PoaExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.PoaExplorer.Interfaces;
using Nomis.PoaExplorer.Interfaces.Models;
using Nomis.PoaExplorer.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.PoaExplorer
{
    /// <inheritdoc cref="IPoaExplorerClient"/>
    internal sealed class PoaExplorerClient :
        IPoaExplorerClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly PoaExplorerSettings _poaExplorerSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="PoaExplorerClient"/>.
        /// </summary>
        /// <param name="poaExplorerSettings"><see cref="PoaExplorerSettings"/>.</param>
        public PoaExplorerClient(
            IOptions<PoaExplorerSettings> poaExplorerSettings)
        {
            _poaExplorerSettings = poaExplorerSettings.Value;
            _client = new()
            {
                BaseAddress = new(poaExplorerSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(poaExplorerSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<PoaExplorerAccount> GetBalanceAsync(string address)
        {
            string request =
                $"api?module=account&action=balance&address={address}&tag=latest";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PoaExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<PoaExplorerAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PoaExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IPoaExplorerTransferList<TResultItem>
            where TResultItem : IPoaExplorerTransfer
        {
            var result = new List<TResultItem>();
            var transactionsData = await GetTransactionListAsync<TResult>(address).ConfigureAwait(false);
            result.AddRange(transactionsData.Data ?? new List<TResultItem>());
            while (transactionsData?.Data?.Count >= ItemsFetchLimit)
            {
                await Task.Delay(200).ConfigureAwait(false);
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
                $"api?module=account&address={address}&sort=asc";

            if (typeof(TResult) == typeof(PoaExplorerAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(PoaExplorerAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(PoaExplorerAccountERC20TokenEvents))
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