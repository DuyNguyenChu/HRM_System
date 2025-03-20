using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class UserRepositories : IUserRepositories

    {
        public User GetUserAcc(string phoneNumber, string passWord) => UserDAO.GetUserAcc(phoneNumber, passWord);
    }
}
