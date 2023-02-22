// ------------------------------------------------------------------------------------------------------
// <copyright file="VelasExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.VelasExplorer.Extensions;
using Nomis.VelasExplorer.Interfaces;

namespace Nomis.VelasExplorer
{
    /// <summary>
    /// Velas Explorer service registrar.
    /// </summary>
    public sealed class VelasExplorer :
        IVelasServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddVelasExplorerService();
        }
    }
}