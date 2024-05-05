namespace GroupCoursework.DTO
{
    public class BlogCommentDTO
    {
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public bool IsCommentDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateBlogCommentDTO
    {
        public string? CommentContent { get; set; }
       
    }
}
