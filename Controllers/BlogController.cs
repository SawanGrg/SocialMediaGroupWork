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
        [HttpGet]
        [ServiceFilter(typeof(AdminAuthFilter))]
        public ActionResult<ApiResponse<IEnumerable<Blog>>> GetAllBlogs()
        {
            var blogs = _blogService.GetAllBlogs();
            var response = new ApiResponse<IEnumerable<Blog>>("200", "Success", blogs);
            return Ok(response);
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
        [HttpPost]
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

            if (postBlogDTO.BlogImage.OpenReadStream().Length > 1000000)
                return BadRequest("Blog image size should not exceed 1MB");

            string imageUrl = _fileUploaderHelper.UploadFile(postBlogDTO.BlogImage);
            
            

           bool stateBlog =  _blogService.AddBlog(postBlogDTO, imageUrl, userDetails);

            if (stateBlog) { 
                var res = new ApiResponse<string>("201", "Blog created", "Blog posted SuccessFul");
                return Ok(res);
            }
            var response = new ApiResponse<string>("400", "Blog not created", "Blog not posted");
            return BadRequest(response);
        }




    }
}
