using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public string UserRoleName { get; set; }

    public string UserRoleDescription { get; set; }

    public virtual ICollection<UserUserRole> UserUserRoles { get; set; } = new List<UserUserRole>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
