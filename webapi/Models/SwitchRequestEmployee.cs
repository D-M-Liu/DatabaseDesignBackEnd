using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class SwitchRequestEmployee
{
    public string SwitchRequestId { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
