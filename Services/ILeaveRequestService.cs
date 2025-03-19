using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface ILeaveRequestService
    {
        List<LeaveRequest> GetAllLeaveRequests();

        List<LeaveRequest> GetLeaveRequestByYear(int year);

        void UpdateLeaveRequestStatus(int leaveId, string status);

    }
}
