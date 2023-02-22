// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.CscExplorer.Interfaces;
using Nomis.CscExplorer.Interfaces.Models;
using Nomis.CscExplorer.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.CscExplorer
{
    /// <inheritdoc cref="ICscExplorerClient"/>
    internal sealed class CscExplorerClient :
        ICscExplorerClient
    {
        private const int ItemsFetchLimit = 200;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="CscExplorerClient"/>.
        /// </summary>
        /// <param name="cscExplorerSettings"><see cref="CscExplorerSettings"/>.</param>
        public CscExplorerClient(
            IOptions<CscExplorerSettings> cscExplorerSettings)
        {
            _client = new()
            {
                BaseAddress = new(cscExplorerSettings.Value.ApiBaseUrl
                                  ?? throw new ArgumentNullException(nameof(cscExplorerSettings.Value.ApiBaseUrl)))
            };
            _client.DefaultRequestHeaders.Add("apikey", cscExplorerSettings.Value.ApiKey);
        }

        /// <inheritdoc/>
        public async Task<CscExplorerAccount> GetBalanceAsync(string address)
        {
            var response = await _client.GetAsync($"/api/v1/addresses/{address}/balance").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CscExplorerAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<CscExplorerAccountTokens> GetTokensAsync(string address)
        {
            var response = await _client.GetAsync($"/api/v1/addresses/{address}/tokens").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CscExplorerAccountTokens>().ConfigureAwait(false) ?? throw new CustomException("Can't get account tokens.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TRecord>> GetTransactionsAsync<TResult, TData, TRecord>(string address)
            where TResult : ICscExplorerTransferList<TData, TRecord>
            where TData : ICscExplorerTransferData<TRecord>
            where TRecord : ICscExplorerTransferRecord
        {
            var result = new List<TRecord>();
            int page = 1;
            var transactionsData = await GetTransactionListAsync<TResult, TData, TRecord>(address).ConfigureAwait(false);
            result.AddRange(transactionsData.Data?.Records ?? new List<TRecord>());
            while (transactionsData.Data?.HasNext == true)
            {
                page++;
                transactionsData = await GetTransactionListAsync<TResult, TData, TRecord>(address, page).ConfigureAwait(false);
                result.AddRange(transactionsData.Data?.Records ?? new List<TRecord>());
            }

            return result;
        }

        private async Task<TResult> GetTransactionListAsync<TResult, TData, TRecord>(
            string address,
            int page = 1)
            where TResult : ICscExplorerTransferList<TData, TRecord>
            where TData : ICscExplorerTransferData<TRecord>
            where TRecord : ICscExplorerTransferRecord
        {
            string request =
                $"/api/v1/addresses/{address}";

            if (typeof(TResult) == typeof(CscExplorerAccountTransactions))
            {
                request = $"{request}/transactions?page={page}&limit={ItemsFetchLimit}";
            }
            else if (typeof(TResult) == typeof(CscExplorerAccountCetTransfers))
            {
                request = $"{request}/transfers/cet?page={page}&limit={ItemsFetchLimit}";
            }
            else if (typeof(TResult) == typeof(CscExplorerAccountCrc20Transfers))
            {
                request = $"{request}/transfers/token?page={page}&limit={ItemsFetchLimit}";
            }
            else if (typeof(TResult) == typeof(CscExplorerAccountCrc721Transfers))
            {
                request = $"{request}/transfers/nft?page={page}&limit={ItemsFetchLimit}";
            }
            else
            {
                return default!;
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}