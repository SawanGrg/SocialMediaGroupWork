namespace GroupCoursework.DTO
{
    public class BlogCommentDTO
    {
        public string CommentContent { get; set; }
      
    }

    public class UpdateBlogCommentDTO
    {
        public string? CommentContent { get; set; }
       
    }

    public class VoteBlogCommentDTO
    {
        public bool vote {  get; set; }
    }
}
