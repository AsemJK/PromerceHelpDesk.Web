namespace PromerceCRM.API.Identity
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? TenantCode { get; set; }
        public string? TenantName { get; set; }
        public string? Token { get; set; }
        public string LoginResult { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public bool IsActive { get; set; } = true;
    }
}
