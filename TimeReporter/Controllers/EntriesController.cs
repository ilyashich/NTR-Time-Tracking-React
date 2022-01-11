using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeReporter.Models;
using TimeReporter.Services;

namespace TimeReporter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly TimeReporterContext _context;
        private readonly ReportService _reportService;

        public EntriesController(TimeReporterContext context)
        {
            _context = context;
            _reportService = new ReportService(_context);
        }

        // GET: api/Entries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entry>> GetEntry(int id)
        {
            var entry = await _context.Entries.FindAsync(id);

            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        // PUT: api/Entries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntry(int id, Entry entry)
        {
            if (id != entry.EntryId)
            {
                return BadRequest();
            }

            var activity = await _context.Activities.FindAsync(entry.ActivityId);

            if(!activity.Active)
            {
                return BadRequest($"Cannot edit because project {activity.Code} is not active");
            }

            _context.Entry(entry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryExists(id))
                {
                    return NotFound("Can't edit because entry has been deleted");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Entries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Entry>> PostEntry([FromForm]DateTime selectedDate,
            [FromForm]int activityId, [FromForm]int? subactivityId, [FromForm]int time, [FromForm]string description)
        {
            var workerId = HttpContext.Session.GetInt32(SessionUser.SessionWorkerId);
            var selectedWorker = await _context.Workers.FindAsync(workerId);

            if (selectedWorker == null)
            {
                return NotFound("You are not logged in");
            }

            var activity = await _context.Activities.FindAsync(activityId);
            var isProjectActive = IsProjectActive(activity);
            

            if (await _reportService.GetReport(selectedWorker, selectedDate) == null
                && isProjectActive == 0
                && selectedDate.Month == DateTime.Now.Month
                && selectedDate.Year == DateTime.Now.Year)
            {
                Report newReport = new Report {Date = selectedDate, Frozen = false, WorkerId = selectedWorker.WorkerId};
                _context.Reports.Add(newReport);
                await _context.SaveChangesAsync();
            }
            else if (selectedDate.Month != DateTime.Now.Month || selectedDate.Year != DateTime.Now.Year)
            {
                return BadRequest("You can add entry only to current month");
            }
            else if (isProjectActive > 0)
            {
                return BadRequest($"Cannot add because project {activity.Code} is not active");
                
            }
            else if (isProjectActive < 0)
            {
                return BadRequest($"Cannot add because project {activity.Code} does not exist");
                
            }
            
            Report report = await _reportService.GetReport(selectedWorker, selectedDate);

            if(report.Frozen)
            {
                return BadRequest("Cannot add beacause month is frozen");
            }

            Entry newEntry = new Entry()
            {
                Date = selectedDate,
                ActivityId = activityId,
                SubactivityId = subactivityId,
                Time = time,
                Description = description,
                WorkerId = selectedWorker.WorkerId,
                ReportId = report.ReportId
            };
            
            _context.Entries.Add(newEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntry", new { id = newEntry.EntryId }, newEntry);
        }

        // DELETE: api/Entries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var entry = await _context.Entries.Include(entry => entry.Report).SingleOrDefaultAsync(entry => entry.EntryId == id);
            if (entry == null)
            {
                return NotFound("Can't delete because entry has been deleted");
            }

            if(entry.Report.Frozen)
            {
                return BadRequest("Cannot delete beacause month is frozen");
            }

            _context.Entries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntryExists(int id)
        {
            return _context.Entries.Any(e => e.EntryId == id);
        }

        private int IsProjectActive(Activity activity)
        {
            if (activity == null)
                return -1;
            return activity.Active switch
            {
                true => 0,
                false => 1
            };
        }
    }
}
