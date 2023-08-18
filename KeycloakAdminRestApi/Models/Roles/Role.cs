using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace KeycloakAdminRestApi.Models.Roles;

public record Role
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("composite")] public bool? Composite { get; set; }
    [JsonPropertyName("composites")] public RoleComposite? Composites { get; set; }
    [JsonPropertyName("clientRole")] public bool? ClientRole { get; set; }
    [JsonPropertyName("containerId")] public string? ContainerId { get; set; }
    [JsonPropertyName("attributes")] public IDictionary<string, object>? Attributes { get; set; }
}

    public class RoleRepresentation : IEquatable<RoleRepresentation>
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>

        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>

        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets ScopeParamRequired
        /// </summary>

        [JsonPropertyName("scopeParamRequired")]
        public bool? ScopeParamRequired { get; set; }

        /// <summary>
        /// Gets or Sets Composites
        /// </summary>

        [JsonPropertyName("composites")]
        public RoleComposite Composites { get; set; }

        /// <summary>
        /// Gets or Sets Composite
        /// </summary>

        [JsonPropertyName("composite")]
        public bool? Composite { get; set; }

        /// <summary>
        /// Gets or Sets ClientRole
        /// </summary>

        [JsonPropertyName("clientRole")]
        public bool? ClientRole { get; set; }

        /// <summary>
        /// Gets or Sets ContainerId
        /// </summary>

        [JsonPropertyName("containerId")]
        public string ContainerId { get; set; }

        /// <summary>
        /// Gets or Sets Attributes
        /// </summary>

        [JsonPropertyName("attributes")]
        public Dictionary<string, List<string>> Attributes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RoleRepresentation {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  ScopeParamRequired: ").Append(ScopeParamRequired).Append("\n");
            sb.Append("  Composites: ").Append(Composites).Append("\n");
            sb.Append("  Composite: ").Append(Composite).Append("\n");
            sb.Append("  ClientRole: ").Append(ClientRole).Append("\n");
            sb.Append("  ContainerId: ").Append(ContainerId).Append("\n");
            sb.Append("  Attributes: ").Append(Attributes).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RoleRepresentation)obj);
        }

        /// <summary>
        /// Returns true if RoleRepresentation instances are equal
        /// </summary>
        /// <param name="other">Instance of RoleRepresentation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RoleRepresentation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) && 
                (
                    ScopeParamRequired == other.ScopeParamRequired ||
                    ScopeParamRequired != null &&
                    ScopeParamRequired.Equals(other.ScopeParamRequired)
                ) && 
                (
                    Composites == other.Composites ||
                    Composites != null &&
                    Composites.Equals(other.Composites)
                ) && 
                (
                    Composite == other.Composite ||
                    Composite != null &&
                    Composite.Equals(other.Composite)
                ) && 
                (
                    ClientRole == other.ClientRole ||
                    ClientRole != null &&
                    ClientRole.Equals(other.ClientRole)
                ) && 
                (
                    ContainerId == other.ContainerId ||
                    ContainerId != null &&
                    ContainerId.Equals(other.ContainerId)
                ) && 
                (
                    Attributes == other.Attributes ||
                    Attributes != null &&
                    Attributes.SequenceEqual(other.Attributes)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                    if (ScopeParamRequired != null)
                    hashCode = hashCode * 59 + ScopeParamRequired.GetHashCode();
                    if (Composites != null)
                    hashCode = hashCode * 59 + Composites.GetHashCode();
                    if (Composite != null)
                    hashCode = hashCode * 59 + Composite.GetHashCode();
                    if (ClientRole != null)
                    hashCode = hashCode * 59 + ClientRole.GetHashCode();
                    if (ContainerId != null)
                    hashCode = hashCode * 59 + ContainerId.GetHashCode();
                    if (Attributes != null)
                    hashCode = hashCode * 59 + Attributes.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(RoleRepresentation left, RoleRepresentation right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RoleRepresentation left, RoleRepresentation right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
