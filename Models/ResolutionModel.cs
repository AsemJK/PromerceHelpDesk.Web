namespace PromerceHelpDesk.Web.Models
{
    public class ResolutionModel
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CreatorId { get; set; } = 0;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public int UpdaterId { get; set; } = 0;

        //
        public int IncidentId { get; set; }
        public string ReplyBody { get; set; }
        public string UserId { get; set; }
    }
}
