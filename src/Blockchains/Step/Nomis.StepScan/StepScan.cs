// ------------------------------------------------------------------------------------------------------
// <copyright file="StepScan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.StepScan.Extensions;
using Nomis.StepScan.Interfaces;

namespace Nomis.StepScan
{
    /// <summary>
    /// StepScan service registrar.
    /// </summary>
    public sealed class StepScan :
        IStepServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddStepScanService();
        }
    }
}