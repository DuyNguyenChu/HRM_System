using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public List<Attendance> GetAllAttendances() => _attendanceRepository.GetAllAttendances();
        public Attendance GetAttendanceByEmployeeId(int id) => _attendanceRepository.GetAttendanceByEmployeeId(id);
        public List<Attendance> GetAllAttendanceByEmployeeId(int employeeId) => _attendanceRepository.GetAllAttendanceByEmployeeId(employeeId);
        public void AddAttendance(Attendance attendance) => _attendanceRepository.AddAttendance(attendance);
        public void UpdateAttendance(Attendance attendance) => _attendanceRepository.UpdateAttendance(attendance);
        public void DeleteAttendance(int id) => _attendanceRepository.DeleteAttendance(id);

        public List<Attendance> GetAttendanceByMonthYear(int month, int year)
        {
            return _attendanceRepository.GetAttendanceByMonthYear(month, year);
        }

    }
}
