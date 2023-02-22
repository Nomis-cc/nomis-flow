// ------------------------------------------------------------------------------------------------------
// <copyright file="PoaExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.PoaExplorer.Extensions;
using Nomis.PoaExplorer.Interfaces;

namespace Nomis.PoaExplorer
{
    /// <summary>
    /// POA Explorer service registrar.
    /// </summary>
    public sealed class PoaExplorer :
        IPoaServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddPoaExplorerService();
        }
    }
}