using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GroupCoursework.Repository
{
    public class BlogCommentRepository
    {
        private readonly UserService _userService;
        private readonly AppDatabaseContext _context;

        private readonly ILogger _logger;

        public BlogCommentRepository(AppDatabaseContext context,
            UserService userService,
            ILogger<BlogCommentRepository> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        private BlogComments PopulateUserDetails(BlogComments blogComments)
        {
            // Check if the blog has a user
            if (blogComments.User != null)
            {
                var userDetails = _userService.GetUserById(blogComments.User.UserId);

                // Populate user details in the blog object
                blogComments.User = userDetails;
            }

            return blogComments;
        }

        public Boolean PostBlogComment(BlogComments blogComments)
        {
            try
            {
                _context.BlogComments.Add(blogComments);
                _context.SaveChanges();
                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                return false; // Operation failed
            }
        }

        public List<BlogComments> GetAllBlogCommentsById(int blogId)
        {
            try
            {
                var blogComments = _context.BlogComments.Where(b => b.Blog.BlogId == blogId).Include(user => user.User).ToList(); 
                List<BlogComments> blogCommentsList = new List<BlogComments>();

                if (blogComments == null)
                {
                    return blogCommentsList;
                }

                // Populate user details for each blog
                foreach (var blogComment in blogComments)
                {
                    blogCommentsList.Add(PopulateUserDetails(blogComment));
                }
                return blogCommentsList;
            }
            catch (Exception ex)
            {
                List<BlogComments> blogCommentsList = new List<BlogComments>();
                return blogCommentsList; // Operation failed
            }
        }

        public BlogComments GetBlogCommentById(int blogCommentId)
        {
            var blogComments = _context.BlogComments.Include(user => user.User).FirstOrDefault(b => b.CommentId == blogCommentId);

            if(blogComments != null)
            {
                blogComments = PopulateUserDetails(blogComments);
                return blogComments;
            }
            return null;
        }

        public Boolean UpdateBlogComment(BlogComments blogComments)
        {
            _context.BlogComments.Update(blogComments);
            _context.SaveChanges();

            return true; // Update successfule
        }
    }
}
