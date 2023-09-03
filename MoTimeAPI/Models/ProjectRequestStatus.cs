using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ProjectRequestStatus
{
    public int ProjectRequestStatusId { get; set; }

    public string ProjectRequestStatusName { get; set; }

    public string ProjectRequestStatusDescription { get; set; }

    public virtual ICollection<ProjectRequest> ProjectRequests { get; set; } = new List<ProjectRequest>();
}
