using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PromerceHelpDesk.Web.Pages
{
    public class LangModel : PageModel
    {
        public void OnGet()
        {
            string? culture = Request.Query["culture"];
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1)
                    });
            }
            string returenUrl = Request.Headers["Referer"].ToString() ?? "/";
            Response.Redirect(returenUrl);
        }
    }
}
