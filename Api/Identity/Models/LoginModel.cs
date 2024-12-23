namespace PromerceCRM.API.Identity
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? TenantCode { get; set; }
    }
}
