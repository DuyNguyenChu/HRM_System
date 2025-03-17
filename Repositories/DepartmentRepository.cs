using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public void AddDepartment(Department p)=>DepartmentDao.AddDepartment(p);

        public void DeleteDepartment(Department p)=>DepartmentDao.DeleteDepartment(p);

        public List<Department> GetAllDepartments()=> DepartmentDao.GetAllDepartments();

        public List<Department> SearchDepartment(string keyword)=> DepartmentDao.SearchDepartment(keyword);

        public void UpdateDepartment(Department p) => DepartmentDao.UpdateDepartment(p);
    }
}
