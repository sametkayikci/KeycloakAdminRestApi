using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace KeycloakAdminRestApi.Configuration;

public static class KeycloakConfiguration
{
    public static void AddKeycloakPreConfigured(this IServiceCollection services,
        Action<KeycloakOptions> setupAction)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(services));

        ArgumentException.ThrowIfNullOrEmpty(nameof(setupAction));

        services.Configure(setupAction);

        var keycloakOptions = services.BuildServiceProvider().GetRequiredService<IOptions<KeycloakOptions>>();

        services.AddAuthentication(o =>
            {
                // Store the session to cookies
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // OpenId authentication
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("OpenIdConnect", oi =>
            {
                oi.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // URL of the Keycloak server
                oi.Authority = keycloakOptions.Value.Authority;
                // Client configured in the Keycloak
                oi.ClientId = keycloakOptions.Value.AdminClientId;
                // For testing we disable https (should be true for production)
                oi.RequireHttpsMetadata = false;
                oi.SaveTokens = true;
                // Client secret shared with Keycloak
                oi.ClientSecret = keycloakOptions.Value.AdminSecret;
                oi.GetClaimsFromUserInfoEndpoint = true;
                // OpenID flow to use
                oi.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                oi.ResponseMode = OpenIdConnectResponseMode.Query;
            });

        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                // only for testing
                cfg.RequireHttpsMetadata = false;
                cfg.Authority = keycloakOptions.Value.Authority;
                cfg.IncludeErrorDetails = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = keycloakOptions.Value.Authority,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
    }
}

public record KeycloakOptions
{
    public string Url { get; init; } = null!;
    public string Realm { get; init; } = null!;
    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
    public string? PublicKey { get; set; }
    public string? Authority { get; init; } = null!;
    public string AdminClientId { get; init; } = null!;
    public string AdminSecret { get; init; } = null!;
}