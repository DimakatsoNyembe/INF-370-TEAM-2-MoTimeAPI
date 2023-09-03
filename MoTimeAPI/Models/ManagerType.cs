using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ManagerType
{
    public int ManagerTypeId { get; set; }

    public int? EmployeeTypeId { get; set; }

    public string ManagerTypeName { get; set; }

    public string ManagerTypeDescription { get; set; }

    public virtual EmployeeType EmployeeType { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
