using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface IAttendanceRepository
    {
        List<Attendance> GetAllAttendances();
        Attendance GetAttendanceByEmployeeId(int id);
        List<Attendance> GetAllAttendanceByEmployeeId(int employeeId);

        void AddAttendance(Attendance attendance);
        void UpdateAttendance(Attendance attendance);
        void DeleteAttendance(int id);

        List<Attendance> GetAttendanceByMonthYear(int month, int year);

    }
}
