using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface IActivityLogServices
    {
        List<ActivityLog> GetActivityLog();
        List<ActivityLog> SearchActivityLog(string keyword);
        void AddActivityLog(ActivityLog p);
        void DeleteActivityLog(ActivityLog p);
    }
}
