// ------------------------------------------------------------------------------------------------------
// <copyright file="AndromedaExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.AndromedaExplorer.Extensions;
using Nomis.AndromedaExplorer.Interfaces;

namespace Nomis.AndromedaExplorer
{
    /// <summary>
    /// Andromeda Explorer service registrar.
    /// </summary>
    public sealed class AndromedaExplorer :
        IMetisServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddAndromedaExplorerService();
        }
    }
}