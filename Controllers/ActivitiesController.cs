using System;
using System.Collections.Generic;
using SysActivity = System.Diagnostics.Activity;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


using TimeReporter.Models;

namespace TimeReporter.Controllers
{
    public class ActivitiesController : Controller
    {
        public ActionResult Index()
        {
            Option selectedOption = new Option();
            
            Data data = JsonSerde.GetData();

            selectedOption.Surnames = data.Workers.Select(worker => worker.Name).ToList();

            if (selectedOption.Surnames.Any())
            {
                selectedOption.SelectedSurname = selectedOption.Surnames[0];
            }

            if (TempData["selectedSurname"] != null)
            {
                selectedOption.SelectedSurname = (string)TempData["selectedSurname"];
            }

            if (TempData["selectedDate"] != null)
            {
                selectedOption.SelectedDate = (DateTime)TempData["selectedDate"];
            }

            Report report = JsonSerde.GetReport(selectedOption.SelectedSurname, selectedOption.SelectedDate);

            List<Entry> entries = new List<Entry>();
            
            List<AcceptedTime> accepted = new List<AcceptedTime>();
            List<string> managers = new List<string>();
            List<string> projects = new List<string>();
            List<int> projectSum = new List<int>();

            if(report != null)
            {
                entries = report.GetDayEntries(selectedOption.SelectedDate.ToString("yyyy-MM-dd"));
                accepted = report.Accepted;
                foreach (var accept in report.Accepted)
                {
                    managers.Add((from activity in data.Activities
                        where activity.Code == accept.Code
                        select activity).ToList()[0].Manager);
                }
                
                projects.AddRange(report.Entries.Select(entry => entry.Code).Distinct());

                foreach (var project in projects)
                {
                    projectSum.Add(report.Entries.Where(entry => entry.Code.Equals(project)).Sum(entry => entry.Time));
                }
            }

            ViewBag.accepted = accepted;
            ViewBag.managers = managers;
            ViewBag.projects = projects;
            ViewBag.projectSum = projectSum;
            

            selectedOption.Entries = entries;

            return View(selectedOption);
        }

