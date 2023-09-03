using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Calendar
{
    public int CalendarId { get; set; }

    public int? TimecardId { get; set; }

    public string CalendarItemName { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string Location { get; set; }

    public string CalendarItemDescription { get; set; }

    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();

    public virtual ICollection<CalendarItem> CalendarItems { get; set; } = new List<CalendarItem>();

    public virtual Timecard Timecard { get; set; }
}
