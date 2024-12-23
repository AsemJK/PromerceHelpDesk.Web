using AutoMapper;
using PromerceCRM.API.Models.ERP;
using PromerceCRM.API.Models.ModelsViews;
using PromerceCRM.API.Repository.Interfaces;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Services.Implements
{
    public class AccountGroupService : IAccountGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<AccountGroupDto> GetAll()
        {
            var result = _unitOfWork.GenericRepository<AccountGroup>().GetAll();
            var listOf = _mapper.Map<IEnumerable<AccountGroup>, IEnumerable<AccountGroupDto>>(result);
            return listOf.ToList();
        }
    }
}
