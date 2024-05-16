using GroupCoursework.Models;

namespace GroupCoursework.DTO
{
    //For deleting the notification of reactions
    public class BlogNotificationDTO
    {
        public Notification userNotifications { get; set; }
        
        public Blog Blog { get; set; }

    }
}
