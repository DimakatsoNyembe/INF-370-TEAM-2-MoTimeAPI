using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MoTimeAPI.Models;

public partial class MoTimeDatabaseContext : DbContext
{
    public MoTimeDatabaseContext()
    {
    }

    public MoTimeDatabaseContext(DbContextOptions<MoTimeDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<AlertType> AlertTypes { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<AuditReason> AuditReasons { get; set; }

    public virtual DbSet<BranchRegion> BranchRegions { get; set; }

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<CalendarItem> CalendarItems { get; set; }

    public virtual DbSet<ClaimCapture> ClaimCaptures { get; set; }

    public virtual DbSet<ClaimItem> ClaimItems { get; set; }

    public virtual DbSet<ClaimType> ClaimTypes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeResource> EmployeeResources { get; set; }

    public virtual DbSet<EmployeeStatus> EmployeeStatuses { get; set; }

    public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Help> Helps { get; set; }

    public virtual DbSet<HelpType> HelpTypes { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<ItDepartment> ItDepartments { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<ManagerType> ManagerTypes { get; set; }

    public virtual DbSet<MaximumHour> MaximumHours { get; set; }

    public virtual DbSet<Password> Passwords { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectAllocation> ProjectAllocations { get; set; }

    public virtual DbSet<ProjectClaimItem> ProjectClaimItems { get; set; }

    public virtual DbSet<ProjectRequest> ProjectRequests { get; set; }

    public virtual DbSet<ProjectRequestStatus> ProjectRequestStatuses { get; set; }

    public virtual DbSet<ProjectStatus> ProjectStatuses { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceLevel> ResourceLevels { get; set; }

    public virtual DbSet<ResourceType> ResourceTypes { get; set; }

    public virtual DbSet<SystemAdministrator> SystemAdministrators { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskPriority> TaskPriorities { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TaskType> TaskTypes { get; set; }

    public virtual DbSet<Timecard> Timecards { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<TimesheetStatus> TimesheetStatuses { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<TokenApi> TokenApis { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserUserRole> UserUserRoles { get; set; }

    public virtual DbSet<Vat> Vats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DIMAKATSO\\SQLEXPRESS01;Database=MoTime_Database;Trusted_Connection=true;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.AlertId).HasName("PK__Alert__DA635237328E04FF");

            entity.ToTable("Alert");

            entity.Property(e => e.AlertId).HasColumnName("Alert_ID");
            entity.Property(e => e.AlertDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Alert_Description");
            entity.Property(e => e.AlertTypeId).HasColumnName("Alert_Type_ID");
            entity.Property(e => e.CalendarId).HasColumnName("Calendar_ID");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.TimeCardId).HasColumnName("TimeCard_ID");
            entity.Property(e => e.UserId).HasColumnName("_User_ID");

            entity.HasOne(d => d.AlertType).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.AlertTypeId)
                .HasConstraintName("FK__Alert__Alert_Typ__3864608B");

            entity.HasOne(d => d.Calendar).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.CalendarId)
                .HasConstraintName("FK__Alert__Calendar___37703C52");

            entity.HasOne(d => d.TimeCard).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.TimeCardId)
                .HasConstraintName("FK__Alert__TimeCard___367C1819");

            entity.HasOne(d => d.User).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Alert___User_ID__3587F3E0");
        });

        modelBuilder.Entity<AlertType>(entity =>
        {
            entity.HasKey(e => e.AlertTypeId).HasName("PK__Alert_Ty__180227E8DB875B9B");

            entity.ToTable("Alert_Type");

            entity.Property(e => e.AlertTypeId).HasColumnName("Alert_Type_ID");
            entity.Property(e => e.AlertTypeDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Alert_Type_Description");
            entity.Property(e => e.AlertTypeName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Alert_Type_Name");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__Audit_Lo__EDBEC7590149E102");

            entity.ToTable("Audit_Log");

            entity.Property(e => e.AuditId).HasColumnName("Audit_ID");
            entity.Property(e => e.ActionPerformed)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Action_Performed");
            entity.Property(e => e.AuditDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Audit_Description");
            entity.Property(e => e.AuditObject)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Audit_Object");
            entity.Property(e => e.AuditReasonId).HasColumnName("Audit_Reason_ID");
            entity.Property(e => e.AuditTimeStamp).HasColumnName("Audit_TimeStamp");
            entity.Property(e => e.DeviceUsed)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Device_Used");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP_Address");
            entity.Property(e => e.ResultFromAction)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Result_From_Action");
            entity.Property(e => e.UserId).HasColumnName("_User_ID");

            entity.HasOne(d => d.AuditReason).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.AuditReasonId)
                .HasConstraintName("FK__Audit_Log__Audit__797309D9");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Audit_Log___User__787EE5A0");
        });

        modelBuilder.Entity<AuditReason>(entity =>
        {
            entity.HasKey(e => e.AuditReasonId).HasName("PK__Audit_Re__CBB101BE606EAB52");

            entity.ToTable("Audit_Reason");

            entity.Property(e => e.AuditReasonId).HasColumnName("Audit_Reason_ID");
            entity.Property(e => e.ReasonDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Reason_Description");
            entity.Property(e => e.ReasonName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Reason_Name");
        });

        modelBuilder.Entity<BranchRegion>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("PK__Branch_R__A9EAD51F861334AA");

            entity.ToTable("Branch_Region");

            entity.Property(e => e.RegionId).HasColumnName("Region_ID");
            entity.Property(e => e.RegionName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Region_Name");
        });

        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.CalendarId).HasName("PK__Calendar__51850A4766E852BE");

            entity.ToTable("Calendar");

            entity.Property(e => e.CalendarId).HasColumnName("Calendar_ID");
            entity.Property(e => e.CalendarItemDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Calendar_Item_Description");
            entity.Property(e => e.CalendarItemName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Calendar_Item_Name");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("_Date");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("End_Time");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("_Location");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("Start_Time");
            entity.Property(e => e.TimecardId).HasColumnName("Timecard_ID");

            entity.HasOne(d => d.Timecard).WithMany(p => p.Calendars)
                .HasForeignKey(d => d.TimecardId)
                .HasConstraintName("FK__Calendar__Timeca__32AB8735");
        });

        modelBuilder.Entity<CalendarItem>(entity =>
        {
            entity.HasKey(e => e.CalendarItemId).HasName("PK__Calendar__E2D79223819D269D");

            entity.ToTable("Calendar_Item");

            entity.Property(e => e.CalendarItemId).HasColumnName("Calendar_Item_ID");
            entity.Property(e => e.CalendarId).HasColumnName("Calendar_ID");
            entity.Property(e => e.TaskId).HasColumnName("Task_ID");
            entity.Property(e => e.TimeCardId).HasColumnName("TimeCard_ID");

            entity.HasOne(d => d.Calendar).WithMany(p => p.CalendarItems)
                .HasForeignKey(d => d.CalendarId)
                .HasConstraintName("FK__Calendar___Calen__3C34F16F");

            entity.HasOne(d => d.Task).WithMany(p => p.CalendarItems)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__Calendar___Task___3D2915A8");

            entity.HasOne(d => d.TimeCard).WithMany(p => p.CalendarItems)
                .HasForeignKey(d => d.TimeCardId)
                .HasConstraintName("FK__Calendar___TimeC__3B40CD36");
        });

        modelBuilder.Entity<ClaimCapture>(entity =>
        {
            entity.HasKey(e => e.ClaimId).HasName("PK__Claim_Ca__811C4A4D0F402449");

            entity.ToTable("Claim_Capture");

            entity.Property(e => e.ClaimId).HasColumnName("Claim_ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProjectAllocationId).HasColumnName("Project_Allocation_ID");
            entity.Property(e => e.UploadProof).HasColumnName("Upload_Proof");

            entity.HasOne(d => d.ProjectAllocation).WithMany(p => p.ClaimCaptures)
                .HasForeignKey(d => d.ProjectAllocationId)
                .HasConstraintName("FK__Claim_Cap__Proje__160F4887");
        });

        modelBuilder.Entity<ClaimItem>(entity =>
        {
            entity.HasKey(e => e.ClaimItemId).HasName("PK__Claim_It__816DC01823EDE651");

            entity.ToTable("Claim_Item");

            entity.Property(e => e.ClaimItemId).HasColumnName("Claim_Item_ID");
            entity.Property(e => e.ClaimItemName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Claim_Item_Name");
            entity.Property(e => e.ClaimTypeId).HasColumnName("Claim_Type_ID");

            entity.HasOne(d => d.ClaimType).WithMany(p => p.ClaimItems)
                .HasForeignKey(d => d.ClaimTypeId)
                .HasConstraintName("FK__Claim_Ite__Claim__0E6E26BF");
        });

        modelBuilder.Entity<ClaimType>(entity =>
        {
            entity.HasKey(e => e.ClaimTypeId).HasName("PK__Claim_Ty__888CAD46F2518027");

            entity.ToTable("Claim_Type");

            entity.Property(e => e.ClaimTypeId).HasColumnName("Claim_Type_ID");
            entity.Property(e => e.ClaimTypeDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Claim_Type_Description");
            entity.Property(e => e.ClaimTypeName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Claim_Type_Name");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Client__75A5D718A9DFF05C");

            entity.ToTable("Client");

            entity.Property(e => e.ClientId).HasColumnName("Client_ID");
            entity.Property(e => e.Account)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountManager)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Manager");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode).HasColumnName("Project_Code");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("Site_Code");

            entity.HasOne(d => d.Admin).WithMany(p => p.Clients)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__Client__Admin_ID__7C4F7684");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.DivisionId).HasName("PK__Division__B8A52D8FEB8B7BB8");

            entity.ToTable("Division");

            entity.Property(e => e.DivisionId).HasColumnName("Division_ID");
            entity.Property(e => e.DivisionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Division_Name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__781134817A738D79");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.DivisionId).HasColumnName("Division_ID");
            entity.Property(e => e.EmployeeStatusId).HasColumnName("Employee_Status_ID");
            entity.Property(e => e.EmployeeTypeId).HasColumnName("Employee_Type_ID");
            entity.Property(e => e.ManagerTypeId).HasColumnName("Manager_Type_ID");
            entity.Property(e => e.RegionId).HasColumnName("Region_ID");
            entity.Property(e => e.ResourceId).HasColumnName("Resource_ID");
            entity.Property(e => e.UserId).HasColumnName("_User_ID");

            entity.HasOne(d => d.Division).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DivisionId)
                .HasConstraintName("FK__Employee__Divisi__6FE99F9F");

            entity.HasOne(d => d.EmployeeStatus).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeStatusId)
                .HasConstraintName("FK__Employee__Employ__6E01572D");

            entity.HasOne(d => d.EmployeeType).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeTypeId)
                .HasConstraintName("FK__Employee__Employ__6B24EA82");

            entity.HasOne(d => d.ManagerType).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ManagerTypeId)
                .HasConstraintName("FK__Employee__Manage__6C190EBB");

            entity.HasOne(d => d.Region).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__Employee__Region__6EF57B66");

            entity.HasOne(d => d.Resource).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ResourceId)
                .HasConstraintName("FK__Employee__Resour__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Employee___User___6D0D32F4");
        });

        modelBuilder.Entity<EmployeeResource>(entity =>
        {
            entity.HasKey(e => e.EmployeeResourceId).HasName("PK__Employee__159AC79235D28BAB");

            entity.ToTable("Employee_Resource");

            entity.Property(e => e.EmployeeResourceId).HasColumnName("Employee_Resource_ID");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.ResourceId).HasColumnName("Resource_ID");
            entity.Property(e => e.ResourceLevelId).HasColumnName("Resource_Level_ID");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeResources)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Employee___Emplo__42E1EEFE");

            entity.HasOne(d => d.Resource).WithMany(p => p.EmployeeResources)
                .HasForeignKey(d => d.ResourceId)
                .HasConstraintName("FK__Employee___Resou__41EDCAC5");

            entity.HasOne(d => d.ResourceLevel).WithMany(p => p.EmployeeResources)
                .HasForeignKey(d => d.ResourceLevelId)
                .HasConstraintName("FK__Employee___Resou__43D61337");
        });

        modelBuilder.Entity<EmployeeStatus>(entity =>
        {
            entity.HasKey(e => e.EmployeeStatusId).HasName("PK__Employee__573C006E0222DE75");

            entity.ToTable("Employee_Status");

            entity.Property(e => e.EmployeeStatusId).HasColumnName("Employee_Status_ID");
            entity.Property(e => e.EmployeeStatusDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Employee_Status_Description");
            entity.Property(e => e.EmployeeStatusName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Employee_Status_Name");
        });

        modelBuilder.Entity<EmployeeType>(entity =>
        {
            entity.HasKey(e => e.EmployeeTypeId).HasName("PK__Employee__9A0C6420597D929E");

            entity.ToTable("Employee_Type");

            entity.Property(e => e.EmployeeTypeId).HasColumnName("Employee_Type_ID");
            entity.Property(e => e.EmployeeTypeDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Employee_Type_Description");
            entity.Property(e => e.EmployeeTypeName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Employee_Type_Name");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__events__3213E83FEC65573A");

            entity.ToTable("events");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.barColor)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("barcolor");
            entity.Property(e => e.Employee)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("employee");
            entity.Property(e => e.End)
                .HasColumnType("datetime")
                .HasColumnName("end");
            entity.Property(e => e.Project)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("project");
            entity.Property(e => e.Start)
                .HasColumnType("datetime")
                .HasColumnName("start");
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("text");
        });

        modelBuilder.Entity<Help>(entity =>
        {
            entity.HasKey(e => e.HelpId).HasName("PK__Help__C6D457128B940C9C");

            entity.ToTable("Help");

            entity.Property(e => e.HelpId).HasColumnName("Help_ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.FileName)
                .HasMaxLength(350)
                .IsUnicode(false);
            entity.Property(e => e.HelpDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Help_Description");
            entity.Property(e => e.HelpName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Help_Name");
            entity.Property(e => e.HelpTypeId).HasColumnName("Help_Type_ID");

            entity.HasOne(d => d.Admin).WithMany(p => p.Helps)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__Help__Admin_ID__04E4BC85");

            entity.HasOne(d => d.HelpType).WithMany(p => p.Helps)
                .HasForeignKey(d => d.HelpTypeId)
                .HasConstraintName("FK__Help__Help_Type___03F0984C");
        });

        modelBuilder.Entity<HelpType>(entity =>
        {
            entity.HasKey(e => e.HelpTypeId).HasName("PK__Help_Typ__3241B9551E9659D1");

            entity.ToTable("Help_Type");

            entity.Property(e => e.HelpTypeId).HasColumnName("Help_Type_ID");
            entity.Property(e => e.HelpTypeDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Help_Type_Description");
            entity.Property(e => e.HelpTypeName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Help_Type_Name");
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__History__A6BABA37C4DED4B9");

            entity.ToTable("History");

            entity.Property(e => e.HistoryId).HasColumnName("History_ID");
            entity.Property(e => e.HistoryAction)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("History_Action");
            entity.Property(e => e.HistoryDate)
                .HasColumnType("date")
                .HasColumnName("History_Date");
            entity.Property(e => e.HistoryTime).HasColumnName("History_Time");
            entity.Property(e => e.Submitted).HasMaxLength(1);
            entity.Property(e => e.TimesheetId).HasColumnName("Timesheet_ID");

            entity.HasOne(d => d.Timesheet).WithMany(p => p.Histories)
                .HasForeignKey(d => d.TimesheetId)
                .HasConstraintName("FK__History__Timeshe__2FCF1A8A");
        });

        modelBuilder.Entity<ItDepartment>(entity =>
        {
            entity.HasKey(e => e.ItId).HasName("PK__IT_Depar__0B6555E8B835217C");

            entity.ToTable("IT_Department");

            entity.Property(e => e.ItId).HasColumnName("It_ID");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Email_Address");
        });

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__Leave__D54C3A6090F669E3");

            entity.ToTable("Leave");

            entity.Property(e => e.LeaveId).HasColumnName("Leave_ID");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.LeaveTypeId).HasColumnName("Leave_Type_ID");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Employee).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Leave__Employee___74AE54BC");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK__Leave__Leave_Typ__75A278F5");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.LeaveTypeId).HasName("PK__Leave_Ty__7AC7AF0DFB6C7451");

            entity.ToTable("Leave_Type");

            entity.Property(e => e.LeaveTypeId).HasColumnName("Leave_Type_ID");
            entity.Property(e => e.LeaveTypeDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Leave_Type_Description");
            entity.Property(e => e.LeaveTypeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Leave_Type_Name");
        });

        modelBuilder.Entity<ManagerType>(entity =>
        {
            entity.HasKey(e => e.ManagerTypeId).HasName("PK__Manager___F3E2311E9251CD6C");

            entity.ToTable("Manager_Type");

            entity.Property(e => e.ManagerTypeId).HasColumnName("Manager_Type_ID");
            entity.Property(e => e.EmployeeTypeId).HasColumnName("Employee_Type_ID");
            entity.Property(e => e.ManagerTypeDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Manager_Type_Description");
            entity.Property(e => e.ManagerTypeName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Manager_Type_Name");

            entity.HasOne(d => d.EmployeeType).WithMany(p => p.ManagerTypes)
                .HasForeignKey(d => d.EmployeeTypeId)
                .HasConstraintName("FK__Manager_T__Emplo__6383C8BA");
        });

        modelBuilder.Entity<MaximumHour>(entity =>
        {
            entity.HasKey(e => e.MaxHoursId).HasName("PK__Maximum___401EBDC7C7F9F489");

            entity.ToTable("Maximum_Hours");

            entity.Property(e => e.MaxHoursId).HasColumnName("MaxHours_ID");
        });

        modelBuilder.Entity<Password>(entity =>
        {
            entity.HasKey(e => e.PasswordId).HasName("PK___Passwor__850E247A7283D995");

            entity.ToTable("_Password");

            entity.Property(e => e.PasswordId).HasColumnName("Password_ID");
            entity.Property(e => e.Password1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("_Password");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Project__1CB92FE3A2BAB41B");

            entity.ToTable("Project");

            entity.Property(e => e.ProjectId).HasColumnName("Project_ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.ClientId).HasColumnName("Client_ID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Project_Name");
            entity.Property(e => e.ProjectStatusId).HasColumnName("Project_Status_ID");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Admin).WithMany(p => p.Projects)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__Project__Admin_I__07C12930");

            entity.HasOne(d => d.Client).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Project__Client___08B54D69");

            entity.HasOne(d => d.ProjectStatus).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ProjectStatusId)
                .HasConstraintName("FK__Project__Project__09A971A2");
        });

        modelBuilder.Entity<ProjectAllocation>(entity =>
        {
            entity.HasKey(e => e.ProjectAllocationId).HasName("PK__Project___FE6F552B22011176");

            entity.ToTable("Project_Allocation");

            entity.Property(e => e.ProjectAllocationId).HasColumnName("Project_Allocation_ID");
            entity.Property(e => e.BillableOverTime).HasColumnName("Billable_OverTime");
            entity.Property(e => e.ClaimItemId).HasColumnName("Claim_Item_ID");
            entity.Property(e => e.ClaimableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.ExpectedNumHours).HasColumnName("Expected_Num_Hours");
            entity.Property(e => e.ProjectId).HasColumnName("Project_ID");
            entity.Property(e => e.TotalNumHours).HasColumnName("Total_Num_Hours");

            entity.HasOne(d => d.ClaimItem).WithMany(p => p.ProjectAllocations)
                .HasForeignKey(d => d.ClaimItemId)
                .HasConstraintName("FK__Project_A__Claim__1332DBDC");

            entity.HasOne(d => d.Employee).WithMany(p => p.ProjectAllocations)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Project_A__Emplo__123EB7A3");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectAllocations)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Project_A__Proje__114A936A");
        });

        modelBuilder.Entity<ProjectClaimItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Project_Claim_Item");

            entity.Property(e => e.ClaimItemId).HasColumnName("Claim_Item_ID");
            entity.Property(e => e.ProjectAllocationId).HasColumnName("Project_Allocation_ID");

            entity.HasOne(d => d.ClaimItem).WithMany()
                .HasForeignKey(d => d.ClaimItemId)
                .HasConstraintName("FK__Project_C__Claim__2BFE89A6");

            entity.HasOne(d => d.ProjectAllocation).WithMany()
                .HasForeignKey(d => d.ProjectAllocationId)
                .HasConstraintName("FK__Project_C__Proje__2CF2ADDF");
        });

        modelBuilder.Entity<ProjectRequest>(entity =>
        {
            entity.HasKey(e => e.ProjectRequestId).HasName("PK__Project___871813F801B90A4E");

            entity.ToTable("Project_Request");

            entity.Property(e => e.ProjectRequestId).HasColumnName("Project_Request_ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.ProjectRequestDate)
                .HasColumnType("date")
                .HasColumnName("Project_Request_Date");
            entity.Property(e => e.ProjectRequestDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Project_Request_Description");
            entity.Property(e => e.ProjectRequestStatusId).HasColumnName("Project_Request_Status_ID");

            entity.HasOne(d => d.Admin).WithMany(p => p.ProjectRequests)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__Project_R__Admin__00200768");

            entity.HasOne(d => d.Employee).WithMany(p => p.ProjectRequests)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Project_R__Emplo__01142BA1");

            entity.HasOne(d => d.ProjectRequestStatus).WithMany(p => p.ProjectRequests)
                .HasForeignKey(d => d.ProjectRequestStatusId)
                .HasConstraintName("FK__Project_R__Proje__7F2BE32F");
        });

        modelBuilder.Entity<ProjectRequestStatus>(entity =>
        {
            entity.HasKey(e => e.ProjectRequestStatusId).HasName("PK__Project___4851B51981869A69");

            entity.ToTable("Project_Request_Status");

            entity.Property(e => e.ProjectRequestStatusId).HasColumnName("Project_Request_Status_ID");
            entity.Property(e => e.ProjectRequestStatusDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Project_Request_Status_Description");
            entity.Property(e => e.ProjectRequestStatusName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Project_Request_Status_Name");
        });

        modelBuilder.Entity<ProjectStatus>(entity =>
        {
            entity.HasKey(e => e.ProjectStatusId).HasName("PK__Project___54FDF821CA1B97A6");

            entity.ToTable("Project_Status");

            entity.Property(e => e.ProjectStatusId).HasColumnName("Project_Status_ID");
            entity.Property(e => e.ProjectStatusDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Project_Status_Description");
            entity.Property(e => e.ProjectStatusName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Project_Status_Name");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK___Resourc__3EB41762593ECF3C");

            entity.ToTable("_Resource");

            entity.Property(e => e.ResourceId).HasColumnName("Resource_ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.ResourceDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Resource_Description");
            entity.Property(e => e.ResourceName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Resource_Name");
            entity.Property(e => e.ResourceTypeId).HasColumnName("Resource_Type_ID");

            entity.HasOne(d => d.Admin).WithMany(p => p.Resources)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK___Resource__Admin__5535A963");

            entity.HasOne(d => d.ResourceType).WithMany(p => p.Resources)
                .HasForeignKey(d => d.ResourceTypeId)
                .HasConstraintName("FK___Resource__Resou__5441852A");
        });

        modelBuilder.Entity<ResourceLevel>(entity =>
        {
            entity.HasKey(e => e.ResourceLevelId).HasName("PK__Resource__5883348B9FEB6A87");

            entity.ToTable("Resource_Level");

            entity.Property(e => e.ResourceLevelId).HasColumnName("Resource_Level_ID");
            entity.Property(e => e.ResourceLevel1)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Resource_Level");
        });

        modelBuilder.Entity<ResourceType>(entity =>
        {
            entity.HasKey(e => e.ResourceTypeId).HasName("PK__Resource__3EA45B8E67F2ED5F");

            entity.ToTable("Resource_Type");

            entity.Property(e => e.ResourceTypeId).HasColumnName("Resource_Type_ID");
            entity.Property(e => e.ResourceTypeDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Resource_Type_Description");
            entity.Property(e => e.ResourceTypeName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Resource_Type_Name");
        });

        modelBuilder.Entity<SystemAdministrator>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__System_A__4A3001179856854E");

            entity.ToTable("System_Administrator");

            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.UserId).HasColumnName("_User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.SystemAdministrators)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__System_Ad___User__4222D4EF");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__716F4ACDAA146D8F");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId).HasColumnName("Task_ID");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("Due_Date");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.PriorityId).HasColumnName("Priority_ID");
            entity.Property(e => e.ProjectId).HasColumnName("Project_ID");
            entity.Property(e => e.TaskDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Task_Description");
            entity.Property(e => e.TaskName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Task_Name");
            entity.Property(e => e.TaskStatusId).HasColumnName("Task_Status_ID");
            entity.Property(e => e.TaskTypeId).HasColumnName("Task_Type_ID");

            entity.HasOne(d => d.Employee).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Task__Employee_I__2180FB33");

            entity.HasOne(d => d.Priority).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.PriorityId)
                .HasConstraintName("FK__Task__Priority_I__236943A5");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Task__Project_ID__208CD6FA");

            entity.HasOne(d => d.TaskStatus).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskStatusId)
                .HasConstraintName("FK__Task__Task_Statu__22751F6C");

            entity.HasOne(d => d.TaskType).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskTypeId)
                .HasConstraintName("FK__Task__Task_Type___245D67DE");
        });

        modelBuilder.Entity<TaskPriority>(entity =>
        {
            entity.HasKey(e => e.PriorityId).HasName("PK__Task_Pri__FF67322022FB5577");

            entity.ToTable("Task_Priority");

            entity.Property(e => e.PriorityId).HasColumnName("Priority_ID");
            entity.Property(e => e.PriorityName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Priority_Name");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.TaskStatusId).HasName("PK__Task_Sta__E9B5D9A64884B4D6");

            entity.ToTable("Task_Status");

            entity.Property(e => e.TaskStatusId).HasColumnName("Task_Status_ID");
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Status_Description");
            entity.Property(e => e.StatusName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Status_Name");
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.HasKey(e => e.TaskTypeId).HasName("PK__Task_Typ__48D50A97C6EB8FD3");

            entity.ToTable("Task_Type");

            entity.Property(e => e.TaskTypeId).HasColumnName("Task_Type_ID");
            entity.Property(e => e.TaskTypeDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Task_Type_Description");
            entity.Property(e => e.TaskTypeName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Task_Type_Name");
        });

        modelBuilder.Entity<Timecard>(entity =>
        {
            entity.HasKey(e => e.TimecardId).HasName("PK__Timecard__C8BCA79393354FBA");

            entity.ToTable("Timecard");

            entity.Property(e => e.TimecardId).HasColumnName("Timecard_ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(350)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.EndTime)
                .HasColumnType("date")
                .HasColumnName("End_Time");
            entity.Property(e => e.ProjectId).HasColumnName("Project_ID");
            entity.Property(e => e.StartTime)
                .HasColumnType("date")
                .HasColumnName("Start_Time");
            entity.Property(e => e.TimecardDate)
                .HasColumnType("date")
                .HasColumnName("Timecard_Date");
            entity.Property(e => e.TimesheetId).HasColumnName("Timesheet_ID");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Admin).WithMany(p => p.Timecards)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__Timecard__Admin___2A164134");

            entity.HasOne(d => d.Employee).WithMany(p => p.Timecards)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Timecard__Employ__2739D489");

            entity.HasOne(d => d.Project).WithMany(p => p.Timecards)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Timecard__Projec__282DF8C2");

            entity.HasOne(d => d.Timesheet).WithMany(p => p.Timecards)
                .HasForeignKey(d => d.TimesheetId)
                .HasConstraintName("FK__Timecard__Timesh__29221CFB");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.TimesheetId).HasName("PK__Timeshee__9E523430710BFDA3");

            entity.ToTable("Timesheet");

            entity.Property(e => e.TimesheetId).HasColumnName("Timesheet_ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.DateSubmitted)
                .HasColumnType("date")
                .HasColumnName("Date_Submitted");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.ProjectId).HasColumnName("Project_ID");
            entity.Property(e => e.TimesheetStatusId).HasColumnName("Timesheet_Status_ID");

            entity.HasOne(d => d.Admin).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__Timesheet__Admin__1BC821DD");

            entity.HasOne(d => d.Employee).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Timesheet__Emplo__19DFD96B");

            entity.HasOne(d => d.Project).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Timesheet__Proje__1AD3FDA4");

            entity.HasOne(d => d.TimesheetStatus).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.TimesheetStatusId)
                .HasConstraintName("FK__Timesheet__Times__18EBB532");
        });

        modelBuilder.Entity<TimesheetStatus>(entity =>
        {
            entity.HasKey(e => e.TimesheetStatusId).HasName("PK__Timeshee__F7E5680E5C0CAEA2");

            entity.ToTable("Timesheet_Status");

            entity.Property(e => e.TimesheetStatusId).HasColumnName("Timesheet_Status_ID");
            entity.Property(e => e.TimesheetStatusDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("Timesheet_Status_Description");
            entity.Property(e => e.TimesheetStatusName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Timesheet_Status_Name");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.TitleId).HasName("PK__Title__01D447407CF0A86F");

            entity.ToTable("Title");

            entity.Property(e => e.TitleId).HasColumnName("Title_ID");
            entity.Property(e => e.TitleName)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Title_Name");
        });

        modelBuilder.Entity<TokenApi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Token_API");

            entity.Property(e => e.AccessToken)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK___User__9EB026C7C95789F6");

            entity.ToTable("_User");

            entity.Property(e => e.UserId).HasColumnName("_User_ID");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Email_Address");
            entity.Property(e => e.FileName)
                .HasMaxLength(350)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.PasswordId).HasColumnName("Password_ID");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.ResetPasswordExpiry).HasColumnType("datetime");
            entity.Property(e => e.ResetPasswordToken)
                .HasMaxLength(350)
                .IsUnicode(false);
            entity.Property(e => e.TitleId).HasColumnName("Title_ID");
            entity.Property(e => e.Token)
                .HasMaxLength(350)
                .IsUnicode(false);
            entity.Property(e => e.TokenExpiration).HasColumnType("datetime");
            entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");

            entity.HasOne(d => d.Password).WithMany(p => p.Users)
                .HasForeignKey(d => d.PasswordId)
                .HasConstraintName("FK___User__Password___3E52440B");

            entity.HasOne(d => d.Title).WithMany(p => p.Users)
                .HasForeignKey(d => d.TitleId)
                .HasConstraintName("FK___User__Title_ID__3D5E1FD2");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK___User__User_Role__3F466844");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__User_Rol__134E48EC0F27BA12");

            entity.ToTable("User_Role");

            entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");
            entity.Property(e => e.UserRoleDescription)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("User_Role_Description");
            entity.Property(e => e.UserRoleName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("User_Role_Name");
        });

        modelBuilder.Entity<UserUserRole>(entity =>
        {
            entity.HasKey(e => e.UserUserRoleId).HasName("PK__User_Use__1AC21F5D6766DA2E");

            entity.ToTable("User_User_Role");

            entity.Property(e => e.UserUserRoleId).HasColumnName("User_User_Role_ID");
            entity.Property(e => e.UserId).HasColumnName("_User_ID");
            entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");

            entity.HasOne(d => d.User).WithMany(p => p.UserUserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__User_User___User__45F365D3");

            entity.HasOne(d => d.UserRole).WithMany(p => p.UserUserRoles)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK__User_User__User___44FF419A");
        });

        modelBuilder.Entity<Vat>(entity =>
        {
            entity.HasKey(e => e.VatId).HasName("PK__Vat__9F07D561A710BA5A");

            entity.ToTable("Vat");

            entity.Property(e => e.VatId).HasColumnName("Vat_ID");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("_Date");
            entity.Property(e => e.VatPercentage)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("Vat_Percentage");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
