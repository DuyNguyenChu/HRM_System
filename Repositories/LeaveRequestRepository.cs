using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly LeaveRequestDAO _leaveRequestDAO;

        public LeaveRequestRepository()
        {
            _leaveRequestDAO = new LeaveRequestDAO();
        }

        public List<LeaveRequest> GetAllLeaveRequests() => _leaveRequestDAO.GetAllLeaveRequests();

        public List<LeaveRequest> GetLeaveRequestByYear( int year)
        {
            return _leaveRequestDAO.GetLeaveRequestByYear( year);
        }
    }
}
