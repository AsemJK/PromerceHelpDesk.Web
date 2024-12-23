using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Web.Controllers.CRM
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SystemModuleController : ControllerBase
    {
        public SystemModuleController(ISystemModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        public ISystemModuleService _moduleService { get; }

        [HttpGet]
        public Task<List<SystemModuleDto>> Get(int projectId = 0)
        {
            return _moduleService.GetList(projectId);
        }
        [HttpPost("Add")]
        public async Task<SystemModuleDto> Add(SystemModuleDto moduleDto)
        {
            return await _moduleService.InsertAsync(moduleDto);
        }
    }
}
