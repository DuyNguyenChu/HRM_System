using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface IActivityLogRepository
    {

        List<ActivityLog> GetActivityLog();
        List<ActivityLog> SearchActivityLog(string keyword);
        void AddActivityLog(ActivityLog p);
        void DeleteActivityLog(ActivityLog p);
        List<ActivityLog> GetAllActivityLogs();


    }
}
