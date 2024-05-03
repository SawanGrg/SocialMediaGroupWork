﻿using GroupCoursework.DTO;
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


        public BlogVote MapToBlogVote(VoteBlogDTO voteBlogDTO, User userDetails)
        {
            BlogVote blogVote = new BlogVote();

            blogVote.User = userDetails;
            blogVote.IsVote = voteBlogDTO.vote;

            return blogVote;
        }
    }
}
