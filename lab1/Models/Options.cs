using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace TimeReporter.Models
{
    public class Option
    {
        public Option()
        {
            SelectedDate = DateTime.Now; 
        }
        public string SelectedSurname { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime SelectedDate { get; set; }
        public List<string> Surnames { get; set; }
        public List<Entry> Entries { get; set; }
    }
}