namespace PromerceHelpDesk.Web.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? TenantCode { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
