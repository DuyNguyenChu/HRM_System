using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ActivityLogDAO
    {
        private readonly HrmSystemContext _context;

        public ActivityLogDAO()
        {
            _context = new HrmSystemContext();
        }

        public List<ActivityLog> GetAllActivityLogs()
        {
            return _context.ActivityLogs.Include(role => role.User).ToList();
        }


    }
}
