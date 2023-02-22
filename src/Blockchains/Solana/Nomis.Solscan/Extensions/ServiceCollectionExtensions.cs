// ------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.Coingecko.Interfaces;
using Nomis.Greysafe.Interfaces;
using Nomis.HapiExplorer.Interfaces;
using Nomis.ScoringService.Interfaces;
using Nomis.Solscan.Interfaces;
using Nomis.Solscan.Settings;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Extensions;

namespace Nomis.Solscan.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Solscan service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddSolscanService(
            this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            serviceProvider.GetRequiredService<ICoingeckoService>();
            serviceProvider.GetRequiredService<IScoringService>();
            serviceProvider.GetRequiredService<IHapiExplorerService>();
            serviceProvider.GetRequiredService<INonEvmSoulboundTokenService>();
            serviceProvider.GetRequiredService<IGreysafeService>();
            services.AddSettings<SolscanSettings>(configuration);
            return services
                .AddTransient<ISolscanClient, SolscanClient>()
                .AddTransientInfrastructureService<ISolanaScoringService, SolscanService>();
        }
    }
}