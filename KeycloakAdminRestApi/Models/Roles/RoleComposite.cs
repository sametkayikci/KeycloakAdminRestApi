namespace KeycloakAdminRestApi.Models.Roles;

public record RoleComposite
{
    [JsonPropertyName("client")]        
    public IDictionary<string, string>? Client { get; set; }
    [JsonPropertyName("realm")]
    public IEnumerable<string>? Realm { get; set; }
}