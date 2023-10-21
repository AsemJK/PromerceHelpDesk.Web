using System.Text.Json.Serialization;

namespace PromerceHelpDesk.Web.Models
{
    public class MyResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }
        [JsonPropertyName("status")]

        public bool Status { get; set; }
    }
}
