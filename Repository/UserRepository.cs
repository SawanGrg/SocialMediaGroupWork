using System.Collections.Generic;
using System.Linq;
using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;

namespace GroupCoursework.Repositories
{
    public class UserRepository
    {
        private readonly AppDatabaseContext _context;

        public UserRepository(AppDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public bool AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                return false; // Operation failed
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return true; // Operation succeeded
                }
                else
                {
                    return false; // User not found
                }
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                return false; // Operation failed
            }
        }

    }
}
