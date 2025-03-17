using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IEmployeeRepository _repository;
        public EmployeeServices()
        {
            _repository = new EmployeeRepository();
        }

        public List<Employee> GetAllEmployees()
        {
            return _repository.GetAllEmployees();
        }

        public List<Employee> SearchEmployee(string keyword)
        {
            return _repository.SearchEmployee(keyword);
        }

        public void AddEmployee(Employee p)
        {
            _repository.AddEmployee(p);
        }
        public void UpdateEmployee(Employee p)
        {
            _repository.UpdateEmployee(p);
        }

        public void DeleteEmployee(Employee p)
        {
            _repository.DeleteEmployee(p);
        }
    }
}
