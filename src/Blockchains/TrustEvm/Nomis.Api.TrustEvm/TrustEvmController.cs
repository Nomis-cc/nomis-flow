// ------------------------------------------------------------------------------------------------------
// <copyright file="TrustEvmController.cs" company="Nomis">
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
using Nomis.Trustscan.Interfaces;
using Nomis.Trustscan.Interfaces.Models;
using Nomis.Trustscan.Interfaces.Requests;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.TrustEvm
{
    /// <summary>
    /// A controller to aggregate all Trust EVM-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Trust EVM Testnet blockchain.")]
    public sealed class TrustEvmController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/trust-evm-testnet";

        /// <summary>
        /// Common tag for Trust EVM actions.
        /// </summary>
        internal const string TrustEvmTag = "TrustEvm";

        private readonly ILogger<TrustEvmController> _logger;
        private readonly ITrustEvmScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="TrustEvmController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="ITrustEvmScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public TrustEvmController(
            ITrustEvmScoringService scoringService,
            ILogger<TrustEvmController> logger)
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
        ///     GET /api/v1/trust-evm-testnet/wallet/0x770bc2BBF68A22AEDA57076a280fdfEe457c2b6A/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetTrustEvmWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetTrustEvmWalletScore",
            Tags = new[] { TrustEvmTag })]
        [ProducesResponseType(typeof(Result<TrustEvmWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetTrustEvmWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] TrustEvmWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<TrustEvmWalletStatsRequest, TrustEvmWalletScore, TrustEvmWalletStats, TrustEvmTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}