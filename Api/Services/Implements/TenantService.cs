using AutoMapper;
using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Repository.Interfaces;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Services.Implements
{
    public class TenantService : ITenantService
    {
        public TenantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IUnitOfWork _unitOfWork { get; }
        public IMapper _mapper { get; }

        public IEnumerable<TenantDto> GetAll()
        {
            var result = _unitOfWork.GenericRepository<Tenant>().GetAll();
            var listOf = _mapper.Map<IEnumerable<Tenant>, IEnumerable<TenantDto>>(result);
            return listOf.ToList();
        }

        public TenantDto GetByCode(string code)
        {
            var result = _unitOfWork.GenericRepository<Tenant>().GetAll(v => v.TenantCode == code).FirstOrDefault();
            if (result == null)
                return new TenantDto();
            var resultDto = _mapper.Map<Tenant, TenantDto>(result);
            return resultDto;
        }
        public async Task<TenantDto> InsertAsync(TenantDto tenantDto)
        {
            var newTenant = await _unitOfWork.GenericRepository<Tenant>().AddAsync(_mapper.Map<TenantDto, Tenant>(tenantDto));
            _unitOfWork.Save();
            return _mapper.Map<Tenant, TenantDto>(newTenant);
        }
        public async Task DeleteAsync(int id)
        {
            var tenant = _unitOfWork.GenericRepository<Tenant>().GetByIdAsync(r => r.Id == id);
            _unitOfWork.GenericRepository<Tenant>().Delete(tenant);
            _unitOfWork.Save();
        }
    }
}
