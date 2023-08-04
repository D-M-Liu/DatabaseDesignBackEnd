using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class BatterySwitchStation
{
    public string BatteryId { get; set; } = null!;

    public string StationId { get; set; } = null!;

    public virtual Battery Battery { get; set; } = null!;

    public virtual SwitchStation Station { get; set; } = null!;
}
