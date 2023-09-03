using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MoTimeAPI.Models;

public partial class Event
{
    public int Id { get; set; }

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public string Text { get; set; }

    public string Employee { get; set; }

    public string Project { get; set; }

    [DisplayName("Barcolor")]
    public string barColor { get; set; }
}
