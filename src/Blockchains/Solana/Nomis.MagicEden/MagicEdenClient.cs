// ------------------------------------------------------------------------------------------------------
// <copyright file="MagicEdenClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Options;
using Nomis.MagicEden.Interfaces;
using Nomis.MagicEden.Interfaces.Models;
using Nomis.MagicEden.Settings;

namespace Nomis.MagicEden
{
    /// <inheritdoc cref="IMagicEdenClient"/>
    internal sealed class MagicEdenClient :
        IMagicEdenClient
    {
        private const int ItemsFetchLimit = 500;

        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="MagicEdenClient"/>.
        /// </summary>
        /// <param name="settings"><see cref="MagicEdenSettings"/>.</param>
        public MagicEdenClient(
            IOptions<MagicEdenSettings> settings)
        {
            _client = new()
            {
                BaseAddress = new(settings.Value.ApiBaseUrl!)
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MagicEdenWalletActivity>> GetWalletActivitiesData(string address)
        {
            var result = new List<MagicEdenWalletActivity>();
            var transactionsData = await GetWalletActivityList(address).ConfigureAwait(false);
            result.AddRange(transactionsData);
            int offset = 0;
            while (transactionsData.Length >= ItemsFetchLimit)
            {
                offset += ItemsFetchLimit;
                transactionsData = await GetWalletActivityList(address, offset: offset).ConfigureAwait(false);
                result.AddRange(transactionsData);
            }

            return result;
        }

        private async Task<MagicEdenWalletActivity[]> GetWalletActivityList(string address, int? offset = null)
        {
            string request =
                $"/v2/wallets/{address}/activities?offset={offset ?? 0}&limit={ItemsFetchLimit}";

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var transactionsData = await response.Content.ReadFromJsonAsync<MagicEdenWalletActivity[]>().ConfigureAwait(false);
            return transactionsData ?? Array.Empty<MagicEdenWalletActivity>();
        }
    }
}