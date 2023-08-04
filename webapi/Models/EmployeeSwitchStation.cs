using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class EmployeeSwitchStation
{
    public string EmployeeId { get; set; } = null!;

    public string StationId { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual SwitchStation Station { get; set; } = null!;
}
