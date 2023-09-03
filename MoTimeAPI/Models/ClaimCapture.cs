using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ClaimCapture
{
    public int ClaimId { get; set; }

    public int? ProjectAllocationId { get; set; }

    public decimal? Amount { get; set; }

    public byte[] UploadProof { get; set; }

    public virtual ProjectAllocation ProjectAllocation { get; set; }
}
