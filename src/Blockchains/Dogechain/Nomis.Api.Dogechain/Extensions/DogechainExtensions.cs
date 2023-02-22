// ------------------------------------------------------------------------------------------------------
// <copyright file="DogechainExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Dogechain.Settings;
using Nomis.DogechainExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Dogechain.Extensions
{
    /// <summary>
    /// Dogechain extension methods.
    /// </summary>
    public static class DogechainExtensions
    {
        /// <summary>
        /// Add Dogechain blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithDogechainBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IDogechainServiceRegistrar, new()
        {
            return optionsBuilder
                .With<DogechainAPISettings, TServiceRegistrar>();
        }
    }
}