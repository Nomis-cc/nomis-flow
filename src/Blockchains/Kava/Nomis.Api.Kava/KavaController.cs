// ------------------------------------------------------------------------------------------------------
// <copyright file="KavaController.cs" company="Nomis">
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
using Nomis.KavaExplorer.Interfaces;
using Nomis.KavaExplorer.Interfaces.Models;
using Nomis.KavaExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Kava
{
    /// <summary>
    /// A controller to aggregate all Kava-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Kava blockchain.")]
    public sealed class KavaController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/kava-evm";

        /// <summary>
        /// Common tag for Kava actions.
        /// </summary>
        internal const string KavaTag = "Kava";

        private readonly ILogger<KavaController> _logger;
        private readonly IKavaScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="KavaController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IKavaScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public KavaController(
            IKavaScoringService scoringService,
            ILogger<KavaController> logger)
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
        ///     GET /api/v1/kava-evm/wallet/0x9E61b82d31387907489ea889158dC41Fe3C895fb/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetKavaWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetKavaWalletScore",
            Tags = new[] { KavaTag })]
        [ProducesResponseType(typeof(Result<KavaWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetKavaWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] KavaWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<KavaWalletStatsRequest, KavaWalletScore, KavaWalletStats, KavaTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}