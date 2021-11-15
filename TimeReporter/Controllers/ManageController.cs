using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


using TimeReporter.Models;


namespace TimeReporter.Controllers
{
    public class ManageController : Controller
    {
        // GET
        public IActionResult Index()
        {
            ManageOption selectedOption = new ManageOption();
            
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
            
            if (TempData["selectedMonth"] != null)
            {
                selectedOption.SelectedMonth = (string)TempData["selectedMonth"];
            }
            
            if (TempData["selectedYear"] != null)
            {
                selectedOption.SelectedYear = (int)TempData["selectedYear"];
            }
            
            selectedOption.Projects = data.Activities
                .FindAll(activity => activity.Manager.Equals(selectedOption.SelectedSurname))
                .ConvertAll(activity => activity.Code);
            
            if (selectedOption.Projects.Any())
            {
                selectedOption.SelectedProject = selectedOption.Projects[0];
            }

            if (TempData["selectedProject"] != null)
            {
                selectedOption.SelectedProject = (string)TempData["selectedProject"];
            }

            Activity activity = data.Activities
                .Find(activity => activity.Code.Equals(selectedOption.SelectedProject));

            if (activity != null)
            {
                selectedOption.ProjectWorkers = activity.Workers.Select(worker => worker.Name).ToList();
                ViewBag.budget = activity.Budget;
                ViewBag.isActive = activity.Active;
            }
            else
            {
                selectedOption.ProjectWorkers = new List<string>();
            }

            foreach (var worker in selectedOption.ProjectWorkers)
            {
                Report report = JsonSerde.GetReport(worker, selectedOption.SelectedMonth, selectedOption.SelectedYear);
                if (report != null)
                {
                    selectedOption.IsFrozen.Add(report.Frozen);
                    AcceptedTime accepted =
                        report.Accepted.Find(accepted => accepted.Code == selectedOption.SelectedProject);
                    if (accepted != null)
                    {
                        selectedOption.AcceptedTime.Add(accepted.Time);
                    }
                    else
                    {
                        selectedOption.AcceptedTime.Add(0);
                    }

                    int timeSum = report.Entries.Where(entry => entry.Code.Equals(selectedOption.SelectedProject)).Sum(entry => entry.Time);
                    selectedOption.SubmittedTime.Add(timeSum);
                }
            }
            return View(selectedOption);
        }

        [HttpGet]
        public ActionResult CreateProjectModal(string selectedSurname, string selectedMonth, int selectedYear, string selectedProject)
        {
            Activity activity = new Activity();

            ViewBag.selectedSurname = selectedSurname;
            ViewBag.selectedMonth = selectedMonth;
            ViewBag.selectedYear = selectedYear;
            ViewBag.selectedProject = selectedProject;

            return PartialView(activity);
        }

