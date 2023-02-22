// ------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Extensions;
using Nomis.WavesExplorer.Interfaces;
using Nomis.WavesExplorer.Settings;

namespace Nomis.WavesExplorer.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Waves Explorer service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddWavesExplorerService(
            this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            serviceProvider.GetRequiredService<ICoingeckoService>();
            serviceProvider.GetRequiredService<IScoringService>();
            serviceProvider.GetRequiredService<INonEvmSoulboundTokenService>();
            serviceProvider.GetRequiredService<IDefiLlamaService>();
            services.AddSettings<WavesExplorerSettings>(configuration);
            return services
                .AddTransient<IWavesExplorerClient, WavesExplorerClient>()
                .AddTransientInfrastructureService<IWavesScoringService, WavesExplorerService>();
        }
    }
}