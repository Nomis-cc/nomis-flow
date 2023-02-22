// ------------------------------------------------------------------------------------------------------
// <copyright file="FlareController.cs" company="Nomis">
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
using Nomis.FlareExplorer.Interfaces;
using Nomis.FlareExplorer.Interfaces.Models;
using Nomis.FlareExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Flare
{
    /// <summary>
    /// A controller to aggregate all Flare-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Flare blockchain.")]
    public sealed class FlareController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/flare";

        /// <summary>
        /// Common tag for Flare actions.
        /// </summary>
        internal const string FlareTag = "Flare";

        private readonly ILogger<FlareController> _logger;
        private readonly IFlareScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="FlareController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IFlareScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public FlareController(
            IFlareScoringService scoringService,
            ILogger<FlareController> logger)
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
        ///     GET /api/v1/flare/wallet/0x2a7FB8B232cbe5265CFc1744C3f5f78D6cADf5b9/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetFlareWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetFlareWalletScore",
            Tags = new[] { FlareTag })]
        [ProducesResponseType(typeof(Result<FlareWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFlareWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] FlareWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<FlareWalletStatsRequest, FlareWalletScore, FlareWalletStats, FlareTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}