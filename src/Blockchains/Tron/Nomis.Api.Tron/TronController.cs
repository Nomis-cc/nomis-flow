// ------------------------------------------------------------------------------------------------------
// <copyright file="TronController.cs" company="Nomis">
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
using Nomis.Tronscan.Interfaces;
using Nomis.Tronscan.Interfaces.Models;
using Nomis.Tronscan.Interfaces.Requests;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Tron
{
    /// <summary>
    /// A controller to aggregate all Tron-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Tron blockchain.")]
    public sealed class TronController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/tron";

        /// <summary>
        /// Common tag for Tron actions.
        /// </summary>
        internal const string TronTag = "Tron";

        private readonly ILogger<TronController> _logger;
        private readonly ITronScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="TronController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="ITronScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public TronController(
            ITronScoringService scoringService,
            ILogger<TronController> logger)
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
        ///     GET /api/v1/tron/wallet/TJ6MpqEjLMzUChyDcBjKPE2WHW5ZGtp6k7/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetTronWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetTronWalletScore",
            Tags = new[] { TronTag })]
        [ProducesResponseType(typeof(Result<TronWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetTronWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] TronWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<TronWalletStatsRequest, TronWalletScore, TronWalletStats, TronTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}