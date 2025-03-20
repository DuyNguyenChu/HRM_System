using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessLayer
{
    public class UserDAO
    {
   

        public static User GetUserAcc(string phoneNumber, string passWord)
        {
            using var context = new HrmSystemContext();
            var user = context.Users.FirstOrDefault(c => c.PhoneNumber.Equals(phoneNumber));

            if (user != null && user.PasswordHash == passWord)
            {
                return user; 
            }
            return null; 
        }
    }
}
