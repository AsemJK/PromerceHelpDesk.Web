using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PromerceCRM.API.Models.ERP
{
    [Table(name: "tbl_AccountGroup")]
    public class AccountGroup
    {
        [Key]
        [Column(TypeName = "numeric(18, 0)")]
        public decimal accountGroupId { get; set; }
        public string accountGroupName { get; set; }
        public string accountGroupNameAr { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal? groupUnder { get; set; }
        public string narration { get; set; }
        public bool? isDefault { get; set; }
        public string nature { get; set; }
        public string affectGrossProfit { get; set; }
        public DateTime? extraDate { get; set; }
        public string extra1 { get; set; }
        public string extra2 { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal? accountNo { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal? parentAccountNo { get; set; }
        public int? accountLevel { get; set; }
        public string mailingName { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal? companyId { get; set; }
    }
}
