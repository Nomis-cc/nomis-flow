// ------------------------------------------------------------------------------------------------------
// <copyright file="FuseController.cs" company="Nomis">
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
using Nomis.FuseExplorer.Interfaces;
using Nomis.FuseExplorer.Interfaces.Models;
using Nomis.FuseExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Fuse
{
    /// <summary>
    /// A controller to aggregate all Fuse-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Fuse blockchain.")]
    public sealed class FuseController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/fuse";

        /// <summary>
        /// Common tag for Fuse actions.
        /// </summary>
        internal const string FuseTag = "Fuse";

        private readonly ILogger<FuseController> _logger;
        private readonly IFuseScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="FuseController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IFuseScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public FuseController(
            IFuseScoringService scoringService,
            ILogger<FuseController> logger)
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
        ///     GET /api/v1/fuse/wallet/0x135E539F0F06AD37070d6b8F444BEc65D50f0768/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetFuseWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetFuseWalletScore",
            Tags = new[] { FuseTag })]
        [ProducesResponseType(typeof(Result<FuseWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFuseWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] FuseWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<FuseWalletStatsRequest, FuseWalletScore, FuseWalletStats, FuseTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}