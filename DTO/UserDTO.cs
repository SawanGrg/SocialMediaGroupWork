namespace GroupCoursework.DTO
{
    public class UserWithBlogsDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<BlogDTO> Blogs { get; set; }
    }

    public class BlogDTO
    {
        public int BlogId { get; set; }

        public string BlogImage { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateOnly blogCreatedAt { get; set; }
        public bool isDeleted { get; set; }

    }

}
