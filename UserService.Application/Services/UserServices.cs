using UserService.Application.Interfaces;
using UserService.Domain.Repositories;

namespace UserService.Application.Services
{
    public class UserServices : IUserService
    {
        public IUserRepository _userRepository { get; set; }
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
