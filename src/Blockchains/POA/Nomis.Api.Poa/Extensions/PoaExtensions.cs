// ------------------------------------------------------------------------------------------------------
// <copyright file="PoaExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Poa.Settings;
using Nomis.PoaExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Poa.Extensions
{
    /// <summary>
    /// POA extension methods.
    /// </summary>
    public static class PoaExtensions
    {
        /// <summary>
        /// Add POA blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IScoringOptionsBuilder WithPOABlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IPoaServiceRegistrar, new()
        {
            return optionsBuilder
                .With<PoaAPISettings, TServiceRegistrar>();
        }
    }
}