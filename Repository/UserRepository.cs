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

        public User AuthenticateUser(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password );
        }

        //Getting blogs posted by the user
        public IEnumerable<Blog> GetBlogsByUser(int userId)
        {
            return _context.Blogs.Where(b => b.user.UserId == userId).ToList();
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
              
                return false; 
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    user.IsUserDeleted = true;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return true; 
                }
                else
                {
                    return false; // User not found
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool changePassword(String email, String password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    user.Password = password;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return true; 
                }
                else
                {
                    return false; // User not found
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
