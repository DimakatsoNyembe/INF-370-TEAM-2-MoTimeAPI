using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class TaskPriority
{
    public int PriorityId { get; set; }

    public string PriorityName { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
