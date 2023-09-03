using Microsoft.EntityFrameworkCore;
using MoTimeAPI.Models;

using MoTimeAPI.Repos;

namespace MoTimeAPI.Repos
{
    public class Repository : IRepository
    {
        private readonly MoTimeDatabaseContext _dbContext;

        public Repository(MoTimeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        //ADD
        public void Add<M>(M entity) where M : class
        {
            _dbContext.Add(entity);
        }
        //DELETE
        public void Delete<M>(M entity) where M : class
        {
            _dbContext.Remove(entity);
        }
        //SAVECHANGES
        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        //GET ALL HELP TYPE
        public async Task<HelpType[]> GettAllHelpTypeAsync()
        {
            IQueryable<HelpType> query = _dbContext.HelpTypes;
            return await query.ToArrayAsync();
        }
        //GET HELP TYPE BY ID
        public async Task<HelpType> GetHelpTypeAsync(int id)
        {
            IQueryable<HelpType> query = _dbContext.HelpTypes.Where(helpT => helpT.HelpTypeId == id);
            return await query.FirstOrDefaultAsync();
        }
        // leave type
        public async Task<LeaveType[]> GettAllLeaveTypeAsync()
        {
            IQueryable<LeaveType> query = _dbContext.LeaveTypes;
            return await query.ToArrayAsync();
        }

        //leave by id
        public async Task<LeaveType> GetLeaveTypeAsync(int id)
        {
            IQueryable<LeaveType> query = _dbContext.LeaveTypes.Where(lvT => lvT.LeaveTypeId == id);
            return await query.FirstOrDefaultAsync();
        }

        //timesheet status
        public async Task<TimesheetStatus> GetTimeSheetStatusAsync(int id)
        {
            IQueryable<TimesheetStatus> query = _dbContext.TimesheetStatuses.Where(TSS => TSS.TimesheetStatusId == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TimesheetStatus[]> GettAllTimeSheetStatusAsync()
        {
            IQueryable<TimesheetStatus> query = _dbContext.TimesheetStatuses;
            return await query.ToArrayAsync();
        }

        public async Task<Models.TaskStatus> GetTaskStatusAsync(int id)
        {
            IQueryable<Models.TaskStatus> query = _dbContext.TaskStatuses.Where(TS => TS.TaskStatusId == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Models.TaskStatus[]> GettAllTaskStatusAsync()
        {
            IQueryable<Models.TaskStatus> query = _dbContext.TaskStatuses;
            return await query.ToArrayAsync();
        }
        //Task priority
        public async Task<TaskPriority> GetTaskPriorityAsync(int id)
        {
            IQueryable<TaskPriority> query = _dbContext.TaskPriorities.Where(Tp => Tp.PriorityId == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TaskPriority[]> GettAllTaskPriorityAsync()
        {
            IQueryable<TaskPriority> query = _dbContext.TaskPriorities;
            return await query.ToArrayAsync();
        }

        // Task Type
        public async Task<TaskType[]> GettAllTaskTypeAsync()
        {
            IQueryable<TaskType> query = _dbContext.TaskTypes;
            return await query.ToArrayAsync();
        }

        //Task by id
        public async Task<TaskType> GetTaskTypeAsync(int id)
        {
            IQueryable<TaskType> query = _dbContext.TaskTypes.Where(TT => TT.TaskTypeId == id);
            return await query.FirstOrDefaultAsync();
        }
        // VAT 
        public async Task<Vat[]> GettAllVatAsync()
        {
            IQueryable<Vat> query = _dbContext.Vats;
            return await query.ToArrayAsync();
        }
        //GET ALL MANAGER TYPES

        // manager
        public async Task<ManagerType> GetManagerTypeAsync(int Id)
        {
            IQueryable<ManagerType> query = _dbContext.ManagerTypes.Where(c => c.ManagerTypeId == Id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<ManagerType[]> GetAllManagerTypeAsync()
        {
            IQueryable<ManagerType> query = _dbContext.ManagerTypes;
            return await query.ToArrayAsync();
        }

        // GET ALL HELP 
        public async Task<Help[]> GetAllHelpAsync()
        {
            IQueryable<Help> query = _dbContext.Helps.Include(H => H.HelpType);

            return await query.ToArrayAsync();
        }
        // GET HELP BY ID
        public async Task<Help> GetHelpByIdAsync(int id)
        {
            IQueryable<Help> query = _dbContext.Helps
                .Include(h => h.HelpType)
                .Where(h => h.HelpId == id);

            return await query.FirstOrDefaultAsync();
        }

        // GET ALL LEAVE
        public async Task<Leave[]> GetAllLeaveAsync()
        {
            IQueryable<Leave> query = _dbContext.Leaves.Include(l => l.Employee).Include(l => l.LeaveType);

            return await query.ToArrayAsync();
        }
        // GET LEAVE BY ID 
        public async Task<Leave> GetLeaveByIdAsync(int id)
        {
            IQueryable<Leave> query = _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Where(l => l.LeaveId == id);

            return await query.FirstOrDefaultAsync();
        }
        //GET ALL EMPLOYEES
        public async Task<Employee[]> GetAllEmployeeAsync()
        {
            IQueryable<Employee> query = _dbContext.Employees.Include(p => p.User)
                                                                .Include(p => p.Division)
                                                                .Include(p => p.Region)
                                                                .Include(p => p.Resource)
                                                                .Include(p => p.EmployeeType)
                                                                .Include(p => p.EmployeeStatus);

            return await query.ToArrayAsync();
        }
        public async Task<Employee> GetEmployeeAsync(int Id)
        {
            IQueryable<Employee> query = _dbContext.Employees.Where(c => c.EmployeeId == Id);

            return await query.FirstOrDefaultAsync();
        }



        //GET CLAIM
        public async Task<ClaimCapture[]> GetAllClaimCaptureAsync()
        {
            IQueryable<ClaimCapture> query = _dbContext.ClaimCaptures.Include(cl => cl.ClaimId);

            return await query.ToArrayAsync();
        }

        public async Task<ClaimCapture> GetClaimCaptureByIdAsync(int id)
        {
            IQueryable<ClaimCapture> query = _dbContext.ClaimCaptures
                .Where(cl => cl.ClaimId == id);

            return await query.FirstOrDefaultAsync();
        }
        //GET ALL ALERT TYPE
        public async Task<AlertType[]> GetAllAlertTypeAsync()
        {
            IQueryable<AlertType> query = _dbContext.AlertTypes;
            return await query.ToArrayAsync();
        }
        //GET ALERT TYPE BY ID
        public async Task<AlertType> GetAlertTypeAsync(int id)
        {
            IQueryable<AlertType> query = _dbContext.AlertTypes.Where(alertT => alertT.AlertTypeId == id);
            return await query.FirstOrDefaultAsync();
        }
        // client
        public async Task<Client> GetClientAsync(int Id)
        {
            IQueryable<Client> query = _dbContext.Clients.Where(c => c.ClientId == Id);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<Client[]> GetAllClientAsync()
        {
            IQueryable<Client> query = _dbContext.Clients;
            return await query.ToArrayAsync();
        }

        //GET ALL ALERT
        public async Task<Alert[]> GetAllAlertAsync()
        {
            IQueryable<Alert> query = _dbContext.Alerts;
            return await query.ToArrayAsync();
        }
        //GET ALERT BY ID
        public async Task<Alert> GetAlertByIdAsync(int id)
        {
            IQueryable<Alert> query = _dbContext.Alerts.Where(alertT => alertT.AlertId == id);
            return await query.FirstOrDefaultAsync();
        }
        //GET ALL RESOURCE TYPES
        public async Task<ResourceType[]> GetAllResourceTypeAsync()
        {
            IQueryable<ResourceType> query = _dbContext.ResourceTypes;
            return await query.ToArrayAsync();
        }
        //GET RESOURCE TYPE BY ID
        public async Task<ResourceType> GetResourceTypeAsync(int id)
        {
            IQueryable<ResourceType> query = _dbContext.ResourceTypes.Where(resourceT => resourceT.ResourceTypeId == id);
            return await query.FirstOrDefaultAsync();
        }
        //GET ALL RESOURCE

        // resource 
        public async Task<Resource[]> GetAllResourceAsync()
        {
            IQueryable<Resource> query = _dbContext.Resources.Include(p => p.ResourceType);
            return await query.ToArrayAsync();
        }
        public async Task<Resource> GetResourceAsync(int Id)
        {
            IQueryable<Resource> query = _dbContext.Resources.Where(c => c.ResourceId == Id);
            return await query.FirstOrDefaultAsync();
        }

        //GET ALL RESOURCE LEVEL
        public async Task<ResourceLevel[]> GetAllResourceLevelAsync()
        {
            IQueryable<ResourceLevel> query = _dbContext.ResourceLevels;
            return await query.ToArrayAsync();
        }
        //GET RESOURCE LEVEL BY ID
        public async Task<ResourceLevel> GetResourceLevelAsync(int id)
        {
            IQueryable<ResourceLevel> query = _dbContext.ResourceLevels.Where(resourceL => resourceL.ResourceLevelId == id);
            return await query.FirstOrDefaultAsync();
        }
        //GET ALL TITLES
        public async Task<Title[]> GetAllTitlesAsync()
        {
            IQueryable<Title> query = _dbContext.Titles;
            return await query.ToArrayAsync();
        }
        //GET TITLE BY ID
        public async Task<Title> GetTitlesAsync(int id)
        {
            IQueryable<Title> query = _dbContext.Titles.Where(tt => tt.TitleId == id);
            return await query.FirstOrDefaultAsync();
        }
        //GET ALL PASSWORDS
        public async Task<Password[]> GetAllPasswordsAsync()
        {
            IQueryable<Password> query = _dbContext.Passwords;
            return await query.ToArrayAsync();
        }
        //GET PASSWORD BY ID
        public async Task<Password> GetPasswordAsync(int id)
        {
            IQueryable<Password> query = _dbContext.Passwords.Where(tt => tt.PasswordId == id);
            return await query.FirstOrDefaultAsync();
        }
        //GET ALL USERS
        public async Task<User[]> GetAllUsersAsync()
        {
            IQueryable<User> query = _dbContext.Users.Include(r => r.UserRoleId).Include(t => t.TitleId);
            return await query.ToArrayAsync();
        }
        //GET USER BY ID
        public async Task<User> GetUserAsync(int id)
        {
            IQueryable<User> query = _dbContext.Users
                .Include(r => r.UserRole)
                .Include(t => t.Title)
                .Where(tt => tt.UserId == id);
            return await query.FirstOrDefaultAsync();
        }
        //GET TIMESHEET RECORDS
        public async Task<Timesheet[]> GetAllTimesheetAsync()
        {
            IQueryable<Timesheet> query = _dbContext.Timesheets
                .Include(p => p.TimesheetStatus)
                .Include(p => p.Employee)
                .Include(p => p.Project)
                .Include(p => p.Admin);

            return await query.ToArrayAsync();
        }

        public async Task<Timesheet> GetTimesheetByIdAsync(int id)
        {
            IQueryable<Timesheet> query = _dbContext.Timesheets.Where(ts => ts.TimesheetId == id);

            return await query.FirstOrDefaultAsync();
        }
        //TIMECARD
        public async Task<Timecard[]> GetAllTimecardAsync()
        {
            IQueryable<Timecard> query = _dbContext.Timecards;

            return await query.ToArrayAsync();
        }
        public async Task<Timecard> GetTimecardByIdAsync(int id)
        {
            IQueryable<Timecard> query = _dbContext.Timecards
                .Include(p => p.ProjectId)
                .Include(emp => emp.EmployeeId)
                .Include(ts => ts.TimesheetId)
                .Include(Ad => Ad.AdminId)
                .Where(tc => tc.TimecardId == id);

            return await query.FirstOrDefaultAsync();
        }
        // project
        public async Task<Project> GetProjectAsync(int Id)
        {
            IQueryable<Project> query = _dbContext.Projects.Where(c => c.ProjectId == Id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Project[]> GetAllProjectAsync()
        {
            IQueryable<Project> query = _dbContext.Projects.Include(p => p.Admin)
                                                              .Include(p => p.Client)
                                                              .Include(p => p.ProjectStatus);
            return await query.ToArrayAsync();
        }

        // project status
        public async Task<ProjectStatus> GetProjectStatusAsync(int Id)
        {
            IQueryable<ProjectStatus> query = _dbContext.ProjectStatuses.Where(c => c.ProjectStatusId == Id);
            return await query.FirstOrDefaultAsync();
        }



        public async Task<ProjectStatus[]> GetAllProjectStatusAsync()
        {
            IQueryable<ProjectStatus> query = _dbContext.ProjectStatuses;
            return await query.ToArrayAsync();
        }



        //GET SYSTEM ADMIN RECORDS
        public async Task<SystemAdministrator[]> GetAllSystemAdministratorAsync()
        {
            IQueryable<SystemAdministrator> query = _dbContext.SystemAdministrators;

            return await query.ToArrayAsync();
        }

        public async Task<SystemAdministrator> GetSystemAdministratorByIdAsync(int id)
        {
            IQueryable<SystemAdministrator> query = _dbContext.SystemAdministrators.Where(p => p.AdminId == id);

            return await query.FirstOrDefaultAsync();
        }
        //GET ALL CLAIM TYPE
        public async Task<ClaimType[]> GetAllClaimTypeAsync()
        {
            IQueryable<ClaimType> query = _dbContext.ClaimTypes;
            return await query.ToArrayAsync();
        }
        //GET CLAIM TYPE BY ID
        public async Task<ClaimType> GetClaimTypeAsync(int id)
        {
            IQueryable<ClaimType> query = _dbContext.ClaimTypes.Where(claimT => claimT.ClaimTypeId == id);
            return await query.FirstOrDefaultAsync();
        }
        // claim item
        public async Task<ClaimItem> GetClaimItemAsync(int Id)
        {
            IQueryable<ClaimItem> query = _dbContext.ClaimItems.Where(c => c.ClaimItemId == Id);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<ClaimItem[]> GetAllClaimItemAsync()
        {
            IQueryable<ClaimItem> query = _dbContext.ClaimItems.Include(p => p.ClaimType);

            return await query.ToArrayAsync();
        }


        //GET ALL USER ROLES
        public async Task<UserRole[]> GetAllUserRoleAsync()
        {
            IQueryable<UserRole> query = _dbContext.UserRoles;
            return await query.ToArrayAsync();
        }
        //GET PROJECT STATUS BY ID
        public async Task<UserRole> GetUserRoleByIdAsync(int id)
        {
            IQueryable<UserRole> query = _dbContext.UserRoles.Where(ur => ur.UserRoleId == id);
            return await query.FirstOrDefaultAsync();
        }
        //GET ALL EMPLOYEE TYPE
        public async Task<EmployeeType[]> GetAllEmployeeTypeAsync()
        {
            IQueryable<EmployeeType> query = _dbContext.EmployeeTypes;
            return await query.ToArrayAsync();
        }
        public async Task<EmployeeType> GetEmployeeTypeAsync(int etypeId)
        {
            IQueryable<EmployeeType> query = _dbContext.EmployeeTypes.Where(c
           => c.EmployeeTypeId == etypeId);
            return await query.FirstOrDefaultAsync();
        }

        //TASK
        public async Task<Models.Task[]> GetAllTasksAsync()
        {
            IQueryable<Models.Task> query = _dbContext.Tasks;

            return await query.ToArrayAsync();
        }
        public async Task<Models.Task> GetTaskByIdAsync(int id)
        {
            IQueryable<Models.Task> query = _dbContext.Tasks
                .Where(t => t.TaskId == id);

            return await query.FirstOrDefaultAsync();
        }
        // project allocation

        public async Task<ProjectAllocation[]> GetAllProjectAllocationAsync()
        {
            IQueryable<ProjectAllocation> query = _dbContext.ProjectAllocations.Include(p => p.Project)
                                                                                  .Include(p => p.Employee.User)
                                                                                  .Include(p => p.ClaimItem);
            return await query.ToArrayAsync();
        }
        public async Task<ProjectAllocation> GetProjectAllocationAsync(int Id)
        {
            IQueryable<ProjectAllocation> query = _dbContext.ProjectAllocations.Where(c => c.ProjectAllocationId == Id);

            return await query.FirstOrDefaultAsync();
        }


        //Get all project requests
        public async Task<ProjectRequest[]> GetAllProjectRequestAsync()
        {
            IQueryable<ProjectRequest> query = _dbContext.ProjectRequests.Include(P => P.Employee)
                                                                               .Include(P => P.ProjectRequestStatus);
            return await query.ToArrayAsync();
        }

        //Get project request by ID
        public async Task<ProjectRequest> GetProjectRequestByIdAsync(int id)
        {
            IQueryable<ProjectRequest> query = _dbContext.ProjectRequests
                .Include(p => p.EmployeeId)
                .Include(p => p.ProjectRequestStatusId)
                .Where(p => p.ProjectRequestId == id);

            return await query.FirstOrDefaultAsync();
        }
        //Project Request Status
        public async Task<ProjectRequestStatus> GetProjectRequestStatusAsync(int id)
        {
            IQueryable<ProjectRequestStatus> query = _dbContext.ProjectRequestStatuses.Where(TSS => TSS.ProjectRequestStatusId == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<ProjectRequestStatus[]> GetAllProjectRequestStatusesAsync()
        {
            IQueryable<ProjectRequestStatus> query = _dbContext.ProjectRequestStatuses;
            return await query.ToArrayAsync();
        }
        //Get employee resource
        public async Task<EmployeeResource[]> GetAllEmployeeResourceAsync()
        {
            IQueryable<EmployeeResource> query = _dbContext.EmployeeResources;
            return await query.ToArrayAsync();
        }

        //Get employee resource by Id
        public async Task<EmployeeResource> GetEmployeeResourceByIdAsync(int id)
        {
            IQueryable<EmployeeResource> query = _dbContext.EmployeeResources
                .Include(e => e.ResourceId)
                .Include(e => e.EmployeeId)
                .Where(e => e.ResourceId == id || e.EmployeeId == id);

            return await query.FirstOrDefaultAsync();
        }


        //Get employee status 
        // employee status 
        public async Task<EmployeeStatus[]> GetAllEmployeeStatusAsync()
        {
            IQueryable<EmployeeStatus> query = _dbContext.EmployeeStatuses;
            return await query.ToArrayAsync();
        }
        public async Task<EmployeeStatus> GetEmployeeStatusAsync(int Id)
        {
            IQueryable<EmployeeStatus> query = _dbContext.EmployeeStatuses.Where(c => c.EmployeeStatusId == Id);
            return await query.FirstOrDefaultAsync();
        }

        //Get all divisions
        // division
        public async Task<Division[]> GetAllDivisionAsync()
        {
            IQueryable<Division> query = _dbContext.Divisions;
            return await query.ToArrayAsync();
        }

        public async Task<Division> GetDivisionAsync(int Id)
        {
            IQueryable<Division> query = _dbContext.Divisions.Where(c => c.DivisionId == Id);
            return await query.FirstOrDefaultAsync();
        }

        // branch region

        public async Task<BranchRegion[]> GetAllRegionAsync()
        {
            IQueryable<BranchRegion> query = _dbContext.BranchRegions;
            return await query.ToArrayAsync();
        }

        public async Task<BranchRegion> GetRegionAsync(int Id)
        {
            IQueryable<BranchRegion> query = _dbContext.BranchRegions.Where(c => c.RegionId == Id);
            return await query.FirstOrDefaultAsync();
        }

        //Get all Calendars
        public async Task<Calendar[]> GetAllCalendarsAsync()
        {
            IQueryable<Calendar> query = _dbContext.Calendars;
            return await query.ToArrayAsync();
        }
        //Get calendar 
        //// calendar 
        public async Task<Calendar[]> GetAllCalendarAsync()
        {
            IQueryable<Calendar> query = _dbContext.Calendars;
            return await query.ToArrayAsync();
        }
        public async Task<Calendar> GetCalendarAsync(int Id)
        {
            IQueryable<Calendar> query = _dbContext.Calendars.Where(c => c.CalendarId == Id);
            return await query.FirstOrDefaultAsync();
        }

        //Get Calendar item by id
        public async Task<CalendarItem> GetCalendarItemByIdAsync(int itemId)
        {
            IQueryable<CalendarItem> query = _dbContext.CalendarItems
                .Include(c => c.CalendarId)
                .Include(t => t.TimeCardId)
                .Include(ta => ta.TaskId)
                .Where(c => c.CalendarId == itemId || c.TaskId == itemId || c.TimeCardId == itemId);
            return await query.FirstOrDefaultAsync();
        }

        ///IT Department 
        public async Task<ItDepartment[]> GetITDepartment()
        {
            IQueryable<ItDepartment> query = _dbContext.ItDepartments;
            return await query.ToArrayAsync();
        }

        //// events
        public async Task<Event[]> GetAllEventReportAsync()
        {
            IQueryable<Event> query = _dbContext.Events;
            return await query.ToArrayAsync();
        }
        public async Task<Event> GetEventReportAsync(int Id)
        {
            IQueryable<Event> query = _dbContext.Events.Where(c => c.Id == Id);
            return await query.FirstOrDefaultAsync();
        }

        //// max hours
        public async Task<MaximumHour[]> GetAllMaximumHourAsync()
        {
            IQueryable<MaximumHour> query = _dbContext.MaximumHours;
            return await query.ToArrayAsync();
        }
        public async Task<MaximumHour> GetMaximumHourAsync(int Id)
        {
            IQueryable<MaximumHour> query = _dbContext.MaximumHours.Where(c => c.MaxHoursId == Id);
            return await query.FirstOrDefaultAsync();
        }

    }
}




