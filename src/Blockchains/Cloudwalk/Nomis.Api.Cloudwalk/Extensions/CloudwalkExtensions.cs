// ------------------------------------------------------------------------------------------------------
// <copyright file="CloudwalkExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Cloudwalk.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.CloudwalkExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Cloudwalk.Extensions
{
    /// <summary>
    /// Cloudwalk extension methods.
    /// </summary>
    public static class CloudwalkExtensions
    {
        /// <summary>
        /// Add Cloudwalk blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithCloudwalkBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ICloudwalkServiceRegistrar, new()
        {
            return optionsBuilder
                .With<CloudwalkAPISettings, TServiceRegistrar>();
        }
    }
}