        [HttpPost]
        public ActionResult ModalAction(string selectedSurname, string selectedMonth, int selectedYear, string selectedProject,
            string code, string name, int budget)
        {
            Data data = JsonSerde.GetData();
            data.Activities.Add(
                new Activity()
                {
                    Code = code,
                    Manager = selectedSurname,
                    Name = name,
                    Budget = budget,
                    Active = true,
                    Subactivities = new List<Subactivity>(),
                    Workers = new List<Worker>()
                });
            
            JsonSerde.SaveDataChanges(data);
            
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedMonth"] = selectedMonth;
            TempData["selectedYear"] = selectedYear;
            TempData["selectedProject"] = selectedProject;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SelectSurname(string selectedSurname, string selectedMonth, int selectedYear)
        {
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedMonth"] = selectedMonth;
            TempData["selectedYear"] = selectedYear;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SelectMonth(string selectedSurname, string selectedMonth, int selectedYear, string selectedProject)
        {
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedMonth"] = selectedMonth;
            TempData["selectedYear"] = selectedYear;
            TempData["selectedProject"] = selectedProject;
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult SelectYear(string selectedSurname, string selectedMonth, int selectedYear, string selectedProject)
        {
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedMonth"] = selectedMonth;
            TempData["selectedYear"] = selectedYear;
            TempData["selectedProject"] = selectedProject;
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult SelectProject(string selectedSurname, string selectedProject)
        {
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedProject"] = selectedProject;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CloseProject(string selectedSurname, string selectedMonth, int selectedYear, string selectedProject)
        {
            if (!IsProjectActive(selectedProject))
            {
                TempData["Alert"] = "This project is already closed or doesn't exist";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedMonth"] = selectedMonth;
                TempData["selectedYear"] = selectedYear;
                TempData["selectedProject"] = selectedProject;
                return RedirectToAction("Index");
            }

            Data data = JsonSerde.GetData();
            data.Activities.Find(activity => activity.Code == selectedProject).Active = false;
            
            JsonSerde.SaveDataChanges(data);
            
            TempData["Success"] = "Successfully closed project";
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedMonth"] = selectedMonth;
            TempData["selectedYear"] = selectedYear;
            TempData["selectedProject"] = selectedProject;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AcceptTime(string selectedSurname, string selectedMonth, int selectedYear, string selectedProject, 
             string worker, bool isMonthSubmitted, int time, int idx)
        {
            
            if (!IsProjectActive(selectedProject))
            {
                TempData["Alert"] = "Can't accept because this project is closed";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedMonth"] = selectedMonth;
                TempData["selectedYear"] = selectedYear;
                TempData["selectedProject"] = selectedProject;
                return RedirectToAction("Index");
            }
            
            if (!isMonthSubmitted)
            {
                TempData["Alert"] = "Can't accept because this report is not submitted";
                TempData["selectedSurname"] = selectedSurname;
                TempData["selectedMonth"] = selectedMonth;
                TempData["selectedYear"] = selectedYear;
                TempData["selectedProject"] = selectedProject;
                return RedirectToAction("Index");
            }
            
            RecalculateBudget(worker, selectedMonth, selectedYear, selectedProject, time);
            AcceptWorker(worker, selectedMonth, selectedYear, selectedProject, time);
            
            TempData["Success"] = "Successfully accepted time";
            TempData["selectedSurname"] = selectedSurname;
            TempData["selectedMonth"] = selectedMonth;
            TempData["selectedYear"] = selectedYear;
            TempData["selectedProject"] = selectedProject;
            return RedirectToAction("Index");
        }

        private void RecalculateBudget(string worker, string selectedMonth, int selectedYear, string selectedProject, int time)
        {
            Report report = JsonSerde.GetReport(worker, selectedMonth, selectedYear);
            
            AcceptedTime acceptedTime = report.Accepted.Find(accepted => accepted.Code == selectedProject);

            int timeChange;

            if (acceptedTime != null)
            {
                timeChange = acceptedTime.Time - time;
            }
            else
            {
                timeChange = -time;
            }

            Data data = JsonSerde.GetData();
            
            Activity activity = data.Activities.Find(activity =>
                activity.Code == selectedProject);
            
            if (activity != null)
            {
                data.Activities[data.Activities.IndexOf(activity)].Budget += timeChange;
            }

            // data.Activities[
            //     data.Activities.IndexOf((from activity in data.Activities
            //         where activity.Code == selectedProject
            //         select activity).ToList()[0])].Budget -= time;
            JsonSerde.SaveDataChanges(data);
        }

        private void AcceptWorker(string worker, string selectedMonth, int selectedYear, string selectedProject, int time)
        {
            Report report = JsonSerde.GetReport(worker, selectedMonth, selectedYear);
            AcceptedTime acceptedTime = report.Accepted.Find(accepted => accepted.Code == selectedProject);
            if (acceptedTime != null)
            {
                report.Accepted[report.Accepted.IndexOf(acceptedTime)].Time = time;
            }
            else
            {

                report.Accepted.Add(
                    new AcceptedTime()
                    {
                        Code = selectedProject,
                        Time = time
                    });
            }

            JsonSerde.SaveReportChanges(report, worker, selectedMonth, selectedYear);
        }

        private bool IsProjectActive(string selectedProject)
        {
            Activity activity = JsonSerde.GetData().Activities.Find(activity => 
                activity.Code == selectedProject);
            return activity != null && activity.Active;
        }
    }
}