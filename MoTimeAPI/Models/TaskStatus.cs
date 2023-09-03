using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class TaskStatus
{
    public int TaskStatusId { get; set; }

    public string StatusName { get; set; }

    public string StatusDescription { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
