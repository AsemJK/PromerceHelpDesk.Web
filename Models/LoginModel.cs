using System.Text.Json.Serialization;

namespace PromerceHelpDesk.Web.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Tenant { get; set; }
        [JsonIgnore]
        public string? Token { get; set; }
    }
}
