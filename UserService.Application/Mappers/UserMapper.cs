using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Mappers
{
    public class UserMapper
    {
        public static UserDTO ToUserDTO(ApplicationUser user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Username = user.UserName ?? "",
                FullName = user.FullName ?? "",
            };
        }
    }
}
