// ------------------------------------------------------------------------------------------------------
// <copyright file="EvmosAPIClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.EvmosAPI.Interfaces;
using Nomis.EvmosAPI.Interfaces.Models;
using Nomis.EvmosAPI.Settings;
using Nomis.Utils.Exceptions;

// ReSharper disable InconsistentNaming

namespace Nomis.EvmosAPI
{
    /// <inheritdoc cref="IEvmosAPIClient"/>
    internal sealed class EvmosAPIClient :
        IEvmosAPIClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly EvmosEVMSettings _evmosEVMSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="EvmosAPIClient"/>.
        /// </summary>
        /// <param name="evmosEVMSettings"><see cref="EvmosEVMSettings"/>.</param>
        public EvmosAPIClient(
            IOptions<EvmosEVMSettings> evmosEVMSettings)
        {
            _evmosEVMSettings = evmosEVMSettings.Value;
            _client = new()
            {
                BaseAddress = new(evmosEVMSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(evmosEVMSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<EvmosAPIAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EvmosAPIAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EvmosAPIAccountCommonTokenEvent>> GetOwnedTokensAsync(string address)
        {
            string request =
                $"/api?module=account&action=tokenlist&address={address}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<EvmosAPIAccountCommonTokenEvents>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");

            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IEvmosAPITransferList<TResultItem>
            where TResultItem : IEvmosAPITransfer
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

            if (typeof(TResult) == typeof(EvmosAPIAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(EvmosAPIAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(EvmosAPIAccountERC20TokenEvents))
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