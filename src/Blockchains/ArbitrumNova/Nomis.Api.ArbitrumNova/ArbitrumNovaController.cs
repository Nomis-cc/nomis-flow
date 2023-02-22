// ------------------------------------------------------------------------------------------------------
// <copyright file="ArbitrumNovaController.cs" company="Nomis">
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
using Nomis.NovaArbiscan.Interfaces;
using Nomis.NovaArbiscan.Interfaces.Models;
using Nomis.NovaArbiscan.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.ArbitrumNova
{
    /// <summary>
    /// A controller to aggregate all Arbitrum Nova-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Arbitrum Nova blockchain.")]
    public sealed class ArbitrumNovaController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/arbitrum-nova";

        /// <summary>
        /// Common tag for Arbitrum Nova actions.
        /// </summary>
        internal const string ArbitrumNovaTag = "ArbitrumNova";

        private readonly ILogger<ArbitrumNovaController> _logger;
        private readonly IArbitrumNovaScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="ArbitrumNovaController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IArbitrumNovaScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public ArbitrumNovaController(
            IArbitrumNovaScoringService scoringService,
            ILogger<ArbitrumNovaController> logger)
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
        ///     GET /api/v1/arbitrum-nova/wallet/0x79e145139D6BA2230F880c8D1474Bc47d6d46928/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetArbitrumNovaWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetArbitrumNovaWalletScore",
            Tags = new[] { ArbitrumNovaTag })]
        [ProducesResponseType(typeof(Result<ArbitrumNovaWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetArbitrumNovaWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] ArbitrumNovaWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<ArbitrumNovaWalletStatsRequest, ArbitrumNovaWalletScore, ArbitrumNovaWalletStats, ArbitrumNovaTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}