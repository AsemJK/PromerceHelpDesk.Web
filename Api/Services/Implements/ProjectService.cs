using AutoMapper;
using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Repository.Interfaces;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Services.Implements
{
    public class ProjectService : IProjectService
    {
        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IUnitOfWork _unitOfWork { get; }
        public IMapper _mapper { get; }

        public IEnumerable<ProjectDto> GetAll()
        {
            var result = _unitOfWork.GenericRepository<Project>().GetAll();
            var listOf = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(result);
            return listOf.ToList();
        }
        public async Task<ProjectDto> InsertAsync(ProjectDto projectDto)
        {
            var newProject = await _unitOfWork.GenericRepository<Project>().AddAsync(_mapper.Map<ProjectDto, Project>(projectDto));
            _unitOfWork.Save();
            return _mapper.Map<Project, ProjectDto>(newProject);
        }
        public async Task DeleteAsync(int id)
        {
            var project = _unitOfWork.GenericRepository<Project>().GetByIdAsync(r => r.Id == id);
            _unitOfWork.GenericRepository<Project>().Delete(project);
            _unitOfWork.Save();
        }
    }
}
