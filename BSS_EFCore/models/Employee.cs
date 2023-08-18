using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Employee
{
    public long EmployeeId { get; set; }

    public string email { get; set; }

    public string Password { get; set; } 

    public byte[]? ProfilePhoto { get; set; }

    public DateTime CreateTime { get; set; }

    public string? PhoneNumber { get; set; }

    public string? IdentityNumber { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public string? Positions { get; set; }

    public int Salary { get; set; }



    public Kpi kpi { get; set; } 

    public List<MaintenanceItem> maintenanceItems { get; set; } = new List<MaintenanceItem>();

    public List<SwitchLog> switchLogs { get; set; } = new List<SwitchLog>();  //可以为0

    public List<SwitchRequest> switchRequests { get; set; } = new List<SwitchRequest>(); //可以为0

    public SwitchStation? switchStation { get; set; }  //nullable

}
