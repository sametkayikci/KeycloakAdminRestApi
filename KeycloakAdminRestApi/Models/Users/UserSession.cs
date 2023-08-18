namespace KeycloakAdminRestApi.Models.Users;

public record UserSession
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("username")] public string? UserName { get; set; }
    [JsonPropertyName("userId")] public string? UserId { get; set; }
    [JsonPropertyName("ipAddress")] public string? IpAddress { get; set; }
    [JsonPropertyName("start")] public long Start { get; set; }
    [JsonPropertyName("lastAccess")] public long LastAccess { get; set; }
    [JsonPropertyName("clients")] public IDictionary<string, string>? Clients { get; set; }
}