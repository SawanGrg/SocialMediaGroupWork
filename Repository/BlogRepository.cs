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

        public IEnumerable<Blog> GetAllBlogs(int pageNumber, int pageSize, string sortOrder)
        {
            //So offset is the starting like the index ho and the page size is the limit
            //Ani skip vanna le kun index bata suru and take vanna le kati limit ko lini
            int offset = (pageNumber - 1) * pageSize;
            IQueryable<Blog> blogs = _context.Blogs.Include(b => b.user);

            if (sortOrder != null && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "random":
                        blogs = blogs.OrderBy(b => Guid.NewGuid());
                        break;
                    case "recent":
                        blogs = blogs.OrderByDescending(b => b.blogCreatedAt);
                        break;
                    default:
                        break;
                }
            }
            blogs = blogs.Where(blog => !blog.isDeleted).Skip(offset).Take(pageSize);

            return blogs.ToList(); 
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


        public IEnumerable<BlogHistory> GetBlogHistories(int blogId)
        {
            return _context.BlogHistory.Where(history => history.Blog.BlogId == blogId);
        }

        public bool UpdateBlog(Blog updatedBlog, Blog oldBlog, string updatedDataMessage)
        {
            Console.WriteLine(oldBlog.blogTitle, "3");
            _context.Update(updatedBlog);
            var blogHistoryEntry = new BlogHistory
            {
                Blog = updatedBlog,
                BlogTitle = oldBlog.blogTitle,
                BlogContent = oldBlog.blogContent,
                BlogImageUrl = oldBlog.blogImageUrl,
                CreatedAt = DateTime.Now,

            };

            _context.BlogHistory.Add(blogHistoryEntry);

            _context.SaveChanges();
            return true;
        }

        //public void AddBlogHistory(Blog blog)
        //{
        //    var blogHistoryEntry = new BlogHistory
        //    {
        //        BlogId = blog.BlogId,
        //        blogTitle = blog.blogTitle,
        //        blogContent = blog.blogContent,
        //        blogImageUrl = blog.blogImageUrl,
        //    };

        //    // Add the new BlogHistory entry to the context
        //    _context.BlogsHistory.Add(blogHistoryEntry);
        //    _context.SaveChanges();
        //}


        //For temporiarily deleting the blog
        public bool TempDeleteBlog(int blogId)
        {
            var blog = _context.Blogs.Find(blogId);
            if (blog != null)
            {
                blog.isDeleted = true;
                _context.Blogs.Update(blog);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        //For recovering the temporiarily deleted blog
        public bool RecoverDeletedBlog(int blogId)
        {
            var blog = _context.Blogs.Find(blogId);
            if (blog != null)
            {
                blog.isDeleted = false;
                _context.Blogs.Update(blog);
                _context.SaveChanges();
                return true;
            }
            return false;
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
