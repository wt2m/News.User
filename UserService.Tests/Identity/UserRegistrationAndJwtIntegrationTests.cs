using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure;
using UserService.Infrastructure.DependencyInjection;
using UserService.Tests.Fixtures;

public class UserRegistrationAndJwtIntegrationTests : IClassFixture<UserAuthTestSetupFixture>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly ITokenService _tokenService;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public UserRegistrationAndJwtIntegrationTests(UserAuthTestSetupFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
        _userRegistrationService = _serviceProvider.GetRequiredService<IUserRegistrationService>();
        _tokenService = _serviceProvider.GetRequiredService<ITokenService>();
        _userAuthenticationService = _serviceProvider.GetRequiredService<IUserAuthenticationService>();
    }

    [Fact]
    public async Task RegisterUserAndGenerateJwtToken_ShouldReturnValidToken()
    {
        // Arrange
        var registerUserRequest = new RegisterUserRequest
        {
            Email = "testuser@example.com",
            FullName = "Test User",
            Username = "testuser",
            Password = "SecurePassword123!"
        };

        // Act - Register the user
        var userDto = await _userRegistrationService.RegisterUserAsync(registerUserRequest);

        // Generate a JWT token for the registered user
        var token = await _userAuthenticationService.AuthenticateUserAsync(registerUserRequest.Email, registerUserRequest.Password, "");

        // Assert - Verify token is not null or empty
        Assert.False(string.IsNullOrEmpty(token));

        // Verify the token for validity and correct claims
        var isValid = _tokenService.VerifyTokenAsync(token);
        Assert.True(isValid);
    }


    /*
     TODO:


        Seggregate tests from token generation and user registration
        
        User Registration
            > not an email error test
            > weak password error test
            > user with null name error

        User Auth test
            > Invalid email error
            > Failed log-in error
            > multiple tries block
            > Log dispach on 3 or more tries


        JWT Tests
            > Invalid user id error
            > JWT generation
            > Forged JWT error
            > Token validation
            > Get user id by token method tests

        
        UoW Tests
            > Maybe a dynamic testing for the generic interface of UoW members
            > i dunno, think more about the UoW tests

        Update Preferences
            > Test the method to insert preferences
            > Test to unique method validation

     
     */

}