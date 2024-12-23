using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Web.Controllers.CRM
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        public ProjectController(IProjectService ProjectService)
        {
            _ProjectService = ProjectService;
        }

        public IProjectService _ProjectService { get; }

        [HttpGet]
        public IEnumerable<ProjectDto> Get()
        {
            return _ProjectService.GetAll();
        }
        [HttpPost]
        public async Task Add(ProjectDto obj)
        {
            await _ProjectService.InsertAsync(obj);
        }
        [HttpPost("[action]")]
        public async Task Delete(int id)
        {
            await _ProjectService.DeleteAsync(id);
        }
    }
}
