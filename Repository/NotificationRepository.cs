using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;
using GroupCoursework.Service;
using MimeKit;

namespace GroupCoursework.Repository
{
    public class NotificationRepository
    {

        private readonly AppDatabaseContext _context;
        private readonly ILogger _logger;
        private readonly Random _random;

        public NotificationRepository(AppDatabaseContext context,ILogger<NotificationRepository> logger)
        {
            _context = context;
            _logger = logger;
            _random = new Random();
        }


        public bool AddNotification(Notification notification)
        {
            // Create a new Notification object
            var noti = new Notification
            {
                SenderId = notification.SenderId,
                ReceiverId = notification.ReceiverId,
                Content = notification.Content,
                IsSeen = false, 
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Notification.Add(noti);
             _context.SaveChanges();

            return true; 
        }

    }
}
