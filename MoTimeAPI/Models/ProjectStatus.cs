using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ProjectStatus
{
    public int ProjectStatusId { get; set; }

    public string ProjectStatusName { get; set; }

    public string ProjectStatusDescription { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
