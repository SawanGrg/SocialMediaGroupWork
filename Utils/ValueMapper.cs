using GroupCoursework.DTO;
using GroupCoursework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using GroupCoursework.DTOs;
using GroupCoursework.Models;
using static System.Net.WebRequestMethods;

namespace GroupCoursework.Utils
{
    public class ValueMapper
    {
        public Blog MapToBlog(PostBlogDTO postBlogDTO, string fileImageNameURL, User userDetails)
        {
            Blog blog = new Blog();

            blog.blogTitle = postBlogDTO.BlogTitle;
            blog.blogContent = postBlogDTO.BlogContent;
            blog.blogImageUrl = fileImageNameURL;
            blog.user = userDetails;
            blog.blogCreatedAt = DateOnly.FromDateTime(DateTime.Now);

            return blog;
        }

        public User MapToAdminUser(CreateAdminDTO userDTO)
        {
            User user = new User
            {
                UserId = userDTO.UserId,
                Username = userDTO.Username,
                Password = userDTO.Password,
                Email = userDTO.Email,
                Phone = userDTO.Phone,
                Gender = userDTO.Gender,
                Role = "Admin",
                IsAdmin = true
            };

            return user;
        }


        public BlogVote MapToBlogVote(Blog blog, VoteBlogDTO voteBlogDTO, User userDetails)
        {
            BlogVote blogVote = new BlogVote();

            blogVote.Blog = blog;
            blogVote.User = userDetails;
            blogVote.IsVote = voteBlogDTO.vote;
            blogVote.CreatedAt = DateTime.Now;

            return blogVote;
        }

        public BlogComments MapToBlogComments(Blog blog, BlogCommentDTO blogCommentDTO, User userDetails)
        {
            BlogComments blogComments = new BlogComments();

            blogComments.CommentContent = blogCommentDTO.CommentContent;
            blogComments.Blog = blog;
            blogComments.User = userDetails;
            blogComments.IsCommentDeleted = false;
            blogComments.CreatedAt = DateTime.Now;

            return blogComments;
        }

        public BlogCommentVote MapToBlogCommentVote(BlogComments blogComment,VoteBlogCommentDTO blogCommentVoteDTO,User userDetails)
        {
            BlogCommentVote blogCommentVote = new BlogCommentVote();

            blogCommentVote.BlogComment = blogComment;
            blogCommentVote.User = userDetails;
            blogCommentVote.IsVote = blogCommentVoteDTO.vote;
            blogCommentVote.CreatedAt = DateTime.Now;

            return blogCommentVote;

        }

    }
}
