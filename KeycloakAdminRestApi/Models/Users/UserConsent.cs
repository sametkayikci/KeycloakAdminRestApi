﻿namespace KeycloakAdminRestApi.Models.Users;

public record UserConsent
{
	[JsonPropertyName("clientId")]
	public string? ClientId { get; set; }
	[JsonPropertyName("grantedClientScopes")]
	public IEnumerable<string>? GrantedClientScopes { get; set; }
	[JsonPropertyName("createdDate")]
	public long? CreatedDate { get; set; }
	[JsonPropertyName("lastUpdatedDate")]
	public long? LastUpdatedDate { get; set; }
}