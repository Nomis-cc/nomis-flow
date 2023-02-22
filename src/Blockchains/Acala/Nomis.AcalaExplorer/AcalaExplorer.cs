// ------------------------------------------------------------------------------------------------------
// <copyright file="AcalaExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.AcalaExplorer.Extensions;
using Nomis.AcalaExplorer.Interfaces;

namespace Nomis.AcalaExplorer
{
    /// <summary>
    /// Acala Explorer service registrar.
    /// </summary>
    public sealed class AcalaExplorer :
        IAcalaServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddAcalaExplorerService();
        }
    }
}