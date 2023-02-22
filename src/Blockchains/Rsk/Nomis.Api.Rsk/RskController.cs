// ------------------------------------------------------------------------------------------------------
// <copyright file="RskController.cs" company="Nomis">
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
using Nomis.RskExplorer.Interfaces;
using Nomis.RskExplorer.Interfaces.Models;
using Nomis.RskExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Rsk
{
    /// <summary>
    /// A controller to aggregate all RSK-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("RSK EVM blockchain.")]
    public sealed class RskController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/rsk";

        /// <summary>
        /// Common tag for Rsk actions.
        /// </summary>
        internal const string RskTag = "Rsk";

        private readonly ILogger<RskController> _logger;
        private readonly IRskScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="RskController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IRskScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public RskController(
            IRskScoringService scoringService,
            ILogger<RskController> logger)
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
        ///     GET /api/v1/rsk/wallet/0xcE204c6c456CE35256500937137e29a0799881ef/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetRskWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetRskWalletScore",
            Tags = new[] { RskTag })]
        [ProducesResponseType(typeof(Result<RskWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetRskWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] RskWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<RskWalletStatsRequest, RskWalletScore, RskWalletStats, RskTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}