using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PromerceHelpDesk.Web.Models;
using PromerceHelpDesk.Web.Models.DTOs;
using RestSharp;
using System.Text.Json;

namespace PromerceHelpDesk.Web.Pages.Admin.Incidents
{
    public class DetailsModel : PageModel
    {
        public DetailsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IncidentDto Incident { get; set; } = new IncidentDto();
        public IConfiguration _configuration { get; }

        public async Task<IActionResult> OnGet(int id)
        {
            if (string.IsNullOrEmpty(GetCurrentUser().Token))
            {
                HttpContext.Session.Remove("user");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect("/");
            }
            else
            {
                //
                var options = new RestClientOptions(_configuration["ApiBaseUrl"]?.ToString());
                var client = new RestClient(options);
                var request = new RestRequest($"api/Incident/{id}", Method.Get);
                request.AddBody(GetCurrentUser());
                client.AddDefaultHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
                Incident = await client.GetAsync<IncidentDto>(request);
            }
            return Page();
        }

        private UserModel GetCurrentUser()
        {
            string userStr = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userStr))
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(userStr);
                if (currentUser != null)
                    return currentUser;
            }
            return new UserModel();
        }
    }
}
