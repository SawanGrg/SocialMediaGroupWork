using System.Collections.Generic;
using GroupCoursework.ApiResponse;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.AspNetCore.Mvc;
using GroupCoursework.Filters;
using Microsoft.AspNetCore.Identity.Data;
using GroupCoursework.DTO; // Import the namespace containing AuthFilter

namespace GroupCoursework.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<User>>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            var response = new ApiResponse<IEnumerable<User>>("200", "Success", users);
            return Ok(response);
        }

        [HttpGet("specific-user/{user_id}")]
        public ActionResult<ApiResponse<UserWithBlogsDTO>> GetSpecificUser(int user_id)
        {
            var user = _userService.UserProfileDetails(user_id);
            if (user != null)
            {
                var response = new ApiResponse<UserWithBlogsDTO>("200", "User's profile", user);

                return Ok(response);

            }
            else
            {
                var response = new ApiResponse<string>("404", "User not found", null);
                return NotFound(response);
            }
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var user = _userService.AuthenticateUser(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                var response = new ApiResponse<User>("401", "Unauthorized register first!", null);
                return Unauthorized(response);
            }

            if (user.IsUserDeleted)
            {
                var response = new ApiResponse<User>("401", "Unauthorized: Your account has been deleted.", null);
                return Unauthorized(response);
            }

            var successResponse = new ApiResponse<User>("200", "Success", user);
            return Ok(successResponse);
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(AuthFilter))] // Apply AuthFilter to this action
        public ActionResult<ApiResponse<User>> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                var response = new ApiResponse<User>("404", "User not found", null);
                return NotFound(response);
            }
            var successResponse = new ApiResponse<User>("200", "Success", user);
            return Ok(successResponse);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            _userService.AddUser(user);
            var response = new ApiResponse<User>("201", "User created", user);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.UserId)
            {
                var errorResponse = new ApiResponse<User>("400", "Bad request", null);
                return BadRequest(errorResponse);
            }
            _userService.UpdateUser(user);
            var successResponse = new ApiResponse<User>("204", "User updated", null);
            return Ok(successResponse);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var isDeleted = _userService.DeleteUser(id);

            if (!isDeleted)
            {
                var response = new ApiResponse<User>("404", "User not found", null);
                return NotFound(response);
            }

            var successResponse = new ApiResponse<User>("204", "User deleted", null);
            return Ok(successResponse);
        }


        [HttpPut("passwordChangeProfile/{id}")]
        public IActionResult UpdatePassword(int id, [FromBody] ChangePassDTO pass)
        {
            var recentUser = _userService.GetUserById(id);

            if (recentUser == null)
            {
                var response = new ApiResponse<User>("404", "User not found", null);
                return NotFound(response);
            }

            if (recentUser.Password != pass.password)
            {
                var response = new ApiResponse<User>("400", "Old password did not match", null);
                return BadRequest(response);
            }

            var isUpdated = _userService.changePassword(recentUser.Email, pass.newPassword);

            if (!isUpdated)
            {
                var response = new ApiResponse<User>("500", "Failed to update password", null);
                return StatusCode(500, response);
            }

            var successResponse = new ApiResponse<User>("200", "User password updated", null);
            return Ok(successResponse);
        }


    }
}
