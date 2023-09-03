using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public int? ProjectId { get; set; }

    public int? EmployeeId { get; set; }

    public int? TaskStatusId { get; set; }

    public int? PriorityId { get; set; }

    public int? TaskTypeId { get; set; }

    public string TaskName { get; set; }

    public string TaskDescription { get; set; }

    public DateTime? DueDate { get; set; }

    public bool? IsComplete { get; set; }

    public virtual ICollection<CalendarItem> CalendarItems { get; set; } = new List<CalendarItem>();

    public virtual Employee Employee { get; set; }

    public virtual TaskPriority Priority { get; set; }

    public virtual Project Project { get; set; }

    public virtual TaskStatus TaskStatus { get; set; }

    public virtual TaskType TaskType { get; set; }
}
