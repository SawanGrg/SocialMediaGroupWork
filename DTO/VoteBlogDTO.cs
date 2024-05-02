using GroupCoursework.Models;

namespace GroupCoursework.DTO
{
    public class VoteBlogDTO
    {
        public Blog Blog { get; set; }
        public bool vote { get; set; }
       

        public VoteBlogDTO()
        {
        }

        public VoteBlogDTO(Blog Blog, bool vote)
        {
            this.Blog = Blog;
            this.vote = vote;
        }

    }
}
