namespace KeycloakAdminRestApi.Controllers;

public class UserController : BaseApiController
{
    private readonly IHttpClientService _httpClient;

    public UserController(IHttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get representation of the user
    /// </summary>
    /// <remarks></remarks>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="id">User id</param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpGet("/{realm}/users/{id}")]
    public async Task<IActionResult> RealmUsersIdGet(string realm, string id,
        CancellationToken cancellationToken = default)
        => Ok(await _httpClient.GetAsync<User>($"/admin/realms/{realm}/users/{id}",
            cancellationToken: cancellationToken));


    /// <summary>
    /// Get representation of the user groups
    /// </summary>
    /// <remarks></remarks>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="id">User id</param>
    /// <param name="search"></param>
    /// <param name="first"></param>
    /// <param name="max"></param>
    /// <param name="briefRepresentation"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    /// <response code="401">Unauthorized</response>
    [Authorize]
    [HttpGet("/{realm}/users/{id}/groups")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserGroupsAsync(string realm, string id, [FromQuery] string? search,
        [FromQuery] int? first, [FromQuery] int? max, [FromQuery] bool? briefRepresentation = true,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object>
        {
            [nameof(first)] = first!,
            [nameof(max)] = max!,
            [nameof(search)] = search!,
            [nameof(briefRepresentation)] = briefRepresentation!
        };
        return Ok(await _httpClient.GetAsync<Group>($"/admin/realms/{realm}/users/{id}/groups", queryParams,
            cancellationToken));
    }

    /// <summary>
    /// Get representation of the user email
    /// </summary>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="search">Email or username</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("/{realm}/users/{search}/users")]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUsersByEmailAsync(string realm, string search,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _httpClient.GetAsync<IEnumerable<User>>($"/admin/realms/{realm}/users?email={search}",
            cancellationToken: cancellationToken));
    }

    /// <summary>
    /// Remove all user sessions associated with the user  Also send notification to all clients
    /// that have an admin URL to invalidate the sessions for the particular user.
    /// </summary>
    /// <remarks></remarks>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="id">User id</param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    [Authorize]
    [HttpPost("/{realm}/users/{id}/logout")]

    public async Task<IActionResult> RemoveUserSessionsAsync(string realm, string id,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostJsonAsync($"/admin/realms/{realm}/users/{id}/logout", "",
            cancellationToken: cancellationToken);

        return StatusCode((int)response.StatusCode);
    }
}