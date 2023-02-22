// ------------------------------------------------------------------------------------------------------
// <copyright file="PoaController.cs" company="Nomis">
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
using Nomis.PoaExplorer.Interfaces;
using Nomis.PoaExplorer.Interfaces.Models;
using Nomis.PoaExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Poa
{
    /// <summary>
    /// A controller to aggregate all POA-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("POA blockchain.")]
    public sealed class PoaController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/poa";

        /// <summary>
        /// Common tag for Poa actions.
        /// </summary>
        internal const string PoaTag = "Poa";

        private readonly ILogger<PoaController> _logger;
        private readonly IPoaScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="PoaController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IPoaScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public PoaController(
            IPoaScoringService scoringService,
            ILogger<PoaController> logger)
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
        ///     GET /api/v1/poa/wallet/0x6a422FB1625aB61e41b3224b549E3F2238590404/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetPoaWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetPoaWalletScore",
            Tags = new[] { PoaTag })]
        [ProducesResponseType(typeof(Result<PoaWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetPoaWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] PoaWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<PoaWalletStatsRequest, PoaWalletScore, PoaWalletStats, PoaTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}