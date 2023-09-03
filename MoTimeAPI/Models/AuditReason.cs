using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class AuditReason
{
    public int AuditReasonId { get; set; }

    public string ReasonName { get; set; }

    public string ReasonDescription { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
