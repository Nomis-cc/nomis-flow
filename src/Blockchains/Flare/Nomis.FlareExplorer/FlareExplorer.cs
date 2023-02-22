// ------------------------------------------------------------------------------------------------------
// <copyright file="FlareExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.FlareExplorer.Extensions;
using Nomis.FlareExplorer.Interfaces;

namespace Nomis.FlareExplorer
{
    /// <summary>
    /// Flare Explorer service registrar.
    /// </summary>
    public sealed class FlareExplorer :
        IFlareServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddFlareExplorerService();
        }
    }
}