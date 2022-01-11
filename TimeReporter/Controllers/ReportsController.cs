using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeReporter.Models;

namespace TimeReporter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly TimeReporterContext _context;

        public ReportsController(TimeReporterContext context)
        {
            _context = context;
        }

        // GET: api/Reports/worker
        [HttpPost("worker")]
        public async Task<ActionResult<Report>> GetMonthReportForWorker([FromForm] DateTime date)
        {
            var workerId = HttpContext.Session.GetInt32(SessionUser.SessionWorkerId);
            var selectedWorker = await _context.Workers.FindAsync(workerId);

            if (selectedWorker == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                                .Include(report => report.Accepteds)
                                .ThenInclude(accepted => accepted.Activity)
                                .ThenInclude(activity => activity.Worker)
                                .Include(report => report.Entries)
                                .ThenInclude(entry => entry.Subactivity)
                                .Include(report => report.Entries)
                                .ThenInclude(entry => entry.Activity)
                                .ThenInclude(activity => activity.Subactivities)
                                .SingleOrDefaultAsync(report => report.WorkerId == selectedWorker.WorkerId && report.Date.Month == date.Month && report.Date.Year == date.Year);

            return report;
        }
    }
}
