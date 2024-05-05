using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GroupCoursework.Models
{
    [Table("BlogVotes")]
    public class BlogVote
    {
        [Key]
        public int VoteId { get; set; }
 
        public Blog Blog { get; set; }

        [Required]
        public bool IsVote { get; set; }

        public User User { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }

        public BlogVote() { }
    }
}
