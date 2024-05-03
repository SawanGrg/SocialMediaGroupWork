﻿using GroupCoursework.DTO;
using GroupCoursework.Models;
using GroupCoursework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //public BlogVote MapToBlogVote(VoteBlogDTO voteBlogDTO, User userDetails)
        //{
        //    BlogVote blogVote = new BlogVote();

        //    blogVote.User = userDetails;
        //    blogVote.Blog = voteBlogDTO.Blog;
        //    blogVote.IsVote = voteBlogDTO.vote;



        //    return blogVote;
        //}
    }
}
