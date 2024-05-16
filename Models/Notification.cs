using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace GroupCoursework.Models
{

    // Define enum for notification content types
    public enum NotificationContent
    {
        Comment,
        Reaction,
    }


    [Table("Notifications")]
    public class Notification
    {

        [Key]
        public int NotificationId { get; set; }

        [Required]
        [Display(Name = "Notification Content")]
        public NotificationContent Content { get; set; }

        [Required]
        [Display(Name = "Receiver Id")]
        public User ReceiverId { get; set; }

        [Required]
        [Display(Name = "User Id")]
        public User SenderId { get; set; }

        [Display(Name = "Is Seen")]
        public bool IsSeen { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; }


    }
}
