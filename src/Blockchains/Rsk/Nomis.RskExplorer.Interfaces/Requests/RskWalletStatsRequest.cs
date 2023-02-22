// ------------------------------------------------------------------------------------------------------
// <copyright file="RskWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Requests;

namespace Nomis.RskExplorer.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for RSK.
    /// </summary>
    public sealed class RskWalletStatsRequest :
        BaseEvmWalletStatsRequest
    {
    }
}