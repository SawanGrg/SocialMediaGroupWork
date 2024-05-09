using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;

namespace GroupCoursework.Repository
{
    public class BlogCommentVoteRepository
    {

        private readonly UserService _userService;
        private readonly AppDatabaseContext _context;

        private readonly ILogger _logger;

        public BlogCommentVoteRepository(AppDatabaseContext context,
            UserService userService,
            ILogger<BlogCommentVoteRepository> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        public BlogCommentVote GetBlogCommentVoteById(int blogCommentId, int userId)
        {
            try
            {
                BlogCommentVote blogCommentVote = _context.BlogCommentVotes.Include(vote => vote.BlogComment).FirstOrDefault(b => b.BlogComment.CommentId == blogCommentId && b.User.UserId == userId);
           
                return blogCommentVote;
            

            }catch (Exception ex)
            {
                return null;

            }


        }

        public Boolean AddVoteBlogComment(BlogCommentVote blogCommentVote)
        {
            try
            {
                Console.WriteLine("inside add blog commen repo");
                _context.BlogCommentVotes.Add(blogCommentVote);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean UpdateBlogCommentVote(BlogCommentVote updateBlogCommentVote)
        {
            try
            {
                _context.BlogCommentVotes.Update(updateBlogCommentVote);
                _context.SaveChanges();

                return true; // Update successful

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean DeleteBlogCommentVote(int blogCommentVoteId)
        {
            var blogCommentVote = _context.BlogCommentVotes.Find(blogCommentVoteId);
            if (blogCommentVote != null)
            {
                _context.BlogCommentVotes.Remove(blogCommentVote);
                _context.SaveChanges();
                return true;
            }
            return false;

        }


    }
}
