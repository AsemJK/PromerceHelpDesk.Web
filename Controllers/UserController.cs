using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PromerceHelpDesk.Web.Models;
using RestSharp;
using System.Security.Claims;
using System.Text.Json;

namespace PromerceHelpDesk.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            MyResponse myResponse = new MyResponse();
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Authentication/Login");
            var client = new RestClient(options);
            var json = JsonSerializer.Serialize(model);
            string token = "";
            model.Token = token;
            var request = new RestRequest().AddJsonBody(json);
            var response = await client.PostAsync(request);
            if (response.IsSuccessful)
            {
                myResponse = JsonSerializer.Deserialize<MyResponse>(response.Content);
                if (myResponse.Status)
                {
                    token = myResponse.Result;
                    myResponse.Result = "success";
                }
            }

            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Session.SetString("user", JsonSerializer.Serialize(new LoginModel
                {
                    UserName = model.UserName,
                    Tenant = model.Tenant,
                    Token = token
                }));
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    // Add other claims as needed
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Configure auth properties as needed
                };

                // Sign the user in
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }


            return Ok(myResponse);
        }
    }
}
