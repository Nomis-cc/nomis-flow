// ------------------------------------------------------------------------------------------------------
// <copyright file="OasisWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.OasisExplorer.Interfaces.Models
{
    /// <summary>
    /// Oasis wallet stats.
    /// </summary>
    public sealed class OasisWalletStats :
        BaseEvmWalletStats<OasisTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "ROSE";
    }
}