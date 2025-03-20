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

        public void AddActivityLog(ActivityLog p) => ActivityLogDAO.AddActivityLog(p);

        public void DeleteActivityLog(ActivityLog p) => ActivityLogDAO.DeleteActivityLog(p);

        public List<ActivityLog> GetActivityLog() => ActivityLogDAO.GetActivityLog();

        public List<ActivityLog> SearchActivityLog(string keyword) => ActivityLogDAO.SearchActivityLog(keyword);

        public List<ActivityLog> GetAllActivityLogs() => _activityLogDAO.GetAllActivityLogs();

    }
}
