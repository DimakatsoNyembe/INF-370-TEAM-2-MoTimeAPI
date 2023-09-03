using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class ResourceType
{
    public int ResourceTypeId { get; set; }

    public string ResourceTypeName { get; set; }

    public string ResourceTypeDescription { get; set; }

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
