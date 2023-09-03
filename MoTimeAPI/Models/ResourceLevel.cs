using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ResourceLevel
{
    public int ResourceLevelId { get; set; }

    public string ResourceLevel1 { get; set; }

    public virtual ICollection<EmployeeResource> EmployeeResources { get; set; } = new List<EmployeeResource>();
}
