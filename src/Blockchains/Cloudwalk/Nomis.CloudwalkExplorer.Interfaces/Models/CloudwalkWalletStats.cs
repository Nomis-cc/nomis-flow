// ------------------------------------------------------------------------------------------------------
// <copyright file="CloudwalkWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.CloudwalkExplorer.Interfaces.Models
{
    /// <summary>
    /// Cloudwalk wallet stats.
    /// </summary>
    public sealed class CloudwalkWalletStats :
        BaseEvmWalletStats<CloudwalkTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "CWN";
    }
}