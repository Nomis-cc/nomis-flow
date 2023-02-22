// ------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.Chainanalysis.Interfaces;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.Greysafe.Interfaces;
using Nomis.ScoringService.Interfaces;
using Nomis.Snapshot.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.StepScan.Interfaces;
using Nomis.StepScan.Settings;
using Nomis.Utils.Extensions;

namespace Nomis.StepScan.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add StepScan service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddStepScanService(
            this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            serviceProvider.GetRequiredService<ICoingeckoService>();
            serviceProvider.GetRequiredService<IScoringService>();
            serviceProvider.GetRequiredService<IEvmSoulboundTokenService>();
            serviceProvider.GetRequiredService<ISnapshotService>();
            serviceProvider.GetRequiredService<IDefiLlamaService>();
            serviceProvider.GetRequiredService<IChainanalysisService>();
            serviceProvider.GetRequiredService<IGreysafeService>();
            services.AddSettings<StepScanSettings>(configuration);
            return services
                .AddTransient<IStepScanClient, StepScanClient>()
                .AddTransientInfrastructureService<IStepScoringService, StepScanService>();
        }
    }
}