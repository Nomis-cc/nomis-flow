// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.AeternityExplorer.Interfaces;
using Nomis.AeternityExplorer.Interfaces.Models;
using Nomis.AeternityExplorer.Settings;
using Nomis.Blockchain.Abstractions.Converters;
using Nomis.Utils.Exceptions;

namespace Nomis.AeternityExplorer
{
    /// <inheritdoc cref="IAeternityExplorerClient"/>
    internal sealed class AeternityExplorerClient :
        IAeternityExplorerClient
    {
        private const int ItemsFetchLimit = 1000;
        private readonly AeternityExplorerSettings _aeternityExplorerSettings;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="AeternityExplorerClient"/>.
        /// </summary>
        /// <param name="aeternityExplorerSettings"><see cref="AeternityExplorerSettings"/>.</param>
        public AeternityExplorerClient(
            IOptions<AeternityExplorerSettings> aeternityExplorerSettings)
        {
            _aeternityExplorerSettings = aeternityExplorerSettings.Value;
            _client = new()
            {
                BaseAddress = new(aeternityExplorerSettings.Value.ApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(aeternityExplorerSettings.Value.ApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<AeternityExplorerAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/v3/accounts/{address}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AeternityExplorerAccount>(new JsonSerializerOptions
            {
                Converters = { new BigIntegerConverter() }
            }).ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address, bool from = true)
            where TResult : IAeternityExplorerTransferList<TResultItem>
            where TResultItem : class
        {
            var result = new List<TResultItem>();
            var transactionsData = await GetTransactionList<TResult>(address, from).ConfigureAwait(false);
            result.AddRange(transactionsData.Data ?? new List<TResultItem>());
            while (!string.IsNullOrWhiteSpace(transactionsData?.Next) && result.Count < ItemsFetchLimit)
            {
                transactionsData = await GetTransactionList<TResult>(address, from, transactionsData.Next).ConfigureAwait(false);
                result.AddRange(transactionsData?.Data ?? new List<TResultItem>());
            }

            return result;
        }

        private async Task<TResult> GetTransactionList<TResult>(
            string address,
            bool from = true,
            string? next = null)
        {
            string request = "/mdw";

            if (typeof(TResult) == typeof(AeternityExplorerAccountNormalTransactions))
            {
                if (!string.IsNullOrWhiteSpace(next))
                {
                    request += next;
                }
                else
                {
                    request += $"/txs/backward?account={address}&limit=100";
                }
            }
            else if (typeof(TResult) == typeof(AeternityExplorerAccountInternalTransactions))
            {
                if (!string.IsNullOrWhiteSpace(next))
                {
                    request += next;
                }
                else
                {
                    request += $"/v2/transfers?account={address}&limit=100";
                }
            }
            else if (typeof(TResult) == typeof(AeternityExplorerAccountAEX9TokenEvents))
            {
                if (!string.IsNullOrWhiteSpace(next))
                {
                    request += next;
                }
                else
                {
                    request += $"/v2/aex9/transfers/{(from ? "from" : "to")}/{address}";
                }
            }
            else if (typeof(TResult) == typeof(AeternityExplorerAccountAEX141TokenEvents))
            {
                if (!string.IsNullOrWhiteSpace(next))
                {
                    request += next;
                }
                else
                {
                    request += $"/v2/aex141/transfers/{(from ? "from" : "to")}/{address}";
                }
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResult>(new JsonSerializerOptions
            {
                Converters = { new BigIntegerConverter() }
            }).ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}