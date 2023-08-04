using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Kpi
{
    public string KpiId { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public decimal TotalPerformance { get; set; }

    public decimal? ServiceFrequency { get; set; }

    public decimal? Score { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
