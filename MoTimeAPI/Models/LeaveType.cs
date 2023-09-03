using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public string LeaveTypeName { get; set; }

    public string LeaveTypeDescription { get; set; }

    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
}
