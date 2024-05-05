using GroupCoursework.Models;

namespace GroupCoursework.DTO
{
    public class VoteBlogDTO
    {
       
        public bool vote { get; set; }
       

        public VoteBlogDTO()
        {
        }

        public VoteBlogDTO(bool vote)
        {
            this.vote = vote;
        }

    }
}
