namespace PromerceCRM.API.Models.DTOs
{
    public class IncidentDto
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CreatorId { get; set; } = 0;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public int UpdaterId { get; set; } = 0;

        public string Subject { get; set; }
        public int TenantId { get; set; }
        public string TenantCode { get; set; }
        public string TenantName { get; set; }
        public string UserId { get; set; }
        public string LastStatus { get; set; }
        public string AttachmentsGuid { get; set; }
        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
        public int Priority { get; set; } = 99;
        public string Resolution { get; set; } = string.Empty;
        public int TechnicianId { get; set; } = 0;
        public int ModuleId { get; set; } = 0;
        public string SystemModule { get; set; } = string.Empty;
        public List<ResolutionDto> Resolutions { get; set; } = new List<ResolutionDto>();

    }
}
