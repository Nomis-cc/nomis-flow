// ------------------------------------------------------------------------------------------------------
// <copyright file="PoaWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Requests;

namespace Nomis.PoaExplorer.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for POA.
    /// </summary>
    public sealed class PoaWalletStatsRequest :
        BaseEvmWalletStatsRequest
    {
    }
}