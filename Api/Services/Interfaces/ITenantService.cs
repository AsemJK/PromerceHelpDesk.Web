using PromerceCRM.API.Models.DTOs;

namespace PromerceCRM.API.Services.Interfaces
{
    public interface ITenantService
    {
        Task DeleteAsync(int id);
        IEnumerable<TenantDto> GetAll();
        TenantDto GetByCode(string code);
        Task<TenantDto> InsertAsync(TenantDto tenantDto);
    }
}
