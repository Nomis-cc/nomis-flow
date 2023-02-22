// ------------------------------------------------------------------------------------------------------
// <copyright file="ShidenWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Requests;

namespace Nomis.ShidenExplorer.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for Shiden.
    /// </summary>
    public sealed class ShidenWalletStatsRequest :
        BaseEvmWalletStatsRequest
    {
    }
}