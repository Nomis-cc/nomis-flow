// ------------------------------------------------------------------------------------------------------
// <copyright file="EtcExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Etc.Settings;
using Nomis.EtcExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Etc.Extensions
{
    /// <summary>
    /// Etc extension methods.
    /// </summary>
    public static class EtcExtensions
    {
        /// <summary>
        /// Add ETC blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IScoringOptionsBuilder WithETCBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IEtcServiceRegistrar, new()
        {
            return optionsBuilder
                .With<EtcAPISettings, TServiceRegistrar>();
        }
    }
}