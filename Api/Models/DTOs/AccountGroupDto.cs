namespace PromerceCRM.API.Models.ModelsViews
{
    public class AccountGroupDto
    {
        public decimal accountGroupId { get; set; }
        public string accountGroupName { get; set; }
        public string accountGroupNameAr { get; set; }
        public decimal? groupUnder { get; set; }
        public string narration { get; set; }
        public bool? isDefault { get; set; }
        public string nature { get; set; }
        public string affectGrossProfit { get; set; }
        public DateTime? extraDate { get; set; }
        public string extra1 { get; set; }
        public string extra2 { get; set; }
        public decimal? accountNo { get; set; }
        public decimal? parentAccountNo { get; set; }
        public int? accountLevel { get; set; }
        public string mailingName { get; set; }
        public decimal? companyId { get; set; }
    }
}
