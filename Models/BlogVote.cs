using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GroupCoursework.Models
{
    public class BlogVote
    {
        [Key]
        public int VoteId { get; set; }

 
        public Blog Blog { get; set; }

        [Required]
        public bool IsVote { get; set; }

        // Update the navigation property and attribute for the User property
        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
