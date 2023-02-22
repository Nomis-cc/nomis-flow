// ------------------------------------------------------------------------------------------------------
// <copyright file="TronExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Tron.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.Tronscan.Interfaces;

namespace Nomis.Api.Tron.Extensions
{
    /// <summary>
    /// Tron extension methods.
    /// </summary>
    public static class TronExtensions
    {
        /// <summary>
        /// Add Tron blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithTronBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ITronServiceRegistrar, new()
        {
            return optionsBuilder
                .With<TronAPISettings, TServiceRegistrar>();
        }
    }
}