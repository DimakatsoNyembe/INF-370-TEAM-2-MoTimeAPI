
using MoTimeAPI.Models;
using System.Threading.Tasks;

namespace MoTimeAPI.Repos
{
    public interface IRepository
    {

        void Add<M>(M entity) where M : class;
        void Delete<M>(M entity) where M : class;
        Task<bool> SaveChangesAsync();
        //// region
        Task<BranchRegion[]> GetAllRegionAsync();
        Task<BranchRegion> GetRegionAsync(int Id);

        //employee type
        Task<EmployeeType[]> GetAllEmployeeTypeAsync();
        Task<EmployeeType> GetEmployeeTypeAsync(int etypeId);


        //////calendar 
        Task<Calendar[]> GetAllCalendarAsync();
        Task<Calendar> GetCalendarAsync(int Id);

        ////employee
        Task<Employee[]> GetAllEmployeeAsync();
        Task<Employee> GetEmployeeAsync(int Id);


        //employee status
        Task<EmployeeStatus[]> GetAllEmployeeStatusAsync();
        Task<EmployeeStatus> GetEmployeeStatusAsync(int Id);



        ////manager type
        Task<ManagerType[]> GetAllManagerTypeAsync();
        Task<ManagerType> GetManagerTypeAsync(int Id);



        //// division
        Task<Division[]> GetAllDivisionAsync();
        Task<Division> GetDivisionAsync(int Id);


        //Get all help type
        Task<HelpType[]> GettAllHelpTypeAsync();

        //get help type by id
        Task<HelpType> GetHelpTypeAsync(int id);

        //get leave type by id
        Task<LeaveType> GetLeaveTypeAsync(int id);

        //leave types
        Task<LeaveType[]> GettAllLeaveTypeAsync();

        // get timesheet status by id 
        Task<TimesheetStatus> GetTimeSheetStatusAsync(int id);
        //get timesheet status
        Task<TimesheetStatus[]> GettAllTimeSheetStatusAsync();

        //task status
        Task<Models.TaskStatus[]> GettAllTaskStatusAsync();

        //task
        Task<Models.TaskStatus> GetTaskStatusAsync(int id);

        //PRIORITY
        Task<TaskPriority[]> GettAllTaskPriorityAsync();
        //Priority
        Task<TaskPriority> GetTaskPriorityAsync(int id);

        Task<TaskType[]> GettAllTaskTypeAsync();
        Task<TaskType> GetTaskTypeAsync(int id);
        Task<Vat[]> GettAllVatAsync();



        //HELP 
        Task<Help> GetHelpByIdAsync(int id);
        Task<Help[]> GetAllHelpAsync();

        //Leave 
        Task<Leave[]> GetAllLeaveAsync();
        Task<Leave> GetLeaveByIdAsync(int id);



        //claim
        Task<ClaimCapture> GetClaimCaptureByIdAsync(int id);
        Task<ClaimCapture[]> GetAllClaimCaptureAsync();

        //Alert Type
        Task<AlertType[]> GetAllAlertTypeAsync();
        //Alert type
        Task<AlertType> GetAlertTypeAsync(int id);


        //client
        Task<Client[]> GetAllClientAsync();
        Task<Client> GetClientAsync(int Id);

        //Alerts
        Task<Alert[]> GetAllAlertAsync();
        //Alerts
        Task<Alert> GetAlertByIdAsync(int id);

        //Resource Type
        Task<ResourceType[]> GetAllResourceTypeAsync();
        //Resource type
        Task<ResourceType> GetResourceTypeAsync(int id);

        // resource
        Task<Resource[]> GetAllResourceAsync();
        Task<Resource> GetResourceAsync(int Id);

        //Resource Level
        Task<ResourceLevel[]> GetAllResourceLevelAsync();
        //Resource Level
        Task<ResourceLevel> GetResourceLevelAsync(int id);

        //Title
        Task<Title[]> GetAllTitlesAsync();
        //Title
        Task<Title> GetTitlesAsync(int id);

        //Password
        Task<Password[]> GetAllPasswordsAsync();
        //Password
        Task<Password> GetPasswordAsync(int id);

        ////User
        Task<User[]> GetAllUsersAsync();

        ////User
        Task<User> GetUserAsync(int id);

        //Timesheet
        Task<Timesheet[]> GetAllTimesheetAsync();

        //Timesheet
        Task<Timesheet> GetTimesheetByIdAsync(int id);

        //Timecard
        Task<Timecard[]> GetAllTimecardAsync();

        //Timecard
        Task<Timecard> GetTimecardByIdAsync(int id);

        //project
        Task<Project[]> GetAllProjectAsync();
        Task<Project> GetProjectAsync(int Id);



        //System Admin
        Task<SystemAdministrator[]> GetAllSystemAdministratorAsync();
        Task<SystemAdministrator> GetSystemAdministratorByIdAsync(int id);

        //Claim Type
        Task<ClaimType[]> GetAllClaimTypeAsync();
        Task<ClaimType> GetClaimTypeAsync(int id);

        //Claim Item
        Task<ClaimItem[]> GetAllClaimItemAsync();
        Task<ClaimItem> GetClaimItemAsync(int id);

        //Project Status
        Task<ProjectStatus[]> GetAllProjectStatusAsync();
        Task<ProjectStatus> GetProjectStatusAsync(int id);
        //User roles
        Task<UserRole[]> GetAllUserRoleAsync();
        Task<UserRole> GetUserRoleByIdAsync(int id);

        //Task
        Task<Models.Task[]> GetAllTasksAsync();
        Task<Models.Task> GetTaskByIdAsync(int id);

        //// project allocation
        Task<ProjectAllocation[]> GetAllProjectAllocationAsync();
        Task<ProjectAllocation> GetProjectAllocationAsync(int Id);


        //Project Request
        Task<ProjectRequest[]> GetAllProjectRequestAsync();
        Task<ProjectRequest> GetProjectRequestByIdAsync(int id);

        //Project Request status
        Task<ProjectRequestStatus[]> GetAllProjectRequestStatusesAsync();
        Task<ProjectRequestStatus> GetProjectRequestStatusAsync(int id);

        //Employee Resource
        Task<EmployeeResource[]> GetAllEmployeeResourceAsync();
        Task<EmployeeResource> GetEmployeeResourceByIdAsync(int id);

        Task<ItDepartment[]> GetITDepartment();

        //max
        Task<Event[]> GetAllEventReportAsync();
        Task<Event> GetEventReportAsync(int Id);
        //alert

        //max
        Task<MaximumHour[]> GetAllMaximumHourAsync();
        Task<MaximumHour> GetMaximumHourAsync(int Id);


    }
}





