using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string? Address { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public int? DepartmentId { get; set; }

    public string? Position { get; set; }

    public decimal Salary { get; set; }

    public DateOnly StartDate { get; set; }

    public string? ProfileImage { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual User? User { get; set; }
}
