using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployees();
        List<Employee> SearchEmployee(string keyword);
        void AddEmployee(Employee p);
        void UpdateEmployee(Employee p);
        void DeleteEmployee(Employee p);
        
    }
}
