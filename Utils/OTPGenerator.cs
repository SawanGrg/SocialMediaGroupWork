namespace GroupCoursework.Utils
{
    public class OTPGenerator
    {
            public static string GenerateOtp()
            {
                // Generate a Guid and extract the first 6 characters
                string guid = Guid.NewGuid().ToString();
                string otp = guid.Substring(0, 6);

                return otp;
            }
    }
}
