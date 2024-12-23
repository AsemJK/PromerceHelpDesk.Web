using System.ComponentModel.DataAnnotations.Schema;

namespace PromerceCRM.API.Models.CRM
{
    [Table("CRM_Tenants")]
    public class Tenant : BaseModel
    {
        public string TenantName { get; set; }
        public string TenantCode { get; set; }
    }
}
