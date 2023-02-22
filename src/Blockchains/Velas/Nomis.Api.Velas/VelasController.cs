// ------------------------------------------------------------------------------------------------------
// <copyright file="VelasController.cs" company="Nomis">
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
using Nomis.Utils.Wrapper;
using Nomis.VelasExplorer.Interfaces;
using Nomis.VelasExplorer.Interfaces.Models;
using Nomis.VelasExplorer.Interfaces.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Velas
{
    /// <summary>
    /// A controller to aggregate all Velas-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Velas EVM blockchain.")]
    public sealed class VelasController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/velas";

        /// <summary>
        /// Common tag for Velas actions.
        /// </summary>
        internal const string VelasTag = "Velas";

        private readonly ILogger<VelasController> _logger;
        private readonly IVelasScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="VelasController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IVelasScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public VelasController(
            IVelasScoringService scoringService,
            ILogger<VelasController> logger)
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
        ///     GET /api/v1/velas/wallet/0x34d1FCa4cCF69be0245775270f8D4f318c173509/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetVelasWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetVelasWalletScore",
            Tags = new[] { VelasTag })]
        [ProducesResponseType(typeof(Result<VelasWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetVelasWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] VelasWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<VelasWalletStatsRequest, VelasWalletScore, VelasWalletStats, VelasTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}