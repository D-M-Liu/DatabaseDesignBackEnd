using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class VehicleParam
{
    public string VehicleModel { get; set; } = null!;

    public string Transmission { get; set; } = null!;

    public DateTime ServiceTerm { get; set; }

    public string Manufacturer { get; set; } = null!;

    public decimal? MaxSpeed { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
