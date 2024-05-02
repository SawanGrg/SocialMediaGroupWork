namespace GroupCoursework.DTOs
{
    public class CreateAdminDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

        public string Role { get; set; }

        public bool IsAdmin { get; set; }
    }
}
