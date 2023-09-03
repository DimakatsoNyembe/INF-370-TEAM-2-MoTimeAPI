using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class EmployeeStatus
{
    public int EmployeeStatusId { get; set; }

    public string EmployeeStatusName { get; set; }

    public string EmployeeStatusDescription { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
