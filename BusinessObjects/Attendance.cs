using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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


    [NotMapped] // Không lưu vào database
    public int WorkDays { get; set; }

    [NotMapped] // Nghỉ phép
    public int OnLeave { get; set; }

    [NotMapped] // Nghỉ bệnh
    public int SickLeave { get; set; }

    [NotMapped] // Nghỉ không lương
    public int LeaveWithoutPay { get; set; }

}
