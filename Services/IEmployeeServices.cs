using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface IEmployeeServices
    {
        List<Employee> GetAllEmployees();
        List<Employee> SearchEmployee(string keyword);
        void AddEmployee(Employee p);
        void UpdateEmployee(Employee p);
        void DeleteEmployee(Employee p);

    }
}
