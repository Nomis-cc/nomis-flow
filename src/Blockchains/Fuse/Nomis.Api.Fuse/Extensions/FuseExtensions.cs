// ------------------------------------------------------------------------------------------------------
// <copyright file="FuseExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Fuse.Settings;
using Nomis.FuseExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Fuse.Extensions
{
    /// <summary>
    /// Fuse extension methods.
    /// </summary>
    public static class FuseExtensions
    {
        /// <summary>
        /// Add Fuse blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithFuseBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IFuseServiceRegistrar, new()
        {
            return optionsBuilder
                .With<FuseAPISettings, TServiceRegistrar>();
        }
    }
}