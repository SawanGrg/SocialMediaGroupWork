using Microsoft.AspNetCore.Mvc;
using System;
using GroupCoursework.MailHandler.MailDTO;
using GroupCoursework.MailHandler.Service;
using System.Threading.Tasks;
using GroupCoursework.Utils;
using System.Collections.Generic;
using GroupCoursework.ApiResponse;
using GroupCoursework.Service; // Import your ApiResponse namespace

namespace GroupCoursework.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly UserService _userService;
        private static readonly Dictionary<string, string> EmailOtpMap = new Dictionary<string, string>();

        public EmailController(IMailService mailService, UserService userService)
        {
            _mailService = mailService;
            _userService = userService;
        }

        // POST api/email/forgot-password?email={email}
        [HttpPost("send-otp")]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPassword([FromQuery] string email)
        {
            try
            {
                string otp = OTPGenerator.GenerateOtp();

                var mailRequest = new MailRequest
                {
                    ToEmail = email,
                    Subject = "Password Reset",
                    Body = $"Your OTP for password reset is: {otp}"
                };

                // Add OTP to the dictionary with email as key
                EmailOtpMap[email] = otp;

                await _mailService.SendEmailAsync(mailRequest);

                return new ApiResponse<string>("200", "Password reset instructions sent to your email.", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>("500", $"Internal server error: {ex.Message}", null));
            }
        }

        //post request where user sends email, otp and new password from request param 
        [HttpPost("reset-password")]
        public ActionResult<ApiResponse<string>> ResetPassword([FromQuery] string email, [FromQuery] string otp, [FromQuery] string newPassword)
        {
            if (EmailOtpMap.ContainsKey(email) && EmailOtpMap[email] == otp)
            {

                //insert new password based on email of a user 
                 bool state = _userService.changePassword(email, newPassword);


                if (!state)
                {
                    return new ApiResponse<string>("400", "Failed to reset password", null);
                }


                // Reset password logic here
                EmailOtpMap.Remove(email);
                return new ApiResponse<string>("200", "Password reset successful", null);
            }
            else
            {
                return new ApiResponse<string>("400", "Invalid OTP", null);
            }
        }
    }
}
