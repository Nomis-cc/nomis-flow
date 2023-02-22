// ------------------------------------------------------------------------------------------------------
// <copyright file="NearExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Near.Settings;
using Nomis.Nearblocks.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Near.Extensions
{
    /// <summary>
    /// Near extension methods.
    /// </summary>
    public static class NearExtensions
    {
        /// <summary>
        /// Add Near blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithNearBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : INearServiceRegistrar, new()
        {
            return optionsBuilder
                .With<NearAPISettings, TServiceRegistrar>();
        }
    }
}