using GroupCoursework.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GroupCoursework.Repositories;
using GroupCoursework.Models;

namespace GroupCoursework.Filters
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        private readonly UserRepository _userRepository;

        public AuthFilter(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Retrieve authorization token from request headers
            string authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

            // Check if authorization header is missing or not
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                // Unauthorized if token is missing
                context.Result = new UnauthorizedResult();
                return;
            }

            bool isTokenValid = ValidateToken(authorizationHeader);

            if (!isTokenValid)
            {
                // Unauthorized if token is invalid
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        private bool ValidateToken(string token)
        {
            int userId = int.Parse(token);
            User user = _userRepository.GetUserById(userId);

            if (user != null)
            {
                return true; // User exists
            }

            return false;
        }
    }
}
