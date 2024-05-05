using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GroupCoursework.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Required]
        [Column(TypeName = "text"), MaxLength(100)]
        public string blogTitle { get; set; }

        [Required]
        [Column(TypeName = "text"), MaxLength(1000)]
        public string blogContent { get; set; }

        [Required]
        [Column(TypeName = "text"), MaxLength(1000)]
        public string blogImageUrl { get; set; }

        public DateOnly blogCreatedAt { get; set; }

        public DateOnly blogUpdatedAt { get; set; }

        [DefaultValue(false)]
        public bool isDeleted { get; set; }
        public User user { get; set; }

        // Additional fields for rendering details
        [NotMapped]
        public int TotalUpVote { get; set; }

        [NotMapped]
        public int TotalDownVote { get; set; }

        [NotMapped]
        public int TotalComment { get; set; }

        public Blog()
        {

        }

        public Blog(string blogTitle, string blogContent, string blogImageUrl, DateOnly blogCreatedAt, DateOnly blogUpdatedAt, User user)
        {
            this.blogTitle = blogTitle;
            this.blogContent = blogContent;
            this.blogImageUrl = blogImageUrl;
            this.blogCreatedAt = blogCreatedAt;
            this.blogUpdatedAt = blogUpdatedAt;
            this.user = user;
        }
    }

    public class SpecificBlogsWithSuggestions
    {
        public Blog SpecificBlog { get; set; }

        public IEnumerable<Blog> BlogSuggestions { get; set; }
        public double Popularity { get; set; } // Add this property to store the popularity score
    }
}
