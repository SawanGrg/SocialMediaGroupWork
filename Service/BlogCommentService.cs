using GroupCoursework.DTO;
using GroupCoursework.Repository;
using GroupCoursework.Utils;
using GroupCoursework.Models;

namespace GroupCoursework.Service
{
    public class BlogCommentService
    {
        private readonly BlogCommentRepository _blogCommentRepository;
        private readonly BlogCommentDTO _blogCommentDTO;
        private readonly ValueMapper _valueMapper;
        private readonly FileUploaderHelper _fileUploaderHelper;


        public BlogCommentService(BlogCommentRepository blogCommentRepository, BlogCommentDTO blogCommentDTO, ValueMapper valueMapper, FileUploaderHelper fileUploaderHelper)
        {
            _blogCommentRepository = blogCommentRepository;
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

        public List<BlogComments> GetAllBlogCommentsById(int blogId)
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
                existingBlogComments.CommentContent = updateBlogCommentDTO.CommentContent;
                existingBlogComments.UpdatedAt = DateTime.Now;

                return _blogCommentRepository.UpdateBlogComment(existingBlogComments);
            }

            return false;
           

            
        }

        public Boolean DeleteBlogComment(BlogComments blogComments)
        {

            blogComments.IsCommentDeleted = true;
            return _blogCommentRepository.UpdateBlogComment(blogComments);
        }

    }
}
