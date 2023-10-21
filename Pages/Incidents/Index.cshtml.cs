using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PromerceHelpDesk.Web.Pages.Incidents
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            int p = 0;
        }
    }
}
