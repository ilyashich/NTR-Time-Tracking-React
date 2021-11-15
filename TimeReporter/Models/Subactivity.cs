using System.Text.Json.Serialization;

namespace TimeReporter.Models
{
    public class Subactivity
    {
        public Subactivity(string code)
        {
            Code = code;
        }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}