// ------------------------------------------------------------------------------------------------------
// <copyright file="RippleController.cs" company="Nomis">
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
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Nomis.Xrpscan.Interfaces;
using Nomis.Xrpscan.Interfaces.Models;
using Nomis.Xrpscan.Interfaces.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Ripple
{
    /// <summary>
    /// A controller to aggregate all Ripple-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Ripple blockchain.")]
    public sealed class RippleController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/ripple";

        /// <summary>
        /// Common tag for Ripple actions.
        /// </summary>
        internal const string RippleTag = "Ripple";

        private readonly ILogger<RippleController> _logger;
        private readonly IRippleScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="RippleController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IRippleScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public RippleController(
            IRippleScoringService scoringService,
            ILogger<RippleController> logger)
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
        ///     GET /api/v1/ripple/wallet/rnaM4f8sDu6XuhRL7sACrwd6Ng5LMz33av/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetRippleWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetRippleWalletScore",
            Tags = new[] { RippleTag })]
        [ProducesResponseType(typeof(Result<RippleWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetRippleWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] RippleWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<RippleWalletStatsRequest, RippleWalletScore, RippleWalletStats, RippleTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}