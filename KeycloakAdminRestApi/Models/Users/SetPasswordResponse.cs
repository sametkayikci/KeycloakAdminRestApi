namespace KeycloakAdminRestApi.Models.Users;

public record SetPasswordResponse
{
    public bool Success { get; set; }
    [JsonPropertyName("error")] public string? Error { get; set; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }
}