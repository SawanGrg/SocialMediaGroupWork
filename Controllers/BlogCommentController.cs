using GroupCoursework.ApiResponse;
using GroupCoursework.DTO;
using GroupCoursework.Models;
using GroupCoursework.Repositories;
using GroupCoursework.Service;
using GroupCoursework.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GroupCoursework.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogCommentController : Controller
    {
        private readonly BlogCommentService _blogCommentService;
        private readonly BlogService _blogService;
        private readonly FileUploaderHelper _fileUploaderHelper;
        private readonly UserRepository _userRepository;
        public BlogCommentController(BlogCommentService blogCommentService, BlogService blogService, FileUploaderHelper fileUploaderHelper, UserRepository userRepository)
        {
            _blogCommentService = blogCommentService;
            _blogService = blogService;
            _fileUploaderHelper = fileUploaderHelper;
            _userRepository = userRepository;
        }


        [HttpPost("postBlogComment/{blogId}")]
        public IActionResult PostBlogComment(int blogId, [FromBody] BlogCommentDTO blogCommentDTO)
        {
            if (blogCommentDTO == null)
            {
                var response = new ApiResponse<string>("404", "Blog comment not found", null);
                return NotFound(response);

            }

            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                var response = new ApiResponse<string>("403", "Login to comment in the blog", null);
                return Unauthorized(response);
            }

            User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));

            if (userDetails == null)
            {
                var response = new ApiResponse<string>("404", "User not found", null);
                return NotFound(response);
            }

            Blog blog = _blogService.GetBlogById(blogId);

            if (blog == null)
            {
                var response = new ApiResponse<string>("404", "Blog not found", null);
                return NotFound(response);
            }


            if (_blogCommentService.PostBlogComment(blog, blogCommentDTO, userDetails))
            {
                var response = new ApiResponse<string>("201", "Comment in the blog posted successfully.", null);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<string>("500", "Failed to comment in the blog", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }


        [HttpGet("getAllBlogComments/{blogId}")]
        public IActionResult GetAllBlogCommentsByBlogId(int blogId)
        {
            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                var response = new ApiResponse<string>("403", "Login to comment in the blog", null);
                return Unauthorized(response);
            }

            var blogComments = _blogCommentService.GetAllBlogCommentsById(blogId);

            if (blogComments == null || blogComments.Count == 0)
            {
                var response = new ApiResponse<string>("404", "Blog comments not found", null);
                return NotFound(response);
            }

            var finalResponse = new ApiResponse<List<BlogComments>>("201", "All blog comments", blogComments);
            return Ok(finalResponse);

        }

        [HttpPost("updateBlogComment")]
        public IActionResult UpdateBlogComment([FromQuery] int blogCommentId, [FromBody] UpdateBlogCommentDTO updateBlogCommentDTO)
        {
            if (updateBlogCommentDTO == null)
            {
                var response = new ApiResponse<string>("404", "Blog comment not found", null);
                return NotFound(response);

            }

            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                var response = new ApiResponse<string>("403", "Login to comment in the blog", null);
                return Unauthorized(response);
            }

            User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));

            if (userDetails == null)
            {
                var response = new ApiResponse<string>("404", "User not found", null);
                return NotFound(response);
            }

            BlogComments existingBlogComments = _blogCommentService.GetBlogCommentByID(blogCommentId);

            if (existingBlogComments == null)
            {
                var response = new ApiResponse<string>("404", "Blog comment not found", null);
                return NotFound(response);
            }

            if (existingBlogComments.User.UserId != userDetails.UserId)
            {
                var response = new ApiResponse<string>("500", "Current user cannot update comment in the blog", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            }

            Boolean update = _blogCommentService.UpdateBlogComment(updateBlogCommentDTO, existingBlogComments);

            if (update)
            {
                var response = new ApiResponse<string>("201", "Comment of the blog updated successfully.", null);
                return Ok(response);
            }

            var finalResponse = new ApiResponse<string>("500", "Failed to update comment in the blog", null);
            return StatusCode(StatusCodes.Status500InternalServerError, finalResponse);
        }

        [HttpDelete("blogCommentDelete/{blogCommentId}")]
        public IActionResult DeleteBlogComment(int blogCommentId)
        {
            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                var response = new ApiResponse<string>("403", "Login to comment in the blog", null);
                return Unauthorized(response);
            }

            User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));

            if (userDetails == null)
            {
                var response = new ApiResponse<string>("404", "User not found", null);
                return NotFound(response);
            }

            BlogComments existingBlogComments = _blogCommentService.GetBlogCommentByID(blogCommentId);

            if (existingBlogComments == null)
            {
                var response = new ApiResponse<string>("404", "Blog comment not found", null);
                return NotFound(response);
            }

            if (existingBlogComments.User.UserId != userDetails.UserId)
            {
                var response = new ApiResponse<string>("403", "Current user cannot delete comment in the blog", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            }

            Boolean deleteBLogComment = _blogCommentService.DeleteBlogComment(existingBlogComments);

            if (deleteBLogComment)
            {
                var response = new ApiResponse<string>("201", "Comment of the blog deleted successfully.", null);
                return Ok(response);

            }

            var finalResponse = new ApiResponse<string>("500", "Failed to delete comment in the blog", null);
            return StatusCode(StatusCodes.Status500InternalServerError, finalResponse);
        }


        [HttpGet("getCommentHistory/{commentId}")]
        public IActionResult GetAllCommentHistory(int commentId)
        {
            IEnumerable<CommentHistory> blogHistories = _blogCommentService.GetBlogCommentHistoryByID(commentId);

            var response = new ApiResponse<IEnumerable<CommentHistory>>("200", "The specified comments's update history.", blogHistories);
            return Ok(response);

        }
    }
}
