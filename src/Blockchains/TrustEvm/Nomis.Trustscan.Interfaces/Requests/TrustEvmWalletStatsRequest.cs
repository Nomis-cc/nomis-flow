// ------------------------------------------------------------------------------------------------------
// <copyright file="TrustEvmWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Requests;

namespace Nomis.Trustscan.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for Trust EVM.
    /// </summary>
    public sealed class TrustEvmWalletStatsRequest :
        BaseEvmWalletStatsRequest
    {
    }
}