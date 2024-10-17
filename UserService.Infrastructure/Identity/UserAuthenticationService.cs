using Microsoft.AspNetCore.Identity;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Identity
{
    public class UserAuthenticationService : AbstractUowService, IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public UserAuthenticationService(IUnitOfWork unitOfWork, IUserRepository userRepository, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, ITokenService tokenService) : base(unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            var userApp = await _userManager.FindByEmailAsync(email);
            if (userApp == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(userApp, password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid credentials.");
            }

            return await _tokenService.GenerateTokenAsync(userApp.Id);
        }

    }
}
