using System.Text.Json.Serialization;

namespace PromerceHelpDesk.Web.Models
{
    public class UserModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("tenantCode")]
        public string? TenantCode { get; set; }
        public string? TenantName { get; set; }
        [JsonPropertyName("token")]
        public string? Token { get; set; }
        [JsonPropertyName("loginResult")]
        public string LoginResult { get; set; }
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }
}
