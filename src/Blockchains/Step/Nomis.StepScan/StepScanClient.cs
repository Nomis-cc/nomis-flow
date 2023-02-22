// ------------------------------------------------------------------------------------------------------
// <copyright file="StepScanClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.StepScan.Interfaces;
using Nomis.StepScan.Interfaces.Models;
using Nomis.StepScan.Settings;
using Nomis.Utils.Exceptions;

namespace Nomis.StepScan
{
    /// <inheritdoc cref="IStepScanClient"/>
    internal sealed class StepScanClient :
        IStepScanClient
    {
        private const int ItemsFetchLimit = 10000;
        private readonly StepScanSettings _stepExplorerSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="StepScanClient"/>.
        /// </summary>
        /// <param name="stepExplorerSettings"><see cref="StepScanSettings"/>.</param>
        public StepScanClient(
            IOptions<StepScanSettings> stepExplorerSettings)
        {
            _stepExplorerSettings = stepExplorerSettings.Value;
            _client = new()
            {
                BaseAddress = new(stepExplorerSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(stepExplorerSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<StepScanAccount> GetBalanceAsync(string address)
        {
            string request =
                $"api?module=account&action=balance&address={address}&tag=latest";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StepScanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<StepScanAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            string request =
                $"api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StepScanAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IStepScanTransferList<TResultItem>
            where TResultItem : IStepScanTransfer
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

            if (typeof(TResult) == typeof(StepScanAccountNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(StepScanAccountInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(StepScanAccountERC20TokenEvents))
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