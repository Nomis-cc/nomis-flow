// ------------------------------------------------------------------------------------------------------
// <copyright file="BobascanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.Bobascan.Interfaces;
using Nomis.Bobascan.Interfaces.Models;
using Nomis.Bobascan.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.Bobascan
{
    /// <inheritdoc cref="IBobascanClient"/>
    internal sealed class BobascanClient :
        IBobascanClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly BobascanSettings _bobascanSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="BobascanClient"/>.
        /// </summary>
        /// <param name="bobascanSettings"><see cref="BobascanSettings"/>.</param>
        public BobascanClient(
            IOptions<BobascanSettings> bobascanSettings)
        {
            _bobascanSettings = bobascanSettings.Value;
            _client = new()
            {
                BaseAddress = new(bobascanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(bobascanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<BobascanAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_bobascanSettings.ApiKey))
            {
                request += $"&apiKey={_bobascanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BobascanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<BobascanAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_bobascanSettings.ApiKey))
            {
                request += $"&apiKey={_bobascanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BobascanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IBobascanTransferList<TResultItem>
            where TResultItem : IBobascanTransfer
        {
            var result = new List<TResultItem>();
            var transactionsData = await GetTransactionList<TResult>(address).ConfigureAwait(false);
            result.AddRange(transactionsData.Data ?? new List<TResultItem>());
            while (transactionsData?.Data?.Count >= ItemsFetchLimit)
            {
                transactionsData = await GetTransactionList<TResult>(address, transactionsData.Data.LastOrDefault()?.BlockNumber).ConfigureAwait(false);
                result.AddRange(transactionsData?.Data ?? new List<TResultItem>());
            }

            return result;
        }

        private async Task<TResult> GetTransactionList<TResult>(
            string address,
            string? startBlock = null)
        {
            string request =
                $"/api?module=account&address={address}&sort=asc";

            if (typeof(TResult) == typeof(BobascanAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(BobascanAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(BobascanAccountERC20TokenEvents))
            {
                request = $"{request}&action=tokentx";
            }
            else if (typeof(TResult) == typeof(BobascanAccountERC721TokenEvents))
            {
                request = $"{request}&action=tokennfttx";
            }
            else if (typeof(TResult) == typeof(BobascanAccountERC1155TokenEvents))
            {
                request = $"{request}&action=token1155tx";
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

            if (!string.IsNullOrWhiteSpace(_bobascanSettings.ApiKey))
            {
                request += $"&apiKey={_bobascanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}