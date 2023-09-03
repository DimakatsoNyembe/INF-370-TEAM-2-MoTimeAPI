using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class EmployeeType
{
    public int EmployeeTypeId { get; set; }

    public string EmployeeTypeName { get; set; }

    public string EmployeeTypeDescription { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<ManagerType> ManagerTypes { get; set; } = new List<ManagerType>();
}
