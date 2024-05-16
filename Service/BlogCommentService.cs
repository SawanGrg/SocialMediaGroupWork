using GroupCoursework.DTO;
using GroupCoursework.Repository;
using GroupCoursework.Utils;
using GroupCoursework.Models;
using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations;

namespace GroupCoursework.Service
{
    public class BlogCommentService
    {
        private readonly BlogCommentRepository _blogCommentRepository;
        private readonly BlogCommentVoteRepository _blogCommentVoteRepository;
        private readonly BlogCommentDTO _blogCommentDTO;
        private readonly ValueMapper _valueMapper;
        private readonly FileUploaderHelper _fileUploaderHelper;


        public BlogCommentService(BlogCommentRepository blogCommentRepository, BlogCommentDTO blogCommentDTO, BlogCommentVoteRepository blogCommentVoteRepository, ValueMapper valueMapper, FileUploaderHelper fileUploaderHelper)
        {
            _blogCommentRepository = blogCommentRepository;
            _blogCommentVoteRepository = blogCommentVoteRepository;
            _blogCommentDTO = blogCommentDTO;
            _valueMapper = valueMapper;
            _fileUploaderHelper = fileUploaderHelper;
        }

        public Boolean PostBlogComment(Blog blog, BlogCommentDTO blogCommentDTO, User user)
        {
            BlogComments blogComments = _valueMapper.MapToBlogComments(blog, blogCommentDTO, user);

            if(_blogCommentRepository.PostBlogComment(blogComments))
            {
                return true;
            }

            return false;
        }

        public List<BlogCommentDto> GetAllBlogCommentsById(int blogId)
        {
            return _blogCommentRepository.GetAllBlogCommentsById(blogId);
        }

        public BlogComments GetBlogCommentByID(int blogCommentId)
        {
            return _blogCommentRepository.GetBlogCommentById(blogCommentId);
        }



        public Boolean UpdateBlogComment(UpdateBlogCommentDTO updateBlogCommentDTO, BlogComments existingBlogComments)
        {
            if(existingBlogComments.CommentContent != updateBlogCommentDTO.CommentContent) 
            {

                //For comment history
                CommentHistory oldComment = new CommentHistory
                {
                    BlogComments = existingBlogComments,
                    CommentContent =  existingBlogComments.CommentContent,
                    CreatedAt  = DateTime.Now,
    };
                existingBlogComments.CommentContent = updateBlogCommentDTO.CommentContent;
                existingBlogComments.UpdatedAt = DateTime.Now;

                return _blogCommentRepository.UpdateBlogComment(existingBlogComments, oldComment);
            }

            return false;
           

            
        }

        public Boolean DeleteBlogComment(BlogComments blogComments)
        {

            blogComments.IsCommentDeleted = true;
            return _blogCommentRepository.UpdateBlogCommentDelete(blogComments);
        }


        public IEnumerable<CommentHistory> GetBlogCommentHistoryByID(int blogCommentId)
        {
            return _blogCommentRepository.GetBlogCommentHistoryById(blogCommentId);
        }

        public BlogCommentVote GetBlogCommentVoteById(int BlogCommentId, int userId)
        {
            if (BlogCommentId <= 0)
            {
                return null;
            }

            return _blogCommentVoteRepository.GetBlogCommentVoteById(BlogCommentId, userId);

        }

        public Boolean VoteBlogComment(BlogComments blogComment,VoteBlogCommentDTO blogCommentVoteDTO,User userDetails)
        {

            if (blogCommentVoteDTO == null)
            {
                return false;
            }
            if (userDetails == null)
            {
                return false;
            }

            BlogCommentVote blogCommentVoteObject = _valueMapper.MapToBlogCommentVote(blogComment, blogCommentVoteDTO, userDetails);

            if (_blogCommentVoteRepository.AddVoteBlogComment(blogCommentVoteObject))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public Boolean UpdateBlogCommentVote(BlogCommentVote blogCommentCheck, VoteBlogCommentDTO blogCommentVote, User user)
        {
            blogCommentCheck.IsVote = blogCommentVote.vote;

            Boolean blogCommentUpdateStatus = _blogCommentVoteRepository.UpdateBlogCommentVote(blogCommentCheck);

            if (blogCommentUpdateStatus)
            {
                return true;
            }
            return false;

        }

        public Boolean DeleteBlogCommentVote(int blogCommentVoteId)
        {
            var deleteBlogCommentVote = _blogCommentVoteRepository.DeleteBlogCommentVote(blogCommentVoteId);
            if (deleteBlogCommentVote)
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
