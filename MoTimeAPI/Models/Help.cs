using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Help
{
    public int HelpId { get; set; }

    public int? HelpTypeId { get; set; }

    public int? AdminId { get; set; }

    public string HelpName { get; set; }

    public string HelpDescription { get; set; }

    public byte[] Material { get; set; }

    public string FileName { get; set; }

    public virtual SystemAdministrator Admin { get; set; }

    public virtual HelpType HelpType { get; set; }
}
