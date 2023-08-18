using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace KeycloakAdminRestApi.Configuration;

public static class SwaggerConfiguration
{
    public static void AddSwaggerPreConfigured(this IServiceCollection services,
        Action<SwaggerOptions> setupAction)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(services));

        ArgumentException.ThrowIfNullOrEmpty(nameof(setupAction));

        services.Configure(setupAction);

        services.AddSwaggerGen(c =>
        {
            var options = services.BuildServiceProvider().GetRequiredService<IOptions<SwaggerOptions>>().Value;

            foreach (var includeXmlComment in options.IncludeXmlComments?.Split(",")!)
            {
                try
                {
                    c.IncludeXmlComments(includeXmlComment);
                }
                catch
                {
                    // Handle exception if needed
                }
            }


            c.SwaggerDoc(options.Version,
                new OpenApiInfo { Title = options.Title, Version = options.Version });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "'Bearer' [space] and then your master token in the text input below.",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void UseSwaggerPreConfigured(this IApplicationBuilder app)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(app));

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Keycloak Admin REST Api Documentation");
        });
    }
}


public record SwaggerOptions
{
    public string? Title { get; init; }
    public string? Version { get; init; }
    public string? IncludeXmlComments { get; init; }
}