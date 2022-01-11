using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeReporter.Models;

namespace TimeReporter.Services
{
    public class ReportService
    {
        private readonly TimeReporterContext _db;

        public ReportService(TimeReporterContext db)
        {
            _db = db;
        }

        public async Task<Report> GetReport(Worker worker, DateTime date)
        {
            return await _db.Reports
                .Include(report => report.Entries)
                .Include(report => report.Worker)
                .Include(report => report.Accepteds)
                .ThenInclude(accepted => accepted.Activity)
                .ThenInclude(activity => activity.Worker)
                .SingleOrDefaultAsync(r =>
                r.WorkerId == worker.WorkerId && r.Date.Month == date.Month && r.Date.Year == date.Year);
        }


    }
}