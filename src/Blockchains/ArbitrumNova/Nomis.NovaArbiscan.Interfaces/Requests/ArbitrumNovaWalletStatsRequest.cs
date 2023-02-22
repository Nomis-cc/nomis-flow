// ------------------------------------------------------------------------------------------------------
// <copyright file="ArbitrumNovaWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Requests;

namespace Nomis.NovaArbiscan.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for Arbitrum Nova.
    /// </summary>
    public sealed class ArbitrumNovaWalletStatsRequest :
        BaseEvmWalletStatsRequest
    {
    }
}