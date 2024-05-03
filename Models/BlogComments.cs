using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace GroupCoursework.Models
{

    [Table("BlogComments")]
    public class BlogComments
    {

        [Key]
        public int CommentId { get; set; }

        [Required]
        [Display(Name = "Comment Content")]
        public string CommentContent { get; set; }

        [Required]
        [Display(Name = "Blog Id")]
        public Blog Blog { get; set; }

        [Required]
        [Display(Name = "User Id")]
        public User User { get; set; }

        [Display(Name = "Is Comment Deleted")]
        public bool IsCommentDeleted { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; }



    }
}
