using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class Vat
{
    public int VatId { get; set; }

    public decimal? VatPercentage { get; set; }

    public DateTime? Date { get; set; }
}
