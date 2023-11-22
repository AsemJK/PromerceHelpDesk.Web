using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PromerceHelpDesk.Web.Models;
using PromerceHelpDesk.Web.Models.DTOs;
using RestSharp;
using System.Text.Json;

namespace PromerceHelpDesk.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        public IncidentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

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

        [HttpGet]
        public async Task<IEnumerable<IncidentDto>> List(string q = "")
        {
            var options = new RestClientOptions(_configuration["ApiBaseUrl"]?.ToString());
            var client = new RestClient(options);
            var request = new RestRequest($"api/Incident?keysearch={q}", Method.Get);
            request.AddBody(GetCurrentUser());
            client.AddDefaultHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            var response = await client.GetAsync<List<IncidentDto>>(request);
            if (response == null)
                return new List<IncidentDto>();
            return response.ToList();
        }
        [HttpGet("{id}")]
        public async Task<IncidentDto> GetById(int id)
        {
            var options = new RestClientOptions(_configuration["ApiBaseUrl"]?.ToString());
            var client = new RestClient(options);
            var request = new RestRequest($"api/Incident/{id}", Method.Get);
            request.AddBody(GetCurrentUser());
            client.AddDefaultHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            var response = await client.GetAsync<IncidentDto>(request);
            if (response == null)
                return new IncidentDto();
            return response;
        }
        [HttpGet]
        public async Task<IEnumerable<AttachmentModel>> Attachments(int id)
        {
            //https://localhost:44327/api/Incident/Attachments?incidentId=1
            var options = new RestClientOptions(_configuration["ApiBaseUrl"]?.ToString());
            var client = new RestClient(options);
            var request = new RestRequest($"api/Incident/Attachments?incidentId={id}", Method.Get);
            try
            {
                string userStr = HttpContext.Session.GetString("user");
                await Console.Out.WriteLineAsync(userStr);
                if (string.IsNullOrEmpty(userStr))
                    return new List<AttachmentModel>();
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(userStr);
                request.AddJsonBody(JsonSerializer.Serialize(currentUser));
                if (currentUser == null)
                    return new List<AttachmentModel>();
                string token = currentUser.Token;
                client.AddDefaultHeader("Authorization", $"Bearer {token}");

                var response = await client.GetAsync<List<AttachmentModel>>(request);
                if (response == null)
                    return new List<AttachmentModel>();
                return response.ToList();
            }
            catch (Exception ex)
            {
                return new List<AttachmentModel>();
                throw;
            }
        }

        [HttpPost]
        public async Task<string> Upload(IFormCollection payload)
        {
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Api/Incident/add-attachments");
            var client = new RestClient(options);
            var request = new RestRequest();
            string fileName = "", extension = "png";
            if (payload.Files.Count == 0)
                return "no-attachments";
            foreach (var file in payload.Files)
            {
                fileName = file.FileName;
                extension = Path.GetExtension(fileName);
                var fileNewNormalizedName = "file_" + DateTime.Now.Year.ToString() + "_" + Guid.NewGuid().ToString().Replace("-", string.Empty) + extension;
                var fulPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "files", fileNewNormalizedName);
                var fileOnServerWRootPath = Path.Combine("/", "files", fileNewNormalizedName);
                using (var fs = new FileStream(fulPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                    fs.Close();
                }

                using var stream = new MemoryStream();
                file.CopyTo(stream);
                byte[] fileBytes = stream.ToArray();
                request.AddFile(file.Name, fileBytes, fileNewNormalizedName);
            }
            foreach (var key in payload.Keys)
            {
                request.AddParameter(key, payload[key]);
            }
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            try
            {
                var response = await client.PostAsync(request);
                if (response.IsSuccessful)
                    return response.Content;
            }
            catch (Exception ex)
            {
                throw;
            }


            return "Error";
        }

        [HttpPost]
        public async Task<bool> Add(IncidentModel obj)
        {
            var user = GetCurrentUser();
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Api/Incident");
            var client = new RestClient(options);
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {user.Token}");
            obj.TenantCode = user.TenantCode;
            obj.UserId = user.Id;
            string jstr = JsonSerializer.Serialize(obj);
            request.AddJsonBody(jstr);
            var response = await client.PostAsync(request);
            return response.IsSuccessful;
        }

        [HttpPost("{id}")]
        public async Task<bool> Close(int id)
        {
            //change status only
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Api/Incident");
            var client = new RestClient(options);
            var request = new RestRequest($"ChangeStatus?id={id}&newStatus=Done");
            request.AddHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            var response = await client.PostAsync(request);
            return response.IsSuccessful;
        }
        [HttpPost]
        public async Task<ResolutionModel> Resolve(ResolutionDto obj)
        {
            obj.UserId = GetCurrentUser().Id;
            var options = new RestClientOptions(_configuration.GetValue<string>("ApiBaseUrl") + "Api/Resolution");
            var client = new RestClient(options);
            var request = new RestRequest($"Resolve");
            request.AddHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
            string strJson = JsonSerializer.Serialize(obj);
            request.AddJsonBody(strJson);
            var response = await client.PostAsync<ResolutionModel>(request);
            return response;
        }
        [HttpGet]
        public async Task<IEnumerable<ResolutionDto>> Resolutions(int id)
        {
            var options = new RestClientOptions(_configuration["ApiBaseUrl"]?.ToString());
            var client = new RestClient(options);
            var request = new RestRequest($"api/Incident/resolutions/{id}", Method.Get);
            try
            {

                client.AddDefaultHeader("Authorization", $"Bearer {GetCurrentUser().Token}");
                var response = await client.GetAsync<List<ResolutionDto>>(request);
                if (response == null)
                    return new List<ResolutionDto>();
                return response.ToList();
            }
            catch (Exception ex)
            {
                return new List<ResolutionDto>();
                throw;
            }
        }
    }
}
