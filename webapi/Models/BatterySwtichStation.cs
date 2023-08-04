using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class BatterySwtichStation
{
    public string BatteryId { get; set; } = null!;

    public string SwtichStationId { get; set; } = null!;

    public virtual Battery Battery { get; set; } = null!;

    public virtual SwtichStation SwtichStation { get; set; } = null!;
}
