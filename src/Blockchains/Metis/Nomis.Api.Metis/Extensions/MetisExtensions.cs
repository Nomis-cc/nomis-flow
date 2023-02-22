// ------------------------------------------------------------------------------------------------------
// <copyright file="MetisExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.AndromedaExplorer.Interfaces;
using Nomis.Api.Common.Extensions;
using Nomis.Api.Metis.Settings;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Metis.Extensions
{
    /// <summary>
    /// Metis extension methods.
    /// </summary>
    public static class MetisExtensions
    {
        /// <summary>
        /// Add Metis blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IScoringOptionsBuilder WithMetisBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IMetisServiceRegistrar, new()
        {
            return optionsBuilder
                .With<MetisAPISettings, TServiceRegistrar>();
        }
    }
}