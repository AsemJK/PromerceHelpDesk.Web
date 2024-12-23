using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PromerceCRM.API.Identity;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Web.Controllers.CRM
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncidentController : ControllerBase
    {
        public IncidentController(IIncidentService incidentService,
            UserManager<UserModel> userManager,
            ITenantService tenantService,
            IResolutionService resolutionService)
        {
            _incidentService = incidentService;
            _userManager = userManager;
            _tenantService = tenantService;
            _resolutionService = resolutionService;
        }

        public IIncidentService _incidentService { get; }
        public UserManager<UserModel> _userManager { get; }
        public ITenantService _tenantService { get; }
        public IResolutionService _resolutionService { get; }

        // GET: api/<IncidentController>
        [HttpGet]
        public IEnumerable<IncidentDto> Get([FromBody] UserModel user = null, string keysearch = "")
        {
            var incidents = new List<IncidentDto>();
            if (user == null)
            {
                return Enumerable.Empty<IncidentDto>();
            }
            else
            {
                var identityuser = _userManager.FindByIdAsync(user.Id.ToString()).Result;
                var userRoles = _userManager.GetRolesAsync(identityuser).Result;
                var userTenant = _tenantService.GetAll().Where(r => r.TenantCode == user.TenantCode).FirstOrDefault();
                if (userTenant != null || _userManager.IsInRoleAsync(user, "Admin").Result)
                {
                    if (userRoles.ToList().Count == 0)
                        return Enumerable.Empty<IncidentDto>();
                    else
                    {
                        if (_userManager.IsInRoleAsync(user, "Admin").Result)
                        {
                            incidents = _incidentService.GetAll().Where(h => h.Subject.ToLower().Contains(keysearch.ToLower()) || h.Id.ToString() == keysearch.ToLower()).ToList();
                        }
                        else
                        {
                            incidents = _incidentService.GetAll().Where(v => v.TenantId == userTenant.Id).Where(v => v.Subject.ToLower().Contains(keysearch.ToLower()) || v.Id.ToString() == keysearch.ToLower()).ToList();
                        }
                    }
                }
                return incidents;
            }

        }

        // GET api/<IncidentController>/5
        [HttpGet("{id}")]
        public IncidentDto Get(int id)
        {
            var incResolutions = _incidentService.GetResolutions(id).Result;
            var incidentDto = _incidentService.GetAll().Where(v => v.Id == id).FirstOrDefault();
            if (incidentDto != null)
            {
                incidentDto.Resolutions = incResolutions;
                foreach (var resol in incidentDto.Resolutions)
                {
                    resol.Attachments = _resolutionService.GetAttachments(resol.Id).ToList();
                }
                return incidentDto;
            }
            return new IncidentDto();
        }

        // POST api/<IncidentController>
        [HttpPost]
        public async Task Post(IncidentDto obj)
        {
            if (obj.Id > 0)
                await _incidentService.UpdateAsync(obj);
            else
                await _incidentService.InsertAsync(obj);
        }

        // PUT api/<IncidentController>/5
        //[HttpPut("{id}")]
        //public async void Put([FromBody] IncidentDto obj)
        //{
        //    await _incidentService.UpdateAsync(obj);
        //}
        [HttpPost("[action]")]
        public void ChangeStatus(int id, string newStatus)
        {
            var incident = _incidentService.Get(id);
            incident.LastStatus = newStatus;
            _incidentService.UpdateAsync(new IncidentDto
            {
                Id = id,
                AttachmentsGuid = incident.AttachmentsGuid,
                CreationTime = incident.CreationTime,
                CreatorId = incident.CreatorId,
                LastStatus = newStatus,
                Priority = incident.Priority,
                Resolution = incident.Resolution,
                Subject = incident.Subject,
                TechnicianId = incident.TechnicianId,
                TenantId = incident.TenantId,
                UpdaterId = incident.UpdaterId,
                UpdateTime = DateTime.Now,
                UserId = incident.UserId
            });
        }

        // DELETE api/<IncidentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

        [HttpGet("attachments")]
        public async Task<IEnumerable<AttachmentDto>> GetIncidentAttachments(int incidentId)
        {
            return _incidentService.GetAttachments(incidentId).ToList();
        }
        [HttpPost("add-attachment")]
        public void AddNewAttachment(AttachmentDto obj)
        {
            _incidentService.InsertAttachmentAsync(obj);
        }
        /// <summary>
        /// This Will Return Upload Signature to use after in saving od incident
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPost("add-attachments")]
        public async Task<List<AttachmentDto>> UploadAsync(IFormCollection formData)
        {
            string newUploadSgnature = Guid.NewGuid().ToString().Replace("-", "");
            AttachmentDto attachmentDto = new AttachmentDto();
            List<AttachmentDto> attachmentDtos = new List<AttachmentDto>();
            attachmentDto.UploadSignature = newUploadSgnature;
            int incidentId = 0;
            if (formData.Keys.Count > 0)
            {
                if (!string.IsNullOrEmpty(formData["incident_id"]))
                {
                    incidentId = int.Parse(formData["incident_id"].ToString());
                }
            }
            if (formData.Files.Count > 0)
            {
                IFormFile file = formData.Files[0];
                string fileName = file.FileName;
                string extension = Path.GetExtension(fileName);
                foreach (IFormFile item in formData.Files)
                {
                    attachmentDto.FileName = item.FileName;
                    attachmentDto.IncidentId = incidentId;
                    attachmentDto.Source = "Issue";
                    if (formData.Keys.Count > 0)
                    {
                        attachmentDto.Source = formData["Source"].ToString();
                    }

                    attachmentDtos.Add(attachmentDto);
                    await _incidentService.InsertAttachmentAsync(attachmentDto);
                }
            }
            return attachmentDtos;
        }
        [HttpGet("resolutions/{id}")]
        public async Task<List<ResolutionDto>> GetIncidentResolutions(int id)
        {
            return await _incidentService.GetResolutions(id);
        }
    }
}
