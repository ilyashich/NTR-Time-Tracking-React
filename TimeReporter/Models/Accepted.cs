using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeReporter.Models
{
    public class Accepted
    {
        [Key] 
        public int AcceptedId { get; set;  }

        [ForeignKey("Activity")]
        public int ActivityId { get; set; }

        [Required]
        public int Time { get; set; }
        
        [ForeignKey("Report")]
        public int ReportId { get; set; }
        
        [ForeignKey("Worker")]
        public int WorkerId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Timestamp { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Report Report { get; set; }
        public virtual Worker Worker { get; set; }
        
    }
}