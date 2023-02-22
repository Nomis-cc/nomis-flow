﻿// ------------------------------------------------------------------------------------------------------
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
using Nomis.Snowtrace.Interfaces;
using Nomis.Snowtrace.Settings;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Extensions;

namespace Nomis.Snowtrace.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Snowtrace service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddSnowtraceService(
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
            services.AddSettings<SnowtraceSettings>(configuration);
            return services
                .AddTransient<ISnowtraceClient, SnowtraceClient>()
                .AddTransientInfrastructureService<IAvalancheScoringService, SnowtraceService>();
        }
    }
}