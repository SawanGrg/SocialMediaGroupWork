using Microsoft.AspNetCore.Mvc;
using GroupCoursework.DTOs;
using GroupCoursework.Service;
using System;

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
        [HttpPost]
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
    }
}
