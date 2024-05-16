using GroupCoursework.DTO;
using GroupCoursework.DTOs;
using GroupCoursework.Models;
using GroupCoursework.Repository;
using GroupCoursework.Utils;

namespace GroupCoursework.Service
{
    public class AdminService
    {
        private AdminRepository _adminRepository;
        private CreateAdminDTO _userDTO;
        private ValueMapper _valueMapper;

        public AdminService(
                       AdminRepository adminRepository,
                                  CreateAdminDTO userDTO,
                                             ValueMapper valueMapper
        )
        {
            _adminRepository = adminRepository;
            _userDTO = userDTO;
            _valueMapper = valueMapper;
        }

        public Boolean CreateAdminAccount(CreateAdminDTO userDTO)
        {
            Console.WriteLine("Creating admin account at service 1 ");

            User user = _valueMapper.MapToAdminUser(userDTO);

            Console.WriteLine("Creating admin account at service 2");

            return _adminRepository.CreateAdminAccount(user);
        }

        public List<SpecificBlogsWithSuggestions> GetTopBlogsAllTime()
        {
            return _adminRepository.Get10TopBlogsAllTime();
        }

        public List<SpecificBlogsWithSuggestions> GetTopBlogsForMonth(String Month)
        {
            return _adminRepository.Get10TopBlogsForMonth(Month);
        }


        public List<User> GetTopBlogger()
        {
            return _adminRepository.Get10TopBlogger();
        }

        //public CumulativeCountsDTO GetCumulativeCounts(string month)
        //{
        //    var allCounts = _adminRepository.GetCumulativeCountsAllTime();
        //    var monthCounts = _adminRepository.GetCumulativeCountsForMonth(month);

        //    return new CumulativeCountsDTO
        //    {
        //        BlogPostsCount = allCounts.BlogPostsCount,
        //        UpvotesCount = allCounts.UpvotesCount,
        //        DownvotesCount = allCounts.DownvotesCount,
        //        CommentsCount = allCounts.CommentsCount,
        //        MonthPostsCount = monthCounts.BlogPostsCount,
        //        MonthUpvotesCount = monthCounts.UpvotesCount,
        //        MonthDownvotesCount = monthCounts.DownvotesCount,
        //        MonthCommentsCount = monthCounts.CommentsCount
        //    };
        //}

        public CumulativeCountsDTO GetCumulativeCounts(string month)
        {
            if (string.IsNullOrEmpty(month))
            {
                return _adminRepository.GetCumulativeCountsAllTime();
            }
            else
            {
                var monthCounts = _adminRepository.GetCumulativeCountsForMonth(month);
                var allCounts = _adminRepository.GetCumulativeCountsAllTime();

                return new CumulativeCountsDTO
                {
                    BlogPostsCount = allCounts.BlogPostsCount,
                    UpvotesCount = allCounts.UpvotesCount,
                    DownvotesCount = allCounts.DownvotesCount,
                    CommentsCount = allCounts.CommentsCount,
                    MonthPostsCount = monthCounts.BlogPostsCount,
                    MonthUpvotesCount = monthCounts.UpvotesCount,
                    MonthDownvotesCount = monthCounts.DownvotesCount,
                    MonthCommentsCount = monthCounts.CommentsCount
                };
            }
        }

    }
}
