using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface IDepartmentServices
    {
        List<Department> GetAllDepartments();
        List<Department> SearchDepartment(string keyword);
        void AddDepartment(Department p);
        void UpdateDepartment(Department p);
        void DeleteDepartment(Department p);
    }
}
