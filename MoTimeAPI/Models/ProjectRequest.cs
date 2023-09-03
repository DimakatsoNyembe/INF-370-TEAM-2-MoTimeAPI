using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ProjectRequest
{
    public int ProjectRequestId { get; set; }

    public int? ProjectRequestStatusId { get; set; }

    public int? AdminId { get; set; }

    public int? EmployeeId { get; set; }

    public string ProjectRequestDescription { get; set; }

    public DateTime? ProjectRequestDate { get; set; }

    public virtual SystemAdministrator Admin { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual ProjectRequestStatus ProjectRequestStatus { get; set; }
}
