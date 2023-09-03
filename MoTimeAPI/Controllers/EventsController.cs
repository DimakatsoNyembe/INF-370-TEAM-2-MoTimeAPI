using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoTimeAPI.Models;

namespace MoTimeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly MoTimeDatabaseContext _context;

        public EventsController(MoTimeDatabaseContext context)
        {
            _context = context;
        }

        public string[] monthNames = new string[]
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };



        [HttpPost("SubmitTimeSheet")]
        public async Task<IActionResult> SubmitTimesheetEvent(timesheetEntry timesheetEntry)
        {

            List<Event> TimesheetRecords = await _context.Events
                .Where(s => s.Employee.Equals(timesheetEntry.id.ToString()) && s.End.Value.Month == timesheetEntry.month)
                .ToListAsync();



            foreach (Event e in TimesheetRecords)
            {
                Event item = _context.Events.FirstOrDefault(s => s.Id == e.Id);
                if (item != null)
                {
                    item.barColor = "#FF0000";
                    _context.SaveChanges();
                }
            }

            if (TimesheetRecords.Count == 0)
            {
                return NotFound();

            }

            try
            {

                Timesheet timesheet = new Timesheet();
                timesheet.ProjectId = Convert.ToInt32(TimesheetRecords[0].Project);
                timesheet.TimesheetStatusId = 1;
                timesheet.DateSubmitted = DateTime.Now;
                timesheet.EmployeeId = _context.Employees.FirstOrDefault(s => s.UserId == timesheetEntry.id).EmployeeId;
                await _context.Timesheets.AddAsync(timesheet);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return NoContent();

        }

        // Post: API/PostAccptOrReject
        [HttpPost("UpdateTimeSheetStatus")]
        public async Task<IActionResult> PostAccptOrReject(SubmitTimesheetUpdate timesheetUpdate)
        {

            var selectedtimesheet = await _context.Timesheets.FirstOrDefaultAsync(s => s.TimesheetId == timesheetUpdate.timesheetId);
            if (selectedtimesheet != null)
            {
                selectedtimesheet.TimesheetStatusId = timesheetUpdate.statusId;
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound(timesheetUpdate.timesheetId);
            }

            return NoContent();
        }


        [HttpGet("GetAllSubmittedTimeSheets")]
        public ActionResult<List<TimesheetVM>> GetTimesheetEvent()
        {
            List<TimesheetVM> AllTimesheets = new List<TimesheetVM>();

            var ts = _context.Events.ToList();
            var projAlloc = _context.ProjectAllocations.ToList();
            var projects = _context.Projects.ToList();
            var employees = _context.Employees.Include(s => s.User).ToList();
            foreach (var item in _context.Timesheets.Include(s => s.TimesheetStatus).Include(s => s.Employee))
            {
                List<Event> TimesheetEvents = ts
                                             .Where(s =>
                                                 s.Employee.Equals(item.Employee.UserId.ToString()) &&
                                                 item.DateSubmitted.HasValue && // Check if DateSubmitted has a value
                                                 s.End.HasValue &&             // Check if End has a value
                                                 s.End.Value.Month == item.DateSubmitted.Value.Month
                                             )
                                             .ToList();


                TimesheetVM timesheetVM = new TimesheetVM();
                timesheetVM.fullname = item.Employee.User.FirstName + " " + item.Employee.User.LastName;
                timesheetVM.timesheetID = item.TimesheetId;
                timesheetVM.status = item.TimesheetStatus.TimesheetStatusName;
                foreach (var i in TimesheetEvents)
                {
                    int projectId = Convert.ToInt32(i.Project);
                    var project = projects.FirstOrDefault(s => s.ProjectId == projectId);


                    if (project != null)
                    {
                        timesheetVM.Projects = project.ProjectName + " , " + timesheetVM.Projects;
                        int empid = Convert.ToInt32(item.EmployeeId);
                        var projectAllocation = projAlloc.FirstOrDefault(s => s.ProjectId == projectId && s.EmployeeId == empid);
                        if (projectAllocation != null)
                        {
                            timesheetVM.TotalHours = (int)projectAllocation.TotalNumHours;
                        }

                    }

                }
                timesheetVM.Submitteddate = item.DateSubmitted.ToString();
                var employee = employees.FirstOrDefault(s => s.UserId == item.EmployeeId);


                timesheetVM.EmpId = (int)item.EmployeeId;

                timesheetVM.month = monthNames[(item.DateSubmitted.Value.Month - 1)];

                timesheetVM.year = item.DateSubmitted.Value.Year;

                timesheetVM.monthnum = item.DateSubmitted.Value.Month;

                AllTimesheets.Add(timesheetVM);


            }


            return AllTimesheets;
        }

        // GET: API/GetProjectHours
        [HttpGet("Employee/{employeeId}/Project/{projectId}/AllocationHours")]
        public async Task<ActionResult<ProjectAllocation>> GetEmployeeProjects(int employeeId, int projectId)
        {
            var EmployeeProjects = await _context.ProjectAllocations.Include(s => s.Employee.User).FirstOrDefaultAsync(s => s.Employee.UserId == employeeId && s.ProjectId == projectId);

            return EmployeeProjects;
        }


        // GET: API/GetProjectHours
        [HttpGet("Employee/{employeeId}/Months/{month}/isTbmittable")]

        public async Task<ActionResult<bool>> isTbmittable(int employeeId, int month)
        {
            month++;
            bool isTbmittable = false;

            foreach (var item in _context.Events.Where(s => s.Employee.Equals(employeeId.ToString()) && s.End.Value.Month == month))
            {
                if (item.barColor == "#6aa84f")
                {
                    isTbmittable = true;
                }
                else
                {
                    isTbmittable = false;
                    break;
                }
            }

            return isTbmittable;
        }



        // GET: API/GetProjectHours
        [HttpGet("Employee/{employeeId}")]
        public async Task<ActionResult<User>> GetEmployee(int employeeId)
        {

            Employee selectEmployee = await _context.Employees.FirstOrDefaultAsync(s => s.EmployeeId == employeeId);


            var Employee = await _context.Employees.FirstOrDefaultAsync(s => s.EmployeeId == selectEmployee.EmployeeId);
            var user = new User();
            if (Employee != null)
            {
                user = await _context.Users.FirstOrDefaultAsync(s => s.UserId == Employee.UserId);
            }
            else
            {
                return NotFound();
            }

            return user;
        }



        // GET: API/GetProjectHours
        [HttpGet("Employee/{employeeId}/Month/{month}/EmployeeReport")]
        public async Task<ActionResult<IEnumerable<EmployeeReportVM>>> GetEmployeeReport(int employeeId, int month)
        {
            Employee selectEmployee = await _context.Employees.FirstOrDefaultAsync(s => s.EmployeeId == employeeId);


            var employee = _context.Employees.FirstOrDefault(s => s.EmployeeId == selectEmployee.EmployeeId);
            var employeeReport = new List<EmployeeReportVM>();
            var employeeEvent = _context.Events.Where((s => s.Employee.Equals(selectEmployee.UserId.ToString()) && (s.Start.Value.Month == (month+1)) && (s.Start.Value.Year == DateTime.Now.Year))).ToList();

            foreach (var e in employeeEvent)
            {
                EmployeeReportVM employeeReportVM = new EmployeeReportVM();
                string? projectName = _context.Projects.FirstOrDefault(s => s.ProjectId == Convert.ToInt32(e.Project)).ProjectName;
                employeeReportVM.ProjectName = projectName;
                employeeReportVM.TotalHours = CalculateTimeDifferenceInHours(e.Start, e.End);
                employeeReportVM.start = e.Start;
                employeeReportVM.end = e.End;
                employeeReport.Add(employeeReportVM);
            }
            return employeeReport;
        }

        public static int CalculateTimeDifferenceInHours(DateTime? start, DateTime? finish)
        {
            if (start.HasValue && finish.HasValue)
            {
                TimeSpan timeDifference = finish.Value - start.Value;
                int totalHours = (int)Math.Ceiling(timeDifference.TotalHours);
                return totalHours;
            }
            else
            {
                return 0; // Or you can return some default value or throw an exception
            }
        }


        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents(DateTime? from, DateTime? to, string employee)
        {
            IQueryable<Event> eventsQuery = _context.Events;

            if (from.HasValue && to.HasValue)
            {
                // Filter events based on the 'from' and 'to' dates
                eventsQuery = eventsQuery.Where(e => e.Start >= from.Value && e.End <= to.Value);
            }

            if (!string.IsNullOrEmpty(employee))
            {
                // Filter events based on the employeeId
                eventsQuery = eventsQuery.Where(e => e.Employee == employee);
            }

            var events = await eventsQuery.ToListAsync();

            if (events == null)
            {
                return NotFound();
            }

            foreach (var item in events)
            {
                item.Text = _context.Projects.FirstOrDefault(s => s.ProjectId == Convert.ToInt32(item.Project)).ProjectName;

            }

            return events;
        }



        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            Event selectedEvent = await _context.Events.FirstOrDefaultAsync(s => s.Id == id);
            if (selectedEvent != null)
            {

                selectedEvent.Project = @event.Project;
                selectedEvent.Start = @event.Start;
                selectedEvent.End = @event.End;

            }
            //  _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'MoTimeDatabaseContext.Events'  is null.");
            }
            _context.Events.Add(@event);
            try
            {
                await _context.SaveChangesAsync();

                int subtract = CalculateDurationInHours(@event);

                var Allocation = await _context.ProjectAllocations.Include(s => s.Employee).FirstOrDefaultAsync(s => s.ProjectId == Convert.ToInt32(@event.Project) && s.Employee.UserId == Convert.ToInt32(@event.Employee));
                if (Allocation != null)
                    Allocation.TotalNumHours = Allocation.TotalNumHours - subtract;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventExists(@event.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        public static int CalculateDurationInHours(Event evt)
        {
            if (evt.Start.HasValue && evt.End.HasValue)
            {
                TimeSpan duration = evt.End.Value - evt.Start.Value;
                return (int)Math.Ceiling(duration.TotalHours);
            }

            return 0; // Or any default value you want
        }


        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();

            }
            int subtract = CalculateDurationInHours(@event);

            var Allocation = await _context.ProjectAllocations.FirstOrDefaultAsync(s => s.ProjectId == Convert.ToInt32(@event.Project) && s.EmployeeId == Convert.ToInt32(@event.Employee));
            if (Allocation != null)
                Allocation.TotalNumHours = Allocation.TotalNumHours + subtract;

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
    public class timesheetEntry
    {
        public int id { get; set; }
        public int month { get; set; }
    }

    public class TimesheetVM
    {
        public int timesheetID { get; set; }
        public int EmpId { get; set; }
        public string? fullname { get; set; }
        public int year { get; set; }
        public string? month { get; set; }

        public int monthnum { get; set; }

        public string Submitteddate { get; set; }

        public string Projects { get; set; }

        public int TotalHours { get; set; }

        public string? status { get; set; }

    }
    public class EmployeeProjectHours
    {
        public int? ProjectID { get; set; }

        public string ProjectName { get; set; }

        public int? Hours { get; set; }
    }
    public class EmployeeReportVM
    {
        public DateTime? start { get; set; }

        public DateTime? end { get; set; }
        public string ProjectName { get; set; }
        public int TotalHours { get; set; }

    }
    public class SubmitTimesheetUpdate
    {
        public int timesheetId { get; set; }
        public int statusId
        {
            get; set;
        }
        public class ClaimVM
        {
            public string FullName { get; set; }

            public string projectName { get; set; }

            public int claimItemID { get; set; }
            public string claimItem { get; set; }
        }
    }
}



