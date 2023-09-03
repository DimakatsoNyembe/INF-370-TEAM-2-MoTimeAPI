using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ClaimItem
{
    public int ClaimItemId { get; set; }

    public int? ClaimTypeId { get; set; }

    public string ClaimItemName { get; set; }

    public virtual ClaimType ClaimType { get; set; }

    public virtual ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();
}
