using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface ISalaryService
    {
        List<Salary> GetAllSalaries();
        Salary GetSalaryByEmployeeId(int id);
        void AddSalary(Salary salary);
        void UpdateSalary(Salary salary);
        void DeleteSalary(int id);
    }
}
