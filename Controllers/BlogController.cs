using Microsoft.AspNetCore.Mvc;
using GroupCoursework.Service;
using GroupCoursework.Models;
using GroupCoursework.ApiResponse;
using System.Collections.Generic;
using GroupCoursework.Filters;
using GroupCoursework.DTO;
using GroupCoursework.Utils;
using Microsoft.AspNetCore.Http;
using GroupCoursework.Repositories;

using GroupCoursework.Filters;
using System.Diagnostics;


namespace GroupCoursework.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : Controller
    {

        private readonly BlogService _blogService;
        private readonly FileUploaderHelper _fileUploaderHelper;
        private readonly UserRepository _userRepository;

        public BlogController(BlogService blogService, FileUploaderHelper fileUploaderHelper, UserRepository userRepository)
        {
            _blogService = blogService;
            _fileUploaderHelper = fileUploaderHelper;
            _userRepository = userRepository;
        }

        //get all blogs
        //[ServiceFilter(typeof(AdminAuthFilter))]
        //public ActionResult<ApiResponse<IEnumerable<Blog>>> GetAllBlogs()
        //{
        //    var blogs = _blogService.GetAllBlogs();
        //    var response = new ApiResponse<IEnumerable<Blog>>("200", "Success", blogs);
        //    return Ok(response);
        //}

        //SetOrder chai filter ko type denote garcha jastai random, popularity and latest and null rakheko chai the filter auna pani sakcha naauna ni so null rakdeko 
        [HttpGet("getAllBlogs")]
        public ActionResult<ApiResponse<BlogPaginationDTO>> GetAllBlogs( string  sortOrder = null, int pageNumber = 1, int pageSize = 4)
        {
            var blogs = _blogService.GetAllBlogs(pageNumber, pageSize, sortOrder);
            var totalCount = _blogService.GetTotalBlogs();
            var blogDetails = new BlogPaginationDTO
            {
                TotalBlogs = totalCount,
                PageSize = pageSize,
                Blog = blogs
            };
            var response = new ApiResponse<BlogPaginationDTO>("200", "Success", blogDetails);
            return Ok(response);
        }


        [HttpGet("specific-blogs/{blog_id}")]
        public ActionResult<ApiResponse<SpecificBlogsWithSuggestions>> GetBlogsById(int blog_id)
        {
            Debug.WriteLine(blog_id.ToString(), "hehe");
            var blogs = _blogService.GetBlogById(blog_id);
            var blogSuggestions = _blogService.GetBlogsSuggestions(blog_id);
            if (blogs == null)
            {
                var response = new ApiResponse<SpecificBlogsWithSuggestions>("404", "Blog not found", null);
                return NotFound(response);
            }
            var blogDetails = new SpecificBlogsWithSuggestions
            {
                SpecificBlog = blogs,
                BlogSuggestions = blogSuggestions
            };

            var successResponse = new ApiResponse<SpecificBlogsWithSuggestions>("200", "Success", blogDetails);
            return Ok(successResponse);
        }


        //get blog by id
        //[HttpGet("{id}")]
        //public IActionResult GetBlogs(int id)
        //{
        //    var blogs = _blogService.GetBlogById(id);
        //    if (blogs == null)
        //    {
        //        var response = new ApiResponse<IEnumerable<Blog>>("404", "Blog not found", null);
        //        return NotFound(response);
        //    }
        //    var successResponse = new ApiResponse<IEnumerable<Blog>>("200", "Success", blogs);
        //    return Ok(successResponse);
        //}

        //post blog 
        [HttpPost("postBlogs")]
        public IActionResult CreateBlog([FromForm] PostBlogDTO postBlogDTO)
        {
            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                // Handle case when Authorization header is missing
                return Unauthorized("Authorization header is missing");
            }

            User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));


            if (postBlogDTO.BlogImage == null || postBlogDTO.BlogImage.Length == 0)
                return BadRequest("Blog image is required");

            if (postBlogDTO.BlogImage.OpenReadStream().Length > 10000000)
                return BadRequest("Blog image size should not exceed 10MB");

            string imageUrl = _fileUploaderHelper.UploadFile(postBlogDTO.BlogImage);



            bool stateBlog = _blogService.AddBlog(postBlogDTO, imageUrl, userDetails);

            if (stateBlog) {
                var res = new ApiResponse<string>("201", "Blog posted SuccessFul", null);
                return Ok(res);
            }
            var response = new ApiResponse<string>("400", "Blog not created", null);
            return BadRequest(response);
        }



        //update blog 
        [HttpPut("updateBlogs/{blogId}")]
        public IActionResult UpdateBlog(int blogId, [FromForm] UpdateBlogDTO updateBlogDTO)
        {
            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                return Unauthorized("Authorization header is missing");
            }

            User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));


            Blog existingBlog = _blogService.GetBlogById(blogId);
            if (existingBlog == null)
            {
                var response = new ApiResponse<string>("404", "The specified blog does not exist.", null);
                return NotFound(response);
            }


            // Check if the user is authorized to update the blog
            if (existingBlog.user.UserId != userDetails.UserId)
            {
                var response = new ApiResponse<string>("403", "You are not authorized to update this blog.", null);
                return Unauthorized(response);
            }

            // Update the blog
            string newImageUrl = null;
            //Check if the blog is null or not
            if (updateBlogDTO.BlogImage != null && updateBlogDTO.BlogImage.Length > 0)
            {
                newImageUrl = _fileUploaderHelper.UploadFile(updateBlogDTO.BlogImage);
            }

            bool isUpdated = _blogService.UpdateBlog(blogId, updateBlogDTO, newImageUrl);

            if (isUpdated)
            {
                var response = new ApiResponse<string>("201", "The blog has been updated successfully.",null);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<string>("500", "An error occurred while updating the blog.", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("getBlogHistory/{blogId}")]
        public IActionResult GetAllHistoryBlog(int blogId)
        {
            Blog blog = _blogService.GetBlogById(blogId);

            if (blog == null)
            {
                return NotFound("The specified blog does not exist.");
            }
            else
            {
                IEnumerable<BlogHistory> blogHistories = _blogService.GetBlogHistories(blogId);
                var response = new ApiResponse<IEnumerable<BlogHistory>>("200", "The specified blog's update history.", blogHistories);
                return Ok(response);
            }
        }



        //Temporary deletion
        [HttpPut("deleteBlogTemp/{blogId}")]
        public IActionResult DeleteBlogTemp(int blogId)
        {
            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                return Unauthorized("Authorization header is missing");
            }

            User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));


            Blog existingBlog = _blogService.GetBlogById(blogId);
            if (existingBlog == null)
            {
                var response = new ApiResponse<string>("404", "The specified blog does not exist.", null);
                return NotFound(response);
            }


            // Check if the user is authorized to update the blog
            if (existingBlog.user.UserId != userDetails.UserId)
            {
                var response = new ApiResponse<string>("403", "You are not authorized to delete this blog.", null);
                return Unauthorized(response);
            }


            var blog = _blogService.TempDeleteBlog(blogId);
            if (blog)
            {
                var response = new ApiResponse<string>("201", "The blog has been deleted  temporarily.", null);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<string>("500", "An error occurred while deleting the blog.", null);
                return BadRequest(response);
            }
        }

        //Temporary deletion
        [HttpPut("recoverDeletedBlog/{blogId}")]
        public IActionResult RecoverBlogDelete(int blogId)
        {
            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                return Unauthorized("Authorization header is missing");
            }

            User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));


            Blog existingBlog = _blogService.GetBlogById(blogId);
            if (existingBlog == null)
            {
                var response = new ApiResponse<string>("404", "The specified blog does not exist.", null);
                return NotFound(response);
            }


            // Check if the user is authorized to update the blog
            if (existingBlog.user.UserId != userDetails.UserId)
            {
                var response = new ApiResponse<string>("403", "You are not authorized to delete this blog.", null);
                return Unauthorized(response);
            }


            var blog = _blogService.RecoverDeletedBlog(blogId);
            if (blog)
            {
                var response = new ApiResponse<string>("201", "The blog has been recovered  successfully.", null);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<string>("500", "An error occurred while deleting the blog.", null);
                return BadRequest(response);
            }
        }

        //Permanent deletion
        [HttpDelete("deleteBlog/{blogId}")]
        public IActionResult DeleteBlog(int blogId)
        {
            // Extracting Authorization header value
            string authorizationValue = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationValue))
            {
                return Unauthorized("Authorization header is missing");
            }

            User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));


            Blog existingBlog = _blogService.GetBlogById(blogId);
            if (existingBlog == null)
            {
                var response = new ApiResponse<string>("404", "The specified blog does not exist.", null);
                return NotFound(response);
            }


            // Check if the user is authorized to update the blog
            if (existingBlog.user.UserId != userDetails.UserId)
            {
                var response = new ApiResponse<string>("403", "You are not authorized to delete this blog.", null);
                return Unauthorized(response);
            }


            var blog= _blogService.DeleteBlog(blogId);
            if (blog)
            {
                var response = new ApiResponse<string>("201", "The blog has been deleted successfully.", null);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<string>("500", "An error occurred while deleting the blog.", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // vote a blog
        [HttpPost("vote/{id}")]
        public IActionResult VoteBlog(int id, [FromBody] VoteBlogDTO blogVote)
        {
            Blog blog = _blogService.GetBlogById(id);
            if (blog == null)
            {
                var response = new ApiResponse<string>("404", "Blog not found", null);
                return NotFound(response);

            }

            // Extracting Authorization header value
            string userId = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(userId))
            {
                var response = new ApiResponse<string>("403", "You are not authorization to vote for blog", null);
                return Unauthorized(response);

            }

            // Retrieve user details
            User userDetails = _userRepository.GetUserById(int.Parse(userId));

            // Check vote
            BlogVote blogCheck = _blogService.GetBlogVote(id);

            if (blogCheck == null)
            {
                // Call service method to handle upvoting
                bool voted = _blogService.VoteBlog(blog, blogVote, userDetails);

                if (voted)
                {
                    var response = new ApiResponse<string>("201", "The blog voted successfully.", null);
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<string>("500", "Failed to vote the blog", null);
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                }
            }

            if (blogCheck.IsVote == blogVote.vote)
            {
                var blogVoteDelete = _blogService.DeleteBlogVote(blogCheck.VoteId);
                if (blogVoteDelete)
                {
                    var response = new ApiResponse<string>("201", "The blog vote has been deleted successfully.", null);
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<string>("500", "An error occurred while deleting the blog vote.", null);
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

            }

            if (blogCheck.IsVote != blogVote.vote)
            {
                Boolean blogUpdate = _blogService.UpdateBlogVote(blogCheck, blogVote, userDetails);

                if (blogUpdate)
                {
                    var response = new ApiResponse<string>("201", "The blog vote has been successfully updated.", null);
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<string>("500", "Failed to update the vote of the blog", null);
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }


            var responseReturn = new ApiResponse<string>("500", "Failed to vote of the blog", null);
            return StatusCode(StatusCodes.Status500InternalServerError, responseReturn);


        }



    }
}
