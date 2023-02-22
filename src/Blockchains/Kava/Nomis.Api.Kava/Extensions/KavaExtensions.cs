// ------------------------------------------------------------------------------------------------------
// <copyright file="KavaExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Kava.Settings;
using Nomis.KavaExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Kava.Extensions
{
    /// <summary>
    /// Kava extension methods.
    /// </summary>
    public static class KavaExtensions
    {
        /// <summary>
        /// Add Kava blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithKavaBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IKavaServiceRegistrar, new()
        {
            return optionsBuilder
                .With<KavaAPISettings, TServiceRegistrar>();
        }
    }
}