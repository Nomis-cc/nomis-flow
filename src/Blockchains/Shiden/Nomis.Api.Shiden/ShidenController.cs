// ------------------------------------------------------------------------------------------------------
// <copyright file="ShidenController.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nomis.Api.Common.Swagger.Examples;
using Nomis.ShidenExplorer.Interfaces;
using Nomis.ShidenExplorer.Interfaces.Models;
using Nomis.ShidenExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Shiden
{
    /// <summary>
    /// A controller to aggregate all Shiden-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Shiden EVM blockchain.")]
    public sealed class ShidenController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/shiden";

        /// <summary>
        /// Common tag for Shiden actions.
        /// </summary>
        internal const string ShidenTag = "Shiden";

        private readonly ILogger<ShidenController> _logger;
        private readonly IShidenScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="ShidenController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IShidenScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public ShidenController(
            IShidenScoringService scoringService,
            ILogger<ShidenController> logger)
        {
            _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get Nomis Score for given wallet address.
        /// </summary>
        /// <param name="request">Request for getting the wallet stats.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>An Nomis Score value and corresponding statistical data.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/shiden/wallet/0x75CDb8E2e5Cd80e6aC27863416bB80483C954dfe/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetShidenWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetShidenWalletScore",
            Tags = new[] { ShidenTag })]
        [ProducesResponseType(typeof(Result<ShidenWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetShidenWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] ShidenWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<ShidenWalletStatsRequest, ShidenWalletScore, ShidenWalletStats, ShidenTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}