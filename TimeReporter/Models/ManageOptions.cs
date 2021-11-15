using System.Collections.Generic;
using System;

namespace TimeReporter.Models
{
    public class ManageOption
    {
        public ManageOption()
        {
            SelectedMonth = DateTime.Now.Month.ToString();
            SelectedYear = DateTime.Now.Year;
            IsFrozen = new List<bool>();
            SubmittedTime = new List<int>();
            AcceptedTime = new List<int>();
        }

        public string SelectedSurname { get; set; }
        
        public string SelectedProject { get; set; }
        
        public string SelectedMonth { get; set; }

        public int SelectedYear { get; set; }
        
        public List<bool> IsFrozen { get; set; }

        public List<int> SubmittedTime { get; set; }

        public List<int> AcceptedTime { get; set; }
        
        public List<string> Surnames { get; set; }

        public List<string> Projects { get; set; }

        public List<string> ProjectWorkers { get; set; }
    }
}