using Microsoft.AspNetCore.Mvc;
using GroupCoursework.DTOs;
using GroupCoursework.Service;
using System;
using GroupCoursework.ApiResponse;
using GroupCoursework.Models;

namespace GroupCoursework.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        // Create admin account
        [HttpPost("/createAdmin")]
        public IActionResult CreateAdminAccount([FromBody] CreateAdminDTO userDTO)
        {
            try
            {
                if (_adminService.CreateAdminAccount(userDTO))
                {
                    return Ok("Admin account created successfully");
                }
                else
                {
                    return BadRequest("Failed to create admin account");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating admin account: {ex.Message}. Inner Exception: {ex.InnerException?.Message}");
            }
        }


        [HttpGet("/GetTopBlogs")]
        public IActionResult GetTopBlog([FromQuery] string month = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(month))
                {
                    Console.WriteLine("top 10 blog of specific month");
                    // Extract blogs based on the provided month
                    // Call a method in your service to fetch top blogs for the specified month
                    var topBlogsSpecificMonth = _adminService.GetTopBlogsForMonth(month);
                    var response = new ApiResponse<List<SpecificBlogsWithSuggestions>>("200", "Success", topBlogsSpecificMonth);
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine("top 10 blog of all time");
                    // Extract blogs of all time
                    var topBlogsAllTime = _adminService.GetTopBlogsAllTime();
                    var response = new ApiResponse<List<SpecificBlogsWithSuggestions>>("200", "Success", topBlogsAllTime);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>("500", $"Error retrieving top blogs: {ex.Message}. Inner Exception: {ex.InnerException?.Message}", null);
                return StatusCode(500, errorResponse);
            }
        }



    }
}
