using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SessionHistory
{
    public int SessionId { get; set; }

    public int UserId { get; set; }

    public DateTime? LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }

    public string? DeviceInfo { get; set; }

    public virtual User User { get; set; } = null!;
}
