using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects;

public partial class LeaveRequest
{
    public int LeaveId { get; set; }

    public int EmployeeId { get; set; }

    public string? LeaveType { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Status { get; set; }

    public virtual Employee Employee { get; set; } = null!;


    [NotMapped] // Nghỉ phép
    public int OnLeave { get; set; }

    [NotMapped] // Nghỉ bệnh
    public int SickLeave { get; set; }

    [NotMapped] // Nghỉ không lương
    public int LeaveWithoutPay { get; set; }
}
