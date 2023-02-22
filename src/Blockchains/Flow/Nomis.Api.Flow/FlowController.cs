// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowController.cs" company="Nomis">
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
using Nomis.Flowscan.Interfaces;
using Nomis.Flowscan.Interfaces.Models;
using Nomis.Flowscan.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Flow
{
    /// <summary>
    /// A controller to aggregate all Flow-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Flow blockchain.")]
    public sealed class FlowController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/flow";

        /// <summary>
        /// Common tag for Flow actions.
        /// </summary>
        internal const string FlowTag = "Flow";

        private readonly ILogger<FlowController> _logger;
        private readonly IFlowScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="FlowController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IFlowScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public FlowController(
            IFlowScoringService scoringService,
            ILogger<FlowController> logger)
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
        ///     GET /api/v1/flow/wallet/0xcf0c62932f6ff1eb/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetFlowWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetFlowWalletScore",
            Tags = new[] { FlowTag })]
        [ProducesResponseType(typeof(Result<FlowWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFlowWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] FlowWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<FlowWalletStatsRequest, FlowWalletScore, FlowWalletStats, FlowTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}