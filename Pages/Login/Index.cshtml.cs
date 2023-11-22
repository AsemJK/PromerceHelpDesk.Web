using Microsoft.AspNetCore.Mvc.RazorPages;
using PromerceHelpDesk.Web.Models;

namespace PromerceHelpDesk.Web.Pages.Login
{
    public class IndexModel : PageModel
    {
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public UserModel LoginModel { get; set; }
        public IConfiguration _configuration { get; }

        public void OnGet()
        {
            LoginModel = new UserModel();
        }
    }
}
