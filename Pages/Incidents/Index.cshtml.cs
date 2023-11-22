using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PromerceHelpDesk.Web.Pages.Incidents
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            string userStr = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userStr))
            {
                HttpContext.Session.Remove("user");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect("/");
            }
            return Page();
        }
    }
}
