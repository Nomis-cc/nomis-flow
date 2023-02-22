// ------------------------------------------------------------------------------------------------------
// <copyright file="AcalaController.cs" company="Nomis">
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
using Nomis.AcalaExplorer.Interfaces;
using Nomis.AcalaExplorer.Interfaces.Models;
using Nomis.AcalaExplorer.Interfaces.Requests;
using Nomis.Api.Common.Swagger.Examples;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Acala
{
    /// <summary>
    /// A controller to aggregate all Acala-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Acala EVM blockchain.")]
    public sealed class AcalaController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/acala";

        /// <summary>
        /// Common tag for Acala actions.
        /// </summary>
        internal const string AcalaTag = "Acala";

        private readonly ILogger<AcalaController> _logger;
        private readonly IAcalaScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="AcalaController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IAcalaScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public AcalaController(
            IAcalaScoringService scoringService,
            ILogger<AcalaController> logger)
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
        ///     GET /api/v1/acala/wallet/0x26f5C2370e563e9f4dDA435f03A63D7C109D8D04/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetAcalaWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetAcalaWalletScore",
            Tags = new[] { AcalaTag })]
        [ProducesResponseType(typeof(Result<AcalaWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAcalaWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] AcalaWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<AcalaWalletStatsRequest, AcalaWalletScore, AcalaWalletStats, AcalaTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}