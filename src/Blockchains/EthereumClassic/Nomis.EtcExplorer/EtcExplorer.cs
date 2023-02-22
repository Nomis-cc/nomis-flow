// ------------------------------------------------------------------------------------------------------
// <copyright file="EtcExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.EtcExplorer.Extensions;
using Nomis.EtcExplorer.Interfaces;

namespace Nomis.EtcExplorer
{
    /// <summary>
    /// ETC Explorer service registrar.
    /// </summary>
    public sealed class EtcExplorer :
        IEtcServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddEtcExplorerService();
        }
    }
}