using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Web.Controllers.CRM
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TenantController : ControllerBase
    {
        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public ITenantService _tenantService { get; }

        [HttpGet]
        public IEnumerable<TenantDto> Get()
        {
            return _tenantService.GetAll();
        }
        [HttpPost]
        public async Task Add(TenantDto obj)
        {
            await _tenantService.InsertAsync(obj);
        }
        [HttpPost("[action]")]
        public async Task Delete(int id)
        {
            await _tenantService.DeleteAsync(id);
        }
    }
}
