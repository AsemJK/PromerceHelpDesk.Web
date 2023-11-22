namespace PromerceHelpDesk.Web.Models
{
    public class IncidentModel
    {
        public string Subject { get; set; }
        public int TenantId { get; set; } = 0;
        public string TenantCode { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string UserId { get; set; }
        public string LastStatus { get; set; }
        public string AttachmentsGuid { get; set; }
        public int Priority { get; set; }
        public string Resolution { get; set; }
        public int TechnicianId { get; set; }
        public int ModuleId { get; set; }
    }
}
