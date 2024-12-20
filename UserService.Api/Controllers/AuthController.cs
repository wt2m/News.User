﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.Services;

namespace UserService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRegistrationService userRegistrationService, IUserAuthenticationService userAuthenticationService, ITokenService tokenService)
        {
            _userAuthenticationService = userAuthenticationService;
            _userRegistrationService = userRegistrationService;
            _tokenService = tokenService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                var result = await _userRegistrationService.RegisterUserAsync(request);
                if (result != null)
                {
                    return Ok("User registered successfully.");
                }
                else
                {
                    return BadRequest("Error trying to register user.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
        {
            var ip2 = HttpContext?.Connection?.RemoteIpAddress!.ToString() ?? "";

            var token2 = await _userAuthenticationService.AuthenticateUserAsync(request.Email, request.Password, ip2);
            try
            {
                var ip = HttpContext?.Connection?.RemoteIpAddress!.ToString() ?? "";

                var token = await _userAuthenticationService.AuthenticateUserAsync(request.Email, request.Password, ip);
                if (token == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("verifyToken")]
        public IActionResult VerifyToken([FromQuery] string token)
        {
            var tokenIsValid = _tokenService.VerifyTokenAsync(token);
            if (!tokenIsValid)
            {
                return Unauthorized("Invalid token");
            }

            return Ok("Token is valid");
        }
    }
}
