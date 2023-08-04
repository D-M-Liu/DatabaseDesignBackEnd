using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Battery
{
    public string BatteryId { get; set; } = null!;

    public string? AvailableStatus { get; set; }

    public string CurrentCapacity { get; set; } = null!;

    public decimal CurrChargeTimes { get; set; }

    public DateTime ManufacturingDate { get; set; }

    public string BatteryTypeId { get; set; } = null!;

    public virtual BatterySwitchStation? BatterySwitchStation { get; set; }

    public virtual BatteryType BatteryType { get; set; } = null!;

    public virtual ICollection<SwitchLog> SwitchLogBatteryIdOffNavigations { get; set; } = new List<SwitchLog>();

    public virtual ICollection<SwitchLog> SwitchLogBatteryIdOnNavigations { get; set; } = new List<SwitchLog>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
