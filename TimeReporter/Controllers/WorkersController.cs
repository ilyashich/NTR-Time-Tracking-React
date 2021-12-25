using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeReporter.Models;

namespace TimeReporter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly TimeReporterContext _context;
        
        public WorkersController(TimeReporterContext context)
        {
            _context = context;
        }

        // GET: api/Workers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
        {
            return await _context.Workers.ToListAsync();
        }

        [HttpGet("session")]
        public async Task<ActionResult<Worker>> GetSessionUser()
        {
            var id = HttpContext.Session.GetInt32(SessionUser.SessionWorkerId);
            var worker =  await _context.Workers.FindAsync(id);
            return worker;
        }

        [HttpGet("session/{id}")]
        public async Task<ActionResult<Worker>> Login(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            HttpContext.Session.SetInt32(SessionUser.SessionWorkerId, id);
            return worker;
        }

        [HttpGet("session/logout")]
        public ActionResult<string> Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }

        // POST: api/Workers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Worker>> PostWorkerAndLogin([FromForm] string name)
        {
            var worker = await _context.Workers.SingleOrDefaultAsync(worker => worker.Name == name);
            if(worker == null)
            {
                worker = new Worker(){ Name = name };
                _context.Workers.Add(worker);
                await _context.SaveChangesAsync();
            }        
            HttpContext.Session.SetInt32(SessionUser.SessionWorkerId, worker.WorkerId);

            return worker;
        }

    }
}
