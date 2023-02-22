﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="CubeWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.Cubescan.Interfaces.Models
{
    /// <summary>
    /// Cube wallet stats.
    /// </summary>
    public sealed class CubeWalletStats :
        BaseEvmWalletStats<CubeTransactionIntervalData>,
        IWalletNftStats
    {
        /// <inheritdoc/>
        public override string NativeToken => "CUBE";

        /// <inheritdoc/>
        [Display(Description = "Total NFTs on wallet", GroupName = "number")]
        public int NftHolding { get; init; }

        /// <inheritdoc/>
        [Display(Description = "NFT trading activity", GroupName = "Native token")]
        public decimal NftTrading { get; init; }

        /// <inheritdoc/>
        [Display(Description = "NFT worth on wallet", GroupName = "Native token")]
        public decimal NftWorth { get; init; }
    }
}