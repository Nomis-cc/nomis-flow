// ------------------------------------------------------------------------------------------------------
// <copyright file="CloudwalkController.cs" company="Nomis">
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
using Nomis.CloudwalkExplorer.Interfaces;
using Nomis.CloudwalkExplorer.Interfaces.Models;
using Nomis.CloudwalkExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Cloudwalk
{
    /// <summary>
    /// A controller to aggregate all Cloudwalk-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Cloudwalk blockchain.")]
    public sealed class CloudwalkController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/cloudwalk";

        /// <summary>
        /// Common tag for Cloudwalk actions.
        /// </summary>
        internal const string CloudwalkTag = "Cloudwalk";

        private readonly ILogger<CloudwalkController> _logger;
        private readonly ICloudwalkScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="CloudwalkController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="ICloudwalkScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public CloudwalkController(
            ICloudwalkScoringService scoringService,
            ILogger<CloudwalkController> logger)
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
        ///     GET /api/v1/cloudwalk/wallet/0xc2F37F287E6c8d4eFE81DFA6b16B736322dc4c0a/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetCloudwalkWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetCloudwalkWalletScore",
            Tags = new[] { CloudwalkTag })]
        [ProducesResponseType(typeof(Result<CloudwalkWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetCloudwalkWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] CloudwalkWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<CloudwalkWalletStatsRequest, CloudwalkWalletScore, CloudwalkWalletStats, CloudwalkTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}