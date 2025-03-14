using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class SalaryDAO
    {
        private readonly HrmSystemContext _context;
        public SalaryDAO()
        {
            _context = new HrmSystemContext();
        }

        public List<Salary> GetAllSalaries()
        {
            return _context.Salaries.Include(fullName => fullName.Employee).ToList();
        }

        public Salary GetSalaryByEmployeeId(int id)
        {
            return _context.Salaries.FirstOrDefault(b => b.EmployeeId == id);
        }

        public void AddSalary(Salary salary)
        {
            _context.Salaries.Add(salary);
            _context.SaveChanges();
        }

        public void UpdateSalary(Salary salary)
        {
            _context.Salaries.Update(salary);
            _context.SaveChanges();
        }

        public void DeleteSalary(int id)
        {
            var salary = GetSalaryByEmployeeId(id);
            if (salary != null)
            {
                _context.Salaries.Remove(salary);
                _context.SaveChanges();
            }
        }
    }
}
