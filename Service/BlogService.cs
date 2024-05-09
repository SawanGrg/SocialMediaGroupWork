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
        private readonly ValueMapper _valueMapper;
        private readonly FileUploaderHelper _fileUploaderHelper;

        public BlogService(
            BlogRepository blogRepository,
            PostBlogDTO postBlogDTO,
            ValueMapper valueMapper,
            FileUploaderHelper fileUploaderHelper,
            BlogVoteRepository blogVoteRepository
            )
        {
            _blogRepository = blogRepository;
            _postBlogDTO = postBlogDTO;
            _valueMapper = valueMapper;
            _fileUploaderHelper = fileUploaderHelper;
            _blogVoteRepository = blogVoteRepository;
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

        public IEnumerable<BlogHistory> GetBlogHistories(int blogId)
        {
            return _blogRepository.GetBlogHistories(blogId);    
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


        public Boolean TempDeleteBlog(int blogId)
        {
            var updateBlog = _blogRepository.TempDeleteBlog(blogId);
            if (updateBlog)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean RecoverDeletedBlog(int blogId)
        {
            var updateBlog = _blogRepository.RecoverDeletedBlog(blogId);
            if (updateBlog)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            Blog existingBlog = _blogRepository.GetBlogById(blogId);

            if (existingBlog == null)
            {
                return false; // Blog not found
            }

            //Yo chai update navako wala paila ko wala for storing on the blog history 
            Blog oldBlog = new Blog
            {
                blogTitle = existingBlog.blogTitle,
                blogContent = existingBlog.blogContent,
                blogImageUrl = existingBlog.blogImageUrl
            };
            Console.WriteLine(oldBlog.blogTitle, "1");

            //For what fields have been updated yetaikai rakhdeko 
            string updatedDataMessage = "You updated: ";
            //Updated blog chai aile user le input gareko wala

            // Update properties only if they are provided in the DTO
            if (!string.IsNullOrEmpty(updateBlogDTO.BlogTitle))
            {
                existingBlog.blogTitle = updateBlogDTO.BlogTitle;
                updatedDataMessage += "title, ";
            }

            if (!string.IsNullOrEmpty(updateBlogDTO.BlogContent))
            {
                existingBlog.blogContent = updateBlogDTO.BlogContent;
                updatedDataMessage += "content, ";

            }

            if (updateBlogDTO.BlogImage != null && updateBlogDTO.BlogImage.Length > 0)
            {
                existingBlog.blogImageUrl = newImageUrl;
                updatedDataMessage += "image, ";

            }
            updatedDataMessage = updatedDataMessage.TrimEnd(',', ' ');

            Console.WriteLine(oldBlog.blogTitle, "2");

            return _blogRepository.UpdateBlog(existingBlog, oldBlog, updatedDataMessage);
        }

        //public Boolean UpdateBlog(Blog blog)
        //{
        //    return _blogRepository.UpdateBlog(blog);
        //}

        //public Boolean DeleteBlog(int blogId)
        //{
        //    return _blogRepository.DeleteBlog(blogId);
        //}

        public Boolean VoteBlog(Blog blog, VoteBlogDTO blogVote,User userDetails)
        {
            if(blogVote == null)
            {
                return false;
            }
            if(userDetails == null)
            {
                return false;
            }

            BlogVote blogVoteObject = _valueMapper.MapToBlogVote(blog, blogVote, userDetails);

         if (_blogVoteRepository.AddVoteBlog(blogVoteObject))
           {
              return true;
           }
           else
           {
               return false;
         }

        }

        public BlogVote GetBlogVote(int blogId, int userId)
        {
            if (blogId <= 0)
            {
                return null;
            }
            Console.WriteLine(blogId.ToString(), "jje");

            BlogVote blogVote = _blogVoteRepository.GetBlogVoteById(blogId, userId);
            return blogVote;
        }

        public Boolean UpdateBlogVote(BlogVote blogCheck, VoteBlogDTO blogVote, User user)
        {
            blogCheck.IsVote = blogVote.vote;

            Boolean blogUpdateStatus = _blogVoteRepository.UpdateBlogVote(blogCheck);

            if (blogUpdateStatus)
            {
                return true;
            }
            return false;

        }

        public Boolean DeleteBlogVote(int blogVoteId)
        {
            var deleteBlogVote = _blogVoteRepository.DeleteBlogVote(blogVoteId);
            if (deleteBlogVote)
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
