using PromerceCRM.API.Models.DTOs;

namespace PromerceCRM.API.Services.Interfaces
{
    public interface ISystemModuleService
    {
        Task<List<SystemModuleDto>> GetList(int projectId = 0);
        Task<SystemModuleDto> InsertAsync(SystemModuleDto obj);
    }
}
