using KeycloakAdminRestApi.Configuration;
using KeycloakAdminRestApi.Models.Error;
using Microsoft.Extensions.Options;

namespace KeycloakAdminRestApi.Services;

public interface ITokenService
{
    Task<AccessToken?> GetAdminAccessTokenAsync(string realm, CancellationToken cancellationToken = default);
    Task<AccessToken?> GetAccessTokenAsync(string realm, CancellationToken cancellationToken = default);

    Task<AccessToken?> GetRefreshTokenAsync(string refreshToken, string realm,
        CancellationToken cancellationToken = default);
}

public class TokenService : ITokenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly KeycloakOptions _options;

    public TokenService(IHttpClientFactory httpClientFactory, IOptions<KeycloakOptions> keycloakOptions)
    {
        _httpClientFactory = httpClientFactory;
        _options = keycloakOptions.Value;
    }

    private async Task<AccessToken?> GetTokenAsync(string realm, Dictionary<string, string> parameters,
        CancellationToken cancellationToken)
    {
        var content = new FormUrlEncodedContent(parameters);

        using var client = _httpClientFactory.CreateClient("KeycloakApiClient");
        var post = await client.PostAsync(
            $"{_options.Url}/realms/{realm}/protocol/openid-connect/token",
            content, cancellationToken);

        var responseContent = await post.Content.ReadAsStringAsync(cancellationToken);
        if (!post.IsSuccessStatusCode)
        {
            var serialize = JsonSerializer.Deserialize<ErrorResponse>(responseContent);

            throw new HttpRequestException(serialize?.ToString(), null, post.StatusCode);
        }

        var accessToken = JsonSerializer.Deserialize<AccessToken>(responseContent);
        return accessToken;
    }

    public async Task<AccessToken?> GetAdminAccessTokenAsync(string realm, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _options.AdminClientId },
            { "client_secret", _options.AdminSecret },
        };

        return await GetTokenAsync(realm, parameters, cancellationToken);
    }

    public async Task<AccessToken?> GetAccessTokenAsync(string realm, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", _options.ClientId },
            { "client_secret", _options.ClientSecret },
            { "username", "yourusername" },
            { "password", "yourpass" },
        };

        return await GetTokenAsync(realm, parameters, cancellationToken);
    }

    public async Task<AccessToken?> GetRefreshTokenAsync(string refreshToken, string realm,
        CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", _options.ClientId },
            { "client_secret", _options.ClientSecret },
            { "refresh_token", refreshToken }
        };

        return await GetTokenAsync(realm, parameters, cancellationToken);
    }
}