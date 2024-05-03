using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GroupCoursework.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Role { get; set; }

        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Is User Deleted")]
        public bool IsUserDeleted { get; set; }


        //icollection is a collection of objects
        [JsonIgnore] // Ignore this property during JSON serialization
        public ICollection <Blog>? Blogs { get; set; }

        [JsonIgnore]
        public ICollection<BlogVote>? BlogVotes { get; set; }


        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }


        [Display(Name = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; }



    }
}
