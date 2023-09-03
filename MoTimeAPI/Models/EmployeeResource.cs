using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class EmployeeResource
{
    public int EmployeeResourceId { get; set; }

    public int? ResourceId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ResourceLevelId { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual Resource Resource { get; set; }

    public virtual ResourceLevel ResourceLevel { get; set; }
}
