using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public List<Employee> GetAllEmployees() => EmployeesDao.GetAllEmployees();

        public List<Employee> SearchEmployee(string keyword) => EmployeesDao.SearchEmployee(keyword);

        public void AddEmployee(Employee p) => EmployeesDao.AddEmployee(p);

        public void UpdateEmployee(Employee p) => EmployeesDao.UpdateEmployee(p);

        public void DeleteEmployee(Employee p) => EmployeesDao.DeleteEmployee(p);

    }
}
