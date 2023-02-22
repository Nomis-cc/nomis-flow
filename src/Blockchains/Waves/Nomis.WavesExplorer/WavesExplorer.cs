// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.WavesExplorer.Extensions;
using Nomis.WavesExplorer.Interfaces;

namespace Nomis.WavesExplorer
{
    /// <summary>
    /// WavesExplorer service registrar.
    /// </summary>
    public sealed class WavesExplorer :
        IWavesServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddWavesExplorerService();
        }
    }
}