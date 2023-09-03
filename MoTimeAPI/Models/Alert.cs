using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Alert
{
    public int AlertId { get; set; }

    public int? UserId { get; set; }

    public int? TimeCardId { get; set; }

    public int? CalendarId { get; set; }

    public int? AlertTypeId { get; set; }

    public string AlertDescription { get; set; }

    public bool? IsActive { get; set; }

    public virtual AlertType AlertType { get; set; }

    public virtual Calendar Calendar { get; set; }

    public virtual Timecard TimeCard { get; set; }

    public virtual User User { get; set; }
}
