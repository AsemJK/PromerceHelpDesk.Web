namespace PromerceCRM.API.Models.DTOs
{
    public class TenantDto
    {
        public string TenantName { get; set; }
        public string TenantCode { get; set; }

        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CreatorId { get; set; } = 0;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public int UpdaterId { get; set; } = 0;
    }
}
