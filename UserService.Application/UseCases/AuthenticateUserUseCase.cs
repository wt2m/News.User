using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Infrastructure.Identity;

namespace UserService.Application.UseCases
{
    public class AuthenticateUserUseCase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticateUserUseCase(
            SignInManager<ApplicationUser> signInManager,
            JwtTokenService jwtTokenService,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
        }

        public async Task<string> AuthenticateAsync(AuthenticateUserRequest request)
        {
            // First check if the user exists by email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Use SignInManager to validate the password
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                throw new Exception("Invalid credentials.");
            }

            // Generate JWT token
            return await _jwtTokenService.GenerateTokenAsync(user);
        }
    }
}
