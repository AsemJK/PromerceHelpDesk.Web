namespace PromerceHelpDesk.Web.Models.DTOs
{
    public class ResolutionDto
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CreatorId { get; set; } = 0;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public int UpdaterId { get; set; } = 0;
        public string UserName { get; set; } = "NA";

        //
        public int IncidentId { get; set; }
        public string ReplyBody { get; set; }
        public string UserId { get; set; }

        public List<AttachmentModel> Attachments { get; set; } = new List<AttachmentModel>();
    }
}
