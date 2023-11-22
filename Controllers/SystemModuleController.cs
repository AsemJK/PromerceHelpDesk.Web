using Microsoft.AspNetCore.Mvc;
using PromerceHelpDesk.Web.Models;
using PromerceHelpDesk.Web.Models.DTOs;
using PromerceHelpDesk.Web.Services;
using RestSharp;
using System.Text.Json;

namespace PromerceHelpDesk.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemModuleController : ControllerBase
    {
        public IConfiguration _configuration { get; }
        private readonly ISharedViewLocalizer _localizer;

        public SystemModuleController(IConfiguration configuration, ISharedViewLocalizer localizer)
        {
            _configuration = configuration;
            _localizer = localizer;
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
        [HttpGet("Main")]
        public async Task<List<SystemModuleDto>> GetMains(int projectId = 0)
        {
            var options = new RestClientOptions(_configuration["ApiBaseUrl"]?.ToString());
            var client = new RestClient(options);
            var request = new RestRequest($"api/SystemModule?projectId={projectId}", Method.Get);
            request.AddBody(GetCurrentUser());
            client.AddDefaultHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            var response = await client.GetAsync<List<SystemModuleDto>>(request);
            if (response == null)
                return new List<SystemModuleDto>();
            foreach (var item in response)
            {
                item.Name = _localizer.GetLocalizedString(item.Name);
            }
            return response.ToList();
        }
    }
}
