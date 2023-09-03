using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Timesheet
{
    public int TimesheetId { get; set; }

    public int? TimesheetStatusId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProjectId { get; set; }

    public int? AdminId { get; set; }

    public DateTime? DateSubmitted { get; set; }

    public virtual SystemAdministrator Admin { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual ICollection<History> Histories { get; set; } = new List<History>();

    public virtual Project Project { get; set; }

    public virtual ICollection<Timecard> Timecards { get; set; } = new List<Timecard>();

    public virtual TimesheetStatus TimesheetStatus { get; set; }
}
