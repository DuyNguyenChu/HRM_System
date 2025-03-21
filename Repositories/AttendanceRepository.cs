using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AttendanceDAO _attendanceDAO;

        public AttendanceRepository()
        {
            _attendanceDAO = new AttendanceDAO();
        }
        public List<Attendance> GetAllAttendances() => _attendanceDAO.GetAllAttendances();
        public Attendance GetAttendanceByEmployeeId(int id) => _attendanceDAO.GetAttendanceByEmployeeId(id);
        public List<Attendance> GetAllAttendanceByEmployeeId(int employeeId)
    => _attendanceDAO.GetAllAttendanceByEmployeeId(employeeId);
        public void AddAttendance(Attendance attendance) => _attendanceDAO.AddAttendance(attendance);
        public void UpdateAttendance(Attendance attendance) => _attendanceDAO.UpdateAttendance(attendance);
        public void DeleteAttendance(int id) => _attendanceDAO.DeleteAttendance(id);

        public List<Attendance> GetAttendanceByMonthYear(int month, int year)
        {
            return _attendanceDAO.GetAttendanceByMonthYear(month, year);
        }

    }
}
