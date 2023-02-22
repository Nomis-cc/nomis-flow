// ------------------------------------------------------------------------------------------------------
// <copyright file="CubescanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.Cubescan.Interfaces;
using Nomis.Cubescan.Interfaces.Models;
using Nomis.Cubescan.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.Cubescan
{
    /// <inheritdoc cref="ICubescanClient"/>
    internal sealed class CubescanClient :
        ICubescanClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly CubescanSettings _cubescanSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="CubescanClient"/>.
        /// </summary>
        /// <param name="cubescanSettings"><see cref="CubescanSettings"/>.</param>
        public CubescanClient(
            IOptions<CubescanSettings> cubescanSettings)
        {
            _cubescanSettings = cubescanSettings.Value;
            _client = new()
            {
                BaseAddress = new(cubescanSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(cubescanSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<CubescanAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api?module=account&action=balance&address={address}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_cubescanSettings.ApiKey))
            {
                request += $"&apiKey={_cubescanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CubescanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<CubescanAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_cubescanSettings.ApiKey))
            {
                request += $"&apiKey={_cubescanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CubescanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : ICubescanTransferList<TResultItem>
            where TResultItem : ICubescanTransfer
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

            if (typeof(TResult) == typeof(CubescanAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(CubescanAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(CubescanAccountERC20TokenEvents))
            {
                request = $"{request}&action=tokentx";
            }
            else if (typeof(TResult) == typeof(CubescanAccountERC721TokenEvents))
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

            if (!string.IsNullOrWhiteSpace(_cubescanSettings.ApiKey))
            {
                request += $"&apiKey={_cubescanSettings.ApiKey}";
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}