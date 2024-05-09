using GroupCoursework.DatabaseConfig;
using GroupCoursework.DTO;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GroupCoursework.Repository
{
    public class AdminRepository
    {
        private readonly UserService _userService;
        private readonly AppDatabaseContext _context;
        private readonly ILogger _logger;
        private readonly Random _random;

        public AdminRepository(AppDatabaseContext context,
                       UserService userService,
                                  ILogger<AdminRepository> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
            _random = new Random();
        }

        public bool CreateAdminAccount(User user)
        {
            Console.WriteLine("Creating admin account at repository 1");

            // Remove the line below
            // user.UserId = _random.Next();

            Console.WriteLine(user.Username + " " + user.Password + " " + user.Email + " " + user.Role + " " + user.UserId);
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating admin account: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return false;
            }
        }

        public List<User> Get10TopBlogger()
        {
            try
            {
                var _allBloggers = _context.Users.Where(u => u.Role == "User").ToList();

                var top10Bloggers = new List<User>();

                foreach (var blogger in _allBloggers)
                {
                    var bloggerBlogs = _context.Blogs.Where(b => b.user.UserId == blogger.UserId).ToList();

                    int totalLikes = 0;
                    int totalDislikes = 0;
                    int totalComments = 0;

                    foreach (var blog in bloggerBlogs)
                    {
                        var blogVotes = _context.BlogVotes.Where(v => v.Blog.BlogId == blog.BlogId).ToList();
                        var blogComments = _context.BlogComments.Where(c => c.Blog.BlogId == blog.BlogId).ToList();

                        totalLikes += blogVotes.Count(v => v.IsVote);
                        totalDislikes += blogVotes.Count(v => !v.IsVote);
                        totalComments += blogComments.Count;
                    }

                    double popularity = CalculatePopularity(totalLikes, totalDislikes, totalComments);

                    blogger.PopularityScore = popularity; // Assuming you have a property PopularityScore in your User class

                    top10Bloggers.Add(blogger);

                }

                // Sort the bloggers based on popularity score
                top10Bloggers = top10Bloggers.OrderByDescending(b => b.PopularityScore).Take(10).ToList();

                return top10Bloggers;
            }
            catch (Exception ex)
            {
               Console.WriteLine("Error retrieving top blogger: " + ex.Message);
                return new List<User>(); // Return an empty list on error

            }
        }



        public List<SpecificBlogsWithSuggestions> Get10TopBlogsAllTime()
        {
            try
            {
                // Retrieve all blogs from the database
                var allBlogs = _context.Blogs.ToList();

                var top10Blogs = new List<SpecificBlogsWithSuggestions>(); // Create a list to store the top 10 blogs with suggestions

                foreach (var blog in allBlogs)
                {
                    // Check if the blog ID exists in BlogVotes
                    var blogVotes = _context.BlogVotes.Where(v => v.Blog.BlogId == blog.BlogId).ToList();

                    // Check if the blog ID exists in Comments
                    var blogComments = _context.BlogComments.Where(c => c.Blog.BlogId == blog.BlogId).ToList();

                    // Calculate total likes and dislikes
                    int likes = blogVotes.Count(v => v.IsVote);
                    int dislikes = blogVotes.Count(v => !v.IsVote);

                    // Calculate total comments
                    int comments = blogComments.Count;

                    // Calculate popularity
                    double popularity = CalculatePopularity(likes, dislikes, comments);

                    // Add blog and its suggestions to the list
                    top10Blogs.Add(new SpecificBlogsWithSuggestions
                    {
                        SpecificBlog = blog,
                        Popularity = popularity
                    });
                }

                // Get top 10 blogs based on popularity
                var top10 = top10Blogs.OrderByDescending(b => b.Popularity).Take(10).ToList();

                return top10;
            }
            catch (Exception ex)
            {
                // Log any errors
                _logger.LogError("Error retrieving top 10 blogs: " + ex.Message);
                return null; // Return null or handle error as appropriate
            }
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


        public List<SpecificBlogsWithSuggestions> Get10TopBlogsForMonth(string month)
        {
            try
            {
                // Check if the month string is in the correct format
                if (!DateTime.TryParseExact(month, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedMonth))
                {
                    throw new ArgumentException("Invalid month format. Please provide month in MM/yyyy format.");
                }

                var top10Blogs = _context.Blogs
                    .Where(b => b.blogCreatedAt.Year == parsedMonth.Year && b.blogCreatedAt.Month == parsedMonth.Month)
                    .ToList() // Bring data to client-side
                    .Select(b => new
                    {
                        Blog = b,
                        Likes = _context.BlogVotes.Count(v => v.Blog.BlogId == b.BlogId && v.IsVote),
                        Dislikes = _context.BlogVotes.Count(v => v.Blog.BlogId == b.BlogId && !v.IsVote),
                        Comments = _context.BlogComments.Count(c => c.Blog.BlogId == b.BlogId)
                    })
                    .OrderByDescending(b => CalculatePopularity(b.Likes, b.Dislikes, b.Comments))
                    .Take(10)
                    .Select(b => new SpecificBlogsWithSuggestions
                    {
                        SpecificBlog = b.Blog,
                        Popularity = CalculatePopularity(b.Likes, b.Dislikes, b.Comments)
                    })
                    .ToList();

                return top10Blogs;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving top 10 blogs: " + ex.Message);
                return new List<SpecificBlogsWithSuggestions>(); // Return an empty list on error
            }
        }


        public CumulativeCountsDTO GetCumulativeCountsAllTime()
        {
            var allPostsCount = _context.Blogs.Count();
            var allUpvotesCount = _context.BlogVotes.Count(v => v.IsVote);
            var allDownvotesCount = _context.BlogVotes.Count(v => !v.IsVote);
            var allCommentsCount = _context.BlogComments.Count();

            return new CumulativeCountsDTO
            {
                BlogPostsCount = allPostsCount,
                UpvotesCount = allUpvotesCount,
                DownvotesCount = allDownvotesCount,
                CommentsCount = allCommentsCount
            };
        }

        public CumulativeCountsDTO GetCumulativeCountsForMonth(string month)
        {
            var parsedMonth = DateTime.ParseExact(month, "MM/yyyy", CultureInfo.InvariantCulture);

            var monthPostsCount = _context.Blogs.Count(b => b.blogCreatedAt.Year == parsedMonth.Year && b.blogCreatedAt.Month == parsedMonth.Month);
            var monthUpvotesCount = _context.BlogVotes.Count(v => v.IsVote && v.Blog.blogCreatedAt.Year == parsedMonth.Year && v.Blog.blogCreatedAt.Month == parsedMonth.Month);
            var monthDownvotesCount = _context.BlogVotes.Count(v => !v.IsVote && v.Blog.blogCreatedAt.Year == parsedMonth.Year && v.Blog.blogCreatedAt.Month == parsedMonth.Month);
            var monthCommentsCount = _context.BlogComments.Count(c => c.Blog.blogCreatedAt.Year == parsedMonth.Year && c.Blog.blogCreatedAt.Month == parsedMonth.Month);

            return new CumulativeCountsDTO
            {
                BlogPostsCount = monthPostsCount,
                UpvotesCount = monthUpvotesCount,
                DownvotesCount = monthDownvotesCount,
                CommentsCount = monthCommentsCount
            };
        }



    }
}