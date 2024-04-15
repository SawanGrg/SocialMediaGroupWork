
namespace GroupCoursework.DTO
{
    public class PostBlogDTO
    {
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public IFormFile BlogImage { get; set; }

        public PostBlogDTO()
        {
        }

        public PostBlogDTO(string blogTitle, string blogContent, IFormFile blogImage)
        {
            BlogTitle = blogTitle;
            BlogContent = blogContent;
            BlogImage = blogImage;
        }
    }
}
