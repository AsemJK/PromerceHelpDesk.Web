using AutoMapper;
using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Repository.Interfaces;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Services.Implements
{
    public class SystemModuleService : ISystemModuleService
    {
        public SystemModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IUnitOfWork _unitOfWork { get; }
        public IMapper _mapper { get; }

        public async Task<List<SystemModuleDto>> GetList(int projectId = 0)
        {
            var result = _unitOfWork.GenericRepository<SystemModule>().GetAll(r => r.ProjectId == (projectId == 0 ? r.ProjectId : projectId));
            return _mapper.Map<List<SystemModuleDto>>(result);
        }
        public async Task<SystemModuleDto> InsertAsync(SystemModuleDto obj)
        {
            await _unitOfWork.GenericRepository<SystemModule>().AddAsync(_mapper.Map<SystemModule>(obj));
            _unitOfWork.Save();
            return obj;
        }
    }
}
