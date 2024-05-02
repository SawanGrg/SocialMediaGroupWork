using System.Collections.Generic;
using GroupCoursework.Models;
using GroupCoursework.Repositories;

namespace GroupCoursework.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }


        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public bool AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public bool DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }

        public bool changePassword(String email, String password)
        {
            return _userRepository.changePassword(email, password);
        }
    }
}
