using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;

namespace PromerceCRM.API.Services.Interfaces
{
    public interface IIncidentService
    {
        IncidentDto Get(int id);
        IEnumerable<IncidentDto> GetAll();
        IEnumerable<AttachmentDto> GetAttachments(int incidentId);
        Task<List<ResolutionDto>> GetResolutions(int incidentId);
        void Insert(IncidentDto incidentDto);
        Task<Incident> InsertAsync(IncidentDto incident);
        Task<AttachmentDto> InsertAttachmentAsync(AttachmentDto attachmentDto);
        Task<IncidentDto> UpdateAsync(IncidentDto incidentDto);
    }
}
