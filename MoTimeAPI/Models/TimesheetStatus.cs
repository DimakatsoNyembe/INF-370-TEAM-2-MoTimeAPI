using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class TimesheetStatus
{
    public int TimesheetStatusId { get; set; }

    public string TimesheetStatusName { get; set; }

    public string TimesheetStatusDescription { get; set; }

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
