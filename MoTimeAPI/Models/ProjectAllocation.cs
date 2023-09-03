using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ProjectAllocation
{
    public int ProjectAllocationId { get; set; }

    public int? ProjectId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ClaimItemId { get; set; }

    public int? TotalNumHours { get; set; }

    public int? ExpectedNumHours { get; set; }

    public int? BillableOverTime { get; set; }

    public bool? IsComplete { get; set; }

    public bool? IsEligibleToClaim { get; set; }

    public decimal? ClaimableAmount { get; set; }

    public bool? IsOperationalManager { get; set; }

    public bool? IsProjectManager { get; set; }

    public virtual ICollection<ClaimCapture> ClaimCaptures { get; set; } = new List<ClaimCapture>();

    public virtual ClaimItem ClaimItem { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual Project Project { get; set; }
}
