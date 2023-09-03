using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public int? ResourceTypeId { get; set; }

    public int? AdminId { get; set; }

    public string ResourceName { get; set; }

    public string ResourceDescription { get; set; }

    public virtual SystemAdministrator Admin { get; set; }

    public virtual ICollection<EmployeeResource> EmployeeResources { get; set; } = new List<EmployeeResource>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ResourceType ResourceType { get; set; }
}
