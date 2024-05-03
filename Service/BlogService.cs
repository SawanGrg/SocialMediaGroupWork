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
        private readonly PostBlogDTO _postBlogDTO;
        private readonly ValueMapper _valueMapper;
        private readonly FileUploaderHelper _fileUploaderHelper;

        public BlogService(
            BlogRepository blogRepository,
            PostBlogDTO postBlogDTO,
            ValueMapper valueMapper,
            FileUploaderHelper fileUploaderHelper
            )
        {
            _blogRepository = blogRepository;
            _postBlogDTO = postBlogDTO;
            _valueMapper = valueMapper;
            _fileUploaderHelper = fileUploaderHelper;
        }

        public IEnumerable<Blog> GetAllBlogs(int pageNumber, int pageSize, string sortOrder)
        {
            return _blogRepository.GetAllBlogs(pageNumber, pageSize, sortOrder);
        }

        public int GetTotalBlogs()
        {
            return _blogRepository.TotalBlogs();
        }


        public Blog GetBlogById(int blogId)
        {
            return _blogRepository.GetBlogById(blogId);
        }

        public IEnumerable<Blog> GetBlogsSuggestions(int blogId)
        {
            return _blogRepository.GetBlogSuggestions(blogId);
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

        public Boolean DeleteBlog(int blogId)
        {
            var updateBlog = _blogRepository.DeleteBlog(blogId);
            if (updateBlog)
            {
                return true ;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateBlog(int blogId, UpdateBlogDTO updateBlogDTO, string newImageUrl)
        {
            //This is the existing blog taken from the db and the updateBlogDTO is the data sent from the user to be updated
            var existingBlog = _blogRepository.GetBlogById(blogId);

            if (existingBlog == null)
            {
                return false; // Blog not found
            }

            // Update properties only if they are provided in the DTO
            if (!string.IsNullOrEmpty(updateBlogDTO.BlogTitle))
            {
                existingBlog.blogTitle = updateBlogDTO.BlogTitle;
            }

            if (!string.IsNullOrEmpty(updateBlogDTO.BlogContent))
            {
                existingBlog.blogContent = updateBlogDTO.BlogContent;
            }

            if (updateBlogDTO.BlogImage != null && updateBlogDTO.BlogImage.Length > 0)
            {
                existingBlog.blogImageUrl = newImageUrl;
            }

            return _blogRepository.UpdateBlog(existingBlog);
        }

        //public Boolean UpdateBlog(Blog blog)
        //{
        //    return _blogRepository.UpdateBlog(blog);
        //}

        //public Boolean DeleteBlog(int blogId)
        //{
        //    return _blogRepository.DeleteBlog(blogId);
        //}

        //public Boolean VoteBlog(VoteBlogDTO blogVote,User userDetails)
        //{
        //    if(blogVote == null)
        //    {
        //        return false;
        //    }
        //    if(userDetails == null)
        //    {
        //        return false;
        //    }

        //    BlogVote blogVoteObject = _valueMapper.MapToBlogVote(blogVote, userDetails);

        //    if (_blogVoteRepository.AddVoteBlog(blogVoteObject))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
    }
}
