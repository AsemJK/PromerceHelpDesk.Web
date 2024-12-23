using AutoMapper;
using PromerceCRM.API.Identity;
using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Repository.Interfaces;
using PromerceCRM.API.Services.Interfaces;

namespace PromerceCRM.API.Services.Implements
{
    public class IncidentService : IIncidentService
    {
        public IncidentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IUnitOfWork _unitOfWork { get; }
        public IMapper _mapper { get; }

        public IEnumerable<IncidentDto> GetAll()
        {
            List<IncidentDto> incidents = new List<IncidentDto>();
            var result = _unitOfWork.GenericRepository<Incident>().GetAll();
            var listOf = _mapper.Map<IEnumerable<Incident>, IEnumerable<IncidentDto>>(result);

            foreach (var item in listOf)
            {
                item.SystemModule = _unitOfWork.GenericRepository<SystemModule>().GetAll().Where(v => v.Id == item.ModuleId).FirstOrDefault()?.Name;
                item.TenantName = _unitOfWork.GenericRepository<Tenant>().GetByIdAsync(v => v.Id == item.TenantId).TenantName;
                var itemAttachemts = _unitOfWork.GenericRepository<Attachment>().GetAll().Where(a => a.IncidentId == item.Id && a.Source == "Issue");
                item.Attachments = _mapper.Map<IEnumerable<Attachment>, IEnumerable<AttachmentDto>>(itemAttachemts).ToList();
                item.Resolutions = _mapper.Map<List<ResolutionDto>>(_unitOfWork.GenericRepository<Resolution>().GetAll().Where(y => y.IncidentId == item.Id).OrderByDescending(p => p.CreationTime).ToList());
                foreach (var resAtt in item.Resolutions)
                {
                    resAtt.Attachments = _mapper.Map<IEnumerable<Attachment>, IEnumerable<AttachmentDto>>(_unitOfWork.GenericRepository<Attachment>().GetAll().Where(a => a.IncidentId == resAtt.IncidentId)).ToList();
                }
            }
            incidents.AddRange(listOf.Where(c => c.LastStatus == "New").OrderByDescending(p => p.UpdateTime));
            incidents.AddRange(listOf.Where(c => c.LastStatus != "New").OrderByDescending(p => p.UpdateTime));

            return incidents.ToList();
        }
        public IncidentDto Get(int id)
        {
            var result = _unitOfWork.GenericRepository<Incident>().GetByIdAsync(r => r.Id == id);
            var resultDto = _mapper.Map<Incident, IncidentDto>(result);
            var itemAttachemts = _unitOfWork.GenericRepository<Attachment>().GetAll().Where(a => a.IncidentId == resultDto.Id);
            resultDto.Attachments = _mapper.Map<IEnumerable<Attachment>, IEnumerable<AttachmentDto>>(itemAttachemts).ToList();

            return resultDto;
        }
        public async Task<Incident> InsertAsync(IncidentDto incident)
        {
            var entity = _mapper.Map<Incident>(incident);
            entity.TenantId = _unitOfWork.GenericRepository<Tenant>().GetAll(v => v.TenantCode == incident.TenantCode).FirstOrDefault().Id;
            await _unitOfWork.GenericRepository<Incident>().AddAsync(entity);
            _unitOfWork.Save();
            if (entity.Id > 0)
            {
                var attachments = _unitOfWork.GenericRepository<Attachment>().GetAll(v => v.UploadSignature == entity.AttachmentsGuid).ToList();
                foreach (var attachment in attachments)
                {
                    attachment.IncidentId = entity.Id;
                    await _unitOfWork.GenericRepository<Attachment>().UpdateAsync(attachment);
                }
            }
            _unitOfWork.Save();
            return entity;
        }
        public void Insert(IncidentDto incidentDto)
        {
            var mappedEntity = _mapper.Map<IncidentDto, Incident>(incidentDto);
            _unitOfWork.GenericRepository<Incident>().Add(mappedEntity);
            _unitOfWork.Save();
        }
        public IEnumerable<AttachmentDto> GetAttachments(int incidentId)
        {
            var result = _unitOfWork.GenericRepository<Attachment>().GetAll().Where(a => a.IncidentId == incidentId);
            var listOf = _mapper.Map<IEnumerable<Attachment>, IEnumerable<AttachmentDto>>(result);
            return listOf.ToList();
        }
        public async Task<AttachmentDto> InsertAttachmentAsync(AttachmentDto attachmentDto)
        {
            var mappedEntity = _mapper.Map<AttachmentDto, Attachment>(attachmentDto);
            await _unitOfWork.GenericRepository<Attachment>().AddAsync(mappedEntity);
            _unitOfWork.Save();
            return _mapper.Map<Attachment, AttachmentDto>(mappedEntity);
        }
        public async Task<IncidentDto> UpdateAsync(IncidentDto incidentDto)
        {
            await _unitOfWork.GenericRepository<Incident>().UpdateAsync(_mapper.Map<IncidentDto, Incident>(incidentDto));
            _unitOfWork.Save();
            return incidentDto;
        }
        public async Task<List<ResolutionDto>> GetResolutions(int incidentId)
        {
            var result = _unitOfWork.GenericRepository<Resolution>().GetAll().Where(a => a.IncidentId == incidentId);
            var listOf = _mapper.Map<IEnumerable<Resolution>, IEnumerable<ResolutionDto>>(result);
            foreach (var item in listOf)
            {
                item.UserName = _unitOfWork.GenericRepository<UserModel>().GetByIdAsync(p => p.Id == item.UserId)?.UserName;
            }
            return listOf.ToList();
        }
    }
}
