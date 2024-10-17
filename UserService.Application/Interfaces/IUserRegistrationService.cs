using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;

namespace UserService.Application.Interfaces
{
    public interface IUserRegistrationService
    {
        Task<UserDTO> RegisterUserAsync(RegisterUserRequest request);
    }
}
