using System.ComponentModel.DataAnnotations.Schema;

namespace PromerceCRM.API.Models.CRM
{
    [Table("CRM_Resolutions")]
    public class Resolution : BaseModel
    {
        public int IncidentId { get; set; }
        public string ReplyBody { get; set; }
        public string UserId { get; set; }
    }
}
