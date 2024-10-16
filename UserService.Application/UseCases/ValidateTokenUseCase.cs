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
    public class ValidateTokenUseCase
    {
        private readonly JwtTokenService _jwtTokenService;

        public ValidateTokenUseCase(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public bool ValidateUserToken(string token)
        {
            var result = _jwtTokenService.VerifyTokenAsync(token);

            if (result == null)
                return false;

            return true;
        }
    }
}
