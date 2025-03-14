using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly SalaryDAO _salaryDAO;

        public SalaryRepository()
        {
            _salaryDAO = new SalaryDAO();
        }
        public List<Salary> GetAllSalaries() => _salaryDAO.GetAllSalaries();
        public Salary GetSalaryByEmployeeId(int id) => _salaryDAO.GetSalaryByEmployeeId(id);
        public void AddSalary(Salary salary) => _salaryDAO.AddSalary(salary);
        public void UpdateSalary(Salary salary) => _salaryDAO.UpdateSalary(salary);
        public void DeleteSalary(int id) => _salaryDAO.DeleteSalary(id);
    }
}
