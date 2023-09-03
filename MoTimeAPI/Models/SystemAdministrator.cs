using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class SystemAdministrator
{
    public int AdminId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Help> Helps { get; set; } = new List<Help>();

    public virtual ICollection<ProjectRequest> ProjectRequests { get; set; } = new List<ProjectRequest>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();

    public virtual ICollection<Timecard> Timecards { get; set; } = new List<Timecard>();

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();

    public virtual User User { get; set; }
}
