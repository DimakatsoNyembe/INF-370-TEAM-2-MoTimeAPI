using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ProjectClaimItem
{
    public int? ClaimItemId { get; set; }

    public int? ProjectAllocationId { get; set; }

    public virtual ClaimItem ClaimItem { get; set; }

    public virtual ProjectAllocation ProjectAllocation { get; set; }
}
