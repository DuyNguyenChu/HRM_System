using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class ActivityLogServices : IActivityLogServices
    {
        private readonly IActivityLogRepository _ActivityLogrepository;
        public ActivityLogServices()
        {
            _ActivityLogrepository = new ActivityLogRepository();
        }

        public void AddActivityLog(ActivityLog p)
        {
            _ActivityLogrepository.AddActivityLog(p);
        }

        public void DeleteActivityLog(ActivityLog p)
        {
            _ActivityLogrepository.DeleteActivityLog(p);
        }

        public List<ActivityLog> GetActivityLog()
        {
            return _ActivityLogrepository.GetActivityLog();
        }

        public List<ActivityLog> SearchActivityLog(string keyword)
        {
            return _ActivityLogrepository.SearchActivityLog(keyword);
        }
    }
}
