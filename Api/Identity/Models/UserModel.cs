using Microsoft.AspNetCore.Identity;

namespace PromerceCRM.API.Identity
{
    public class UserModel : IdentityUser
    {
        //Here You can add extra fields If you like
        public string? ExtraInfo { get; set; }
        public string? TenantCode { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
