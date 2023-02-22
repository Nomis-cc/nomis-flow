// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Klaytn.Settings;
using Nomis.Klaytnscope.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Klaytn.Extensions
{
    /// <summary>
    /// Klaytn extension methods.
    /// </summary>
    public static class KlaytnExtensions
    {
        /// <summary>
        /// Add Klaytn blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithKlaytnBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IKlaytnServiceRegistrar, new()
        {
            return optionsBuilder
                .With<KlaytnAPISettings, TServiceRegistrar>();
        }
    }
}