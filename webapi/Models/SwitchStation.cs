using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class SwitchStation
{
    public string StationId { get; set; } = null!;

    public string? StationName { get; set; }

    public decimal QueueLength { get; set; }

    public decimal ServiceFee { get; set; }

    public decimal ElectricityFee { get; set; }

    public decimal Longtitude { get; set; }

    public decimal Latitude { get; set; }

    public string? FaliureStatus { get; set; }

    public decimal BatteryCapacity { get; set; }

    public decimal? AvailableBatteryCount { get; set; }

    public virtual ICollection<BatterySwitchStation> BatterySwitchStations { get; set; } = new List<BatterySwitchStation>();

    public virtual ICollection<EmployeeSwitchStation> EmployeeSwitchStations { get; set; } = new List<EmployeeSwitchStation>();
}
