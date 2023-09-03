using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public int? AdminId { get; set; }

    public string Account { get; set; }

    public string Department { get; set; }

    public string SiteCode { get; set; }

    public int? ProjectCode { get; set; }

    public string AccountManager { get; set; }

    public virtual SystemAdministrator Admin { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
