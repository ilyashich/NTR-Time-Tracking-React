using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace TimeReporter.Models
{
    public class Data
    {
        [JsonPropertyName("allWorkers")]
        public List<Worker> Workers { get; set; }

        [JsonPropertyName("activities")]
        public List<Activity> Activities { get; set; }

        public List<string> GetAllCodes()
        {
            return Activities.Select(activity => activity.Code).ToList();
        }
        
    }
}