// ------------------------------------------------------------------------------------------------------
// <copyright file="Nearblocks.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Nearblocks.Extensions;
using Nomis.Nearblocks.Interfaces;

namespace Nomis.Nearblocks
{
    /// <summary>
    /// Nearblocks service registrar.
    /// </summary>
    public sealed class Nearblocks :
        INearServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddNearblocksService();
        }
    }
}