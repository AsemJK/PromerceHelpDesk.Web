using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PromerceHelpDesk.Web.Pages.Logout
{
    public class IndexModel : PageModel
    {
        public async Task<ActionResult> OnGet()
        {
            HttpContext.Session.Remove("user");
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
