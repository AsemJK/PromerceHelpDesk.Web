using System.ComponentModel.DataAnnotations;

namespace PromerceHelpDesk.Web.Models
{
    public class RegisterModel
    {
        public string? UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string? TenantCode { get; set; }
        public string Role { get; set; }
    }
}
