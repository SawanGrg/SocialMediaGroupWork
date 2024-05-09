using GroupCoursework.DatabaseConfig;
using GroupCoursework.DTO;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;

namespace GroupCoursework.Repository
{
    public class BlogVoteRepository
    {
        private readonly UserService _userService;
        private readonly AppDatabaseContext _context;
        private readonly NotificationRepository _notification;

        private readonly ILogger _logger;

        public BlogVoteRepository(AppDatabaseContext context,
            UserService userService, NotificationRepository notification,
            ILogger<BlogVoteRepository> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
            _notification = notification;

        }


        public Boolean AddVoteBlog(BlogVote blogVote)
        {
            try
            {
                Notification notification = new Notification
                {
                    Content = NotificationContent.Reaction,
                    SenderId = blogVote.User,
                    ReceiverId = blogVote.Blog.user,
                    CreatedAt = DateTime.Now,
                    IsSeen = false,
                    UpdatedAt = DateTime.Now,
                };

                _notification.AddNotification(notification);
                _context.BlogVotes.Add(blogVote);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public BlogVote GetBlogVoteById(int blogId, int userId)
        {
            try
            {

                BlogVote blogVote = _context.BlogVotes.Include(blogVote => blogVote.Blog).FirstOrDefault(b => b.Blog.BlogId == blogId && b.User.UserId == userId); 
                return blogVote;
          
            }catch(Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<BlogVote> GetBlogVotes(int blogId)
        {
            IEnumerable<BlogVote> blogVotes = _context.BlogVotes
                .Include(blogVote => blogVote.User)
                .Where(b => b.Blog.BlogId == blogId)
                .ToList(); 

            return blogVotes;
        }


        public Boolean UpdateBlogVote(BlogVote updatedBlogVote)
        {
            try
            {
                Notification notification = new Notification
                {
                    Content = NotificationContent.Reaction,
                    SenderId = updatedBlogVote.User,
                    ReceiverId = updatedBlogVote.Blog.user,
                    CreatedAt = DateTime.Now,
                    IsSeen = false,
                    UpdatedAt = DateTime.Now,
                };

                _notification.AddNotification(notification);
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
                Notification notification = new Notification
                {
                    Content = NotificationContent.Reaction,
                    SenderId = blogVote.User,
                    ReceiverId = blogVote.Blog.user,
                    CreatedAt = DateTime.Now,
                    IsSeen = false,
                    UpdatedAt = DateTime.Now,
                };

                _context.BlogVotes.Remove(blogVote);
                _notification.AddNotification(notification);
                _context.SaveChanges();
                return true;
            }
            return false;

        }

    }
}
