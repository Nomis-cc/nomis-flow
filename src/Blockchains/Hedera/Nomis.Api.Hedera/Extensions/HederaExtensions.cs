// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Hedera.Settings;
using Nomis.HederaMirrorNode.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Hedera.Extensions
{
    /// <summary>
    /// Hedera extension methods.
    /// </summary>
    public static class HederaExtensions
    {
        /// <summary>
        /// Add Hedera blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithHederaBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IHederaServiceRegistrar, new()
        {
            return optionsBuilder
                .With<HederaAPISettings, TServiceRegistrar>();
        }
    }
}