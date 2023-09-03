using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public int? AdminId { get; set; }

    public int? ClientId { get; set; }

    public int? ProjectStatusId { get; set; }

    public string ProjectName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual SystemAdministrator Admin { get; set; }

    public virtual Client Client { get; set; }

    public virtual ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();

    public virtual ProjectStatus ProjectStatus { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Timecard> Timecards { get; set; } = new List<Timecard>();

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
