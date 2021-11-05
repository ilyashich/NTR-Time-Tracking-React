using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace TimeReporter.Models
{
    public class Report
    {
        [JsonPropertyName("frozen")]
        public bool Frozen { get; set; }

        [JsonPropertyName("entries")]
        public List<Entry> Entries { get; set; }

        [JsonPropertyName("accepted")]
        public List<AcceptedTime> Accepted { get; set; }

        public List<Entry> GetDayEntries(string date)
        {
            return Entries.FindAll(entry => entry.Date.Equals(date));
        }

    }
}