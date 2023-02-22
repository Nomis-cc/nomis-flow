// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Options;
using Nomis.Solscan.Interfaces;
using Nomis.Solscan.Interfaces.Extensions;
using Nomis.Solscan.Interfaces.Models;
using Nomis.Solscan.Settings;

namespace Nomis.Solscan
{
    /// <inheritdoc cref="ISolscanClient"/>
    internal sealed class SolscanClient :
        ISolscanClient
    {
        private const int ItemsFetchLimit = 50;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="SolscanClient"/>.
        /// </summary>
        /// <param name="solscanSettings"><see cref="SolscanSettings"/>.</param>
        public SolscanClient(
            IOptions<SolscanSettings> solscanSettings)
        {
            _client = new()
            {
                BaseAddress = new(solscanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(solscanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<decimal> GetBalanceAsync(string address)
        {
            var response = await _client.GetAsync($"/account/{address}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var accountInfo = await response.Content.ReadFromJsonAsync<SolscanAccount>().ConfigureAwait(false);
            return accountInfo?.Lamports.ToSol() ?? 0;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SolscanTransaction>> GetTransactionsDataAsync(string address)
        {
            var result = new List<SolscanTransaction>();
            var transactionsData = await GetTxListAsync(address).ConfigureAwait(false);
            result.AddRange(transactionsData);
            while (transactionsData.Length >= ItemsFetchLimit)
            {
                transactionsData = await GetTxListAsync(address, transactionsData.LastOrDefault()?.TxHash).ConfigureAwait(false);
                result.AddRange(transactionsData);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<TResult> GetTransfersDataAsync<TResult, TResultItem>(string address)
            where TResult : ISolscanTransferList<TResultItem>, new()
            where TResultItem : ISolscanTransfer
        {
            var result = new TResult
            {
                Data = new List<TResultItem>()
            };
            int offset = 0;
            var transactionsData = await GetTransfersListAsync<TResult, TResultItem>(address).ConfigureAwait(false);
            foreach (var transactionData in transactionsData?.Data ?? new List<TResultItem>())
            {
                result.Data.Add(transactionData);
            }

            while (transactionsData?.Data?.Count > 0)
            {
                offset += ItemsFetchLimit;
                transactionsData = await GetTransfersListAsync<TResult, TResultItem>(address, offset: offset).ConfigureAwait(false);
                foreach (var transactionData in transactionsData?.Data ?? new List<TResultItem>())
                {
                    result.Data.Add(transactionData);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SolscanToken>> GetTokensAsync(string address)
        {
            var response = await _client.GetAsync($"/account/tokens?account={address}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<SolscanToken[]>().ConfigureAwait(false);
            return result!;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SolscanStakeAccount>> GetStakesAsync(string address)
        {
            var response = await _client.GetAsync($"/account/stakeAccounts?account={address}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<JsonObject>().ConfigureAwait(false);
            var accounts = result?
                .Where(x => x.Value != null)
                .Select(x => JsonSerializer.Deserialize<SolscanStakeAccount>(x.Value?.ToJsonString() ?? string.Empty));

            return accounts!;
        }

        private async Task<SolscanTransaction[]> GetTxListAsync(string address, string? beforeHash = null)
        {
            string request =
                $"/account/transactions?account={address}&limit={ItemsFetchLimit}";
            if (!string.IsNullOrWhiteSpace(beforeHash))
            {
                request = $"{request}&beforeHash={beforeHash}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var transactionsData = await response.Content.ReadFromJsonAsync<SolscanTransaction[]>().ConfigureAwait(false);
            return transactionsData ?? Array.Empty<SolscanTransaction>();
        }

        private async Task<TResult?> GetTransfersListAsync<TResult, TResultItem>(
            string address,
            long? fromTime = null,
            long? toTime = null,
            int? offset = null)
                where TResult : ISolscanTransferList<TResultItem>
                where TResultItem : ISolscanTransfer
        {
            string request = "/account";
            if (typeof(TResult) == typeof(SolscanSplTransferList))
            {
                request = $"{request}/splTransfers";
            }
            else if (typeof(TResult) == typeof(SolscanSolTransferList))
            {
                request = $"{request}/solTransfers";
            }
            else
            {
                return default;
            }

            request = $"{request}?account={address}&limit={ItemsFetchLimit}";

            if (fromTime != null)
            {
                request = $"{request}&fromTime={fromTime}";
            }

            if (toTime != null)
            {
                request = $"{request}&toTime={toTime}";
            }

            request = $"{request}&offset={offset ?? 0}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false);
        }
    }
}