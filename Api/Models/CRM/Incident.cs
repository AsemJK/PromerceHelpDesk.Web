using System.ComponentModel.DataAnnotations.Schema;

namespace PromerceCRM.API.Models.CRM
{
    [Table("CRM_Incidents")]
    public class Incident : BaseModel
    {
        public string Subject { get; set; }
        public int TenantId { get; set; }
        public string UserId { get; set; }
        public string LastStatus { get; set; }
        public string AttachmentsGuid { get; set; }
        public int Priority { get; set; }
        public int TechnicianId { get; set; }
        public int ModuleId { get; set; }
    }
}
