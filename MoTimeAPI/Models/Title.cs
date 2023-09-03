using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Title
{
    public int TitleId { get; set; }

    public string TitleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
