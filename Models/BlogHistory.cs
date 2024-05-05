using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GroupCoursework.Models
{
    [Table("BlogHistory")]
    public class BlogHistory
    {
        [Key]
        public int BlogHistoryId { get; set; }

        [Required]
        public Blog Blog { get; set; }

        [Display(Name = "Blog Title")]
        public string BlogTitle { get; set; }

        [Display(Name = "Blog Content")]
        public string BlogContent { get; set; }

        [Display(Name = "Blog Image Url")]
        public string BlogImageUrl { get; set; }

        [Display(Name ="CreatedAt")]
        public DateTime CreatedAt { get; set; }

    }
}
