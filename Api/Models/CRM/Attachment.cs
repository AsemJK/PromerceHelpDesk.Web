using System.ComponentModel.DataAnnotations.Schema;

namespace PromerceCRM.API.Models.CRM
{
    [Table("CRM_Attachments")]
    public class Attachment : BaseModel
    {
        public int IncidentId { get; set; }
        public string FileName { get; set; }
        public string UploadSignature { get; set; }
        public string Source { get; set; } = "Issue";//Second option is Resolution
    }
}
