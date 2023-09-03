using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Password
{
    public int PasswordId { get; set; }

    public string Password1 { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
