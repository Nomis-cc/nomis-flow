// ------------------------------------------------------------------------------------------------------
// <copyright file="HaqqController.cs" company="Nomis">
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
using Nomis.HaqqExplorer.Interfaces;
using Nomis.HaqqExplorer.Interfaces.Models;
using Nomis.HaqqExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Haqq
{
    /// <summary>
    /// A controller to aggregate all Haqq-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("HAQQ blockchain.")]
    public sealed class HaqqController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/haqq";

        /// <summary>
        /// Common tag for Haqq actions.
        /// </summary>
        internal const string HaqqTag = "Haqq";

        private readonly ILogger<HaqqController> _logger;
        private readonly IHaqqScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="HaqqController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IHaqqScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public HaqqController(
            IHaqqScoringService scoringService,
            ILogger<HaqqController> logger)
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
        ///     GET /api/v1/haqq/wallet/0xb67208222505D78b467BDAf2013b95ABC9a57f6C/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetHaqqWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetHaqqWalletScore",
            Tags = new[] { HaqqTag })]
        [ProducesResponseType(typeof(Result<HaqqWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetHaqqWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] HaqqWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<HaqqWalletStatsRequest, HaqqWalletScore, HaqqWalletStats, HaqqTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}