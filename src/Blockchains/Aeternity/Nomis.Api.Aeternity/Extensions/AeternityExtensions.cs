// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.AeternityExplorer.Interfaces;
using Nomis.Api.Aeternity.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Aeternity.Extensions
{
    /// <summary>
    /// Aeternity extension methods.
    /// </summary>
    public static class AeternityExtensions
    {
        /// <summary>
        /// Add Aeternity blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithAeternityBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IAeternityServiceRegistrar, new()
        {
            return optionsBuilder
                .With<AeternityAPISettings, TServiceRegistrar>();
        }
    }
}