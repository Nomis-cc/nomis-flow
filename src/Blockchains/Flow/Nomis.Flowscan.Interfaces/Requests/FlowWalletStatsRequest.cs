// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Utils.Contracts.Requests;

namespace Nomis.Flowscan.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for Flow.
    /// </summary>
    public sealed class FlowWalletStatsRequest :
        WalletStatsRequest,
        IWalletTokensBalancesRequest
    {
        /// <inheritdoc />
        /// <example>true</example>
        [FromQuery]
        public bool GetHoldTokensBalances { get; set; } = true;

        /// <inheritdoc />
        /// <example>6</example>
        [FromQuery]
        [Range(typeof(int), "1", "8760")]
        public int SearchWidthInHours { get; set; } = 6;

        /// <inheritdoc />
        /// <example>true</example>
        [FromQuery]
        public bool UseTokenLists { get; set; } = true;

        /// <inheritdoc />
        /// <example>false</example>
        [FromQuery]
        public bool IncludeUniversalTokenLists { get; set; } = false;
    }
}