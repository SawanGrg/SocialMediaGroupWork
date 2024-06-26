﻿using GroupCoursework.DatabaseConfig;
using GroupCoursework.DTO;
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
        private readonly NotificationRepository _notification;
        private readonly BlogCommentVoteRepository _blogCommentVoteRepository;

        private readonly ILogger _logger;

        public BlogCommentRepository(AppDatabaseContext context,
            UserService userService,
            ILogger<BlogCommentRepository> logger, NotificationRepository notification, BlogCommentVoteRepository blogCommentVoteRepository)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
            _notification = notification;
            _blogCommentVoteRepository = blogCommentVoteRepository;
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

        public bool PostBlogComment(BlogComments blogComments)
        {
            try
            {
                _context.BlogComments.Add(blogComments);

                //The blog comments ko user ma chai sender huncha and blogs bhitra ko chai user so 
                //Comments ko chai sender ho and blogs bhitra ko chai user ho la
                // Create a Notification object
                Notification notification = new Notification
                {
                    Content = NotificationContent.Comment, 
                    SenderId = blogComments.User,
                    ReceiverId = blogComments.Blog.user,
                    CreatedAt = DateTime.Now,
                    IsSeen = false,
                    UpdatedAt = DateTime.Now,
                };

                _notification.AddNotification(notification);
                _context.SaveChanges();
                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                return false; // Operation failed
            }
        }


        public List<BlogCommentDto> GetAllBlogCommentsById(int blogId)
        {
            try
            {
                var blogComments = _context.BlogComments
                    .Where(b => b.Blog.BlogId == blogId && b.IsCommentDeleted == false)
                    .Include(user => user.User)
                    .ToList();

                List<BlogCommentDto> blogCommentsList = new List<BlogCommentDto>();

                // Populate user details for each blog
                foreach (var blogComment in blogComments)
                {
                    BlogCommentVote blogCommentVotes = PopulateBlogCommentVote(blogComment);
                    List<BlogCommentVote> blogCommentVotesList = new List<BlogCommentVote> { blogCommentVotes };

                    BlogCommentDto blogCommentDTO = new BlogCommentDto
                    {
                        CommentId = blogComment.CommentId,
                        CommentContent = blogComment.CommentContent,
                        CommentVotes = blogCommentVotesList,
                        CreatedAt = blogComment.CreatedAt,
                        IsCommentDeleted = blogComment.IsCommentDeleted,
                        User = blogComment.User,
                    };

                    blogCommentsList.Add(blogCommentDTO);
                }

                return blogCommentsList;
            }
            catch (Exception ex)
            {
                List<BlogCommentDto> blogCommentsList = new List<BlogCommentDto>();
                return blogCommentsList; // Operation failed
            }
        }

        public BlogCommentVote PopulateBlogCommentVote(BlogComments blogComments)
        {
            var blogCommentVotes = _blogCommentVoteRepository.GetBlogCommentVoteId(blogComments.CommentId);
            return blogCommentVotes;

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

        public Boolean UpdateBlogComment(BlogComments blogComments, CommentHistory oldComment)

        {
            //For comment history
            _context.CommentHistory.Add(oldComment);

            //For comment updation
            _context.BlogComments.Update(blogComments);
            _context.SaveChanges();

            return true; // Update successfule
        }

        //For temp deletion of the blogs
        public Boolean UpdateBlogCommentDelete(BlogComments blogComments)

        {
            //For comment updation
            _context.BlogComments.Update(blogComments);
            _context.SaveChanges();

            return true; // Update successfule
        }

        public IEnumerable<CommentHistory> GetBlogCommentHistoryById(int blogCommentId)
        {
            IEnumerable<CommentHistory> blogCommentsHistory = _context.CommentHistory.Where(b => b.BlogComments.CommentId == blogCommentId);
            return blogCommentsHistory;
           
        }
    }
}
