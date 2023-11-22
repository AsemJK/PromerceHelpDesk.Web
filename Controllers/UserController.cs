using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PromerceHelpDesk.Web.Models;
using PromerceHelpDesk.Web.Models.DTOs;
using PromerceHelpDesk.Web.Services;
using RestSharp;
using System.Security.Claims;
using System.Text.Json;

namespace PromerceHelpDesk.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(IConfiguration configuration, ISharedViewLocalizer localizer)
        {
            _configuration = configuration;
            _localizer = localizer;
        }

        public IConfiguration _configuration { get; }
        public ISharedViewLocalizer _localizer { get; }

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

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Authentication/Register");
            var client = new RestClient(options);
            var json = JsonSerializer.Serialize(model);
            var request = new RestRequest().AddJsonBody(json);
            var response = await client.PostAsync(request);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var userLogin = new UserModel();
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Authentication/Login");
            var client = new RestClient(options);
            var json = JsonSerializer.Serialize(model);
            var request = new RestRequest().AddJsonBody(json);
            var response = await client.PostAsync(request);
            if (response.IsSuccessful)
            {
                userLogin = JsonSerializer.Deserialize<UserModel>(response.Content);
                if (userLogin.LoginResult == "Ok")
                {
                    HttpContext.Session.SetString("user", JsonSerializer.Serialize(userLogin));
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    // Add other claims as needed
                };
                    foreach (var userRole in userLogin.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
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
            }
            //userLogin.LoginResult = _localizer[userLogin.LoginResult];

            return Ok(userLogin);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dtoObj)
        {
            UserModel user = GetCurrentUser();
            dtoObj.UserId = user.Id;
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Authentication/ResetPassword");
            var client = new RestClient(options);
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {user.Token}");
            string jstr = JsonSerializer.Serialize(dtoObj);
            request.AddJsonBody(jstr);
            var response = await client.PostAsync(request);
            return Ok(response.Content);
        }
        [HttpGet("[action]")]
        public async Task<List<UserModel>> List()
        {
            var options = new RestClientOptions(_configuration["ApiBaseUrl"]?.ToString());
            var client = new RestClient(options);
            var request = new RestRequest("Authentication/Users", Method.Get);
            client.AddDefaultHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            var response = await client.GetAsync<List<UserModel>>(request);
            if (response == null)
                return new List<UserModel>();
            return response.ToList();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(UserDto dtoObj)
        {
            UserModel user = GetCurrentUser();
            dtoObj.Id = user.Id;
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Authentication/Update");
            var client = new RestClient(options);
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {user.Token}");
            string jstr = JsonSerializer.Serialize(dtoObj);
            request.AddJsonBody(jstr);
            var response = await client.PostAsync(request);
            return Ok(response.Content);
        }
    }
}
