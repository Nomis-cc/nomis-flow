// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Requests;

namespace Nomis.Klaytnscope.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for Klaytn.
    /// </summary>
    public sealed class KlaytnWalletStatsRequest :
        BaseEvmWalletStatsRequest
    {
    }
}