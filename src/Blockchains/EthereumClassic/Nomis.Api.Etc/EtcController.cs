// ------------------------------------------------------------------------------------------------------
// <copyright file="EtcController.cs" company="Nomis">
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
using Nomis.EtcExplorer.Interfaces;
using Nomis.EtcExplorer.Interfaces.Models;
using Nomis.EtcExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Etc
{
    /// <summary>
    /// A controller to aggregate all ETC-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Ethereum Classic blockchain.")]
    public sealed class EtcController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/etc";

        /// <summary>
        /// Common tag for Etc actions.
        /// </summary>
        internal const string EtcTag = "Etc";

        private readonly ILogger<EtcController> _logger;
        private readonly IEtcScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="EtcController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IEtcScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public EtcController(
            IEtcScoringService scoringService,
            ILogger<EtcController> logger)
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
        ///     GET /api/v1/etc/wallet/0xd4e36Ae112915C9b18700505ee5898B562af5D4d/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetEtcWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetEtcWalletScore",
            Tags = new[] { EtcTag })]
        [ProducesResponseType(typeof(Result<EtcWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetEtcWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] EtcWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<EtcWalletStatsRequest, EtcWalletScore, EtcWalletStats, EtcTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}