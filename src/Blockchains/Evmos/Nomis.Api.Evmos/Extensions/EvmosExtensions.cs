// ------------------------------------------------------------------------------------------------------
// <copyright file="EvmosExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Evmos.Settings;
using Nomis.EvmosAPI.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Evmos.Extensions
{
    /// <summary>
    /// Evmos extension methods.
    /// </summary>
    public static class EvmosExtensions
    {
        /// <summary>
        /// Add Evmos blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithEvmosBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IEvmosServiceRegistrar, new()
        {
            return optionsBuilder
                .With<EvmosAPISettings, TServiceRegistrar>();
        }
    }
}