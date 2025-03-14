using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface ISalaryRepository
    {
        List<Salary> GetAllSalaries();
        Salary GetSalaryByEmployeeId(int id);
        void AddSalary(Salary salary); 
        void UpdateSalary(Salary salary);
        void DeleteSalary(int id);
    }
}
