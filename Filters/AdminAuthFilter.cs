using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GroupCoursework.Repository;
using GroupCoursework.Repositories;
using GroupCoursework.Models;
using System.Numerics;


namespace GroupCoursework.Filters
{
    public class AdminAuthFilter : Attribute, IAuthorizationFilter
    {
        private readonly UserRepository _userRepository;

        public AdminAuthFilter(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Retrieve authorization token from request headers
            string authorizationHeaderValue = context.HttpContext.Request.Headers["Authorization"];

            // Check if authorization header is missing or not
            if (string.IsNullOrEmpty(authorizationHeaderValue))
            {
                // Unauthorized if token is missing
                context.Result = new UnauthorizedResult();
                return;
            }

            bool isAdmin = IsAdmin(authorizationHeaderValue);

            if (!isAdmin)
            {
                // Unauthorized if user is not an admin
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        private bool IsAdmin(string token)
        {
            int userId = int.Parse(token);
            User user = _userRepository.GetUserById(userId);

            if(user != null && user.Role == "Admin")
            {
                return true; // User is an admin
            }

            return false; // Default: Not an admin
        }
    }
}
