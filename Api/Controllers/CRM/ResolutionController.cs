using Microsoft.AspNetCore.Mvc;
using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Web.Controllers.CRM
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResolutionController : ControllerBase
    {
        public ResolutionController(IResolutionService resolutionService)
        {
            _resolutionService = resolutionService;
        }

        public IResolutionService _resolutionService { get; }

        [HttpPost("[action]")]
        public async Task<Resolution> Resolve(ResolutionDto resolutionDto)
        {
            return await _resolutionService.InsertAsync(resolutionDto);
        }
    }
}
