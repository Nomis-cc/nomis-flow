// ------------------------------------------------------------------------------------------------------
// <copyright file="MantleExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.MantleExplorer.Interfaces;
using Nomis.MantleExplorer.Interfaces.Models;
using Nomis.MantleExplorer.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.MantleExplorer
{
    /// <inheritdoc cref="IMantleExplorerClient"/>
    internal sealed class MantleExplorerClient :
        IMantleExplorerClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly MantleExplorerSettings _mantleExplorerSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="MantleExplorerClient"/>.
        /// </summary>
        /// <param name="mantleExplorerSettings"><see cref="MantleExplorerSettings"/>.</param>
        public MantleExplorerClient(
            IOptions<MantleExplorerSettings> mantleExplorerSettings)
        {
            _mantleExplorerSettings = mantleExplorerSettings.Value;
            _client = new()
            {
                BaseAddress = new(mantleExplorerSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(mantleExplorerSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<MantleExplorerAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MantleExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<MantleExplorerAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MantleExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IMantleExplorerTransferList<TResultItem>
            where TResultItem : IMantleExplorerTransfer
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

            if (typeof(TResult) == typeof(MantleExplorerAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(MantleExplorerAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(MantleExplorerAccountERC20TokenEvents))
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