// ------------------------------------------------------------------------------------------------------
// <copyright file="AstarExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.AstarExplorer.Extensions;
using Nomis.AstarExplorer.Interfaces;

namespace Nomis.AstarExplorer
{
    /// <summary>
    /// Astar Explorer service registrar.
    /// </summary>
    public sealed class AstarExplorer :
        IAstarServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddAstarExplorerService();
        }
    }
}