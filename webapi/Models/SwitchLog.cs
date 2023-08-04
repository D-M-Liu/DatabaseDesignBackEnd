using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class SwitchLog
{
    public string SwitchServiceId { get; set; } = null!;

    public string VehicleId { get; set; } = null!;

    public DateTime SwitchTime { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string BatteryIdOn { get; set; } = null!;

    public string BatteryIdOff { get; set; } = null!;

    public string? Evaluations { get; set; }

    public decimal Longtitude { get; set; }

    public decimal Latitude { get; set; }

    public string? Position { get; set; }

    public virtual Battery BatteryIdOffNavigation { get; set; } = null!;

    public virtual Battery BatteryIdOnNavigation { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual SwitchRequest LatitudeNavigation { get; set; } = null!;

    public virtual SwitchRequest LongtitudeNavigation { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
