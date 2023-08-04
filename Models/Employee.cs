using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? ProfilePhoto { get; set; }

    public DateTime CreateTime { get; set; }

    public string? PhoneNumber { get; set; }

    public string IdentityNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Positions { get; set; }

    public decimal? Salary { get; set; }

    public virtual EmployeeSwitchStation? EmployeeSwitchStation { get; set; }

    public virtual ICollection<Kpi> Kpis { get; set; } = new List<Kpi>();

    public virtual ICollection<MaintenanceItemEmployee> MaintenanceItemEmployees { get; set; } = new List<MaintenanceItemEmployee>();

    public virtual ICollection<SwitchLog> SwitchLogs { get; set; } = new List<SwitchLog>();

    public virtual ICollection<SwitchRequestEmployee> SwitchRequestEmployees { get; set; } = new List<SwitchRequestEmployee>();
}
