using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class BranchRegion
{
    public int RegionId { get; set; }

    public string RegionName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
