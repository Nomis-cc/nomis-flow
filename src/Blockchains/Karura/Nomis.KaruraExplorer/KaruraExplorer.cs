// ------------------------------------------------------------------------------------------------------
// <copyright file="KaruraExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.KaruraExplorer.Extensions;
using Nomis.KaruraExplorer.Interfaces;

namespace Nomis.KaruraExplorer
{
    /// <summary>
    /// Karura Explorer service registrar.
    /// </summary>
    public sealed class KaruraExplorer :
        IKaruraServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddKaruraExplorerService();
        }
    }
}