using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly ActivityLogDAO _activityLogDAO;

        public ActivityLogRepository()
        {
            _activityLogDAO = new ActivityLogDAO();
        }

        public List<ActivityLog> GetAllActivityLogs() => _activityLogDAO.GetAllActivityLogs();

    }
}
