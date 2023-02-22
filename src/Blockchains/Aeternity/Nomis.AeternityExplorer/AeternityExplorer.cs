// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.AeternityExplorer.Extensions;
using Nomis.AeternityExplorer.Interfaces;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;

namespace Nomis.AeternityExplorer
{
    /// <summary>
    /// AeternityExplorer service registrar.
    /// </summary>
    public sealed class AeternityExplorer :
        IAeternityServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            serviceProvider.GetRequiredService<ICoingeckoService>();
            serviceProvider.GetRequiredService<IScoringService>();
            serviceProvider.GetRequiredService<INonEvmSoulboundTokenService>();
            serviceProvider.GetRequiredService<IDefiLlamaService>();
            return services
                .AddAeternityExplorerService(configuration);
        }
    }
}