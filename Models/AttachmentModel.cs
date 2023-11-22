namespace PromerceHelpDesk.Web.Models
{
    public class AttachmentModel
    {
        public int IncidentId { get; set; }
        public string FileName { get; set; }
        public string UploadSignature { get; set; }
        public string Source { get; set; } = "Issue";
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CreatorId { get; set; } = 0;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public int UpdaterId { get; set; } = 0;
    }
}
