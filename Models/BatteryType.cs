using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class BatteryType
{
    public string BatteryTypeId { get; set; } = null!;

    public decimal MaxChargeTiems { get; set; }

    public string TotalCapacity { get; set; } = null!;

    public virtual ICollection<Battery> Batteries { get; set; } = new List<Battery>();
}
