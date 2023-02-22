// ------------------------------------------------------------------------------------------------------
// <copyright file="OasisController.cs" company="Nomis">
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
using Nomis.OasisExplorer.Interfaces;
using Nomis.OasisExplorer.Interfaces.Models;
using Nomis.OasisExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Oasis
{
    /// <summary>
    /// A controller to aggregate all Oasis-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Oasis Emerald blockchain.")]
    public sealed class OasisController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/oasis-emerald";

        /// <summary>
        /// Common tag for Oasis actions.
        /// </summary>
        internal const string OasisTag = "Oasis";

        private readonly ILogger<OasisController> _logger;
        private readonly IOasisScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="OasisController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IOasisScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public OasisController(
            IOasisScoringService scoringService,
            ILogger<OasisController> logger)
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
        ///     GET /api/v1/oasis-emerald/wallet/0x858BAb63629f8b1BA4679AdC7765A6dad91d5198/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetOasisWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetOasisWalletScore",
            Tags = new[] { OasisTag })]
        [ProducesResponseType(typeof(Result<OasisWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetOasisWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] OasisWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<OasisWalletStatsRequest, OasisWalletScore, OasisWalletStats, OasisTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}