using GroupCoursework.DatabaseConfig;
using GroupCoursework.Models;
using GroupCoursework.Service;

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
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
