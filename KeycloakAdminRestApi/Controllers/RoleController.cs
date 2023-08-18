namespace KeycloakAdminRestApi.Controllers;

public sealed class RoleController : BaseApiController
{
    private readonly IHttpClientService _httpClient;

    public RoleController(IHttpClientService httpClient)
    {
        _httpClient = httpClient;
    }


    ///<summary>Create a new role for the realm or client.</summary>
    /// <remarks>
    /// Sample request:
    /// 
    /// 
    ///     POST /{realm}/clients/{id}/roles
    ///     {        
    ///       "id": "Test Role",
    ///       "name": "Test Role",
    ///       "description": "Test Role",
    ///       "composite": false,
    ///       "clientRole": true
    ///     }
    /// </remarks>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="id">id of client (not client-id)</param>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    [Authorize]
    [HttpPost("/{realm}/clients/{id}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateRoleAsync(string realm, string id,
        Role role,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _httpClient.PostJsonAsync($"/admin/realms/{realm}/clients/{id}/roles", role, cancellationToken);
        return StatusCode((int)response.StatusCode);
    }


    /// <summary>
    /// Delete a role by name
    /// </summary>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="id">id of client (not client-id)</param>
    /// <param name="roleName">role name (not id!)</param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    [Authorize]
    [HttpDelete("/{realm}/clients/{id}/roles/{roleName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoleByNameAsync(string realm, string id, string roleName,
        CancellationToken cancellationToken = default)

    {
        var response =
            await _httpClient.DeleteAsync($"/admin/realms/{realm}/clients/{id}/roles/{roleName}", cancellationToken);
        return StatusCode((int)response.StatusCode);
    }

    /// <summary>
    /// Delete a role by name (Realms Roles(
    /// </summary>
    /// <remarks></remarks>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="roleName">roles name (not id!)</param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    [HttpDelete("/{realm}/roles/{roleName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoleByNameAsync(string realm, string roleName,
        CancellationToken cancellationToken = default)

    {
        var response =
            await _httpClient.DeleteAsync($"/admin/realms/{realm}/roles/{roleName}", cancellationToken);
        return StatusCode((int)response.StatusCode);
    }

    /// <summary>
    /// Get all roles for the realm or client
    /// </summary>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="search"></param>
    /// <param name="first">1</param>
    /// <param name="max">10</param>
    /// <param name="briefRepresentation"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="2XX">Success</response>
    [Authorize]
    [HttpGet("/{realm}/roles")]
    [ProducesResponseType(typeof(IEnumerable<Role>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RealmRolesGet(string realm, [FromQuery] string? search,
        [FromQuery] int? first, [FromQuery] int? max, [FromQuery] bool? briefRepresentation = true,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object>
        {
            [nameof(first)] = first!.Value,
            [nameof(max)] = max!.Value,
            [nameof(search)] = search!,
            [nameof(briefRepresentation)] = briefRepresentation!.Value
        };
        var response =
            await _httpClient.GetAsync<IEnumerable<Role>>($"/admin/realms/{realm}/roles", queryParams,
                cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Belirli bir rolün temsilini alın
    /// </summary>
    /// <param name="realm">realm adı (kimlik değil!)</param>
    /// <param name="roleId">rol kimliği</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("/{realm}/roles-by-id/{roleId}")]
    [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleByIdAsync(string realm, string roleId,
        CancellationToken cancellationToken = default)
        => Ok(await _httpClient.GetAsync<Role>($"/admin/realms/{realm}/roles-by-id/{roleId}",
            cancellationToken: cancellationToken));


    /// <summary>
    /// Get all roles for the realm or client
    /// </summary>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="id">id of client (not client-id)</param>
    /// <param name="search"></param>
    /// <param name="first"></param>
    /// <param name="max"></param>
    /// <param name="briefRepresentation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("/{realm}/clients/{id}/roles")]
    public async Task<IActionResult> GetRolesAsync(string realm, string id, [FromQuery] string? search,
        [FromQuery] int? first, [FromQuery] int? max, [FromQuery] bool? briefRepresentation = true,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object>
        {
            [nameof(first)] = first,
            [nameof(max)] = max,
            [nameof(search)] = briefRepresentation,
        };
        var response = await _httpClient.GetAsync<IEnumerable<Role>>($"/admin/realms/{realm}/clients/{id}/roles",
            queryParams,
            cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Get a role by name
    /// </summary>
    /// <param name="realm">realm name (not id!)</param>
    /// <param name="id">id of client (not client-id)</param>
    /// <param name="roleName">role’s name (not id!)</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("/{realm}/clients/{id}/roles/{roleName}")]
    public async Task<IActionResult> GetRolesAsync(string realm, string id, string roleName,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _httpClient.GetAsync<Role>($"/admin/realms/{realm}/clients/{id}/roles/{roleName}",
                cancellationToken: cancellationToken);
        return Ok(response);
    }
}