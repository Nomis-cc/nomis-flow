// ------------------------------------------------------------------------------------------------------
// <copyright file="BitTorrentWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Requests;

namespace Nomis.Bttcscan.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for BitTorrent.
    /// </summary>
    public sealed class BitTorrentWalletStatsRequest :
        BaseEvmWalletStatsRequest
    {
    }
}