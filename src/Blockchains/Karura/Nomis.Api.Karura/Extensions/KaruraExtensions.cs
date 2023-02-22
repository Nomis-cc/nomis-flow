// ------------------------------------------------------------------------------------------------------
// <copyright file="KaruraExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Karura.Settings;
using Nomis.KaruraExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Karura.Extensions
{
    /// <summary>
    /// Karura extension methods.
    /// </summary>
    public static class KaruraExtensions
    {
        /// <summary>
        /// Add Karura blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithKaruraBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IKaruraServiceRegistrar, new()
        {
            return optionsBuilder
                .With<KaruraAPISettings, TServiceRegistrar>();
        }
    }
}