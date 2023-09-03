using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Leave
{
    public int LeaveId { get; set; }

    public int? EmployeeId { get; set; }

    public int? LeaveTypeId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual LeaveType LeaveType { get; set; }
}
