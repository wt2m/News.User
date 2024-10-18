﻿using Microsoft.AspNetCore.Identity;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Identity
{
    internal class UserAuthenticationService : AbstractUowService, IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationLogs _authenticationLogs;

        public UserAuthenticationService(IUnitOfWork unitOfWork, IUserRepository userRepository, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, ITokenService tokenService, IAuthenticationLogs authenticationLogs) : base(unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _authenticationLogs = authenticationLogs;
        }

        public async Task<string> AuthenticateUserAsync(string email, string password, string remoteIpAddress)
        {
            var userApp = await _userManager.FindByEmailAsync(email);
            if (userApp == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(userApp, password, false);
            if (!result.Succeeded)
            {

                var attempts = await _userManager.GetAccessFailedCountAsync(userApp);

                if (attempts > 3)
                {
                    var failedLogin = new FailedLoginAttemptLog(userApp.Id, remoteIpAddress);
                    _ = _authenticationLogs.LogFailedLoginAsync(failedLogin);
                }

                throw new Exception("Invalid credentials.");
            }

            return await _tokenService.GenerateTokenAsync(userApp.Id);
        }

    }
}
