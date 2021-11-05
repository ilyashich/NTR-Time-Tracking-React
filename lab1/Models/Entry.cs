using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;

namespace TimeReporter.Models
{
    public class Entry
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("subcode")]
        public string Subcode { get; set; }

        [JsonPropertyName("time")]
        public int Time { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}