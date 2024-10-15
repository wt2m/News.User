using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.UseCases;

namespace UserService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUserUseCase _registerUserUseCase;
        private readonly AuthenticateUserUseCase _authenticateUserUseCase;

        public AuthController(RegisterUserUseCase registerUserUseCase, AuthenticateUserUseCase authenticateUserUseCase)
        {
            _registerUserUseCase = registerUserUseCase;
            _authenticateUserUseCase = authenticateUserUseCase;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                var result = await _registerUserUseCase.RegisterAsync(request);
                if (result.Succeeded)
                {
                    return Ok("User registered successfully.");
                }
                else
                {
                    return BadRequest(result.Errors);
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
            try
            {
                var token = await _authenticateUserUseCase.AuthenticateAsync(request);
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
    }
}
