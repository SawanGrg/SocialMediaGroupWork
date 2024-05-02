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
        
    }
}
