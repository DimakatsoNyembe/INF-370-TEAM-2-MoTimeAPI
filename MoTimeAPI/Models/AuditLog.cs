using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class AuditLog
{
    public int AuditId { get; set; }

    public int? UserId { get; set; }

    public int? AuditReasonId { get; set; }

    public TimeSpan? AuditTimeStamp { get; set; }

    public string ActionPerformed { get; set; }

    public string DeviceUsed { get; set; }

    public string AuditDescription { get; set; }

    public string ResultFromAction { get; set; }

    public string IpAddress { get; set; }

    public string AuditObject { get; set; }

    public virtual AuditReason AuditReason { get; set; }

    public virtual User User { get; set; }
}
