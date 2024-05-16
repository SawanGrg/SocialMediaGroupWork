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
            // Fetch blogs from the database with required details
            var blogs = _context.Blogs
                            .Include(b => b.user)
                            .Where(blog => !blog.isDeleted)
                            .Select(blog => new
                            {
                                Blog = blog,
                                TotalUpVote = _context.BlogVotes.Count(v => v.Blog.BlogId == blog.BlogId && v.IsVote),
                                TotalDownVote = _context.BlogVotes.Count(v => v.Blog.BlogId == blog.BlogId && !v.IsVote),
                                TotalComment = _context.BlogComments.Count(c => c.Blog.BlogId == blog.BlogId && !c.IsCommentDeleted)
                            })
                            .ToList();

            // Calculate popularity and sort based on sortOrder
            switch (sortOrder)
            {
                case "random":
                    blogs = blogs.OrderBy(b => Guid.NewGuid()).ToList();
                    break;
                case "recent":
                    blogs = blogs.OrderByDescending(b => b.Blog.blogCreatedAt).ToList();
                    break;
                case "popularity":
                    blogs = blogs.OrderByDescending(b => CalculatePopularity(b.TotalUpVote, b.TotalDownVote, b.TotalComment)).ToList();
                    break;
                default:
                    break;
            }

            // Apply pagination
            blogs = blogs.Skip(offset).Take(pageSize).ToList();

            // Populate user details for each blog
            foreach (var item in blogs)
            {
                PopulateUserDetails(item.Blog);
            }

            // Return the list of populated blogs
            return blogs.Select(item => item.Blog).ToList();
        }

        private double CalculatePopularity(int upvotes, int downvotes, int comments)
        {
            // Define weightage values
            const int upvoteWeightage = 2;
            const int downvoteWeightage = -1;
            const int commentWeightage = 1;

            // Calculate popularity using the formula
            double popularity = upvoteWeightage * upvotes + downvoteWeightage * downvotes + commentWeightage * comments;

            return popularity;
        }

        private Blog PopulateUserDetails(Blog blog)
        {
            // Check if the blog has a user
            if (blog.user != null)
            {
                var userDetails = _userService.GetUserById(blog.user.UserId);

                // Populate user details in the blog object
                blog.user = userDetails;

                // Calculate total upvotes for the blog
                blog.TotalUpVote = _context.BlogVotes
                                                     .Count(v => v.Blog.BlogId == blog.BlogId && v.IsVote);

                // Calculate total downvotes for the blog
                blog.TotalDownVote = _context.BlogVotes
                                                       .Count(v => v.Blog.BlogId == blog.BlogId && !v.IsVote);

                // Calculate total comments for the blog
                blog.TotalComment = _context.BlogComments
                                                      .Count(c => c.Blog.BlogId == blog.BlogId && c.IsCommentDeleted == false);
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
