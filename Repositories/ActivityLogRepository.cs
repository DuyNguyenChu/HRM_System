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
        public void AddActivityLog(ActivityLog p) => ActivityLogDao.AddActivityLog(p);

        public void DeleteActivityLog(ActivityLog p) => ActivityLogDao.DeleteActivityLog(p);

        public List<ActivityLog> GetActivityLog() => ActivityLogDao.GetActivityLog();

        public List<ActivityLog> SearchActivityLog(string keyword) => ActivityLogDao.SearchActivityLog(keyword);
    }
}
