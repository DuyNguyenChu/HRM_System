using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int EmployeeId { get; set; }

    public decimal BasicSalary { get; set; }

    public decimal? Allowance { get; set; }

    public decimal? Bonus { get; set; }

    public decimal? Deduction { get; set; }

    public DateOnly PayDate { get; set; }

    public decimal TotalIncome => BasicSalary + (Allowance ?? 0) + (Bonus ?? 0) - (Deduction ?? 0);
    public virtual Employee Employee { get; set; } = null!;
}
