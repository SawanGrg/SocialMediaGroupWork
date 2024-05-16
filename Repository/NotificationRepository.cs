using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;
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


        public IEnumerable<Notification> GetAllNotifications(int userId)
        {
            return _context.Notification.Include(n => n.SenderId)
                .Where(n => n.ReceiverId.UserId == userId)
                .ToList();
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

        public int NotificationCounts(int userId)
        {
            return _context.Notification.Where(user => user.ReceiverId.UserId == userId && user.IsSeen == false).Count();
        }

        public bool ReadNotifications(int userId)
        {
            var notifications = _context.Notification.Where(n => n.ReceiverId.UserId == userId && n.IsSeen == false);

            foreach (var notification in notifications)
            {
                notification.IsSeen = true;
            }

            _context.Notification.UpdateRange(notifications);
            _context.SaveChanges();

            return true;
        }

    }
}
