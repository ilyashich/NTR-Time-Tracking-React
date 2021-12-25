using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeReporter.Models
{
    public class Worker
    {
        [Key]
        public int WorkerId { get; set;  }
        
        [MaxLength(40)]
        [Required]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Timestamp { get; set; }
        
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Report> Reports { get; set; }

    }
}