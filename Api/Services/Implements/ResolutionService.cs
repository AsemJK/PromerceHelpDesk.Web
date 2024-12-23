using AutoMapper;
using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Repository.Interfaces;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Services.Implements
{
    public class ResolutionService : IResolutionService
    {
        public ResolutionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IUnitOfWork _unitOfWork { get; }
        public IMapper _mapper { get; }

        public ResolutionDto Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ResolutionDto> GetAll()
        {
            var resolutionsDto = _mapper.Map<List<ResolutionDto>>(_unitOfWork.GenericRepository<Resolution>().GetAll());
            foreach (var resolution in resolutionsDto)
            {
                var itemAttachemts = _unitOfWork.GenericRepository<Attachment>().GetAll().Where(a => a.IncidentId == resolution.IncidentId && a.Source == "Resolution");
                resolution.Attachments = _mapper.Map<IEnumerable<Attachment>, IEnumerable<AttachmentDto>>(itemAttachemts).ToList();
            }
            return resolutionsDto;
        }

        public IEnumerable<AttachmentDto> GetAttachments(int resolutionId)
        {
            var resolution = _unitOfWork.GenericRepository<Resolution>().GetAll(v => v.Id == resolutionId).FirstOrDefault();
            var itemAttachemts = _unitOfWork.GenericRepository<Attachment>().GetAll().Where(a => a.IncidentId == resolution.IncidentId && a.Source == "Resolution");
            return _mapper.Map<IEnumerable<Attachment>, IEnumerable<AttachmentDto>>(itemAttachemts).ToList();
        }

        public void Insert(ResolutionDto resolutionDto)
        {
            throw new NotImplementedException();
        }

        public Task<Resolution> InsertAsync(ResolutionDto resolution)
        {
            var addedObj = _unitOfWork.GenericRepository<Resolution>().AddAsync(_mapper.Map<Resolution>(resolution));
            _unitOfWork.Save();
            return addedObj;
        }

        public Task<AttachmentDto> InsertAttachmentAsync(AttachmentDto attachmentDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResolutionDto> UpdateAsync(ResolutionDto resolutionDto)
        {
            throw new NotImplementedException();
        }
    }
}
