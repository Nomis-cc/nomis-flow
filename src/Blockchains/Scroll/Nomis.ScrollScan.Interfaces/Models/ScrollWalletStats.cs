// ------------------------------------------------------------------------------------------------------
// <copyright file="ScrollWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.ScrollScan.Interfaces.Models
{
    /// <summary>
    /// Scroll wallet stats.
    /// </summary>
    public sealed class ScrollWalletStats :
        BaseEvmWalletStats<ScrollTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "ETH";
    }
}