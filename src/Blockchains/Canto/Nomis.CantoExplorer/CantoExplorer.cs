// ------------------------------------------------------------------------------------------------------
// <copyright file="CantoExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.CantoExplorer.Extensions;
using Nomis.CantoExplorer.Interfaces;

namespace Nomis.CantoExplorer
{
    /// <summary>
    /// Canto Explorer service registrar.
    /// </summary>
    public sealed class CantoExplorer :
        ICantoServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddCantoExplorerService();
        }
    }
}