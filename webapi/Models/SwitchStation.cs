using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class SwitchStation
{
    public string StationId { get; set; } = null!;

    public string? StationName { get; set; } = "";

    public decimal QueueLength { get; set; } = 0;

    public decimal ServiceFee { get; set; } = 0;

    public decimal ElectricityFee { get; set; } = 0;

    public decimal Longtitude { get; set; } = 0;

    public decimal Latitude { get; set; } = 0;

    public string? FaliureStatus { get; set; } = "OK";

    public decimal BatteryCapacity { get; set; }=0;

    public decimal? AvailableBatteryCount { get; set; } = 0;

    public virtual ICollection<BatterySwitchStation> BatterySwitchStations { get; set; } = new List<BatterySwitchStation>();

    public virtual ICollection<EmployeeSwitchStation> EmployeeSwitchStations { get; set; } = new List<EmployeeSwitchStation>();
}
