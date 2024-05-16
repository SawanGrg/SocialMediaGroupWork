using GroupCoursework.ApiResponse;
using GroupCoursework.Models;
using GroupCoursework.Repositories;
using GroupCoursework.Repository;
using GroupCoursework.Service;
using GroupCoursework.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GroupCoursework.Controllers
{
  
        [ApiController]
        [Route("api/[controller]")]
        public class NotificationController : Controller
        {

            private readonly NotificationService _notificationService;
            private readonly UserService _userService;
            public NotificationController(NotificationService notificationService, UserService userService)
            {
                _notificationService = notificationService;
                _userService = userService;
            }

            [HttpGet("getNotifications/{UserId}")]
            public IActionResult GetAllNotifications (int UserId){

                User user = _userService.GetUserById(UserId);

                if(user != null) {
                    IEnumerable<Notification> notifications = _notificationService.GetAllNotifications(UserId);
                    var response = new ApiResponse<IEnumerable<Notification>>("200", "Notifications of the required user.", notifications);
                    return Ok(response);

                }
                else
                {
                    var response = new ApiResponse<string>("404", "No user found.", null);
                    return NotFound(response);
                }
            }


        [HttpGet("getUnreadNotis/{userId}")]
        public IActionResult GetNotificationCounts (int userId) {
            User user = _userService.GetUserById(userId);

            if (user != null)
            {
                int notifications = _notificationService.GetNotificationCounts(userId);
                
                return Ok(notifications);

            }
            else
            {
                var response = new ApiResponse<string>("404", "No user found.", null);
                return NotFound(response);
            }

           }

        [HttpPut("readNoti/{userId}")]
        public IActionResult ReadNotification(int userId)
        {
            User user = _userService.GetUserById(userId);

            if (user != null)
            {
                
                bool notifications = _notificationService.ReadNotifications(userId);

                return Ok(notifications);

            }
            else
            {
                var response = new ApiResponse<string>("404", "No user found.", null);
                return NotFound(response);
            }

        }
    }

}
