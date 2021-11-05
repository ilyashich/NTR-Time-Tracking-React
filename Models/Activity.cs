using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace TimeReporter.Models
{
    public class Activity
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("manager")]
        public string Manager { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("budget")]
        public int Budget { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("subactivities")]
        public List<Subactivity> Subactivities { get; set; }

        [JsonPropertyName("workers")]
        public List<Worker> Workers { get; set; }

        public List<string> GetAllSubactivities()
        {
            return Subactivities.Select(subactivity => subactivity.Code).ToList();
        }
    }
}