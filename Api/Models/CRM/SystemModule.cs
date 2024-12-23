using System.ComponentModel.DataAnnotations.Schema;

namespace PromerceCRM.API.Models.CRM
{
    [Table("CRM_SystemModules")]
    public class SystemModule : BaseModel
    {
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public int ProjectId { get; set; } = 0;

    }
}
