// ------------------------------------------------------------------------------------------------------
// <copyright file="IMoonriverScoringService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions;
using Nomis.Utils.Contracts.Services;

namespace Nomis.MoonriverMoonscan.Interfaces
{
    /// <summary>
    /// Moonriver scoring service.
    /// </summary>
    public interface IMoonriverScoringService :
        IBlockchainScoringService,
        IBlockchainDescriptor,
        IInfrastructureService
    {
    }
}