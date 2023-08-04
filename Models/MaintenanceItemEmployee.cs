using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class MaintenanceItemEmployee
{
    public string MaintenanceItemId { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual MaintenanceItem MaintenanceItem { get; set; } = null!;
}
