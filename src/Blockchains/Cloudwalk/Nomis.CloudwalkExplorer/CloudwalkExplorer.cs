// ------------------------------------------------------------------------------------------------------
// <copyright file="CloudwalkExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.CloudwalkExplorer.Extensions;
using Nomis.CloudwalkExplorer.Interfaces;

namespace Nomis.CloudwalkExplorer
{
    /// <summary>
    /// Cloudwalk Explorer service registrar.
    /// </summary>
    public sealed class CloudwalkExplorer :
        ICloudwalkServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddCloudwalkExplorerService();
        }
    }
}