using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int? ResourceId { get; set; }

    public int? EmployeeTypeId { get; set; }

    public int? ManagerTypeId { get; set; }

    public int? UserId { get; set; }

    public int? EmployeeStatusId { get; set; }

    public int? RegionId { get; set; }

    public int? DivisionId { get; set; }

    public virtual Division Division { get; set; }

    public virtual ICollection<EmployeeResource> EmployeeResources { get; set; } = new List<EmployeeResource>();

    public virtual EmployeeStatus EmployeeStatus { get; set; }

    public virtual EmployeeType EmployeeType { get; set; }

    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();

    public virtual ManagerType ManagerType { get; set; }

    public virtual ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();

    public virtual ICollection<ProjectRequest> ProjectRequests { get; set; } = new List<ProjectRequest>();

    public virtual BranchRegion Region { get; set; }

    public virtual Resource Resource { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Timecard> Timecards { get; set; } = new List<Timecard>();

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();

    public virtual User User { get; set; }
}
