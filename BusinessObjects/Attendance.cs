using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly WorkDate { get; set; }

    public TimeOnly? CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    public decimal? OvertimeHours { get; set; }

    public string? LeaveType { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
