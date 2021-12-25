using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeReporter.Models
{
    public class Entry
    {
        [Key] 
        public int EntryId { get; set;  }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [ForeignKey("Activity")]
        public int ActivityId { get; set; }

        [ForeignKey("Subactivity")]
        public int? SubactivityId { get; set; }

        [Required]
        public int Time { get; set; }
        
        [MaxLength(255)]
        public string Description { get; set; }
        
        [ForeignKey("Worker")]
        public int WorkerId { get; set; }
        
        [ForeignKey("Report")]
        public int ReportId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Timestamp { get; set; }

        public virtual Worker Worker { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Subactivity Subactivity { get; set; }
        public virtual Report Report { get; set; }
        
    }
}