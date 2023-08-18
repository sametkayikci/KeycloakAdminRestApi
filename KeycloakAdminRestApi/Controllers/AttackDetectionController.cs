namespace KeycloakAdminRestApi.Controllers;

public sealed class AttackDetectionController : BaseApiController
{
    private readonly IHttpClientService _httpClient;

    public AttackDetectionController(IHttpClientService httpClient)
    {
        _httpClient = httpClient;
    }


    /// <summary>
    /// Clear any user login failures for all users This can release temporary disabled users
    /// </summary>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    /// <response code="401">Unauthorized</response>
    [Authorize]
    [HttpDelete("/{realm}/attack-detection/brute-force/users")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RealmAttackDetectionBruteForceUsersDelete([FromRoute] string realm,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync($"/admin/realms/{realm}/attack-detection/brute-force/users", cancellationToken);
        return StatusCode((int)response.StatusCode);
    }
}