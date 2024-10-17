using Microsoft.AspNetCore.Identity;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.Mappers;
using UserService.Domain.Entities;
using UserService.Application.Services;

namespace UserService.Infrastructure.Identity
{
    public class UserRegistrationService : AbstractUowService, IUserRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRegistrationService(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> userManager) : base(_unitOfWork)
        {
            _userManager = userManager;
        }

        public async Task<UserDTO> RegisterUserAsync(RegisterUserRequest request)
        {
            var userDomain = new User(request.Email, request.FullName, request.Username);


            var userApp = new ApplicationUser(
                userDomain.Id,
                request.Email,
                request.Username,
                request.FullName
            );

            // Identity automatically hashes the password here
            var result = await _userManager.CreateAsync(userApp, request.Password);

            if (!result.Succeeded)
            {
                var error = string.Join(',', result.Errors.Select(e => e.Description!).ToList());
                throw new Exception(error);
            }

            
            try
            {
                await _unitOfWork.Users.AddAsync(userDomain);
                await _unitOfWork.CompleteAsync();
            } catch
            {
                await _userManager.DeleteAsync(userApp);
                throw new Exception("Error on trying to create your account.");
            }

            return UserMapper.ToUserDTO(userDomain);
        }
    }
}
