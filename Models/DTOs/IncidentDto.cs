namespace PromerceHelpDesk.Web.Models.DTOs
{
    public class IncidentDto
    {
        public string Subject { get; set; }
        public int TenantId { get; set; }
        public string TenantCode { get; set; }
        public string TenantName { get; set; }
        public string UserId { get; set; }
        public string LastStatus { get; set; }
        public string AttachmentsGuid { get; set; }
        public List<AttachmentModel> Attachments { get; set; }
        public int Priority { get; set; }
        public string Resolution { get; set; }
        public int TechnicianId { get; set; }
        public int ModuleId { get; set; } = 0;
        public string SystemModule { get; set; } = string.Empty;
        public List<ResolutionDto> Resolutions { get; set; } = new List<ResolutionDto>();
        //
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CreatorId { get; set; } = 0;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public int UpdaterId { get; set; } = 0;

    }
}
