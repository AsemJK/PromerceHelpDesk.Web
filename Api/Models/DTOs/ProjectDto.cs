namespace PromerceCRM.API.Models.DTOs
{
    public class ProjectDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }


        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CreatorId { get; set; } = 0;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public int UpdaterId { get; set; } = 0;
    }
}
