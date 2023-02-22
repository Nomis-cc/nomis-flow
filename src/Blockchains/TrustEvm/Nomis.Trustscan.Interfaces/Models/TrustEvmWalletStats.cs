// ------------------------------------------------------------------------------------------------------
// <copyright file="TrustEvmWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.Trustscan.Interfaces.Models
{
    /// <summary>
    /// Trust EVM wallet stats.
    /// </summary>
    public sealed class TrustEvmWalletStats :
        BaseEvmWalletStats<TrustEvmTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "ETH";
    }
}