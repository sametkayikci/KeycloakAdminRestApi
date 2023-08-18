namespace KeycloakAdminRestApi.Controllers;

public sealed class TokenController : BaseApiController
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get Access Token For Admin Client
    /// </summary>
    /// <returns>Access Token</returns>
    /// <response code="2XX">Success</response>
    /// <response code="401">Unauthorized</response>
    [AllowAnonymous]
    [HttpPost("/admin-client/{realm}")]
    [ProducesResponseType(typeof(AccessToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAccesesToken(string realm, CancellationToken cancellationToken = default)

    {
        var accessToken = await _tokenService.GetAdminAccessTokenAsync(realm, cancellationToken);
        return Ok(accessToken);
    }

    /// <summary>
    /// Get Access Token For Specific Realm
    /// </summary>
    /// <returns>Access Token</returns>
    /// <response code="2XX">Success</response>
    /// <response code="401">Unauthorized</response>
    [AllowAnonymous]
    [HttpPost("/specific-client/{realm}")]
    [ProducesResponseType(typeof(AccessToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAccesesTokenForSpesicifRealm(string realm)
    {
        var accessToken = await _tokenService.GetAccessTokenAsync(realm);
        return Ok(accessToken);
    }

    /// <summary>
    /// Get Refresh Token
    /// </summary>
    /// <returns>Refresh Token</returns>
    /// <response code="2XX">Success</response>
    /// <response code="401">Unauthorized</response>
    [AllowAnonymous]
    [HttpPost("/{refreshtoken}/{realm}")]
    [ProducesResponseType(typeof(AccessToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRefreshToken(string refreshtoken, string realm)
    {
        var refreshToken = await _tokenService.GetRefreshTokenAsync(refreshtoken, realm);
        return Ok(refreshToken);
    }
}