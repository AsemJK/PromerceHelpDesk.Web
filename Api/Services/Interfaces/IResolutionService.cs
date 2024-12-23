using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;

namespace PromerceCRM.API.Services.Interfaces
{
    public interface IResolutionService
    {
        ResolutionDto Get(int id);
        IEnumerable<ResolutionDto> GetAll();
        IEnumerable<AttachmentDto> GetAttachments(int incidentId);
        void Insert(ResolutionDto incidentDto);
        Task<Resolution> InsertAsync(ResolutionDto incident);
        Task<AttachmentDto> InsertAttachmentAsync(AttachmentDto attachmentDto);
        Task<ResolutionDto> UpdateAsync(ResolutionDto incidentDto);
    }
}
