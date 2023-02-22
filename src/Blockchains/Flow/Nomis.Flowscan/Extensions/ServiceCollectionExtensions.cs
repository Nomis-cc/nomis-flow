// ------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.DefiLlama.Interfaces;
using Nomis.Flowscan.Interfaces;

using Nomis.Flowscan.Settings;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Extensions;

namespace Nomis.Flowscan.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Flowscan service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddFlowscanService(
            this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var settings = configuration.GetSettings<FlowscanSettings>();
            serviceProvider.GetRequiredService<IScoringService>();
            serviceProvider.GetRequiredService<INonEvmSoulboundTokenService>();
            services.AddSingleton<IFlowscanGraphQLClient>(_ =>
            {
                var graphQlOptions = new GraphQLHttpClientOptions
                {
                    EndPoint = new($"{settings.ApiBaseUrl}?token={settings.ApiKey}")
                };
                return new FlowscanGraphQLClient(graphQlOptions, new SystemTextJsonSerializer());
            });

            return services
                .AddScopedInfrastructureService<IFlowScoringService, FlowscanService>();
        }
    }
}