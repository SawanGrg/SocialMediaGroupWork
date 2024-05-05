using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GroupCoursework.Models
{
    [Table("CommentHistory")]
    public class CommentHistory
    {
        [Key]
        public int CommentHistoryId { get; set; }

        [Required]
        public BlogComments BlogComments { get; set; }

        [Display(Name = "Comment Content")]
        public String CommentContent { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }

        public CommentHistory()
        {

        }


    }
}
