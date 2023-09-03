using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class TaskType
{
    public int TaskTypeId { get; set; }

    public string TaskTypeName { get; set; }

    public string TaskTypeDescription { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
