using System.Collections.Generic;
using GroupCoursework.ApiResponse;
using GroupCoursework.Models;
using GroupCoursework.Service;
using Microsoft.AspNetCore.Mvc;

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
            Console.WriteLine("User data:");
            foreach (var user in users)
            {
                Console.WriteLine($"User ID: {user.UserId}, Username: {user.Username}, Email: {user.Email}, ...");
            }

            var response = new ApiResponse<IEnumerable<User>>("200", "Success", users);
            return Ok(response);
        }


        //http://localhost:5000/api/user/1
        [HttpGet("{id}")]
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

        //http://localhost:5000/api/user
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            _userService.AddUser(user);
            var response = new ApiResponse<User>("201", "User created", user);
            // Now, let's create a response with status code 201 Created and include the newly created user in the response body.
            // In this example, we assume 'user' has been populated with data after being added to the database.
            return Ok(response); // Return the response with status code 201 Created and the newly created user in the response body.
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
    }
}
