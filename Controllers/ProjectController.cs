using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PromerceHelpDesk.Web.Models;
using PromerceHelpDesk.Web.Models.DTOs;
using PromerceHelpDesk.Web.Services;
using RestSharp;
using System.Text.Json;

namespace PromerceHelpDesk.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        public ProjectController(IConfiguration configuration, ISharedViewLocalizer localizer)
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
        public IConfiguration _configuration { get; }
        public ISharedViewLocalizer _localizer { get; }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ProjectDto>> List()
        {
            var options = new RestClientOptions(_configuration["ApiBaseUrl"]?.ToString());
            var client = new RestClient(options);
            var request = new RestRequest("api/Project", Method.Get);
            client.AddDefaultHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            var response = await client.GetAsync<List<ProjectDto>>(request);
            if (response == null)
                return new List<ProjectDto>();
            foreach (var item in response)
            {
                item.Name = _localizer.GetLocalizedString(item.Name);
            }
            return response.ToList();
        }
        [HttpPost]
        public async Task<bool> Add([FromBody] ProjectDto obj)
        {
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Api/Project");
            var client = new RestClient(options);
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            string jstr = JsonSerializer.Serialize(obj);
            request.AddJsonBody(jstr);
            var response = await client.PostAsync(request);
            return response.IsSuccessful;
        }

        [HttpPost("[action]/{id}")]
        public async Task<bool> Delete(int id)
        {
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Api/Project");
            var client = new RestClient(options);
            var request = new RestRequest($"Delete?id={id}");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            var response = await client.PostAsync(request);
            return response.IsSuccessful;
        }
    }
}
