using GroupCoursework.DTO;
using GroupCoursework.Models;
using GroupCoursework.Repository;
using GroupCoursework.Utils;

namespace GroupCoursework.Service
{
    public class NotificationService
    {
        private readonly NotificationRepository _notificationRepository;
        

        public NotificationService(
            NotificationRepository notificationRepository
            )
        {
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<Notification> GetAllNotifications(int userId)
        {
            return _notificationRepository.GetAllNotifications(userId);
        }

        public int GetNotificationCounts(int id)
        {
            return _notificationRepository.NotificationCounts(id);
        }

        public bool ReadNotifications(int userId)
        {
            return _notificationRepository.ReadNotifications(userId);
        }
    }
}
