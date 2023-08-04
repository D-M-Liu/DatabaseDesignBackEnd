using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class VehicleOwner
{
    public string OwnerId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? ProfilePhoto { get; set; }

    public DateTime CreateTime { get; set; }

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public string? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
