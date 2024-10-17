using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Infrastructure.Identity;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    public UserController()
    {
    }

    [Authorize] 
    [HttpPut("update-fullname")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequest model)
    {
        // Get the user ID from the JWT token claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("Invalid token. No user ID found.");
        }

        await Task.Delay(1);
        
        //TODO: Implement the update user function
        throw new NotImplementedException();
    }

    
}