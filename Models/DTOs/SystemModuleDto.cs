namespace PromerceHelpDesk.Web.Models.DTOs
{
    public class SystemModuleDto
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CreatorId { get; set; } = 0;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public int UpdaterId { get; set; } = 0;

        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
    }
}
