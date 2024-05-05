using System.Collections.Generic;
using System.Numerics;
using GroupCoursework.DTO;
using GroupCoursework.Models;
using GroupCoursework.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GroupCoursework.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }



        public IEnumerable<Blog> GetBlogsByUser(int userId)
        {
            return _userRepository.GetBlogsByUser(userId);
        }

 
        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }

        //For user profile
        public UserWithBlogsDTO UserProfileDetails(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return null;
            }

            var blogs = _userRepository.GetBlogsByUser(userId)
                .Select(b => new BlogDTO
                {
                    BlogId = b.BlogId,
                    BlogImage = b.blogImageUrl,
                    Title = b.blogTitle,
                    Content = b.blogContent,
                    blogCreatedAt = b.blogCreatedAt,
                    isDeleted = b.isDeleted
                })
                .ToList();

            return new UserWithBlogsDTO
            {
                UserId = user.UserId,
                UserName = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Gender = user.Gender,
                Blogs = blogs
            };
        }


        public User AuthenticateUser(string Email, string password)
        {
            return _userRepository.AuthenticateUser(Email, password);
        }


        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public bool AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public bool DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }


        public bool changePassword(String email, String password)
        {
            return _userRepository.changePassword(email, password);
        }
    }
}
