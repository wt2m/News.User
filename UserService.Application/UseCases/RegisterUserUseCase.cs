using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Domain.Repositories;
using UserService.Domain.Services;
using UserService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace UserService.Application.UseCases
{
    public class RegisterUserUseCase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUserUseCase(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserRequest request)
        {
            var newUser = new ApplicationUser(
                request.Email,
                request.Username,
                request.FullName
            );

            // Identity automatically hashes the password here
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }
    }
}
