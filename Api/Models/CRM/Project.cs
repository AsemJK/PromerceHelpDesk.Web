using System.ComponentModel.DataAnnotations.Schema;

namespace PromerceCRM.API.Models.CRM
{
    [Table("CRM_Projects")]
    public class Project : BaseModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
