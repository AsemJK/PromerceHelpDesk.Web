using PromerceCRM.API.Models.DTOs;

namespace PromerceCRM.API.Services.Interfaces
{
    public interface IProjectService
    {
        Task DeleteAsync(int id);
        IEnumerable<ProjectDto> GetAll();
        Task<ProjectDto> InsertAsync(ProjectDto projectDto);
    }
}
