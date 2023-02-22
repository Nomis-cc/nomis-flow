// ------------------------------------------------------------------------------------------------------
// <copyright file="MantleExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.MantleExplorer.Extensions;
using Nomis.MantleExplorer.Interfaces;

namespace Nomis.MantleExplorer
{
    /// <summary>
    /// Mantle Explorer service registrar.
    /// </summary>
    public sealed class MantleExplorer :
        IMantleServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddMantleExplorerService();
        }
    }
}