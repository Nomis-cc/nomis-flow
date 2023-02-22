// ------------------------------------------------------------------------------------------------------
// <copyright file="SolanaExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Solana.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.Solscan.Interfaces;

namespace Nomis.Api.Solana.Extensions
{
    /// <summary>
    /// Solana extension methods.
    /// </summary>
    public static class SolanaExtensions
    {
        /// <summary>
        /// Add Solana blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithSolanaBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ISolanaServiceRegistrar, new()
        {
            return optionsBuilder
                .With<SolanaAPISettings, TServiceRegistrar>();
        }
    }
}