// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Waves.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.WavesExplorer.Interfaces;

namespace Nomis.Api.Waves.Extensions
{
    /// <summary>
    /// Waves extension methods.
    /// </summary>
    public static class WavesExtensions
    {
        /// <summary>
        /// Add Waves blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithWavesBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IWavesServiceRegistrar, new()
        {
            return optionsBuilder
                .With<WavesAPISettings, TServiceRegistrar>();
        }
    }
}