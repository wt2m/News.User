using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Services
{
    public interface IUserAuthenticationService
    {
        Task<string> AuthenticateUserAsync(string email, string password);
    }
}
