namespace KeycloakAdminRestApi.Controllers;

public class OpenIdConfigurationController : BaseApiController
{
    private readonly IHttpClientService _httpClient;

    public OpenIdConfigurationController(IHttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get representation of the openid configuration
    /// </summary>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    /// <response code="401">Unauthorized</response>
    [AllowAnonymous]
    [HttpGet("/{realm}/openid-configuration")]
    [ProducesResponseType(typeof(OpenIdConfiguration), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetOpenIdConfigurationAsync(string realm,
        CancellationToken cancellationToken = default)
        => Ok(await _httpClient.GetAsync<OpenIdConfiguration>($"/realms/{realm}/.well-known/openid-configuration",
            cancellationToken: cancellationToken));
}