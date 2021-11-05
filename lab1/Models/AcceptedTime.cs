using System.Text.Json.Serialization;

namespace TimeReporter.Models
{
    public class AcceptedTime
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("time")]
        public int Time { get; set; }
        

    }
}