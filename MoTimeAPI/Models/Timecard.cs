using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Timecard
{
    public int TimecardId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProjectId { get; set; }

    public int? TimesheetId { get; set; }

    public int? AdminId { get; set; }

    public string Title { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public DateTime? TimecardDate { get; set; }

    public string Comment { get; set; }

    public virtual SystemAdministrator Admin { get; set; }

    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();

    public virtual ICollection<CalendarItem> CalendarItems { get; set; } = new List<CalendarItem>();

    public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();

    public virtual Employee Employee { get; set; }

    public virtual Project Project { get; set; }

    public virtual Timesheet Timesheet { get; set; }
}
