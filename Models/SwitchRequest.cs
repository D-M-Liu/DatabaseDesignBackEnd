using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class SwitchRequest
{
    public string SwitchRequestId { get; set; } = null!;

    public string VehicleId { get; set; } = null!;

    public string? SwitchType { get; set; }

    public DateTime RequestTime { get; set; }

    public decimal Longtitude { get; set; }

    public decimal Latitude { get; set; }

    public string? Position { get; set; }

    public string? Remarks { get; set; }

    public virtual SwitchLog? SwitchLogLatitudeNavigation { get; set; }

    public virtual SwitchLog? SwitchLogLongtitudeNavigation { get; set; }
}
