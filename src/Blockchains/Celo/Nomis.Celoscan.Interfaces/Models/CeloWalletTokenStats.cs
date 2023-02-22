// ------------------------------------------------------------------------------------------------------
// <copyright file="CeloWalletTokenStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.Celoscan.Interfaces.Models
{
    /// <summary>
    /// Celo wallet token stats.
    /// </summary>
    public sealed class CeloWalletTokenStats :
        BaseEvmWalletStats<CeloTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "CELO";

        /// <summary>
        /// Token.
        /// </summary>
        [Display(Description = "Token used for calculating", GroupName = "value")]
        public string? Token { get; set; }

        /// <inheritdoc cref="IWalletCommonStats{ITransactionIntervalData}.ExcludedStatDescriptions"/>
        [JsonIgnore]
        public override IEnumerable<string> ExcludedStatDescriptions =>
            base.ExcludedStatDescriptions
                .Union(new List<string>
                {
                    nameof(Token)
                });
    }
}