using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeReporter.Models
{
    public class Subactivity
    {
        [Key]
        public int SubactivityId { get; set;  }
        
        [MaxLength(20)]
        [Required]
        public string Code { get; set; }
        
        [ForeignKey("Activity")]
        public int ActivityId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Timestamp { get; set; }

        public virtual Activity Activity { get; set;  }
        public virtual ICollection<Entry> Entries { get; set; }
        
        
    }
}