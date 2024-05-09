using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GroupCoursework.Models
{
    [Table("BlogCommentVotes")]
    public class BlogCommentVote
    {
        [Key]
        public int BlogCommentVoteId { get; set; }

        [Required]
        [Display(Name = "BlogComment Id")]
        public BlogComments BlogComment { get; set; }

        [Required]
        public bool IsVote { get; set; }

        [Required]
        [Display(Name = "User Id")]
        public User User { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }

             
    }
}
