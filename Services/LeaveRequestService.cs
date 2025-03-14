using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }
        public List<LeaveRequest> GetAllLeaveRequests() => _leaveRequestRepository.GetAllLeaveRequests();

        public List<LeaveRequest> GetLeaveRequestByYear(int year)
        {
            return _leaveRequestRepository.GetLeaveRequestByYear(year);
        }
    }
}
