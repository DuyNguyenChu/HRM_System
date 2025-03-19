using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class LeaveRequestDAO
    {
        private readonly HrmSystemContext _context;

        public LeaveRequestDAO()
        {
            _context = new HrmSystemContext();
        }
        public List<LeaveRequest> GetAllLeaveRequests()
        {
            return _context.LeaveRequests
                .Include(lr => lr.Employee)
                .OrderBy(lr => lr.EmployeeId)        // Sắp xếp EmployeeId từ bé đến lớn
                .ThenByDescending(lr => lr.EndDate)  // EndDate gần ngày hiện tại nhất trước
                .ThenByDescending(lr => lr.StartDate) // StartDate gần ngày hiện tại nhất trước
                .ToList();
        }

        public List<LeaveRequest> GetLeaveRequestByYear(int year)
        {
            var leaveRequests = _context.LeaveRequests
                .Include(a => a.Employee)
                .Where(a => a.EndDate.Year == year)
                .ToList(); // Lấy toàn bộ dữ liệu mà không GroupBy

            return leaveRequests; // Trả về danh sách đầy đủ các trường
        }

        public void UpdateLeaveRequestStatus(int leaveId, string status)
        {
            var leaveRequest = _context.LeaveRequests.FirstOrDefault(lr => lr.LeaveId == leaveId);
            if (leaveRequest != null)
            {
                leaveRequest.Status = status;
                _context.SaveChanges();
            }
        }
    }
}
