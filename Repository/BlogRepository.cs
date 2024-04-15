using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;


namespace GroupCoursework.Repository
{
    public class BlogRepository
    {
        private readonly UserService _userService;
        private readonly AppDatabaseContext _context;

        private readonly ILogger _logger;

        public BlogRepository(AppDatabaseContext context,
            UserService userService,
            ILogger<BlogRepository> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        public IEnumerable<Blog> GetAllBlogs()
        {
            var blogs = _context.Blogs.Include(b => b.user).ToList();

            List<Blog> blogList = new List<Blog>();

            // Populate user details for each blog
            foreach (var blog in blogs)
            {
                blogList.Add(PopulateUserDetails(blog));
            }

            return blogList;
        }

        private Blog PopulateUserDetails(Blog blog)
        {
            // Check if the blog has a user
            if (blog.user != null)
            {
                // Here you can extract user details using blog.user.UserId
                // For example, you can fetch user details from the database using the user ID
                // Or you can use a cached lookup if user details are readily available
                // For demonstration purposes, let's assume you fetch user details from a service
                var userDetails = _userService.GetUserById(blog.user.UserId);

                // Populate user details in the blog object
                blog.user = userDetails;
            }

            return blog; // Return the blog object in all cases
        }



        public Blog GetBlogById(int blogId)
        {
            return _context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
        }

        public Boolean AddBlog(Blog blog)
        {
            try
            {
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding blog");
                return false; // Operation failed
            }
        }
    }
}
