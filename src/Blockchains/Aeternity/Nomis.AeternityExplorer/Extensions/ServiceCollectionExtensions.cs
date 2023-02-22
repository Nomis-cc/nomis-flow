// ------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.AeternityExplorer.Interfaces;
using Nomis.AeternityExplorer.Settings;
using Nomis.DefiLlama.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Extensions;

namespace Nomis.AeternityExplorer.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add AeternityExplorer service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddAeternityExplorerService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<IDefiLlamaService>();
            services.AddSettings<AeternityExplorerSettings>(configuration);
            serviceProvider.GetRequiredService<INonEvmSoulboundTokenService>();
            return services
                .AddTransient<IAeternityExplorerClient, AeternityExplorerClient>()
                .AddTransientInfrastructureService<IAeternityScoringService, AeternityExplorerService>();
        }
    }
}