// ------------------------------------------------------------------------------------------------------
// <copyright file="ArbitrumNovaExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.ArbitrumNova.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.NovaArbiscan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.ArbitrumNova.Extensions
{
    /// <summary>
    /// Arbitrum Nova extension methods.
    /// </summary>
    public static class ArbitrumNovaExtensions
    {
        /// <summary>
        /// Add Arbitrum Nova blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithArbitrumNovaBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IArbitrumNovaServiceRegistrar, new()
        {
            return optionsBuilder
                .With<ArbitrumNovaAPISettings, TServiceRegistrar>();
        }
    }
}