using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    public UserController(UserManager<ApplicationUser> userManager)
    {
    }

    [Authorize] 
    [HttpPut("update-fullname")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateFullNameRequest model)
    {
        // Get the user ID from the JWT token claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("Invalid token. No user ID found.");
        }

        // Find the user by ID
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found.");

        user.UpdateProfile(model.FullName);

        // Save the changes
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return Ok("Profile updated successfully.");
        
        // If updating fails, return the errors
        return BadRequest(result.Errors);
    }

    
}