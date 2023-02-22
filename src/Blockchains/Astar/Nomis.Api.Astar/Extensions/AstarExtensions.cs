// ------------------------------------------------------------------------------------------------------
// <copyright file="AstarExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Astar.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.AstarExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Astar.Extensions
{
    /// <summary>
    /// Astar extension methods.
    /// </summary>
    public static class AstarExtensions
    {
        /// <summary>
        /// Add Astar blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithAstarBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IAstarServiceRegistrar, new()
        {
            return optionsBuilder
                .With<AstarAPISettings, TServiceRegistrar>();
        }
    }
}