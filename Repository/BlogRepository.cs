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

        public IEnumerable<Blog> GetAllBlogs(int pageNumber, int pageSize)
        {
            //So offset is the starting like the index ho and the page size is the limit
            //Ani skip vanna le kun index bata suru and take vanna le kati limit ko lini
            int offset = (pageNumber - 1) * pageSize;

            var blogs = _context.Blogs
                                   .Include(b => b.user)
                                   .OrderByDescending(b => b.blogCreatedAt) 
                                   .Skip(offset)
                                   .Take(pageSize)
                                   .ToList();

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


        public int TotalBlogs()
        {
            return _context.Blogs.Count();
        }


        public Blog GetBlogById(int blogId)
        {
            var blog =  _context.Blogs.Include(user => user.user).FirstOrDefault(b => b.BlogId == blogId);
            if (blog != null)
            {
                blog = PopulateUserDetails(blog);
            }
            return blog;
        }

        //This is for suggestions like the present specific blogs data wont be showed bu others will be shown
        public IEnumerable<Blog> GetBlogSuggestions(int blogId)
        {
            var blog = _context.Blogs.Where(b => b.BlogId != blogId).OrderByDescending(b => b.BlogId).Take(4).ToList();
            return blog;
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


        public bool UpdateBlog(Blog updatedBlog)
        {
            _context.Blogs.Update(updatedBlog);
            _context.SaveChanges();

            return true; // Update successful
        }

        public bool DeleteBlog(int blogId)
        {
            var blog = _context.Blogs.Find(blogId);
            if(blog  != null)
            {
                _context.Blogs.Remove(blog);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

      
    }
}
