using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class HelpType
{
    public int HelpTypeId { get; set; }

    public string HelpTypeName { get; set; }

    public string HelpTypeDescription { get; set; }

    public virtual ICollection<Help> Helps { get; set; } = new List<Help>();
}
