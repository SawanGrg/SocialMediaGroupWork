using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GroupCoursework.Models
{
    [Table("CommentReactions")]
    public class CommentReaction
    {

        [Key]
        public int commentReactionId { get; set; }

        [Required]
        public BlogComments BlogComment { get; set; }

        [Required]
        [Display(Name = "User Id")]
        public User User { get; set; }

        [Required]
        [Display(Name = "Is Like")]
        public bool IsLike { get; set; }

        [Display(Name = "Is Comment Deleted")]
        public bool IsCommentDeleted { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
