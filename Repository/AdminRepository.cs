using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;
using GroupCoursework.Service;

namespace GroupCoursework.Repository
{
    public class AdminRepository
    {
        private readonly UserService _userService;
        private readonly AppDatabaseContext _context;
        private readonly ILogger _logger;
        private readonly Random _random;

        public AdminRepository(AppDatabaseContext context,
                       UserService userService,
                                  ILogger<AdminRepository> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
            _random = new Random();
        }

        public bool CreateAdminAccount(User user)
        {
            Console.WriteLine("Creating admin account at repository 1");

            // Remove the line below
            // user.UserId = _random.Next();

            Console.WriteLine(user.Username + " " + user.Password + " " + user.Email + " " + user.Role + " " + user.UserId);
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating admin account: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return false;
            }
        }

    }
}