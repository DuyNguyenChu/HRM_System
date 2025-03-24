using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class AttendanceDAO
    {
        private readonly HrmSystemContext _context;

        public AttendanceDAO()
        {
            _context = new HrmSystemContext();
        }

        public List<Attendance> GetAllAttendances()
        {
            return _context.Attendances.Include(fullName => fullName.Employee).ToList();
        }

        public Attendance? GetAttendanceByEmployeeId(int id)
        {
            return _context.Attendances.FirstOrDefault(b => b.EmployeeId == id);
        }

        public List<Attendance> GetAllAttendanceByEmployeeId(int employeeId)
        {
            return _context.Attendances
                           .Include(a => a.Employee)
                           .Where(a => a.EmployeeId == employeeId)
                           .OrderByDescending(a => a.WorkDate)
                           .ToList();
        }



        public void AddAttendance(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            _context.SaveChanges();
        }

        public void UpdateAttendance(Attendance attendance)
        {
            var existingAttendance = _context.Attendances.FirstOrDefault(a => a.AttendanceId == attendance.AttendanceId);
            if (existingAttendance != null)
            {
                _context.Entry(existingAttendance).CurrentValues.SetValues(attendance);
                _context.SaveChanges();
            }
        }


        public void DeleteAttendance(int id)
        {
            var attendance = GetAttendanceByEmployeeId(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                _context.SaveChanges();
            }
        }

        public List<Attendance> GetAttendanceByMonthYear(int month, int year)
        {
            var attendances = _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.WorkDate.Year == year && a.WorkDate.Month == month)
                .GroupBy(a => a.EmployeeId)
                .Select(g => new Attendance
                {
                    EmployeeId = g.Key,
                    Employee = g.First().Employee, // Lấy thông tin nhân viên
                    WorkDays = g.Count(a => a.LeaveType == "Đi làm"), //Số lần Đi làm
                    OnLeave = g.Count(a => a.LeaveType == "Nghỉ phép"), // Số lần làm đơn nghỉ phép
                    SickLeave = g.Count(a => a.LeaveType == "Nghỉ bệnh"), // Số lần làm đơn nghỉ phép
                    LeaveWithoutPay = g.Count(a => a.LeaveType == "Nghỉ không lương") // Số lần làm đơn nghỉ phép
                })
                .ToList();

            return attendances;
        }
    }
}
