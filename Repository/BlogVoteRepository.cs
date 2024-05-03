using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;

namespace GroupCoursework.Repository
{
    public class BlogVoteRepository
    {
        private readonly UserService _userService;
        private readonly AppDatabaseContext _context;

        private readonly ILogger _logger;

        public BlogVoteRepository(AppDatabaseContext context,
            UserService userService,
            ILogger<BlogVoteRepository> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }


        public Boolean AddVoteBlog(BlogVote blogVote)
        {
            try
            {
                _context.BlogVotes.Add(blogVote);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public BlogVote GetBlogVoteById(int blogId)
        {
            BlogVote blogVote = _context.BlogVotes.Include(blogVote => blogVote.Blog).FirstOrDefault(b => b.Blog.BlogId == blogId);
            if (blogVote != null)
            {
                return blogVote;
            }

            return null;

        }

        public Boolean UpdateBlogVote(BlogVote updatedBlogVote)
        {
            try
            {
            _context.BlogVotes.Update(updatedBlogVote);
            _context.SaveChanges();

            return true; // Update successful

            }catch(Exception ex)
            {
                return false;
            }
        }

        public Boolean DeleteBlogVote(int blogVoteId)
        {
            var blogVote = _context.BlogVotes.Find(blogVoteId);
            if (blogVote != null)
            {
                _context.BlogVotes.Remove(blogVote);
                _context.SaveChanges();
                return true;
            }
            return false;

        }

    }
}
