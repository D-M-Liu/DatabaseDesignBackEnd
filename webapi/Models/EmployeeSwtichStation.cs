using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class EmployeeSwtichStation
{
    public string EmployeeId { get; set; } = null!;

    public string SwtichStationId { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual SwtichStation SwtichStation { get; set; } = null!;
}
