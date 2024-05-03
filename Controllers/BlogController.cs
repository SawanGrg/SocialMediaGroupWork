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
        public ActionResult<ApiResponse<BlogPaginationDTO>> GetAllBlogs( string  sortOrder = null, int pageNumber = 1, int pageSize = 6)
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



        //// vote a blog
        //[HttpPost("{id}/vote")]
        //public IActionResult VoteBlog(int id, [FromBody] VoteBlogDTO blogVote)
        //{
        //    var blog = _blogService.GetBlogById(id);
        //    if (blog == null)
        //    {
        //        return NotFound("Blog not found");
        //    }

        //    // Extracting Authorization header value
        //    string authorizationValue = HttpContext.Request.Headers["Authorization"];
        //    if (string.IsNullOrEmpty(authorizationValue))
        //    {
        //        // Handle case when Authorization header is missing
        //        return Unauthorized("Authorization header is missing");
        //    }

        //    // Retrieve user details
        //    User userDetails = _userRepository.GetUserById(int.Parse(authorizationValue));

        //    // Call service method to handle upvoting
        //    bool voted = _blogService.VoteBlog(blogVote, userDetails);

        //    if (voted)
        //    {
        //        return Ok("Blog voted successfully");
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to upvote blog");
        //    }
        //}



    }
}
