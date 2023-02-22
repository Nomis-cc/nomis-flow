// ------------------------------------------------------------------------------------------------------
// <copyright file="IScrollScoringService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions;
using Nomis.Utils.Contracts.Services;

namespace Nomis.ScrollScan.Interfaces
{
    /// <summary>
    /// Scroll scoring service.
    /// </summary>
    public interface IScrollScoringService :
        IBlockchainScoringService,
        IBlockchainDescriptor,
        IInfrastructureService
    {
    }
}