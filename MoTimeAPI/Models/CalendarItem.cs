using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class CalendarItem
{
    public int CalendarItemId { get; set; }

    public int? TimeCardId { get; set; }

    public int? CalendarId { get; set; }

    public int? TaskId { get; set; }

    public virtual Calendar Calendar { get; set; }

    public virtual Task Task { get; set; }

    public virtual Timecard TimeCard { get; set; }
}
