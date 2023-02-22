// ------------------------------------------------------------------------------------------------------
// <copyright file="RskExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.RskExplorer.Extensions;
using Nomis.RskExplorer.Interfaces;

namespace Nomis.RskExplorer
{
    /// <summary>
    /// RSK Explorer service registrar.
    /// </summary>
    public sealed class RskExplorer :
        IRskServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddRskExplorerService();
        }
    }
}