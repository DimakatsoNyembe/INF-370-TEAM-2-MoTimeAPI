using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class UserUserRole
{
    public int UserUserRoleId { get; set; }

    public int? UserRoleId { get; set; }

    public int? UserId { get; set; }

    public virtual User User { get; set; }

    public virtual UserRole UserRole { get; set; }
}
