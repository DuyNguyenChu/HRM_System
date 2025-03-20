using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepositories _userRepository;

        public UserServices()
        {
            _userRepository = new UserRepositories();   
        }

        public User GetUserAcc(string phoneNumber, string passWord)
        {
           return _userRepository.GetUserAcc(phoneNumber, passWord);
        }
    }
}
