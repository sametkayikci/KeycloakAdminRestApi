using System.Text;

namespace KeycloakAdminRestApi.Models.Error;

public record ErrorResponse
{
    [JsonPropertyName("error")] public string? Error { get; set; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }

    [JsonPropertyName("errorMessage")] public string? ErrorMessage { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();

        if (!string.IsNullOrEmpty(Error))
            builder.Append($"Error: {Error}");

        if (!string.IsNullOrEmpty(ErrorDescription))
            builder.Append($"ErrorDescription: {ErrorDescription}");

        if (!string.IsNullOrEmpty(ErrorMessage))
            builder.Append($"ErrorMessage: {ErrorMessage}");

        return builder.ToString();
    }
}