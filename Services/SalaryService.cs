using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;

        public SalaryService(ISalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }
        public List<Salary> GetAllSalaries() => _salaryRepository.GetAllSalaries();
        public Salary GetSalaryByEmployeeId(int id) => _salaryRepository.GetSalaryByEmployeeId(id);
        public void AddSalary(Salary salary) => _salaryRepository.AddSalary(salary);
        public void UpdateSalary(Salary salary) => _salaryRepository.UpdateSalary(salary);
        public void DeleteSalary(int id) => _salaryRepository.DeleteSalary(id);
    }
}
