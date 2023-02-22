// ------------------------------------------------------------------------------------------------------
// <copyright file="FuseExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.FuseExplorer.Extensions;
using Nomis.FuseExplorer.Interfaces;

namespace Nomis.FuseExplorer
{
    /// <summary>
    /// Fuse Explorer service registrar.
    /// </summary>
    public sealed class FuseExplorer :
        IFuseServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddFuseExplorerService();
        }
    }
}