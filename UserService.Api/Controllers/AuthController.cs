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
        private readonly ValidateTokenUseCase _validateTokenUseCase;

        public AuthController(RegisterUserUseCase registerUserUseCase, AuthenticateUserUseCase authenticateUserUseCase, ValidateTokenUseCase validateTokenUseCase)
        {
            _registerUserUseCase = registerUserUseCase;
            _authenticateUserUseCase = authenticateUserUseCase;
            _validateTokenUseCase = validateTokenUseCase;
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


        [HttpGet("verifyToken")]
        public IActionResult VerifyToken([FromQuery] string token)
        {
            var validated = _validateTokenUseCase.ValidateUserToken(token);
            if (!validated)
            {
                return Unauthorized("Invalid token");
            }

            return Ok("Token is valid");
        }
    }
}
