using GroupCoursework.Repository;
using System.Collections.Generic;
using GroupCoursework.Models;
using GroupCoursework.Repositories;
using GroupCoursework.DTO;
using GroupCoursework.Utils;
using System.Reflection.Metadata;

namespace GroupCoursework.Service
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;
        private readonly BlogVoteRepository _blogVoteRepository;
        private readonly PostBlogDTO _postBlogDTO;
        private readonly VoteBlogDTO _voteBlogDTO;
        private readonly ValueMapper _valueMapper;
        private readonly FileUploaderHelper _fileUploaderHelper;

        public BlogService(
            BlogRepository blogRepository,
            BlogVoteRepository blogVoteRepository,
            PostBlogDTO postBlogDTO,
            VoteBlogDTO voteBlogDTO,
            ValueMapper valueMapper,
            FileUploaderHelper fileUploaderHelper
            )
        {
            _blogRepository = blogRepository;
            _blogVoteRepository = blogVoteRepository;
            _postBlogDTO = postBlogDTO;
            _voteBlogDTO = voteBlogDTO;
            _valueMapper = valueMapper;
            _fileUploaderHelper = fileUploaderHelper;
        }

        public IEnumerable<Blog> GetAllBlogs()
        {
            return _blogRepository.GetAllBlogs();
        }

        public Blog GetBlogById(int blogId)
        {
            return _blogRepository.GetBlogById(blogId);
        }

        public Boolean AddBlog(PostBlogDTO postBlogDTO, string imageUrl, User userDetails )
        {
            //saving the blog image in the local server folder

            Blog blogObject = _valueMapper.MapToBlog(postBlogDTO, imageUrl, userDetails);

            if (_blogRepository.AddBlog(blogObject))
            {
                return true;
            }
            return false;
        }

        //public Boolean UpdateBlog(Blog blog)
        //{
        //    return _blogRepository.UpdateBlog(blog);
        //}

        //public Boolean DeleteBlog(int blogId)
        //{
        //    return _blogRepository.DeleteBlog(blogId);
        //}

        public Boolean VoteBlog(VoteBlogDTO blogVote,User userDetails)
        {
            if(blogVote == null)
            {
                return false;
            }
            if(userDetails == null)
            {
                return false;
            }

            BlogVote blogVoteObject = _valueMapper.MapToBlogVote(blogVote, userDetails);

            if (_blogVoteRepository.AddVoteBlog(blogVoteObject))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