        [HttpPost]
        public ActionResult SelectSurname(string selectedSurname, DateTime selectedDate)
        {
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedDate"] = selectedDate;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SelectDate(string selectedSurname, DateTime selectedDate)
        {
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedDate"] = selectedDate;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SubmitMonth(string selectedSurname, DateTime selectedDate)
        {
            Report report = JsonSerde.GetReport(selectedSurname, selectedDate);
            if (report == null)
            {
                TempData["Alert"] = "Can't submit because report for this month doesn't exist";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedDate"] = selectedDate;
                return RedirectToAction("Index");
            }

            if (report.Frozen)
            {
                TempData["Alert"] = "This month is already submitted";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedDate"] = selectedDate;
                return RedirectToAction("Index");
            }

            report.Frozen = true;
            
            JsonSerde.SaveReportChanges(report, selectedSurname, selectedDate);
            TempData["Success"] = "Successfully submited month: " + selectedDate.ToString("MMMM");
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedDate"] = selectedDate;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string selectedSurname, DateTime selectedDate, int deleteIdx)
        { 
            Report report = JsonSerde.GetReport(selectedSurname, selectedDate);
            if(report.Frozen ||  !IsProjectActive(report, selectedDate, deleteIdx))
            {
                TempData["Alert"] = "Month is frozen or project is not active";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedDate"] = selectedDate;
                return RedirectToAction("Index");
            }
            DeleteEntry(report, selectedSurname, selectedDate, deleteIdx);
            TempData["Success"] = "Successfully deleted entry";
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedDate"] = selectedDate;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EntryModal(string selectedSurname, DateTime selectedDate, int idx)
        {
            Data data = JsonSerde.GetData();

            Report report = JsonSerde.GetReport(selectedSurname, selectedDate);

            Entry entry;

            if(idx >= 0)
            {
                entry = report.Entries.FindAll(e => 
                    e.Date.Equals(selectedDate.ToString("yyyy-MM-dd")))[idx];
            }
            else
            {
                entry = new Entry();
            }

            ViewBag.selectedSurname = selectedSurname;
            ViewBag.selectedDate = selectedDate;
            ViewBag.idx = idx;
            ViewBag.codes = data.GetAllCodes();

            return PartialView(entry);
        }

        [HttpPost]
        public ActionResult ModalAction(string selectedSurname, DateTime selectedDate, int idx,
            string code, string subcode, int time, string description)
        {
            
            if (JsonSerde.GetReport(selectedSurname, selectedDate) == null
                && IsProjectActive(code)
                && selectedDate.Month == DateTime.Now.Month
                && selectedDate.Year == DateTime.Now.Year)
            {
                JsonSerde.CreateNewMonthReport(selectedSurname, selectedDate);
            }
            else if (selectedDate.Month != DateTime.Now.Month || selectedDate.Year != DateTime.Now.Year)
            {
                TempData["Alert"] = "Entries can be added only to current month";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedDate"] = selectedDate;
                return RedirectToAction("Index");
            }
            else if (!IsProjectActive(code))
            {
                TempData["Alert"] = "Project " + code + " is not active";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedDate"] = selectedDate;
                return RedirectToAction("Index");
                
            }
            
            Report report = JsonSerde.GetReport(selectedSurname, selectedDate);

            if(report.Frozen)
            {
                TempData["Alert"] = "Month is frozen";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedDate"] = selectedDate;
                return RedirectToAction("Index");
            }
        
            Entry entry = new Entry()
            {
                Date = selectedDate.ToString("yyyy-MM-dd"),
                Code = code,
                Subcode = subcode,
                Time = time,
                Description = description
            };

            if(idx == -1)
            {
                AddNewEntry(selectedSurname, selectedDate, entry);
            }
            else
            {
                EditEntry(selectedSurname, selectedDate, entry, idx);
            }

            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedDate"] = selectedDate;
            return RedirectToAction("Index");
        }

        private void DeleteEntry(Report report, string selectedSurname, DateTime selectedDate, int deleteIdx)
        {
            report.Entries.Remove(report.Entries.FindAll(entry => 
                entry.Date.Equals(selectedDate.ToString("yyyy-MM-dd")))[deleteIdx]);
            JsonSerde.SaveReportChanges(report, selectedSurname, selectedDate);
        }

        private void AddNewEntry(string selectedSurname, DateTime selectedDate, Entry entry)
        {
            Report report = JsonSerde.GetReport(selectedSurname, selectedDate);
            report.Entries.Add(entry);
            Data data = JsonSerde.GetData();
            Activity activity = data.Activities.Find(activity => activity.Code.Equals(entry.Code));
            
            if (activity != null && !activity.GetAllSubactivities().Contains(entry.Subcode))
            {
                data.Activities[data.Activities.IndexOf(activity)].Subactivities.Add(new Subactivity(entry.Subcode));
            }
            
            if (activity != null && !activity.Workers.Select(worker => worker.Name).ToList().Contains(selectedSurname))
            {
                data.Activities[data.Activities.IndexOf(activity)].Workers.Add(new Worker(selectedSurname));
            }
            
            JsonSerde.SaveDataChanges(data);
            JsonSerde.SaveReportChanges(report, selectedSurname, selectedDate);
        }

        private void EditEntry(string selectedSurname, DateTime selectedDate, Entry entry, int idx)
        {
            Report report = JsonSerde.GetReport(selectedSurname, selectedDate);
            report.Entries[report.Entries.IndexOf(report.Entries.FindAll(e => 
                e.Date.Equals(selectedDate.ToString("yyyy-MM-dd")))[idx])] = entry;
            JsonSerde.SaveReportChanges(report, selectedSurname, selectedDate);
        }

        private bool IsMonthFrozen(string selectedSurname, DateTime selectedDate)
        {
            return JsonSerde.GetReport(selectedSurname, selectedDate).Frozen;
        }

        private bool IsProjectActive(Report report, DateTime selectedDate, int idx)
        {
            string checkCode = report.Entries.FindAll(entry => entry.Date.Equals(selectedDate.ToString("yyyy-MM-dd")))[idx].Code;
            Activity activity = JsonSerde.GetData().Activities.Find(activity => 
                activity.Code.Equals(checkCode));
            return activity != null && activity.Active;
        }

        private bool IsProjectActive(string code)
        {
            Activity activity = JsonSerde.GetData().Activities.Find(activity => 
                activity.Code.Equals(code));
            return activity != null && activity.Active;
        }
    }
}
