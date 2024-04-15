using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GroupCoursework.Filters
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
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
            if (token == "user_id")
            {
                // Logic to validate token
                return true;
            }
            return false;
        }
    }
}
