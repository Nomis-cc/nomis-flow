// ------------------------------------------------------------------------------------------------------
// <copyright file="OasisExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.OasisExplorer.Extensions;
using Nomis.OasisExplorer.Interfaces;

namespace Nomis.OasisExplorer
{
    /// <summary>
    /// Oasis Explorer service registrar.
    /// </summary>
    public sealed class OasisExplorer :
        IOasisServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddOasisExplorerService();
        }
    }
}