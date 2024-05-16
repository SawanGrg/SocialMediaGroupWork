using GroupCoursework.Models;

namespace GroupCoursework.DTO
{
   
        //DTO for blog post
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
    
        //DTO for blog pagination home view

        public class BlogPaginationDTO
        {
            public int TotalBlogs { get; set; }
            public int PageSize { get; set; }
            public string ?SortOrder { get; set; }
            public IEnumerable<Blog> Blog { get; set; }

            //public BlogPaginationDTO(int totalCount, int pageSize, IEnumerable<Blog> blog)
            //{
            //    TotalBlogs = totalCount;
            //    PageSize = pageSize;
            //    Blog = blog;
            //}
        }

        //DTO for blog updation

        public class UpdateBlogDTO
        {
            public string? BlogTitle { get; set; }

            public string? BlogContent { get; set; }

            public IFormFile? BlogImage { get; set; }
        }

}

