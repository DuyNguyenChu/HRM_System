using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IDepartmentRepository _repository;
        public DepartmentServices()
        {
            _repository = new DepartmentRepository();
        }

        public void AddDepartment(Department p)
        {
            _repository.AddDepartment(p);
        }

        public void DeleteDepartment(Department p)
        {
            _repository.DeleteDepartment(p);
        }

        public List<Department> GetAllDepartments()
        {
            return _repository.GetAllDepartments();
        }

        public List<Department> SearchDepartment(string keyword)
        {
            return _repository.SearchDepartment(keyword);
        }

        public void UpdateDepartment(Department p)
        {
            _repository.UpdateDepartment(p);
        }
    }
}
