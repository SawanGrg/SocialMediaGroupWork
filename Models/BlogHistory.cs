using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GroupCoursework.Models
{
    public class BlogHistory
    {
        [Key]
        public int BlogHistoryId{ get; set; }

        [Required]
        [Column(TypeName = "text"), MaxLength(100)]
        public string blogTitle { get; set; }

        [Required]
        [Column(TypeName = "text"), MaxLength(1000)]
        public string blogContent { get; set; }

        [Required]
        [Column(TypeName = "text"), MaxLength(1000)]

        public string blogImageUrl { get; set; }

        public int BlogId { get; set; }

        public DateOnly CreatedAt { get; set; }

        public DateOnly UpdatedAt { get; set; }

    }
}
