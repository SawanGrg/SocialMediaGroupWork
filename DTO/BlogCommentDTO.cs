using GroupCoursework.Models;

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

    public class BlogCommentDto
    {
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public Blog Blog { get; set; }
        public User User { get; set; }
        public bool IsCommentDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<BlogCommentVote> CommentVotes { get; set; }
    }
}
