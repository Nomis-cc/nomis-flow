// ------------------------------------------------------------------------------------------------------
// <copyright file="AuroraController.cs" company="Nomis">
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
using Nomis.Aurorascan.Interfaces;
using Nomis.Aurorascan.Interfaces.Models;
using Nomis.Aurorascan.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Aurora
{
    /// <summary>
    /// A controller to aggregate all Aurora-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Aurora blockchain.")]
    public sealed class AuroraController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/aurora";

        /// <summary>
        /// Common tag for Aurora actions.
        /// </summary>
        internal const string AuroraTag = "Aurora";

        private readonly ILogger<AuroraController> _logger;
        private readonly IAuroraScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="AuroraController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IAuroraScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public AuroraController(
            IAuroraScoringService scoringService,
            ILogger<AuroraController> logger)
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
        ///     GET /api/v1/aurora/wallet/0x46522617D727ec19744C23E31fF843FFb6DC7FD7/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetAuroraWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetAuroraWalletScore",
            Tags = new[] { AuroraTag })]
        [ProducesResponseType(typeof(Result<AuroraWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAuroraWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] AuroraWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<AuroraWalletStatsRequest, AuroraWalletScore, AuroraWalletStats, AuroraTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}