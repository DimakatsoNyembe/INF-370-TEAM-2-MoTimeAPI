using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class History
{
    public int HistoryId { get; set; }

    public int? TimesheetId { get; set; }

    public DateTime? HistoryDate { get; set; }

    public TimeSpan? HistoryTime { get; set; }

    public byte[] Submitted { get; set; }

    public string HistoryAction { get; set; }

    public virtual Timesheet Timesheet { get; set; }
}
